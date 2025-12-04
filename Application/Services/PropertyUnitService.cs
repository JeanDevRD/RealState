using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealState.Core.Application.DTOs.Chat;
using RealState.Core.Application.DTOs.Common;
using RealState.Core.Application.DTOs.PropertyOffer;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Application.DTOs.User;
using RealState.Core.Application.Interfaces;
using RealState.Core.Domain.Common.Enums;
using RealState.Core.Domain.Entities;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Services
{
    public class PropertyUnitService : GenericService<PropertyUnit, PropertyUnitDto>, IPropertyUnitService
    {
        private readonly IPropertyUnitRepository _propertyUnitRepo;
        private readonly IChatRepository _chatRepo;
        private readonly IPropertyOfferRepository _offerRepo;
        private readonly IAccountServiceForApp _clientService;
        private readonly IMapper _mapper;
        
        public PropertyUnitService(IPropertyUnitRepository propertyUnitRepo, IMapper mapper, IChatRepository chatRepo, IAccountServiceForApp clientService,
            IPropertyOfferRepository offerRepo) : base(propertyUnitRepo, mapper)
        {
            _propertyUnitRepo = propertyUnitRepo;
            _mapper = mapper;
            _chatRepo = chatRepo;
            _clientService = clientService;
            _offerRepo = offerRepo;
        }

        public async Task<List<PropertyUnitDto>> GetAllWithInclude()
        {
            try
            {
                var propertyUnits = await _propertyUnitRepo.GetAllListIncluide(["PropertyType", "SaleType", "ImprovementTypes", "Chats", "PropertyOffers"]);
                if (propertyUnits == null)
                {
                    return new List<PropertyUnitDto>();
                }
                return _mapper.Map<List<PropertyUnitDto>>(propertyUnits);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving property units with included data: " + ex.Message);
            }
        }

        #region Property Unit Counting by Admin

        public async Task<int> TotalPropertyUnitsAsync()
        {
            var propertyUnits = await _propertyUnitRepo.GetAllListAsync();
            return propertyUnits.Count();
        }

        #endregion

        #region Propierty Details whith message and offer by Agent

        public async Task<ResultDto<PropertyDetailsDto>> GetPropertyDetailByAgent(int idProperty)
        {
            var result = new ResultDto<PropertyDetailsDto>
            {
                Message = new List<string>()
            };

            try
            {
                var propertyIncludes = new List<string>
                {
                 "PropertyType",
                 "SaleType",
                 "ImprovementTypes",
                 "Images"
                };

                var propertyQuery = _propertyUnitRepo.GetAllQueryIncluide(propertyIncludes);
                var getProperty = await propertyQuery.FirstOrDefaultAsync(p => p.Id == idProperty);

                if (getProperty == null)
                {
                    result.IsError = true;
                    result.Message.Add("La propiedad no existe");
                    return result;
                }

                var propertyDetails = _mapper.Map<PropertyDetailsDto>(getProperty);
                propertyDetails.PropertyTypeName = getProperty.PropertyType?.Name ?? "N/A";
                propertyDetails.SalesName = getProperty.SaleType?.Name ?? "N/A";
                propertyDetails.ImprovementTypesNames = getProperty.ImprovementTypes?.Select(i => i.Name).ToList()
                    ?? new List<string>();


                var chats = await _chatRepo.GetAllQueryAsync().Where(c => c.IdProperty == idProperty).ToListAsync();

                var chatDtos = new List<ChatWithPropertyDetails>();

                foreach (var chat in chats)
                {
                    var client = await _clientService.GetUserById(chat!.IdClient);

                    chatDtos.Add(new ChatWithPropertyDetails
                    {
                        Id = chat.Id,
                        IdClient = chat.IdClient,
                        NameClient = client != null? $"{client.FirstName} {client.LastName}" : "N/A"
                    });
                }

                propertyDetails.Chats = chatDtos;


                var offers = await _offerRepo.GetAllQueryAsync().Where(o => o != null && o.IdProperty == idProperty)
                    .Select(o => o!) .Include(o => o.Property).ToListAsync();

                var offerDtos = new List<PropertyOfferWithPropertyDetails>();

             

                foreach (var offer in offers)
                {
                    var client = await _clientService.GetUserById(offer.IdClient);

                    offerDtos.Add(new PropertyOfferWithPropertyDetails
                    {
                        Id = offer.Id,
                        IdClient = offer.IdClient,
                        NameClient = client != null ? $"{client.FirstName} {client.LastName}" : "Usuario eliminado",
                        OfferDate = offer.OfferDate,
                        OfferAmount = offer.OfferAmount,
                        OfferStatus = offer.OfferStatus,
                        Property = _mapper.Map<PropertyUnitDto>(offer.Property)
                    });
                }

                propertyDetails.ClientWithOffer = offerDtos.GroupBy(o => o.IdClient)
                    .Select(g => new ClientWithPropertyOffer
                    {
                        NameClient = g.First().NameClient,
                        PropertyOffers = g.ToList()

                    }).ToList();


                result.Data = propertyDetails;
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message.Add(ex.Message);
            }

            return result;
        }

        #endregion

        #region Property Units by Agent

        public async Task<ResultDto<List<PropertyUnitDto>>> GetAllPropertyUnitsByAgent(string idAgent, bool onlyAvailable = false)
        {
            var result = new ResultDto<List<PropertyUnitDto>>
            {
                Data = new List<PropertyUnitDto>(),
                Message = new List<string>()
            };
            try
            {
                var propertyUnits = await _propertyUnitRepo.GetAllQueryAsync().Where(p => p.IdAgent == idAgent).ToListAsync();

                if (onlyAvailable == true)
                {
                    propertyUnits = propertyUnits.Where(p => p.StateProperty == 1).ToList();
                }

                if (!propertyUnits.Any())
                {
                    result.IsError = true;
                    result.Message.Add("Este agente no tiene propiedades");
                    return result;
                }
                result.Data = _mapper.Map<List<PropertyUnitDto>>(propertyUnits);
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message.Add(ex.Message);
            }
            return result;
        }

        #endregion

        #region Home - List all available properties
        public async Task<ResultDto<List<PropertyCardDto>>> GetAllAvailablePropertiesAsync()
        {
            var result = new ResultDto<List<PropertyCardDto>>
            {
                Data = new List<PropertyCardDto>(),
                Message = new List<string>()
            };

            try
            {
                var propertyIncludes = new List<string> { "PropertyType", "SaleType" };

                var properties = await _propertyUnitRepo.GetAllQueryIncluide(propertyIncludes)
                    .Where(p => p!.StateProperty == (int)StateProperty.Available).OrderByDescending(p => p!.Id).ToListAsync();

                if (!properties.Any())
                {
                    result.IsError = true;
                    result.Message.Add("No hay propiedades disponibles");
                    return result;
                }

                result.Data = properties.Select(p => new PropertyCardDto
                {
                    Id = p!.Id,
                    PropertyTypeName = p.PropertyType?.Name ?? "N/A",
                    FirstImage = p.Images.FirstOrDefault() ?? "",
                    CodeProperty = p.CodeProperty,
                    SaleTypeName = p.SaleType?.Name ?? "N/A",
                    Price = p.Price,
                    Bedrooms = p.Bedrooms,
                    Bathrooms = p.Bathrooms,
                    SizeM = p.SizeM
                }).ToList();
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message.Add(ex.Message);
            }

            return result;
        }
        #endregion

        #region Search property by code

        public async Task<ResultDto<PropertyCardDto>> GetPropertyByCodeAsync(string code)
        {
            var result = new ResultDto<PropertyCardDto>
            {
                Message = new List<string>()
            };

            try
            {
                var propertyIncludes = new List<string> { "PropertyType", "SaleType" };

                var property = await _propertyUnitRepo.GetAllQueryIncluide(propertyIncludes)
                    .FirstOrDefaultAsync(p => p!.CodeProperty == code && p.StateProperty == (int)StateProperty.Available);

                if (property == null)
                {
                    result.IsError = true;
                    result.Message.Add($"No se encontró ninguna propiedad disponible con el código: {code}");
                    return result;
                }

                result.Data = new PropertyCardDto
                {
                    Id = property.Id,
                    PropertyTypeName = property.PropertyType?.Name ?? "N/A",
                    FirstImage = property.Images.FirstOrDefault() ?? "",
                    CodeProperty = property.CodeProperty,
                    SaleTypeName = property.SaleType?.Name ?? "N/A",
                    Price = property.Price,
                    Bedrooms = property.Bedrooms,
                    Bathrooms = property.Bathrooms,
                    SizeM = property.SizeM
                };
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message.Add(ex.Message);
            }

            return result;
        }

        #endregion

        #region Filter properties

        public async Task<ResultDto<List<PropertyCardDto>>> FilterPropertiesAsync(PropertyFilterDto filter)
        {
            var result = new ResultDto<List<PropertyCardDto>>
            {
                Data = new List<PropertyCardDto>(),
                Message = new List<string>()
            };

            try
            {
                var propertyIncludes = new List<string> { "PropertyType", "SaleType" };

                var query = _propertyUnitRepo.GetAllQueryIncluide(propertyIncludes)
                    .Where(p => p!.StateProperty == (int)StateProperty.Available);

                if (filter.PropertyTypeId.HasValue)
                {
                    query = query.Where(p => p!.PropertyTypeId == filter.PropertyTypeId.Value);
                }

                if (filter.MinPrice.HasValue)
                {
                    query = query.Where(p => p!.Price >= filter.MinPrice.Value);
                }

                if (filter.MaxPrice.HasValue)
                {
                    query = query.Where(p => p!.Price <= filter.MaxPrice.Value);
                }

                if (filter.Bedrooms.HasValue)
                {
                    query = query.Where(p => p!.Bedrooms >= filter.Bedrooms.Value);
                }

                if (filter.Bathrooms.HasValue)
                {
                    query = query.Where(p => p!.Bathrooms >= filter.Bathrooms.Value);
                }

                var properties = await query.OrderByDescending(p => p!.Id).ToListAsync();

                if (!properties.Any())
                {
                    result.IsError = true;
                    result.Message.Add("No se encontraron propiedades con los filtros aplicados");
                    return result;
                }

                var result1 = _mapper.Map<List<PropertyCardDto>>(properties);

                result.Data = result1;
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message.Add(ex.Message);
            }

            return result;
        }

        #endregion

        #region Property details for Home (con info del agente)

        public async Task<ResultDto<PropertyDetailHomeDto>> GetPropertyDetailForHomeAsync(int id)
        {
            var result = new ResultDto<PropertyDetailHomeDto>
            {
                Message = new List<string>()
            };

            try
            {
                var propertyIncludes = new List<string>
                {
                 "PropertyType",
                 "SaleType",
                 "ImprovementTypes"
                };

                var property = await _propertyUnitRepo.GetAllQueryIncluide(propertyIncludes)
                    .FirstOrDefaultAsync(p => p!.Id == id);

                if (property == null)
                {
                    result.IsError = true;
                    result.Message.Add("La propiedad no existe");
                    return result;
                }

                var agent = await _clientService.GetUserById(property.IdAgent);

                if (agent == null)
                {
                    result.IsError = true;
                    result.Message.Add("No se encontró información del agente");
                    return result;
                }

                result.Data = new PropertyDetailHomeDto
                {
                    Id = property.Id,
                    PropertyTypeName = property.PropertyType?.Name ?? "N/A",
                    SaleTypeName = property.SaleType?.Name ?? "N/A",
                    CodeProperty = property.CodeProperty,
                    Price = property.Price,
                    Bedrooms = property.Bedrooms,
                    Bathrooms = property.Bathrooms,
                    SizeM = property.SizeM,
                    Description = property.Description,
                    Images = property.Images,
                    ImprovementNames = property.ImprovementTypes?.Select(i => i.Name).ToList() ?? new List<string>(),
                    AgentName = $"{agent.FirstName} {agent.LastName}",
                    AgentPhone = agent.Phone ?? "N/A",
                    AgentEmail = agent.Email,
                    AgentPhoto = null
                };
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message.Add(ex.Message);
            }

            return result;
        }

        #endregion

        #region Generate unique property code

        public async Task<string> GenerateUniquePropertyCodeAsync()
        {
            string code;
            bool exists;
            do
            {
                code = new Random().Next(100000, 999999).ToString();
                exists = await _propertyUnitRepo.GetAllQueryAsync().AnyAsync(p => p!.CodeProperty == code);
            } while (exists);

            return code;
        }

        #endregion
    }

}
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealState.Core.Application.DTOs.Chat;
using RealState.Core.Application.DTOs.Common;
using RealState.Core.Application.DTOs.PropertyOffer;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Application.DTOs.User;
using RealState.Core.Application.Interfaces;
using RealState.Core.Domain.Entities;
using RealState.Core.Domain.Interfaces;
using System.Xml.Linq;

namespace RealState.Core.Application.Services
{
    public class PropertyUnitService : GenericService<PropertyUnit, PropertyUnitDto>
    {
        private readonly IPropertyUnitRepository _propertyUnitRepo;
        private readonly IChatRepository _chatRepo;
        private readonly IPropertyOfferRepository _offerRepo;
        private readonly IAccountServiceForApp _clientService;
        private readonly IMapper _mapper;
        public PropertyUnitService(IPropertyUnitRepository propertyUnitRepo, IMapper mapper, IChatRepository chatRepo, IAccountServiceForApp clientService, IPropertyOfferRepository offerRepo) : base(propertyUnitRepo, mapper)
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

        public async Task<ResultDto<PropertyDetails>> GetPropertyDetailByAgent(int idProperty)
        {
            var result = new ResultDto<PropertyDetails>
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

                var propertyDetails = _mapper.Map<PropertyDetails>(getProperty);
                propertyDetails.PropertyTypeName = getProperty.PropertyType?.Name ?? "N/A";
                propertyDetails.SalesName = getProperty.SaleType?.Name ?? "N/A";
                propertyDetails.ImprovementTypesNames = getProperty.ImprovementTypes?
                    .Select(i => i.Name).ToList()
                    ?? new List<string>();


                var chats = await _chatRepo.GetAllQueryAsync()
                    .Where(c => c.IdProperty == idProperty)
                    .ToListAsync();

                var chatDtos = new List<ChatWithPropertyDetails>();

                foreach (var chat in chats)
                {
                    var client = await _clientService.GetUserById(chat!.IdClient);

                    chatDtos.Add(new ChatWithPropertyDetails
                    {
                        Id = chat.Id,
                        IdClient = chat.IdClient,
                        NameClient = client != null
                            ? $"{client.FirstName} {client.LastName}"
                            : "N/A"
                    });
                }

                propertyDetails.Chats = chatDtos;


                var offers = await _offerRepo.GetAllQueryAsync().Where(o => o.IdProperty == idProperty).Include(o => o.Property)   
                    .ToListAsync();

                var offerDtos = new List<PropertyOfferWithPropertyDetails>();

                foreach (var offer in offers)
                {
                    var client = await _clientService.GetUserById(offer.IdClient);

                    offerDtos.Add(new PropertyOfferWithPropertyDetails
                    {
                        Id = offer.Id,
                        IdClient = offer.IdClient,
                        NameClient = client != null
                            ? $"{client.FirstName} {client.LastName}"
                            : "Usuario eliminado",
                        OfferDate = offer.OfferDate,
                        OfferAmount = offer.OfferAmount,
                        OfferStatus = offer.OfferStatus,
                        Property = _mapper.Map<PropertyUnitDto>(offer.Property)
                    });
                }

                propertyDetails.ClientWithOffer = offerDtos
                    .GroupBy(o => o.IdClient)
                    .Select(g => new ClientWithPropertyOffer
                    {
                        NameClient = g.First().NameClient,
                        PropertyOffers = g.ToList()
                    })
                    .ToList();


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

                if(onlyAvailable == true) 
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
    }
}
 
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealState.Core.Application.DTOs.PropertyOffer;
using RealState.Core.Application.Interfaces;
using RealState.Core.Domain.Entities;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Services
{
    public class PropertyOfferService : GenericService<PropertyOffer, PropertyOfferDto>, IPropertyOfferService
    {
        IPropertyOfferRepository _propertyOffer;
        IMapper _mapper;

        public PropertyOfferService(IPropertyOfferRepository propertyOffer, IMapper mapper) : base(propertyOffer, mapper)
        {
            _propertyOffer = propertyOffer;
            _mapper = mapper;
        }

        public async Task<List<PropertyOfferDto>> GetAllWhithInclude()
        {
            try
            {
                var messages = await _propertyOffer.GetAllListIncluide(["Property"]);
                if (messages == null)
                {
                    return new List<PropertyOfferDto>();
                }
                return _mapper.Map<List<PropertyOfferDto>>(messages);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving messages: " + ex.Message);
            }
        }

        public async Task<List<PropertyOfferDto>> GetByClientAndProperty(string idClient, int idProperty)
        {
            try
            {
                var offers = await _propertyOffer.GetAllQueryAsync()
               .Where(o => o.IdClient == idClient && o.IdProperty == idProperty)
               .ToListAsync();

                return _mapper.Map<List<PropertyOfferDto>>(offers);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving offers by client and property: " + ex.Message);
            }
        }

        public async Task<PropertyOfferDto> UpdateStatus(int propertyOfferId, bool status)
        {
            try
            {
                var offer = await _propertyOffer.GetByIdAsync(propertyOfferId);
                if (offer == null)
                {
                    throw new Exception("Offer not found");
                }

                if (status)
                {
                    offer.OfferStatus = 1;
                }
                else
                {
                    offer.OfferStatus = 2;
                }

                var updatedOffer = await _propertyOffer.UpdateAsync(offer, propertyOfferId);
                return _mapper.Map<PropertyOfferDto>(updatedOffer);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating offer status: " + ex.Message);
            }
        }
    }



}

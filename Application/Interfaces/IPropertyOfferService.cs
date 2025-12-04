using RealState.Core.Application.DTOs.PropertyOffer;

namespace RealState.Core.Application.Interfaces
{
    public interface IPropertyOfferService : IGenericService<PropertyOfferDto>
    {
        Task<List<PropertyOfferDto>> GetAllWhithInclude();
        Task<List<PropertyOfferDto>> GetByClientAndProperty(string idClient, int idProperty);
        Task<PropertyOfferDto> UpdateStatus(int propertyOfferId, bool status);
    }
}
using RealState.Core.Application.DTOs.UserFavoritePropertyUnit;

namespace RealState.Core.Application.Interfaces
{
    public interface IFavoritePropertyServices : IGenericService<UserFavoritePropertyUnitDto>
    {
        Task<List<UserFavoritePropertyUnitDto>> GetFavoritesByClient(string clientId);
        
    }
}
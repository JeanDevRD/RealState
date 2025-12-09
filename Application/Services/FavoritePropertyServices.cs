using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealState.Core.Application.DTOs.UserFavoritePropertyUnit;
using RealState.Core.Application.Interfaces;
using RealState.Core.Domain.Entities;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Services
{
    public class FavoritePropertyServices : GenericService<UserFavoritePropertyUnit, UserFavoritePropertyUnitDto>, IFavoritePropertyServices
    {
        IUserFavoritePropertyUnitRepository _favoritePropertyRepo;
        IMapper _mapper;


        public FavoritePropertyServices(IUserFavoritePropertyUnitRepository favoritePropertyRepo, IMapper mapper)
            : base(favoritePropertyRepo, mapper)
        {
            _favoritePropertyRepo = favoritePropertyRepo;
            _mapper = mapper;
        }

        public async Task<List<UserFavoritePropertyUnitDto>> GetFavoritesByClient(string clientId)
        {
            try
            {
                var favoriteProperties = await _favoritePropertyRepo.GetAllQueryAsync().Where(f => f!.IdClient == clientId).ToListAsync();

                if (favoriteProperties == null)
                {
                    return new List<UserFavoritePropertyUnitDto>();
                }
                return _mapper.Map<List<UserFavoritePropertyUnitDto>>(favoriteProperties);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving favorite properties: " + ex.Message);
            }
        }
    }
}

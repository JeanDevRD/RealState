using RealState.Core.Domain.Entities;
using RealState.Core.Domain.Interfaces;
using RealState.Infrastructure.Persistence.Context;

namespace RealState.Infrastructure.Persistence.Repositories
{
    public class UserFavoritePropertyUnitRepository : GenericRepository<UserFavoritePropertyUnit>, IUserFavoritePropertyUnitRepository
    {
        public UserFavoritePropertyUnitRepository(RealStateContextSql context) : base(context)
        {
        }
    }
}

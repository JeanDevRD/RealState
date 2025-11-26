using RealState.Core.Domain.Entities;
using RealState.Core.Domain.Interfaces;
using RealState.Infrastructure.Persistence.Context;

namespace RealState.Infrastructure.Persistence.Repositories
{
    public class ChatRepository : GenericRepository<Chat>, IChatRepository
    {
        public ChatRepository(RealStateContextSql context) : base(context)
        {
        }
    }
}

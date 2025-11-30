namespace RealState.Core.Domain.Interfaces
{
    public interface IGenericRepository<Entity> where Entity : class
    {
        Task<Entity?> AddAsync(Entity entity);
        Task DeleteAsync(int Id);
        Task<List<Entity>> GetAllListAsync();
        Task<List<Entity>> GetAllListIncluide(List<string> properties);
        IQueryable<Entity?> GetAllQueryAsync();
        IQueryable<Entity?> GetAllQueryIncluide(List<string> properties);
        Task<Entity?> GetByIdAsync(int Id);
        Task<Entity?> UpdateAsync(Entity entity, int Id);
    }
}
namespace RealState.Core.Application.Interfaces
{
    public interface IGenericService<EntityDto> where EntityDto : class
    {
        Task<EntityDto> AddAsync(EntityDto entityDto);
        Task DeleteAsync(int id);
        Task<List<EntityDto>> GetAllAsync();
        Task<EntityDto> GetByIdAsync(int id);
        Task<EntityDto> UpdateAsync(int id, EntityDto entityDto);
    }
}
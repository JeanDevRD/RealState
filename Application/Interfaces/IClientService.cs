namespace RealState.Core.Application.Interfaces
{
    public interface IClientService
    {
        Task<int> GetTotalClientsForAppAsync();
    }
}
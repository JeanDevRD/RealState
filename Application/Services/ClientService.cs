using RealState.Core.Application.Interfaces;
using RealState.Core.Domain.Common.Enums;

namespace RealState.Core.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IAccountServiceForApp _UserforApp;

        public ClientService(IAccountServiceForApp userforApp)
        {
            _UserforApp = userforApp;
        }

        #region Client Counting By Admin 

        public async Task<int> GetTotalClientsForAppAsync()
        {
            var clients = await _UserforApp.GetAllUsersByRole(UserRole.Client.ToString());
            return clients.Count();
        }

        #endregion

    }
}

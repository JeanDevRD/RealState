using RealState.Core.Application.Interfaces;
using RealState.Core.Domain.Common.Enums;

namespace RealState.Core.Application.Services
{
    public class DeveloperService
    {
        private readonly IAccountServiceForApp _UserforApp;

        public DeveloperService(IAccountServiceForApp userforApp)
        {
            _UserforApp = userforApp;
        }

        #region Developer Counting By Admin 

        public async Task<int> GetTotalDevelopersForAppAsync()
        {
            var developers = await _UserforApp.GetAllUsersByRole(UserRole.Developer.ToString());
            return developers.Count();
        }

        #endregion




    }
}

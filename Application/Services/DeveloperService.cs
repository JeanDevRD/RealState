using RealState.Core.Application.DTOs.Agent;
using RealState.Core.Application.DTOs.Common;
using RealState.Core.Application.Interfaces;
using RealState.Core.Domain.Common.Enums;

namespace RealState.Core.Application.Services
{
    public class DeveloperService : IDeveloperService
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

        #region List Developer by Admin

        public async Task<ResultDto<List<DeveloperDto>>> GetAllDevelopersAsync()
        {
            var result = new ResultDto<List<DeveloperDto>>
            {
                Message = new List<string>(),
                Data = new List<DeveloperDto>()
            };

            try
            {
                var developers = await _UserforApp.GetAllUsersByRole(UserRole.Developer.ToString());
                if (!developers.Any())
                {
                    result.IsError = true;
                    result.Message.Add("No se encontraron desarrolladores");
                    return result;
                }

                var developerDtos = developers.Select(d => new DeveloperDto
                {
                    Id = d.Id,
                    Name = d.FirstName,
                    LastName = d.LastName,
                    UserName = d.UserName!,
                    IdentityNumber = d.DocumentId!,
                    Email = d.Email!
                }).ToList();

                result.Data = developerDtos;
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message.Add(ex.Message);
            }

            return result;
        }

        #endregion

        #region Activate or Deactivate Developer by Admin

        public async Task<bool> ChangeStatusDeveloperAsync(string developerId)
        {
            var developer = await _UserforApp.GetUserById(developerId);

            if (developer == null)
            {
                return false;
            }

            developer.IsActive = !developer.IsActive;
            await _UserforApp.SetActivated(developer);
            return true;
        }

        #endregion
    }
}

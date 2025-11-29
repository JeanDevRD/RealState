using RealState.Core.Application.DTOs.Agent;
using RealState.Core.Application.DTOs.Common;
using RealState.Core.Application.Interfaces;
using RealState.Core.Domain.Common.Enums;
using RealState.Core.Domain.Interfaces;

namespace RealState.Core.Application.Services
{
    public class AdminService
    {
        private readonly IAccountServiceForApp _UserforApp;
        private readonly IPropertyUnitRepository _propertyUnitRepo;

        public AdminService(IAccountServiceForApp userforApp, IPropertyUnitRepository propertyUnitRepo)
        {
            _UserforApp = userforApp;
            _propertyUnitRepo = propertyUnitRepo;
        }

        #region List Admin by Admin

        public async Task<ResultDto<List<AdminDto>>> GetAllAdminAsync()
        {
            var result = new ResultDto<List<AdminDto>>()
            {
                Data = new List<AdminDto>(),
                Message = new List<string>()
            };

            try
            {
                var admins = await _UserforApp.GetAllUsersByRole(UserRole.Admin.ToString());

                if (!admins.Any())
                {
                    result.IsError = true;
                    result.Message.Add("No se encontraron administradores");
                    return result;
                }

               var adminDtos = admins.Select(admin => new AdminDto
                {
                    Id = admin.Id,
                    IdentityNumber = admin.DocumentId!,
                    Name = admin.FirstName,
                    LastName = admin.LastName,
                    UserName = admin.UserName,
                    Email = admin.Email
                }).ToList();

                result.Data = adminDtos;

            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Message.Add(ex.Message);
            }

            return result;
        }

        #endregion


    }
}

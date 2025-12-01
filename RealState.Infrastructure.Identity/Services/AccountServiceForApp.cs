using Microsoft.AspNetCore.Identity;
using RealState.Core.Application.DTOs.User;
using RealState.Core.Application.Interfaces;
using RealState.Infrastructure.Identity.Entities;

namespace RealState.Infrastructure.Identity.Services
{
    public class AccountServiceForApp : BaseAccountService, IAccountServiceForApp
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountServiceForApp(UserManager<User> userManager,SignInManager<User> signInManager,IEmailService emailService)
            : base(userManager, emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<LoginResponseDto> AuthenticateAsync(LoginDto loginDto)
        {
            LoginResponseDto response = new()
            {
                Email = "",
                Id = "",
                LastName = "",
                FirstName = "",
                UserName = "",
                HasError = false,
                Errors = []
            };

            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null)
            {
                response.HasError = true;
                response.Errors.Add($"No existe una cuenta registrada con el usuario: {loginDto.UserName}");
                return response;
            }

            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Errors.Add($"La cuenta {loginDto.UserName} no está activa. Debes verificar tu correo electrónico.");
                return response;
            }

            if (!user.IsActive)
            {
                response.HasError = true;
                response.Errors.Add($"La cuenta {loginDto.UserName} está inactiva.");
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName ?? "", loginDto.Password, false, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                response.HasError = true;
                if (result.IsLockedOut)
                {
                    response.Errors.Add($"Tu cuenta {loginDto.UserName} ha sido bloqueada debido a múltiples intentos fallidos. " +
                        $"Intenta de nuevo en 10 minutos.");
                }
                else
                {
                    response.Errors.Add($"Las credenciales son incorrectas para el usuario: {user.UserName}");
                }
                return response;
            }

            var rolesList = await _userManager.GetRolesAsync(user);

            response.Id = user.Id;
            response.Email = user.Email ?? "";
            response.UserName = user.UserName ?? "";
            response.FirstName = user.FirstName;
            response.LastName = user.LastName;
            response.IsVerified = user.EmailConfirmed;
            response.Roles = rolesList.ToList();

            return response;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}

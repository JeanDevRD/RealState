using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealState.Core.Application.DTOs.User;
using RealState.Core.Application.Interfaces;
using RealState.Core.Domain.Settings;
using RealState.Infrastructure.Identity.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RealState.Infrastructure.Identity.Services
{
    public class AccountServiceForApi : BaseAccountService, IAccountServiceForApi
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtSettings _jwtSettings;

        public AccountServiceForApi(UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService,
            IOptions<JwtSettings> jwtSettings) : base(userManager, emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<LoginResponseForApiDto> AuthenticateAsync(LoginDto loginDto)
        {
            LoginResponseForApiDto response = new()
            {
                LastName = "",
                FirstName = "",
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

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Client") || roles.Contains("Agent"))
            {
                response.HasError = true;
                response.Errors.Add($"Los usuarios de tipo {roles.First()} no tienen permiso para acceder a la API.");
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

            JwtSecurityToken jwtSecurityToken = await GenerateJwtToken(user);

            response.FirstName = user.FirstName;
            response.LastName = user.LastName;
            response.AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return response;
        }


        #region Private methods
        private async Task<JwtSecurityToken> GenerateJwtToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var rolesClaims = new List<Claim>();
            foreach (var role in roles)
            {
                rolesClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim("uid", user.Id ?? "")
            }.Union(userClaims).Union(rolesClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials
            );

            return jwtSecurityToken;
        }
        #endregion
    }
}


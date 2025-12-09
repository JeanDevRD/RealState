using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using RealState.Core.Application.DTOs.Email;
using RealState.Core.Application.DTOs.User;
using RealState.Core.Application.Interfaces;
using RealState.Core.Domain.Common.Enums;
using RealState.Infrastructure.Identity.Entities;
using RealState.Infrastructure.Identity.Seeds;
using System.Text;


namespace RealState.Infrastructure.Identity.Services
{
    public class BaseAccountService : IBaseAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        protected BaseAccountService(UserManager<User> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public virtual async Task<RegisterResponseDto> RegisterUser(SaveUserDto saveDto, string? origin, bool? isApi = false)
        {
            RegisterResponseDto response = new()
            {
                Email = "",
                Id = "",
                LastName = "",
                FirstName = "",
                UserName = "",
                HasError = false
            };

            var userWithSameUserName = await _userManager.FindByNameAsync(saveDto.UserName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.ErrorMessage = $"El nombre de usuario: {saveDto.UserName} ya está en uso.";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(saveDto.Email);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.ErrorMessage = $"El correo: {saveDto.Email} ya está en uso.";
                return response;
            }



            User user = new()
            {
                FirstName = saveDto.FirstName,
                LastName = saveDto.LastName,
                DocumentId = saveDto.DocumentId,
                Email = saveDto.Email,
                UserName = saveDto.UserName,
                EmailConfirmed = saveDto.Role == UserRole.Admin.ToString() ||
                saveDto.Role == UserRole.Developer.ToString() ? true : false,
                PhoneNumber = saveDto.Phone,
                PhotoUrl = saveDto.PhotoUrl,
                IsActive = saveDto.Role == UserRole.Admin.ToString() ||
                saveDto.Role == UserRole.Developer.ToString() ? true : false
            };

            var result = await _userManager.CreateAsync(user, saveDto.Password);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return response;
            }

            await _userManager.AddToRoleAsync(user, saveDto.Role);

            if (saveDto.Role != UserRole.Admin.ToString() || saveDto.Role != UserRole.Developer.ToString())
            {

                if (isApi != null && !isApi.Value)
                {
                    string verificationUri = await GetVerificationEmailUri(user, origin ?? "");
                    await _emailService.SendEmailAsync(new EmailRequestDto
                    {
                        To = saveDto.Email,
                        HtmlBody = $"<p>Por favor confirma tu cuenta visitando esta URL: <a href='{verificationUri}'>Confirmar cuenta</a></p>",
                        Subject = "Confirmar registro - Real Estate"
                    });
                }
                else
                {
                    string? verificationToken = await GetVerificationEmailToken(user);
                    await _emailService.SendEmailAsync(new EmailRequestDto
                    {
                        To = saveDto.Email,
                        HtmlBody = $"<p>Por favor confirma tu cuenta usando este token: <strong>{verificationToken}</strong></p>",
                        Subject = "Confirmar registro - Real Estate"
                    });
                }

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

        public virtual async Task<EditResponseDto> EditUser(SaveUserDto saveDto, string? origin, bool? isCreated = false, bool? isApi = false)
        {
            bool isNotcreated = !isCreated ?? false;
            EditResponseDto response = new()
            {
                Email = "",
                Id = "",
                LastName = "",
                FirstName = "",
                UserName = "",
                HasError = false,
                Errors = []
            };

            var userWithSameUserName = await _userManager.Users.FirstOrDefaultAsync(w => w.UserName == saveDto.UserName && w.Id != saveDto.Id);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Errors.Add($"El nombre de usuario: {saveDto.UserName} ya está en uso.");
                return response;
            }

            var userWithSameEmail = await _userManager.Users.FirstOrDefaultAsync(w => w.Email == saveDto.Email && w.Id != saveDto.Id);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Errors.Add($"El correo: {saveDto.Email} ya está en uso.");
                return response;
            }

            var user = await _userManager.FindByIdAsync(saveDto.Id!);
            if (user == null)
            {
                response.HasError = true;
                response.Errors.Add($"No existe una cuenta registrada con este usuario");
                return response;
            }

            user.FirstName = saveDto.FirstName;
            user.LastName = saveDto.LastName;
            user.Email = saveDto.Email;
            user.UserName = saveDto.UserName;
            user.PhoneNumber = saveDto.Phone;
            user.DocumentId = saveDto.DocumentId;
            user.PhotoUrl = saveDto.PhotoUrl;
            user.EmailConfirmed = user.EmailConfirmed && user.Email == saveDto.Email;

            if (!string.IsNullOrWhiteSpace(saveDto.Password) && isNotcreated)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resultChange = await _userManager.ResetPasswordAsync(user, token, saveDto.Password);

                if (resultChange != null && !resultChange.Succeeded)
                {
                    response.HasError = true;
                    response.Errors.AddRange(resultChange.Errors.Select(s => s.Description).ToList());
                    return response;
                }
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var rolesList = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, rolesList.ToList());
                await _userManager.AddToRoleAsync(user, saveDto.Role);

                if (!user.EmailConfirmed && isNotcreated)
                {

                    if (isApi != null && !isApi.Value)
                    {
                        string verificationUri = await GetVerificationEmailUri(user, origin ?? "");
                        await _emailService.SendEmailAsync(new EmailRequestDto()
                        {
                            To = saveDto.Email,
                            HtmlBody = $"<p>Por favor confirma tu cuenta visitando esta URL: <a href='{verificationUri}'>Confirmar cuenta</a></p>",
                            Subject = "Confirmar registro - Real Estate"
                        });
                    }
                    else
                    {
                        string? verificationToken = await GetVerificationEmailToken(user);
                        await _emailService.SendEmailAsync(new EmailRequestDto()
                        {
                            To = saveDto.Email,
                            HtmlBody = $"<p>Por favor confirma tu cuenta usando este token: <strong>{verificationToken}</strong></p>",
                            Subject = "Confirmar registro - Real Estate"
                        });
                    }
                }

                var updatedRolesList = await _userManager.GetRolesAsync(user);
                response.Id = user.Id;
                response.Email = user.Email ?? "";
                response.UserName = user.UserName ?? "";
                response.FirstName = user.FirstName;
                response.LastName = user.LastName;
                response.IsVerified = user.EmailConfirmed;
                response.Roles = updatedRolesList.ToList();

                return response;
            }
            else
            {
                response.HasError = true;
                response.Errors.AddRange(result.Errors.Select(s => s.Description).ToList());
                return response;
            }
        }

        public virtual async Task<UserResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto request, bool? isApi = false)
        {
            UserResponseDto response = new() { HasError = false, Errors = [] };

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                response.HasError = true;
                response.Errors.Add($"No existe una cuenta registrada con el usuario {request.UserName}");
                return response;
            }

            user.EmailConfirmed = false;
            user.IsActive = false;
            await _userManager.UpdateAsync(user);

            if (isApi != null && !isApi.Value)
            {
                var resetUri = await GetResetPasswordUri(user, request.Origin ?? "");
                await _emailService.SendEmailAsync(new EmailRequestDto()
                {
                    To = user.Email,
                    HtmlBody = $"<p>Por favor resetea tu contraseña visitando esta URL: <a href='{resetUri}'>Resetear contraseña</a></p>",
                    Subject = "Resetear contraseña - Real Estate"
                });
            }
            else
            {
                string? resetToken = await GetResetPasswordToken(user);
                await _emailService.SendEmailAsync(new EmailRequestDto()
                {
                    To = user.Email,
                    HtmlBody = $"<p>Por favor resetea tu contraseña usando este token: <strong>{resetToken}</strong></p>",
                    Subject = "Resetear contraseña - Real Estate"
                });
            }

            return response;
        }

        public virtual async Task<UserResponseDto> ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            UserResponseDto response = new() { HasError = false, Errors = [] };

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                response.HasError = true;
                response.Errors.Add($"No existe una cuenta registrada con este usuario");
                return response;
            }

            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _userManager.ResetPasswordAsync(user, token, request.Password);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Errors.AddRange(result.Errors.Select(s => s.Description).ToList());
                return response;
            }

            user.EmailConfirmed = true;
            user.IsActive = true;
            await _userManager.UpdateAsync(user);

            return response;
        }

        public virtual async Task<UserResponseDto> DeleteAsync(string id)
        {
            UserResponseDto response = new() { HasError = false, Errors = [] };
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                response.HasError = true;
                response.Errors.Add($"No existe una cuenta registrada con este usuario");
                return response;
            }

            await _userManager.DeleteAsync(user);
            return response;
        }

        public virtual async Task<UserDto?> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;

            var rolesList = await _userManager.GetRolesAsync(user);

            return new UserDto()
            {
                Id = user.Id,
                Email = user.Email ?? "",
                LastName = user.LastName,
                FirstName = user.FirstName,
                UserName = user.UserName ?? "",
                DocumentId = user.DocumentId,
                Phone = user.PhoneNumber,
                IsVerified = user.EmailConfirmed,
                IsActive = user.IsActive,
                Role = rolesList.FirstOrDefault() ?? ""
            };
        }

        public virtual async Task<UserDto?> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            var rolesList = await _userManager.GetRolesAsync(user);

            return new UserDto()
            {
                Id = user.Id,
                Email = user.Email ?? "",
                LastName = user.LastName,
                FirstName = user.FirstName,
                UserName = user.UserName ?? "",
                DocumentId = user.DocumentId,
                Phone = user.PhoneNumber,
                IsVerified = user.EmailConfirmed,
                IsActive = user.IsActive,
                PhotoUrl = user.PhotoUrl,
                Role = rolesList.FirstOrDefault() ?? ""
            };
        }

        public virtual async Task<UserDto?> GetUserByUserName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return null;

            var rolesList = await _userManager.GetRolesAsync(user);

            return new UserDto()
            {
                Id = user.Id,
                Email = user.Email ?? "",
                LastName = user.LastName,
                FirstName = user.FirstName,
                UserName = user.UserName ?? "",
                DocumentId = user.DocumentId,
                Phone = user.PhoneNumber,
                IsVerified = user.EmailConfirmed,
                IsActive = user.IsActive,
                Role = rolesList.FirstOrDefault() ?? ""
            };
        }

        public virtual async Task<List<UserDto>> GetAllUser()
        {
            List<UserDto> listUsersDtos = new();

            var users = _userManager.Users;
            var listUser = await users.ToListAsync();

            foreach (var item in listUser)
            {
                var roleList = await _userManager.GetRolesAsync(item);

                listUsersDtos.Add(new UserDto()
                {
                    Id = item.Id,
                    Email = item.Email ?? "",
                    LastName = item.LastName,
                    FirstName = item.FirstName,
                    UserName = item.UserName ?? "",
                    DocumentId = item.DocumentId,
                    Phone = item.PhoneNumber,
                    IsVerified = item.EmailConfirmed,
                    IsActive = item.IsActive,
                    Role = roleList.FirstOrDefault() ?? ""
                });
            }

            return listUsersDtos;
        }

        public virtual async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return "There is no account registered with this user";
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                user.IsActive = true;
                await _userManager.UpdateAsync(user);
                return $"Account confirmed for {user.Email}. You can now use the app";
            }
            else
            {
                return $"An error occurred while confirming this email {user.Email}";
            }
        }

        public virtual async Task<UserDto?> SetActivated(UserDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id!);
            if (user == null) return null;

            user.IsActive = dto.IsActive;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return null;

            var rolesList = await _userManager.GetRolesAsync(user);

            return new UserDto()
            {
                Id = user.Id,
                Email = user.Email ?? "",
                LastName = user.LastName,
                FirstName = user.FirstName,
                UserName = user.UserName ?? "",
                DocumentId = user.DocumentId,
                Phone = user.PhoneNumber,
                IsVerified = user.EmailConfirmed,
                IsActive = user.IsActive,
                Role = rolesList.FirstOrDefault() ?? ""
            };
        }

        public async Task<List<UserDto>>GetAllUsersByRole(string role) 
        {
            var users = await _userManager.GetUsersInRoleAsync(role);

            if (users == null) return [];

            List<UserDto> result = new List<UserDto>();

            foreach (var u in users) 
            {
                var rolesList = await _userManager.GetRolesAsync(u);

                var User = new UserDto
                {
                    Id = u.Id,
                    Email = u.Email ?? "",
                    LastName = u.LastName,
                    FirstName = u.FirstName,
                    UserName = u.UserName ?? "",
                    DocumentId = u.DocumentId,
                    Phone = u.PhoneNumber,
                    IsVerified = u.EmailConfirmed,
                    IsActive = u.IsActive,
                    PhotoUrl = u.PhotoUrl,
                    Role = rolesList.FirstOrDefault() ?? ""

                };

                result.Add(User);
            
            }
            return result; 
        }

        #region Private methods
        private async Task<string> GetVerificationEmailUri(User user, string origin)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var route = "User/ConfirmEmail";
            var completeUrl = new Uri(string.Concat(origin, "/", route));
            var verificationUri = QueryHelpers.AddQueryString(completeUrl.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri.ToString(), "token", token);
            return verificationUri;
        }

        private async Task<string?> GetVerificationEmailToken(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            return token;
        }

        private async Task<string> GetResetPasswordUri(User user, string origin)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var route = "User/ResetPassword";
            var completeUrl = new Uri(string.Concat(origin, "/", route));
            var resetUri = QueryHelpers.AddQueryString(completeUrl.ToString(), "userId", user.Id);
            resetUri = QueryHelpers.AddQueryString(resetUri.ToString(), "token", token);
            return resetUri;
        }

        private async Task<string?> GetResetPasswordToken(User user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            return token;
        }
        #endregion
    }
}


using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.User;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.User;
using RealState.Core.Domain.Common.Enums;
using RealStateApp.Helpers;
using System.Reflection.Metadata.Ecma335;

namespace RealStateApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IAccountServiceForApp _accountService;
        private readonly IMapper _mapper;

        public UserController(IAccountServiceForApp accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public IActionResult Register()
        {
            return View(new SaveUserViewModel
            {
                Id = "",
                FirstName = "",
                LastName = "",
                DocumentId = " ",
                Email = "",
                UserName = "",
                Password = "",
                ConfirmPassword = "",
                Role = UserRole.Client.ToString(),
                Phone = "",
            });
        }

        [HttpPost]
        public async Task<IActionResult> Register(SaveUserViewModel vm)
        {
            if ((vm.Role == UserRole.Client.ToString() || vm.Role == UserRole.Agent.ToString())
               && vm.DocumentId == null)
            {
                ModelState.Remove(nameof(vm.DocumentId));
            }

            if (!ModelState.IsValid)
            {

                TempData["Error"] = "Por favor corrige los errores en el formulario de registro.";
                return View(vm);
            }

            var dto = _mapper.Map<SaveUserDto>(vm);

            if (vm.Photo != null)
            {
                var tempId = Guid.NewGuid().ToString();
                var photoPath = UploadFile.Uploader(vm.Photo, tempId, "Users");
                dto.PhotoUrl = photoPath;
            }

            var origin = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var result = await _accountService.RegisterUser(dto, origin);

            if (result.HasError)
            {
                TempData["Error"] = result.ErrorMessage;
                return View(vm);
            }

            if (vm.Photo != null && !string.IsNullOrEmpty(result.Id))
            {
                var tempPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/Images/Users/{Guid.NewGuid()}");

                var finalPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/Images/Users/{result.Id}");

                if (Directory.Exists(tempPath))
                {
                    Directory.Move(tempPath, finalPath);
                }
            }

            if (vm.Role == UserRole.Client.ToString())
            {
                TempData["Success"] = "Registro exitoso. Por favor verifica tu correo electrónico para activar tu cuenta.";
            }
            else
            {
                TempData["Success"] = "Registro exitoso. Tu cuenta será activada por un administrador.";
            }

            return RedirectToAction("Index", "Login");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Link de confirmación inválido";
                return RedirectToAction("Index", "Login");
            }

            var result = await _accountService.ConfirmAccountAsync(userId, token);
            ViewBag.Message = result;

            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                TempData["Error"] = "Debe ingresar su nombre de usuario";
                return View();
            }

            var origin = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var request = new ForgotPasswordRequestDto
            {
                UserName = userName.Trim(),
                Origin = origin
            };

            var result = await _accountService.ForgotPasswordAsync(request);

            if (result.HasError)
            {
                TempData["Error"] = result.Errors;
                return View();
            }

            TempData["Success"] = "Se ha enviado un enlace de recuperación a tu correo electrónico";
            return RedirectToAction("Index", "Login");
        }

        public IActionResult ResetPassword(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Link de recuperación inválido";
                return RedirectToAction("Index", "Login");
            }

            return View(new ResetPasswordRequestViewModel
            {
                UserId = userId,
                Token = token,
                Password = "",
                ConfirmPassword = ""
            });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var dto = _mapper.Map<ResetPasswordRequestDto>(vm);
            var result = await _accountService.ResetPasswordAsync(dto);

            if (result.HasError)
            {
                TempData["Error"] = result.Errors;
                return View(vm);
            }

            TempData["Success"] = "Contraseña restablecida exitosamente. Ya puedes iniciar sesión.";
            return RedirectToAction("Index", "Login");
        }
    }
}

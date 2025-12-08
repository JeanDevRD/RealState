using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.User;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.User;
using RealState.Core.Domain.Common.Enums;
using RealState.Infrastructure.Identity.Entities;

namespace RealStateApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IMapper _map;
        private readonly IAccountServiceForApp _forApp;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public LoginController(ILogger<LoginController> logger, IAccountServiceForApp forApp, IMapper map, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _logger = logger;
            _map = map;
            _forApp = forApp;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var userSession = await _userManager.GetUserAsync(User);

            if (userSession != null)
            {
                var dtoUser = await _forApp.GetUserByUserName(userSession.UserName ?? string.Empty);

                if (dtoUser != null && dtoUser.Role == UserRole.Admin.ToString())
                    return RedirectToRoute(new { controller = "HomeAdmin", action = "Index" });
                if (dtoUser != null && dtoUser.Role == UserRole.Agent.ToString())
                    return RedirectToRoute(new { controller = "HomeAgent", action = "Index" });
                if (dtoUser != null && dtoUser.Role == UserRole.Client.ToString())
                    return RedirectToRoute(new { controller = "HomeClient", action = "Index" });
            }
            return View(new LoginViewModel { Password = "", UserName = "" });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            var userSession = await _userManager.GetUserAsync(User);
            if (userSession != null)
            {
                var dtoUser = await _forApp.GetUserByUserName(userSession.UserName ?? string.Empty);
                if (dtoUser != null && dtoUser.Role == UserRole.Admin.ToString())
                    return RedirectToRoute(new { controller = "HomeAdmin", action = "Index" });
                if (dtoUser != null && dtoUser.Role == UserRole.Agent.ToString())
                    return RedirectToRoute(new { controller = "HomeAgent", action = "Index" });
                if (dtoUser != null && dtoUser.Role == UserRole.Client.ToString())
                    return RedirectToRoute(new { controller = "HomeClient", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var dto = _map.Map<LoginDto>(loginViewModel);
            var result = await _forApp.AuthenticateAsync(dto);

            if (result == null || result.HasError)
            {
                foreach (var error in result?.Errors ?? Enumerable.Empty<string>())
                {
                    TempData["Error"] = error;
                }
                return View(loginViewModel);
            }
            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
            if (user == null)
            {
                ModelState.AddModelError("userValidation", "Usuario no encontrado.");
                return View(loginViewModel);
            }


            var signIn = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, true);

            if (!signIn.Succeeded)
            {
                TempData["Error"] = "Credenciales inválidas o cuenta bloqueada.";
                return View(loginViewModel);
            }

            if (result.Roles != null && result.Roles.Contains(UserRole.Admin.ToString()))
                return RedirectToRoute(new { controller = "HomeAdmin", action = "Index" });
            if (result.Roles != null && result.Roles.Contains(UserRole.Agent.ToString()))
                return RedirectToRoute(new { controller = "HomeAgent", action = "Index" });
            if (result.Roles != null && result.Roles.Contains(UserRole.Client.ToString()))
                return RedirectToRoute(new { controller = "HomeClient", action = "Index" });

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> AccessDenied()
        {
            User? userSession = await _userManager.GetUserAsync(User);

            if (userSession != null)
            {
                return View();
            }

            return RedirectToAction("Index");
        }

        public IActionResult SessionExpired()
        {
            TempData["Error"] = "Tu sesión ha expirado. Inicia sesión nuevamente.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                ViewBag.Message = "Link de confirmación inválido";
                return View();
            }

            string response = await _forApp.ConfirmAccountAsync(userId, token);
            return View(response);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
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

            var result = await _forApp.ForgotPasswordAsync(request);

            if (result.HasError)
            {
                TempData["Error"] = result.Errors;
                return View();
            }

            TempData["Success"] = "Se ha enviado un enlace de recuperación a tu correo";
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Link de recuperación inválido";
                return RedirectToAction("Index");
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
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            ResetPasswordRequestDto dto = _map.Map<ResetPasswordRequestDto>(vm);
            UserResponseDto returnUser = await _forApp.ResetPasswordAsync(dto);

            if (returnUser.HasError)
            {
                TempData["Error"] = returnUser.Errors;
                return View(vm);
            }

            TempData["Success"] = "Contraseña restablecida exitosamente";
            return RedirectToAction("Index");
        }

      
    }
}

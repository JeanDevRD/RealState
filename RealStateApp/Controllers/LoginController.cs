using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.User;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.User;
using RealState.Core.Domain.Common.Enums;
using RealState.Infrastructure.Identity.Entities;
using RealStateApp.Models;
using System.Diagnostics;

namespace RealStateApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IMapper _map;
        private readonly IAccountServiceForApp _forApp;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public LoginController(ILogger<LoginController> logger, IAccountServiceForApp forApp,IMapper map, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _logger = logger;
            _map = map;
            _forApp = forApp;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
            {
                if (User.IsInRole(UserRole.Admin.ToString()))
                    return RedirectToAction("Index", "HomeAdmin");
                if (User.IsInRole(UserRole.Agent.ToString()))
                    return RedirectToAction("Index", "HomeAgent");
                if (User.IsInRole(UserRole.Client.ToString()))
                    return RedirectToAction("Index", "HomeClient");
                
            }
            return View(new LoginViewModel { Password = "", UserName = ""});
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", loginViewModel);
            }

            var dto = _map.Map<LoginDto>(loginViewModel);
            var result = await _forApp.AuthenticateAsync(dto);

            if (result.HasError)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return View("Index", loginViewModel);
            }

            if (result.Roles.Contains(UserRole.Admin.ToString()))
            {
                return RedirectToAction("Index", "AdminHome");
            }

            if (result.Roles.Contains(UserRole.Agent.ToString()))
            {
                return RedirectToAction("Index", "AgentHome");
            }

            return RedirectToAction("Index", "ClientHome");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> AccessDenied()
        {
            User? userSession = await _userManager.GetUserAsync(User);

            if (userSession != null)
            {
                var user = await _forApp.GetUserByUserName(userSession.UserName!);
                return View();
            }

            return RedirectToRoute(new { controller = "Login", action = "Index" });
        }

        public IActionResult SessionExpired()
        {
            TempData["Error"] = "Tu sesión ha expirado. Inicia sesión nuevamente.";
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            string response = await _forApp.ConfirmAccountAsync(userId, token);
            return View("ConfirmEmail", response);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            ResetPasswordRequestDto dto = _map.Map<ResetPasswordRequestDto>(vm);

            UserResponseDto? returnUser = await _forApp.ResetPasswordAsync(dto);

            if (returnUser.HasError)
            {
                ViewBag.HasError = true;
                ViewBag.Errors = returnUser.Errors;
                return View(vm);
            }

            return RedirectToRoute(new { controller = "Login", action = "Index" });
        }

    }
}

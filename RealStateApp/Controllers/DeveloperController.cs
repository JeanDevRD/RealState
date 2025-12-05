using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.User;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.Developer;
using RealState.Core.Application.ViewModels.User;
using RealState.Core.Domain.Common.Enums;
using RealState.Infrastructure.Identity.Entities;

namespace RealStateApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DeveloperController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDeveloperService _developerService;
        private readonly IAccountServiceForApp _forApp;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public DeveloperController(ILogger<HomeController> logger, IDeveloperService developerService, IMapper mapper, IAccountServiceForApp forApp, UserManager<User> userManager)
        {
            _logger = logger;
            _mapper = mapper;
            _developerService = developerService;
            _forApp = forApp;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var admins = await _developerService.GetAllAdminAsync();
            var result = _mapper.Map<List<DeveloperViewModel>>(admins.Data);
            return View("Index", result);
        }

        public IActionResult Create()
        {
            return View("Create", new SaveUserViewModel 
            {
                Id = "",
                FirstName = "",
                LastName = "",
                DocumentId = "",
                Email = "",
                UserName = "",
                Password = "",
                ConfirmPassword = "",
                Role = UserRole.Developer.ToString(),
                Phone = "",
                PhotoUrl = ""
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveUserViewModel save) 
        { 
            if(!ModelState.IsValid)
            {
                return View("Save", save);
            }
            
            var origin = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var dto = _mapper.Map<SaveUserDto>(save);
            var result = await _forApp.RegisterUser(dto, origin);
            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var userDto = await _forApp.GetUserById(id);
            var viewModel = _mapper.Map<SaveUserViewModel>(userDto);
            return View("Save", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveUserViewModel save)
        {
            if (!ModelState.IsValid)
            {
                return View("Save", save);
            }

            var dto = _mapper.Map<SaveUserDto>(save);
            var origin = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var result = await _forApp.EditUser(dto, origin);
            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> Delete(string id)
        {
            var userDto = await _forApp.GetUserById(id);
            var currentUser = await _userManager.GetUserAsync(User);

            var viewModel = _mapper.Map<UserViewModel>(userDto);
            return View("Delete", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAdmin(string id)
        {
            await _forApp.DeleteAsync(id);
            return RedirectToAction("Index", "PropertyType");
        }

        public IActionResult ChangeStatusUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatusUser(string id)
        {
            var res = await _developerService.ChangeStatusAdminAsync(id);
            return RedirectToAction("Index");
        }

    }
}

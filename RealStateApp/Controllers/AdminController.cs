using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.User;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.Services;
using RealState.Core.Application.ViewModels.Admin;
using RealState.Core.Application.ViewModels.User;
using RealState.Core.Domain.Common.Enums;
using RealState.Infrastructure.Identity.Entities;
using RealStateApp.Helpers;

namespace RealStateApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAdminService _adminService;
        private readonly IAccountServiceForApp _forApp;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public AdminController(ILogger<HomeController> logger, IAdminService adminService, IMapper mapper, IAccountServiceForApp forApp, UserManager<User> userManager)
        {
            _logger = logger;
            _mapper = mapper;
            _adminService = adminService;
            _forApp = forApp;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var admins = await _adminService.GetAllAdminAsync();
            var result = _mapper.Map<List<AdminViewModel>>(admins.Data);
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
                Role = UserRole.Admin.ToString(),
                Phone = "",
                Photo = null,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveUserViewModel save) 
        {
            if (!ModelState.IsValid)
            {
                return View("Save", save);
            }

            var dto = _mapper.Map<SaveUserDto>(save);

            if (save.Photo != null)
            {
                var tempId = Guid.NewGuid().ToString();
                var photoPath = UploadFile.Uploader(save.Photo, tempId, "Users");
                dto.PhotoUrl = photoPath;
            }

            var origin = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var result = await _forApp.RegisterUser(dto, origin);

            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var userDto = await _forApp.GetUserById(id);
            var currentUser = await _userManager.GetUserAsync(User);

            if (userDto!.Id == currentUser!.Id) 
            {
                return View("Index");
            }

            var viewModel = _mapper.Map<EditUserViewModel>(userDto);
            return View("Edit", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel save)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", save);
            }

            var dto = _mapper.Map<SaveUserDto>(save);

            if (save.Photo != null)
            {
                var photoPath = UploadFile.Uploader(save.Photo, save.Id!, "Users", true, save.ExistingPhotoUrl);
                dto.PhotoUrl = photoPath;
            }
            else
            {
                dto.PhotoUrl = save.ExistingPhotoUrl;
            }

            var origin = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var result = await _forApp.EditUser(dto, origin);
            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> Delete(string id)
        {
            var userDto = await _forApp.GetUserById(id);
            var currentUser = await _userManager.GetUserAsync(User);

            if (userDto!.Id == currentUser!.Id)
            {
                return View("Index");
            }

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
            var res = await _adminService.ChangeStatusAdminAsync(id);
            return RedirectToAction("Index");
        }

    }
}

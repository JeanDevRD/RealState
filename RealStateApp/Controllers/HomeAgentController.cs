using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Application.DTOs.User;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.PropertyUnit;
using RealState.Core.Application.ViewModels.User;
using RealState.Infrastructure.Identity.Entities;
using RealStateApp.Helpers;

namespace RealStateApp.Controllers
{
    
    public class HomeAgentController : Controller
    {
        private readonly IPropertyUnitService _propertyService;
        private readonly IAccountServiceForApp _accountService;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeAgentController> _logger;
        private readonly UserManager<User> _userManager;

        public HomeAgentController(IPropertyUnitService propertyService, IAccountServiceForApp accountService,
            IMapper mapper, ILogger<HomeAgentController> logger, UserManager<User> user)
        {
            _propertyService = propertyService;
            _accountService = accountService;
            _mapper = mapper;
            _logger = logger;
            _userManager = user;
        }

        public async Task<IActionResult> Index()
        {

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Login");
            }

            var result = await _propertyService.GetAllPropertyUnitsByAgent(userId, onlyAvailable: false); 

            if (result.IsError)
            {
                ViewBag.Message = ("Error al obtener Propiedades ", result.Message);
            }

            var property = _mapper.Map<List<PropertyUnitViewModel>>(result.Data);

            return View(property);
        }

    

        #region Editar Perfil
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Login");
            }

            var user = await _accountService.GetUserById(userId);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var vm = new EditProfileViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DocumentId = user.DocumentId,
                Email = user.Email,
                UserName = user.UserName,
                Phone = user.Phone,
                Role = user.Role,
                ExistingPhotoUrl = user.PhotoUrl ?? null,
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(EditProfileViewModel vm, IFormFile? photoFile)
        {
         
            ModelState.Remove(nameof(SaveUserDto.Password));
            ModelState.Remove(nameof(SaveUserDto.ConfirmPassword));

            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Por favor corrige los errores en el formulario";
                return View(vm);
            }

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId) || vm.Id != userId)
            {
                return RedirectToAction("Index", "Login");
            }

            var dto = new SaveUserDto
            {
                Id = vm.Id,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                DocumentId = vm.DocumentId,
                Email = vm.Email,
                UserName = vm.UserName,
                Phone = vm.Phone,
                Role = vm.Role,
                Password = "", 
                ConfirmPassword = "",
                PhotoUrl = vm.ExistingPhotoUrl
            };

            if (photoFile != null)
            {
                var photoPath = UploadFile.Uploader(photoFile, userId, "Users", true, dto.PhotoUrl);
                dto.PhotoUrl = photoPath;
            }

            var origin = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var result = await _accountService.EditUser(dto, origin, isCreated: false);

            if (result.HasError)
            {
                ViewBag.Error = result.Errors;
                return View(vm);
            }

            ViewBag.Success = "Perfil actualizado exitosamente";
            TempData["Success"] = "Perfil actualizado exitosamente";

            return RedirectToAction("Profile");
        }
        #endregion

        #region Change Password

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(user, vm.CurrentPassword);
            if (!passwordCheck)
            {
                ModelState.AddModelError(nameof(vm.CurrentPassword), "La contraseña actual es incorrecta");
                return View(vm);
            }

            var changeResult = await _userManager.ChangePasswordAsync(user, vm.CurrentPassword, vm.NewPassword);

            if (!changeResult.Succeeded)
            {
                foreach (var error in changeResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(vm);
            }

            TempData["Success"] = "Contraseña cambiada exitosamente";
            return RedirectToAction("Profile");
        }
#endregion
    }
}

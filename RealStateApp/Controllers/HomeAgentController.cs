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

            var result = await _propertyService.GetAllPropertyUnitsByAgent(userId, onlyAvailable: true); 

            if (result.IsError)
            {
                ViewBag.Message = ("Error al obtener Propiedades ", result.Message);
            }

            var property = _mapper.Map<List<PropertyUnitViewModel>>(result.Data);

            return View(property);
        }

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

            var vm = _mapper.Map<EditUserViewModel>(user);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(EditUserViewModel vm, IFormFile? photoFile)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId) || vm.Id != userId)
            {
                return RedirectToAction("Index", "Login");
            }

            var dto = _mapper.Map<SaveUserDto>(vm);

            if (photoFile != null)
            {
                var photoPath = UploadFile.Uploader(photoFile, userId, "Users", true, dto.PhotoUrl);
                dto.PhotoUrl = photoPath;
            }

            var origin = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var result = await _accountService.EditUser(dto, origin);

            if (result.HasError)
            {
                ViewBag.Error = ("Error al editar perfil", result.Errors);
                return View(vm);
            }

            ViewBag.Success = "Perfil actualizado exitosamente";
            return RedirectToAction("Profile");
        }
    }
}

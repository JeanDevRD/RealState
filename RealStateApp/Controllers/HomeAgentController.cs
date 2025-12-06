using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Application.DTOs.User;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.User;
using RealStateApp.Helpers;

namespace RealStateApp.Controllers
{
    [Authorize(Roles = "Agent")]
    public class HomeAgentController : Controller
    {
        private readonly IPropertyUnitService _propertyService;
        private readonly IAccountServiceForApp _accountService;
        private readonly IMapper _mapper;

        public HomeAgentController(IPropertyUnitService propertyService, IAccountServiceForApp accountService,
            IMapper mapper)
        {
            _propertyService = propertyService;
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Login");
            }

            var result = await _propertyService.GetAllPropertyUnitsByAgent(userId, onlyAvailable: false);

            if (result.IsError)
            {
                ViewBag.Message = ("Error al obtener Propiedades ", result.Message);
            }

            return View(result.Data ?? new List<PropertyUnitDto>());
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
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

            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (string.IsNullOrEmpty(userId) || vm.Id != userId)
            {
                return RedirectToAction("Index", "Login");
            }

            var dto = _mapper.Map<SaveUserDto>(vm);

            if (photoFile != null)
            {
                var photoPath = UploadFile.Uploader(photoFile, userId, "Users", true, vm.PhotoUrl);
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

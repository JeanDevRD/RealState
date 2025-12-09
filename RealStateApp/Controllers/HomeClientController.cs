using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.Services;
using RealState.Core.Application.ViewModels.PropertyUnit;
using RealState.Infrastructure.Identity.Entities;

namespace RealStateApp.Controllers
{
    [Authorize(Roles = "Client")]
    public class HomeClientController : Controller
    {
        private readonly IPropertyUnitService _propertyService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IPropertyTypeService _propertyTypeService;
        public HomeClientController(IPropertyUnitService propertyUnit, IMapper mapper, UserManager<User> user, 
            IPropertyTypeService property) 
        { 
           _propertyService = propertyUnit;
           _mapper = mapper;
           _userManager = user;
           _propertyTypeService = property;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Login");
            }

            var result = await _propertyService.GetAllAvailablePropertiesAsync();
            ViewBag.PropertyTypes = await _propertyTypeService.GetAllAsync();

            if (result.IsError)
            {
                ViewBag.Message = ("Error al obtener Propiedades ", result.Message);
            }

            var property = _mapper.Map<List<PropertyCardViewModel>>(result.Data);

           

            return View(property);
        }

        [HttpPost]
        public async Task<IActionResult> Search(PropertyFilterViewModel VM)
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Login");
            }

            var filterDto = _mapper.Map<PropertyFilterDto>(VM);

            var result = await _propertyService.FilterPropertiesAsync(filterDto);

            ViewBag.PropertyTypes = await _propertyTypeService.GetAllAsync();

            if (result.IsError)
            {
                ViewBag.Message = ("Error al buscar Propiedades ", result.Message);
            }
            var property = _mapper.Map<List<PropertyCardViewModel>>(result.Data);

            return View("Index", property);
        }

        [HttpPost]
        public async Task<IActionResult> SearchByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                TempData["Error"] = "Debe ingresar un código de propiedad";
                return RedirectToAction("Index");
            }

            var result = await _propertyService.GetPropertyByCodeAsync(code.Trim());

            if (result.IsError)
            {
                TempData["Error"] = result.Message.FirstOrDefault();
                return RedirectToAction("Index");
            }

            var properties = new List<PropertyCardViewModel>
            {
                _mapper.Map<PropertyCardViewModel>(result.Data),

            };

            ViewBag.PropertyTypes = await _propertyTypeService.GetAllAsync();

            return View("Index", properties);
        }
    }
}

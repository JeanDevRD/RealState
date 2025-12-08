using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.PropertyUnit;
using RealStateApp.Models;
using System.Diagnostics;

namespace RealStateApp.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IPropertyUnitService _propertyService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IMapper _mapper;

        public HomeController(IPropertyUnitService propertyService, IPropertyTypeService propertyTypeService, IMapper mapper)
        {
            _propertyService = propertyService;
            _propertyTypeService = propertyTypeService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _propertyService.GetAllAvailablePropertiesAsync();

            if (result.IsError)
            {
                TempData["Error"] = "Error al obtener propiedades" + result.Message;
                return View(new List<PropertyCardViewModel>());
            }

            var properties = _mapper.Map<List<PropertyCardViewModel>>(result.Data);
            ViewBag.PropertyTypes = await _propertyTypeService.GetAllAsync();

            return View(properties);
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
                TempData["Error"] = result.Message;
                return RedirectToAction("Index");
            }

            var properties = new List<PropertyUnitViewModel> 
            { 
                _mapper.Map<PropertyUnitViewModel>(result.Data) 
            
            };
            ViewBag.PropertyTypes = await _propertyTypeService.GetAllAsync();

            return View("Index", properties);
        }

        [HttpPost]
        public async Task<IActionResult> Filter(PropertyFilterViewModel filter)
        {
            var filterDto = _mapper.Map<PropertyFilterDto>(filter);
            var result = await _propertyService.FilterPropertiesAsync(filterDto);

            if (result.IsError)
            {
                TempData["Error"] = string.Join(", ", result.Message);
                return RedirectToAction("Index");
            }

            var properties = _mapper.Map<List<PropertyUnitViewModel>>(result.Data);
            ViewBag.PropertyTypes = await _propertyTypeService.GetAllAsync();

            return View("Index", properties);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _propertyService.GetPropertyDetailForHomeAsync(id);

            if (result.IsError || result.Data == null)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction("Index");
            }

           var property = _mapper.Map<PropertyDetailHomeViewModel>(result.Data);

            return View(property);
        }
    }
}

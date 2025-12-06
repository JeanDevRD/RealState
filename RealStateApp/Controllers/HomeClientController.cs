using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.PropertyUnit;

namespace RealStateApp.Controllers
{
    [Authorize(Roles = "Client")]
    public class HomeClientController : Controller
    {
        private readonly IPropertyUnitService _propertyService;
        private readonly IMapper _mapper;
        public HomeClientController(IPropertyUnitService propertyUnit, IMapper mapper) 
        { 
           _propertyService = propertyUnit;
           _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Login");
            }

            var result = await _propertyService.GetAllAvailablePropertiesAsync();

            if (result.IsError)
            {
                ViewBag.Message = ("Error al obtener Propiedades ", result.Message);
            }

            var property = _mapper.Map<List<PropertyUnitViewModel>>(result.Data);

           

            return View("Home", property);
        }

        [HttpPost]
        public async Task<IActionResult> Search(PropertyFilterViewModel VM)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Login");
            }

            var filterDto = _mapper.Map<PropertyFilterDto>(VM);

            var result = await _propertyService.FilterPropertiesAsync(filterDto);

            if (result.IsError)
            {
                ViewBag.Message = ("Error al buscar Propiedades ", result.Message);
            }
            var property = _mapper.Map<List<PropertyUnitViewModel>>(result.Data);

            return View("Index", property);
        }
    }
}

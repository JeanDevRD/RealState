using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Application.Interfaces;
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
        public HomeClientController(IPropertyUnitService propertyUnit, IMapper mapper, UserManager<User> user) 
        { 
           _propertyService = propertyUnit;
           _mapper = mapper;
           _userManager = user;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

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
            var userId = _userManager.GetUserId(User);

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

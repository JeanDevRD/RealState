using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.PropertyUnit;

namespace RealStateApp.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientPropertyDetailController : Controller
    {
        private readonly IPropertyUnitService _propertyService;
        private readonly IMapper _mapper;

        public ClientPropertyDetailController(IPropertyUnitService propertyService, IMapper mapper)
        {
            _propertyService = propertyService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Details(int id)
        {
            var result = await _propertyService.GetPropertyDetailForHomeAsync(id);

            if (result.IsError || result.Data == null)
            {
                TempData["Error"] = "Error al obtener detalles: " + result.Message;
                return RedirectToAction("Index", "HomeClient");
            }

            ViewBag.ClientRole = "Client";

            var propertyDto = await _propertyService.GetByIdAsync(id);
            ViewBag.AgentId = propertyDto?.IdAgent; 

            var details = _mapper.Map<PropertyDetailHomeViewModel>(result.Data);
            return View(details);
        }
    }
}

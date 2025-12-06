using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.Message;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.PropertyUnit;

namespace RealStateApp.Controllers
{
    [Authorize(Roles = "Agent")]
    public class AgentPropertyDetailController : Controller
    {
        private readonly IPropertyUnitService _propertyService;
        private readonly IMapper _mapper;

        public AgentPropertyDetailController(IPropertyUnitService propertyService, IMapper mapper)
        {
            _propertyService = propertyService;
            _mapper = mapper;

        }

        public async Task<IActionResult> Detail(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Login");
            }

            var result = await _propertyService.GetPropertyDetailByAgent(id);

            if (result.IsError || result.Data == null)
            {
                TempData["Error"] = "Error al obtener detalles: " + result.Message;
                return RedirectToAction("Index", "Agent");
            }

            var property = await _propertyService.GetByIdAsync(id);

            if (property?.IdAgent != userId)
            {
                return RedirectToAction("Index", "Agent");
            }

            var details = _mapper.Map<PropertyDetailViewModel>(result.Data);

            return View(details);
        }
    }
}



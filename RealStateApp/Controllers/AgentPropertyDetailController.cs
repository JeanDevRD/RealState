using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg;
using RealState.Core.Application.DTOs.Message;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.PropertyUnit;
using RealState.Infrastructure.Identity.Entities;

namespace RealStateApp.Controllers
{
    [Authorize(Roles = "Agent")]
    public class AgentPropertyDetailController : Controller
    {
        private readonly IPropertyUnitService _propertyService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public AgentPropertyDetailController(IPropertyUnitService propertyService, IMapper mapper, UserManager<User> userManager)
        {
            _propertyService = propertyService;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IActionResult> Detail(int id)
        {

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Login");
            }

            var result = await _propertyService.GetPropertyDetailByAgent(id);

            if (result.IsError || result.Data == null)
            {
                TempData["Error"] = "Error al obtener detalles: " + result.Message;
                return RedirectToAction("Index", "HomeAgent");
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



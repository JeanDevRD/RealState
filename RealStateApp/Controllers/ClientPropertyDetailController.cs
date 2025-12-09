using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.PropertyOffer;
using RealState.Core.Application.ViewModels.PropertyUnit;
using RealState.Infrastructure.Identity.Entities;

namespace RealStateApp.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientPropertyDetailController : Controller
    {
        private readonly IPropertyUnitService _propertyService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IPropertyOfferService _offerService;

        public ClientPropertyDetailController(IPropertyUnitService propertyService, IMapper mapper,UserManager<User> user,
            IPropertyOfferService propertyOffer)
        {
            _propertyService = propertyService;
            _mapper = mapper;
            _userManager = user;
            _offerService = propertyOffer;
        }
        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);

            var result = await _propertyService.GetPropertyDetailForHomeAsync(id);

            if (result.IsError || result.Data == null)
            {
                TempData["Error"] = "Error al obtener detalles: " + result.Message;
                return RedirectToAction("Index", "HomeClient");
            }

            var offerDto = await _offerService.GetByClientAndProperty(userId!, id);

            var offers = _mapper.Map<List<PropertyOfferViewModel>>(offerDto);

            ViewBag.ClientOffers = offers;

            ViewBag.ClientRole = "Client";

            var details = _mapper.Map<PropertyDetailHomeViewModel>(result.Data);
            return View(details);
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.PropertyOffer;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.PropertyOffer;
using RealState.Core.Application.ViewModels.PropertyUnit;
using RealState.Core.Domain.Common.Enums;
using RealState.Infrastructure.Identity.Entities;

namespace RealStateApp.Controllers
{
    [Authorize(Roles = "Client")]
    public class PropertyOfferController : Controller
    {

        private readonly IPropertyOfferService _propertyOffer;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public PropertyOfferController(IPropertyOfferService propertyOffer, IMapper mapper, UserManager<User> userManager)
        {
            _propertyOffer = propertyOffer;
            _mapper = mapper;
            _userManager = userManager;
        }

        public IActionResult Create()
        {

            return View(new SavePropertyViewModel
            {
                Id = 0,
                PropertyTypeId = 0,
                SaleTypeId = 0,
                Price = 0,
                Description = "",
                SizeM = 0,
                Bedrooms = 0,
                Bathrooms = 0,
                ImprovementTypeIds = new List<int>()
            });
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Login");
            }
            var result = await _propertyOffer.GetAllWhithInclude();

            var propertiesByClient = result.Where(po => po.IdClient == userId).ToList();

            if (propertiesByClient.Count == 0)
            {
                ViewBag.Message = ("No hay Propiedades para mostrar");
            }
            var propertyOffers = _mapper.Map<List<PropertyOfferViewModel>>(propertiesByClient);

            return View("Index", propertyOffers);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SavePropertyOfferViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Login");
            }

            var dto = _mapper.Map<PropertyOfferDto>(vm);
            dto.IdClient = userId;                 
            dto.IdProperty = vm.IdProperty;        
            dto.OfferDate = DateTime.Now;         
            dto.OfferAmount = vm.OfferAmount;      
            dto.OfferStatus = (int)OfferStatus.Pending;


            var result = await _propertyOffer.AddAsync(dto);

            if (result == null)
            {
                TempData["Error"] = "Error al crear propiedad";

                return View(vm);
            }

            TempData["Success"] = "Propiedad enviada exitosamente";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);

            var property = await _propertyOffer.GetByIdAsync(id);

            if (property == null || property.IdClient != userId)
            {
                TempData["Error"] = "No tienes permiso para editar esta propiedad.";
                return RedirectToAction("Index");
            }

            var vm = _mapper.Map<SavePropertyOfferViewModel>(property);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SavePropertyOfferViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Por favor corrige los errores en el formulario.";
                return View(vm);
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;

            var property = await _propertyOffer.GetByIdAsync(vm.Id);

            if (property == null || property.IdClient != userId)
            {

                return RedirectToAction("Index");
            }

            property.OfferDate = DateTime.Now;
            property.OfferAmount = vm.OfferAmount;
       

            await _propertyOffer.UpdateAsync(vm.Id, property);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);

            var property = await _propertyOffer.GetByIdAsync(id);

            if (property == null || property.IdClient != userId)
            {
                return RedirectToAction("Index");
            }

            return View(property);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);

            var property = await _propertyOffer.GetByIdAsync(id);

            if (property == null || property.IdClient != userId)
            {
                return RedirectToAction("Index");
            }

            await _propertyOffer.DeleteAsync(id);

            return RedirectToAction("Index");
        }

    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.PropertyOffer;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.PropertyOffer;
using RealState.Core.Application.ViewModels.PropertyUnit;
using RealState.Core.Domain.Common.Enums;
using RealStateApp.Helpers;

namespace RealStateApp.Controllers
{
    public class PropertyOfferController : Controller
    {

        private readonly IPropertyOfferService _propertyOffer;
        private readonly IMapper _mapper;

        public PropertyOfferController(IPropertyOfferService propertyOffer, IMapper mapper)
        {
            _propertyOffer = propertyOffer;
            _mapper = mapper;
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

        [HttpPost]
        public async Task<IActionResult> Create(SavePropertyOfferViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

   
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Login");
            }

            var dto = _mapper.Map<PropertyOfferDto>(vm);

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
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
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
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
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
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
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

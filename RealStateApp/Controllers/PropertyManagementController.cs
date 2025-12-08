using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.PropertyUnit;
using RealState.Infrastructure.Identity.Entities;
using RealStateApp.Helpers;

namespace RealStateApp.Controllers
{
    [Authorize(Roles = "Agent")]
    public class PropertyManagementController : Controller
    {
        private readonly IPropertyUnitService _propertyService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly ISaleTypeService _saleTypeService;
        private readonly IImprovementTypeService _improvementTypeService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userService;

        public PropertyManagementController(IPropertyUnitService propertyService, IPropertyTypeService propertyTypeService,
            ISaleTypeService saleTypeService, IImprovementTypeService improvementTypeService, IMapper mapper, UserManager<User> User)
        {
            _propertyService = propertyService;
            _propertyTypeService = propertyTypeService;
            _saleTypeService = saleTypeService;
            _improvementTypeService = improvementTypeService;
            _mapper = mapper;
            _userService = User;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userService.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Login");
            }

            var result = await _propertyService.GetAllPropertyUnitsByAgent(userId, onlyAvailable: true);

            if (result.IsError)
            {
                ViewBag.Message = ("Error al obtener propiedades ", result.Message);
            }

            var property = _mapper.Map<List<PropertyUnitViewModel>>(result.Data);

            return View(property);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadSelectLists();

            var propertyTypes = await _propertyTypeService.GetAllAsync();
            var saleTypes = await _saleTypeService.GetAllAsync();
            var improvements = await _improvementTypeService.GetAllAsync();

            if (!propertyTypes.Any() || !saleTypes.Any() || !improvements.Any())
            {
                TempData["Error"] = ViewBag.Error = "No se puede crear propiedades, deben existir tipos de propiedades, " +
                    "tipos de ventas y mejoras en el sistema.";
              
                return RedirectToAction("Index");
            }

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
        public async Task<IActionResult> Create(SavePropertyViewModel vm, List<IFormFile> images)
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectLists();
                return View(vm);
            }

            if (images == null || !images.Any())
            {
                TempData["Error"] = "Debes subir almenos una imagen";
                await LoadSelectLists();
                return View(vm);
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Login");
            }

            var code = await _propertyService.GenerateUniquePropertyCodeAsync();

            var dto = _mapper.Map<PropertyUnitDto>(vm);

            dto.Id = 0;
            dto.IdAgent = userId;
            dto.CodeProperty = await _propertyService.GenerateUniquePropertyCodeAsync();

         
            var result = await _propertyService.AddAsync(dto);

            if (result == null)
            {
                TempData["Error"] = "Error al crear propiedad";
                await LoadSelectLists();
                return View(vm);
            }

            var imagePaths = new List<string>();
            var imagesToUpload = images.Take(4).ToList();

            foreach (var image in imagesToUpload)
            {
                var path = UploadFile.Uploader(image, result.Id.ToString(), "Properties");
                if (!string.IsNullOrEmpty(path))
                {
                    imagePaths.Add(path);
                }
            }

            result.Images = imagePaths;
            await _propertyService.UpdateAsync(result.Id, result);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userService.GetUserId(User);

            var property = await _propertyService.GetByIdAsync(id);

            if (property == null || property.IdAgent != userId)
            {
                return RedirectToAction("Index");
            }

            await LoadSelectLists();

            var vm = new SavePropertyViewModel
            {
                Id = property.Id,
                PropertyTypeId = property.PropertyTypeId,
                SaleTypeId = property.SaleTypeId,
                Price = property.Price,
                Description = property.Description,
                SizeM = property.SizeM,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                ImprovementTypeIds = new List<int>(),
                ExistingImages = property.Images
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SavePropertyViewModel vm, List<IFormFile>? images)
        {
            if (!ModelState.IsValid)
            {
                await LoadSelectLists();
                return View(vm);
            }

            var userId = _userService.GetUserId(User);

            var property = await _propertyService.GetByIdAsync(vm.Id);

            if (property == null || property.IdAgent != userId)
            {
                return RedirectToAction("Index");
            }

            property.PropertyTypeId = vm.PropertyTypeId;
            property.SaleTypeId = vm.SaleTypeId;
            property.Price = vm.Price;
            property.Description = vm.Description;
            property.SizeM = vm.SizeM;
            property.Bedrooms = vm.Bedrooms;
            property.Bathrooms = vm.Bathrooms;

            if (images != null && images.Any())
            {
                UploadFile.Delete(vm.Id, "Properties");

                var imagePaths = new List<string>();

                var imagesToUpload = images.Take(4).ToList();

                foreach (var image in imagesToUpload)
                {
                    var path = UploadFile.Uploader(image, vm.Id.ToString(), "Properties");
                    if (!string.IsNullOrEmpty(path))
                    {
                        imagePaths.Add(path);
                    }
                }

                property.Images = imagePaths;
            }

            await _propertyService.UpdateAsync(vm.Id, property);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userService.GetUserId(User);
            var property = await _propertyService.GetByIdAsync(id);

            if (property == null || property.IdAgent != userId)
            {
                return RedirectToAction("Index");
            }
            
            var PropertyVM = _mapper.Map<PropertyUnitViewModel>(property);

            return View(PropertyVM);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var property = await _propertyService.GetByIdAsync(id);

            if (property == null || property.IdAgent != userId)
            {
                return RedirectToAction("Index");
            }

            UploadFile.Delete(id, "Properties");

            await _propertyService.DeleteAsync(id);

            return RedirectToAction("Index");
        }

        private async Task LoadSelectLists()
        {
            ViewBag.PropertyTypes = await _propertyTypeService.GetAllAsync();
            ViewBag.SaleTypes = await _saleTypeService.GetAllAsync();
            ViewBag.Improvements = await _improvementTypeService.GetAllAsync();
        }
    }
}

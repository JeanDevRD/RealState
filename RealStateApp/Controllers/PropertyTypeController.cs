using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.PropertyType;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.PropertyType;

namespace RealStateApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PropertyTypeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IMapper _mapper;

        public PropertyTypeController(ILogger<HomeController> logger,IPropertyTypeService propertyTypeService, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _propertyTypeService = propertyTypeService;
        }

        public async Task<IActionResult> Index()
        {
            var propertyType = await _propertyTypeService.GetAllPropertyType();
            var result = _mapper.Map<List<PropertyTypeViewModel>>(propertyType.Data);
            return View("Index", result);
        }

        public IActionResult Create()
        {
            return View("Create", new SavePropertyTypeViewModel 
            {
                Id = 0,
                Name = "",
                Description = ""
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SavePropertyTypeViewModel save) 
        { 
            if(!ModelState.IsValid)
            {
                return View("Save", save);
            }

            var dto = _mapper.Map<PropertyTypeDto>(save);
            var result = await _propertyTypeService.AddAsync(dto);
            return RedirectToAction("Index", "PropertyType");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var propertyTypeDto = await _propertyTypeService.GetByIdAsync(id);
            var viewModel = _mapper.Map<SavePropertyTypeViewModel>(propertyTypeDto);
            return View("Save", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SavePropertyTypeViewModel save)
        {
            if (!ModelState.IsValid)
            {
                return View("Save", save);
            }

            var dto = _mapper.Map<PropertyTypeDto>(save);
            var result = await _propertyTypeService.UpdateAsync(dto.Id, dto);
            return RedirectToAction("Index", "PropertyType");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var propertyTypeDto = await _propertyTypeService.GetByIdAsync(id);
            var viewModel = _mapper.Map<PropertyTypeViewModel>(propertyTypeDto);
            return View("Delete", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            await _propertyTypeService.DeleteAsync(id);
            return RedirectToAction("Index", "PropertyType");
        }

    }
}

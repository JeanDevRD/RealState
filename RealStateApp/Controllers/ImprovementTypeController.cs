using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.ImprovementType;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.ImprovementType;
using RealState.Core.Application.ViewModels.SalesType;

namespace RealStateApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ImprovementTypeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IImprovementTypeService _improvementTypeService;
        private readonly IMapper _mapper;

        public ImprovementTypeController(ILogger<HomeController> logger, IImprovementTypeService improvementTypeService, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _improvementTypeService = improvementTypeService;
        }

        public async Task<IActionResult> Index()
        {
            var improvementType = await _improvementTypeService.GetAllImprovementTypes();
            var result = _mapper.Map<List<ImprovementTypeViewModel>>(improvementType.Data);
            return View("Index", result);
        }

        public IActionResult Create()
        {
            return View("Save", new SaleTypeViewModel 
            {
                Id = 0,
                Name = "",
                Description = ""
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveImprovementTypeViewModel save) 
        { 
            if(!ModelState.IsValid)
            {
                return View("Save", save);
            }

            var dto = _mapper.Map<ImprovementTypeDto>(save);
            var result = await _improvementTypeService.AddAsync(dto);
            return RedirectToAction("Index", "ImprovementType");
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.EditMode = true;
            var propertyTypeDto = await _improvementTypeService.GetByIdAsync(id);
            var viewModel = _mapper.Map<ImprovementTypeViewModel>(propertyTypeDto);
            return View("Save", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveImprovementTypeViewModel save)
        {
            if (!ModelState.IsValid)
            {
                return View("Save", save);
            }

            var dto = _mapper.Map<ImprovementTypeDto>(save);
            var result = await _improvementTypeService.UpdateAsync(dto.Id, dto);
            return RedirectToAction("Index", "ImprovementType");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var propertyTypeDto = await _improvementTypeService.GetByIdAsync(id);
            var viewModel = _mapper.Map<ImprovementTypeDto>(propertyTypeDto);
            return View("Delete", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            await _improvementTypeService.DeleteAsync(id);
            return RedirectToAction("Index", "ImprovementType");
        }

    }
}

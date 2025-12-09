using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.SaleType;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.PropertyType;
using RealState.Core.Application.ViewModels.SalesType;

namespace RealStateApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SaleTypeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISaleTypeService _saleTypeService;
        private readonly IMapper _mapper;

        public SaleTypeController(ILogger<HomeController> logger,ISaleTypeService saleTypeService, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _saleTypeService = saleTypeService;
        }

        public async Task<IActionResult> Index()
        {
            var saleType = await _saleTypeService.GetAllSaleType();
            var result = _mapper.Map<List<SaleTypeViewModel>>(saleType.Data);
            return View("Index", result);
        }

        public IActionResult Create()
        {
            return View("Save", new SaveSaleTypeViewModel 
            {
                Id = 0,
                Name = "",
                Description = ""
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveSaleTypeViewModel save) 
        { 
            if(!ModelState.IsValid)
            {
                return View("Save", save);
            }

            var dto = _mapper.Map<SaleTypeDto>(save);
            var result = await _saleTypeService.AddAsync(dto);
            return RedirectToAction("Index", "SaleType");
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.EditMode = true;
            var propertyTypeDto = await _saleTypeService.GetByIdAsync(id);
            var viewModel = _mapper.Map<SaveSaleTypeViewModel>(propertyTypeDto);
            return View("Save", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveSaleTypeViewModel save)
        {
            if (!ModelState.IsValid)
            {
                return View("Save", save);
            }

            var dto = _mapper.Map<SaleTypeDto>(save);
            var result = await _saleTypeService.UpdateAsync(dto.Id, dto);
            return RedirectToAction("Index", "SaleType");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var sale = await _saleTypeService.GetByIdAsync(id);
            var viewModel = _mapper.Map<SaleTypeViewModel>(sale);
            return View("Delete", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSale(int id)
        {
            await _saleTypeService.DeleteAsync(id);
            return RedirectToAction("Index", "SaleType");
        }

    }
}

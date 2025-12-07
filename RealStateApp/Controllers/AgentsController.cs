using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.PropertyUnit;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.PropertyUnit;

namespace RealStateApp.Controllers
{
    public class AgentsController : Controller
    {
        private readonly IAgentService _agentService;
        private readonly IMapper _mapper;

        public AgentsController(IAgentService agentService, IMapper mapper)
        {
            _agentService = agentService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _agentService.GetAllActiveAgentsAsync();

            if (result.IsError)
            {
                TempData["Error"] = string.Join(", ", result.Message);
                return View(new List<AgentCardDto>());
            }

            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToAction("Index");
            }

            var result = await _agentService.SearchAgentsByNameAsync(searchTerm.Trim());

            if (result.IsError)
            {
                TempData["Info"] = string.Join(", ", result.Message);
            }

            return View("Index", result.Data ?? new List<AgentCardDto>());
        }

        public async Task<IActionResult> Properties(string agentId)
        {
            if (string.IsNullOrEmpty(agentId))
            {
                TempData["Error"] = "ID de agente inválido";
                return RedirectToAction("Index");
            }

            var result = await _agentService.GetAgentAvailablePropertiesAsync(agentId);

            if (result.IsError)
            {
                TempData["Error"] = string.Join(", ", result.Message);
                return RedirectToAction("Index");
            }

            var properties = _mapper.Map<List<PropertyUnitViewModel>>(result.Data);
            return View(properties);
        }
    }
}

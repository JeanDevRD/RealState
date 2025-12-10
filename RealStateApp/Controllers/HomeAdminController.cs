using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.HomeAdmin;
using RealStateApp.Models;

namespace RealStateApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeAdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPropertyUnitService _propertyUnitService;
        private readonly IAgentService _agentService;
        private readonly IClientService _clientService;
        private readonly IDeveloperService _developerService;

        
        public HomeAdminController(ILogger<HomeController> logger, IPropertyUnitService propertyUnitService, IAgentService agentService, IClientService clientService, IDeveloperService developerService)
        {
            _logger = logger;
            _propertyUnitService = propertyUnitService;
            _agentService = agentService;
            _clientService = clientService;
            _developerService = developerService;
        }

        public async Task<IActionResult> Index()
        {
            var propertyUnitCount = await _propertyUnitService.TotalPropertyUnitsAsync();
            var agentCount = await _agentService.GetTotalAgentsForAppAsync();
            var clientCount = await _clientService.GetTotalClientsForAppAsync();
            var developerCount = await _developerService.GetTotalDevelopersForAppAsync();

            var viewModel = new HomeAdminViewModel
            { 
                PropertyUnitCount = propertyUnitCount,
                AgentCount = agentCount,
                ClientCount = clientCount,
                DeveloperCount = developerCount
            };
            return View("Index", viewModel);
        }

    }
}

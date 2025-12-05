using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.Agent;
using RealState.Core.Application.ViewModels.HomeAdmin;
using RealState.Core.Domain.Common.Enums;
using RealStateApp.Models;

namespace RealStateApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AgentController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAgentService _agentService;
        private readonly IMapper _mapper;

        public AgentController(ILogger<HomeController> logger,IAgentService agentService, IMapper mapper)
        {
            _logger = logger;
            _agentService = agentService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var agents = await _agentService.GetAllAgentsAsync();
            var result = _mapper.Map<List<AgentViewModel>>(agents.Data);
            return View("AgentList", result);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatusUser(string id)
        {
            var res = await _agentService.ChangeStatusAgentAsync(id);
            return RedirectToAction("AgentList");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var res = await _agentService.DeleteAgentAsync(id);
            return RedirectToAction("AgentList");
        }

    }
}

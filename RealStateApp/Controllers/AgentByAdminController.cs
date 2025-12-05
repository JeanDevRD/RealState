using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.Agent;


namespace RealStateApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AgentByAdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAgentService _agentService;
        private readonly IMapper _mapper;

        public AgentByAdminController(ILogger<HomeController> logger,IAgentService agentService, IMapper mapper)
        {
            _logger = logger;
            _agentService = agentService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var agents = await _agentService.GetAllAgentsAsync();
            var result = _mapper.Map<List<AgentViewModel>>(agents.Data);
            return View("Index", result);
        }

        public IActionResult ChangeStatusUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatusUser(string id)
        {
            var res = await _agentService.ChangeStatusAgentAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            return View("Delete", id);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var res = await _agentService.DeleteAgentAsync(id);
            return RedirectToAction("AgentList");
        }

    }
}

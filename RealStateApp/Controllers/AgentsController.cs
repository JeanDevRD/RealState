using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.Agent;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.Agent;
using RealState.Core.Application.ViewModels.PropertyUnit;
using RealState.Infrastructure.Identity.Entities;

namespace RealStateApp.Controllers
{
    public class AgentsController : Controller
    {
        private readonly IAgentService _agentService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IAccountServiceForApp _accountServiceForApp;

        public AgentsController(IAgentService agentService, IMapper mapper, UserManager<User> user, 
            IAccountServiceForApp accountServiceFor)
        {
            _agentService = agentService;
            _mapper = mapper;
            _userManager = user;
            _accountServiceForApp = accountServiceFor;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _agentService.GetAllActiveAgentsAsync();

            if (result.IsError)
            {
                TempData["Error"] = result.Message;
                return View(new List<AgentCardDto>());
            }

            var agents = _mapper.Map<List<AgentCardViewModel>>(result.Data);

            return View(agents);
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
                TempData["Info"] = result.Message;
            }
            var agents = _mapper.Map<List<AgentCardViewModel>>(result.Data);

            return View("Index", agents ?? new List<AgentCardViewModel>());
        }

        public async Task<IActionResult> Properties(string agentId)
        {
            if (string.IsNullOrEmpty(agentId))
            {
                TempData["Error"] = "ID de agente inválido";
                return RedirectToAction("Index");
            }

            var id = _userManager.GetUserId(User);

            if(!string.IsNullOrEmpty(id))
            {
                var user = _accountServiceForApp.GetUserById(id);

                ViewBag.Role = user.Result?.Role;
            }

            var result = await _agentService.GetAgentAvailablePropertiesAsync(agentId);

            if (result.IsError)
            {
                TempData["Error"] = result.Message.FirstOrDefault();
                return RedirectToAction("Index");
            }

            var properties = _mapper.Map<List<PropertyUnitViewModel>>(result.Data);
            return View(properties);
        }
    }
}

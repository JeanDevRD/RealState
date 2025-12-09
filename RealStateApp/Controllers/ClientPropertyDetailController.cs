using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.Message;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.Services;
using RealState.Core.Application.ViewModels.Message;
using RealState.Core.Application.ViewModels.PropertyOffer;
using RealState.Core.Application.ViewModels.PropertyUnit;
using RealState.Infrastructure.Identity.Entities;

namespace RealStateApp.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientPropertyDetailController : Controller
    {
        private readonly IPropertyUnitService _propertyService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IPropertyOfferService _offerService;
        private readonly IChatService _chatService;

        public ClientPropertyDetailController(IPropertyUnitService propertyService, IMapper mapper,UserManager<User> user,
            IPropertyOfferService propertyOffer, IChatService chat)
        {
            _propertyService = propertyService;
            _mapper = mapper;
            _userManager = user;
            _offerService = propertyOffer;
            _chatService = chat;
        }
        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);

            var result = await _propertyService.GetPropertyDetailForHomeAsync(id);

            if (result.IsError || result.Data == null)
            {
                TempData["Error"] = "Error al obtener detalles: " + result.Message;
                return RedirectToAction("Index", "HomeClient");
            }

            var offerDto = await _offerService.GetByClientAndProperty(userId!, id);
            var offers = _mapper.Map<List<PropertyOfferViewModel>>(offerDto);
            ViewBag.ClientOffers = offers;
            ViewBag.ClientRole = "Client";

            var propertyDto = await _propertyService.GetByIdAsync(id);
            ViewBag.AgentId = propertyDto?.IdAgent;


            var chatDto = await _chatService.GetConversation(id, userId!);
            ViewBag.ChatExists = chatDto != null;
            ViewBag.ChatId = chatDto?.Id;

            var messagesViewModel = chatDto?.Messages?.OrderBy(m => m.SentAt)
                .Select(m => new MessageViewModel
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    Content = m.Content,
                    SentAt = m.SentAt,
                    IsFromAgent = m.SenderId == propertyDto?.IdAgent

                }).ToList() ?? new List<MessageViewModel>();

            ViewBag.Messages = messagesViewModel;
            ViewBag.CurrentUserId = userId;

            var details = _mapper.Map<PropertyDetailHomeViewModel>(result.Data);
            return View(details);
        }
    }
    
}

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.Chat;
using RealState.Core.Application.DTOs.Message;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.Chat;
using RealState.Core.Application.ViewModels.Message;
using RealState.Infrastructure.Identity.Entities;
using System.Security.Claims;

namespace RealStateApp.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;
        private readonly IAccountServiceForApp _accountService;
        private readonly IPropertyUnitService _propertyService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userSession;

        public ChatController(IChatService chatService, IMessageService messageService,
            IAccountServiceForApp accountService, IPropertyUnitService propertyService, IMapper mapper, UserManager<User> userSession)
        {
            _chatService = chatService;
            _messageService = messageService;
            _accountService = accountService;
            _propertyService = propertyService;
            _mapper = mapper;
            _userSession = userSession;
        }

        public async Task<IActionResult> Detail(int chatId)
        {
            var currentUserId = _userSession.GetUserId(User);

            if (string.IsNullOrEmpty(currentUserId))
            {
                TempData["Error"] = "Debe iniciar sesión para ver los detalles del chat.";
                return RedirectToAction("Index", "Login");
            }

            var chat = await _chatService.GetByIdAsync(chatId);

            if (chat == null || (chat.IdAgent != currentUserId && chat.IdClient != currentUserId))
            {
                TempData["Error"] = "No tiene permiso para acceder a este chat.";
                return RedirectToAction("Index", "Home");
            }

            var messages = await _messageService.GetConversation(chatId);
            var client = await _accountService.GetUserById(chat.IdClient);
            var property = await _propertyService.GetByIdAsync(chat.IdProperty);

            var detailVM = new ChatDetailViewModel
            {
                ChatId = chatId,
                ClientName = client != null ? $"{client.FirstName} {client.LastName}" : "Cliente Desconocido",
                PropertyId = chat.IdProperty,
                PropertyCode = property?.CodeProperty ?? "N/A",
                Messages = messages.Select(m => new MessageViewModel
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    Content = m.Content,
                    SentAt = m.SentAt,
                    IsFromAgent = m.SenderId == chat.IdAgent
                }).ToList(),
                CurrentUserId = currentUserId
            };


            return View(detailVM);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(int? chatId, string content, string? agentId, int? IdProperty)
        {
            var currentUserId = _userSession.GetUserId(User);

            if (string.IsNullOrEmpty(currentUserId))
            {
                TempData["Error"] = "Debe iniciar sesión.";
                return RedirectToAction("Index", "Login");
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["Error"] = "El mensaje no puede estar vacío.";
                return chatId.HasValue ? RedirectToAction("Detail", new { chatId = chatId }) : RedirectToAction("Index", "Home");
            }

            int finalChatId = chatId ?? 0;

            // Si no hay chatId, crear uno nuevo (solo para clientes)
            if (!chatId.HasValue && IdProperty.HasValue && !string.IsNullOrEmpty(agentId))
            {
                var existingChat = await _chatService.GetConversation(IdProperty.Value, currentUserId);

                if (existingChat != null)
                {
                    finalChatId = existingChat.Id;
                }
                else
                {
                    var newChatDto = await _chatService.AddAsync(new ChatDto
                    {
                        Id = 0,
                        IdAgent = agentId!,
                        IdClient = currentUserId,
                        IdProperty = IdProperty.Value
                    });
                    finalChatId = newChatDto.Id;
                }
            }

            if (finalChatId == 0)
            {
                TempData["Error"] = "Error al identificar o crear el chat. Asegúrese de que todos los parámetros sean correctos.";
                return RedirectToAction("Index", "Home");
            }

            var chat = await _chatService.GetByIdAsync(finalChatId);

            // ✅ CORRECCIÓN: Validación mejorada
            if (chat == null)
            {
                TempData["Error"] = "El chat no existe.";
                return RedirectToAction("Index", "Home");
            }

            // Verificar que el usuario sea parte del chat
            if (chat.IdAgent != currentUserId && chat.IdClient != currentUserId)
            {
                TempData["Error"] = "No tiene permisos para enviar mensajes en este chat.";

                // Redirigir según el rol del usuario
                var user = await _accountService.GetUserById(currentUserId);
                if (user?.Role == "Agent")
                    return RedirectToAction("Index", "HomeAgent");
                else if (user?.Role == "Client")
                    return RedirectToAction("Index", "HomeClient");
                else
                    return RedirectToAction("Index", "Home");
            }

            try
            {
                await _messageService.AddAsync(new MessageDto
                {
                    Id = 0,
                    IdChat = finalChatId,
                    SenderId = currentUserId,
                    Content = content,
                    SentAt = DateTime.Now
                });

                TempData["Success"] = "Mensaje enviado exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al enviar el mensaje: {ex.Message}";
            }

            return RedirectToAction("Detail", new { chatId = finalChatId });
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.Message;
using RealState.Core.Application.Interfaces;

namespace RealStateApp.Controllers
{
    public class AgentChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;
        private readonly IAccountServiceForApp _accountService;
        private readonly IPropertyUnitService _propertyService;

        public AgentChatController(IChatService chatService, IMessageService messageService,
            IAccountServiceForApp accountService,IPropertyUnitService propertyService)
        {
            _chatService = chatService;
            _messageService = messageService;
            _accountService = accountService;
            _propertyService = propertyService;
        }

        public async Task<IActionResult> Detail(int chatId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var chat = await _chatService.GetByIdAsync(chatId);

            if (chat == null || chat.IdAgent != userId)
            {
                return RedirectToAction("Index", "Agent");
            }

            var messages = await _messageService.GetConversation(chatId);
            ViewBag.Messages = messages;

            var client = await _accountService.GetUserById(chat.IdClient);
            ViewBag.ClientName = client != null ? $"{client.FirstName} {client.LastName}" : "Cliente";

            var property = await _propertyService.GetByIdAsync(chat.IdProperty);
            ViewBag.PropertyCode = property?.CodeProperty ?? "";

            return View(chat);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(int chatId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                TempData["Error"] = "El mensaje no puede estar vacío.";
                return RedirectToAction("Detail", new { chatId = chatId });
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Debe iniciar sesión.";
                return RedirectToAction("Index", "Login");
            }

            var chat = await _chatService.GetByIdAsync(chatId);
            if (chat == null || chat.IdAgent != userId)
            {
                TempData["Error"] = "No tiene permisos para enviar mensajes en este chat.";
                return RedirectToAction("Index", "Agent");
            }

            try
            {
                await _messageService.AddAsync(new MessageDto
                {
                    Id = 0,
                    IdChat = chatId,
                    SenderId = userId,
                    Content = content,
                    SentAt = DateTime.Now
                });

                TempData["Success"] = "Mensaje enviado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al enviar el mensaje: {ex.Message}";
            }

            return RedirectToAction("Detail", new { chatId = chatId });
        }
    }
}

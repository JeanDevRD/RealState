using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.Interfaces;
using RealState.Core.Domain.Common.Enums;

namespace RealStateApp.Controllers
{
    public class AgentOfferController : Controller
    {
        private readonly IPropertyOfferService _offerService;
        private readonly IAccountServiceForApp _accountService;
        private readonly IPropertyUnitService _propertyService;

        public AgentOfferController(IPropertyOfferService offerService, IAccountServiceForApp accountService, 
            IPropertyUnitService propertyService)
        {
            _offerService = offerService;
            _accountService = accountService;
            _propertyService = propertyService;
        }

        public async Task<IActionResult> ClientOffers(string clientId, int propertyId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var property = await _propertyService.GetByIdAsync(propertyId);

            if (property?.IdAgent != userId)
            {
                TempData["Error"] = "No tiene permiso para ver estas ofertas.";
                return RedirectToAction("Index", "Agent");
            }

            var offers = await _offerService.GetByClientAndProperty(clientId, propertyId);

            var client = await _accountService.GetUserById(clientId);
            ViewBag.ClientName = client != null ? $"{client.FirstName} {client.LastName}" : "Cliente";
            ViewBag.PropertyCode = property?.CodeProperty ?? "";
            ViewBag.PropertyId = propertyId;

            return View(offers);
        }


        [HttpPost]
        public async Task<IActionResult> RespondOffer(int offerId, bool accept)
        {
            try
            {
                var offer = await _offerService.GetByIdAsync(offerId);
                if (offer == null)
                {
                    TempData["Error"] = "La oferta no existe.";
                    return RedirectToAction("ClientOffers", new { clientId = "", propertyId = 0 });
                }

                var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
                var property = await _propertyService.GetByIdAsync(offer.IdProperty);

                if (property?.IdAgent != userId)
                {
                    TempData["Error"] = "No está autorizado para responder esta oferta.";
                    return RedirectToAction("Index", "Agent");
                }

                await _offerService.UpdateStatus(offerId, accept);

                if (accept)
                {
                    var allOffers = await _offerService.GetAllWhithInclude();
                    var propertyOffers = allOffers.Where(o => o.IdProperty == offer.IdProperty 
                        && o.Id != offerId&& o.OfferStatus == (int)OfferStatus.Pending).ToList();

                    foreach (var pendingOffer in propertyOffers)
                    {
                        await _offerService.UpdateStatus(pendingOffer.Id, false);
                    }

                    if (property != null)
                    {
                        property.StateProperty = (int)StateProperty.Sold;
                        await _propertyService.UpdateAsync(offer.IdProperty, property);
                    }

                    TempData["Success"] = "Has aceptado la oferta. La propiedad fue marcada como vendida.";
                }
                else
                {
                    TempData["Success"] = "Has rechazado la oferta.";
                }

                return RedirectToAction("ClientOffers", new { clientId = offer.IdClient, propertyId = offer.IdProperty });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al procesar la oferta: " + ex.Message;
                return RedirectToAction("Index", "Agent");
            }
        }
    }
}

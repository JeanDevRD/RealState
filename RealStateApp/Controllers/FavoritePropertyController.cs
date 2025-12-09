using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.DTOs.UserFavoritePropertyUnit;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.UserFavoritePropertyUnit;
using RealState.Infrastructure.Identity.Entities;

namespace RealStateApp.Controllers
{
    public class FavoritePropertyController : Controller
    {
        IFavoritePropertyServices _favoriteProperty;
        IMapper _mapper;
        UserManager<User> _userManager;

        public FavoritePropertyController(IFavoritePropertyServices favoriteProperty, IMapper mapper, UserManager<User> userManager) 
        { 
            _favoriteProperty = favoriteProperty;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Login");
            }

            var propertiesDto =  await _favoriteProperty.GetFavoritesByClient(userId);

            var properties = _mapper.Map<List<UserFavoritePropertyUnitViewModel>>(propertiesDto);

            return View(properties);
        }

        [HttpPost]
        public async Task<IActionResult> Toggle(int propertyId)
        {
            var userId = _userManager.GetUserId(User);

            var favorites = await _favoriteProperty.GetFavoritesByClient(userId!);

            var existing = favorites.FirstOrDefault(f => f.IdProperty == propertyId);

            if (existing != null)
            {
                await _favoriteProperty.DeleteAsync(existing.Id);
                TempData["Success"] = "La propiedad ha sido eliminada de tus favoritos.";
            }
            else
            {
                await _favoriteProperty.AddAsync(new UserFavoritePropertyUnitDto
                {
                    Id = 0,
                    IdClient = userId!,
                    IdProperty = propertyId
                });

                TempData["Success"] = "La propiedad ha sido agregada a tus favoritos.";
            }

            return RedirectToAction("Index", "HomeClient");
        }
    }
}

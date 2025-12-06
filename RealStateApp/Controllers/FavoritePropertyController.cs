using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealState.Core.Application.Interfaces;
using RealState.Core.Application.ViewModels.UserFavoritePropertyUnit;

namespace RealStateApp.Controllers
{
    public class FavoritePropertyController : Controller
    {
        IFavoritePropertyServices _favoriteProperty;
        IMapper _mapper;

        public FavoritePropertyController(IFavoritePropertyServices favoriteProperty, IMapper mapper) 
        { 
            _favoriteProperty = favoriteProperty;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Login");
            }

            var propertiesDto = _favoriteProperty.GetFavoritesByClient(userId);

            var properties = _mapper.Map<List<UserFavoritePropertyUnitViewModel>>(propertiesDto);

            return View(properties);
        }
    }
}

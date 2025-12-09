using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RealState.Core.Application.ViewModels.User
{
    public class EditUserViewModel
    {
        public string? Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? DocumentId { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string? ConfirmPassword { get; set; }
        public required string Role { get; set; }
        public string? Phone { get; set; }
        public IFormFile? Photo { get; set; }
        public string? ExistingPhotoUrl { get; set; }
    }
}

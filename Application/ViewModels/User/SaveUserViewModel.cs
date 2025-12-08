using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RealState.Core.Application.ViewModels.User
{
    public class SaveUserViewModel
    {
        public string? Id { get; set; }
        [Required(ErrorMessage = "El nombre es necesario")]
        [DataType(DataType.Text)]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "El apellido es necesario")]
        [DataType(DataType.Text)]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "El documento de identidad es necesario")]
        [DataType(DataType.Text)]
        public string? DocumentId { get; set; }

        [Required(ErrorMessage = "El correo es necesario")]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [Required(ErrorMessage = "El nombre de usuario")]
        [DataType(DataType.Text)]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "La contraseña es necesaria")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no son iguales")]
        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "El rol es necesario")]
        [DataType(DataType.Password)]
        public required string Role { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }
        public IFormFile? Photo { get; set; }
        public string? ExistingPhotoUrl { get; set; }
    }
}

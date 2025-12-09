using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealState.Core.Application.ViewModels.User
{
    public class EditProfileViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [DataType(DataType.Text)]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "El apellido es requerido")]
        [DataType(DataType.Text)]
        public required string LastName { get; set; }

        [DataType(DataType.Text)]
        public string? DocumentId { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [DataType(DataType.Text)]
        public required string UserName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        public IFormFile? Photo { get; set; }
        public string? PhotoUrl { get; set; }

        public string? ExistingPhotoUrl { get; set; }

        public required string Role { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace RealState.Core.Application.ViewModels.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es necesario")]
        [DataType(DataType.Text)]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "La contraseña es necesaria")]
        [DataType(DataType.Text)]
        public required string Password { get; set; }
    }
}

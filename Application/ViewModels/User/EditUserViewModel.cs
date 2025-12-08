using Microsoft.AspNetCore.Http;

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
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
        public required string Role { get; set; }
        public string? Phone { get; set; }
        public IFormFile? Photo { get; set; }
        public string? ExistingPhotoUrl { get; set; }
    }
}

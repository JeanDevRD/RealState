namespace RealState.Core.Application.DTOs.User
{
    public class SaveUserDto
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
        public string? PhotoUrl { get; set; }
    }
}

namespace RealState.Core.Application.DTOs.User
{
    public class LoginResponseForApiDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AccessToken { get; set; }
        public bool HasError { get; set; }
        public List<string> Errors { get; set; } = [];
    }
}

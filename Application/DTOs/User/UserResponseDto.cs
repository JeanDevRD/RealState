namespace RealState.Core.Application.DTOs.User
{
    public class UserResponseDto
    {
        public bool HasError { get; set; }
        public List<string> Errors { get; set; } = [];
        public string? Message { get; set; }
    }
}

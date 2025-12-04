
namespace RealState.Core.Application.DTOs.PropertyUnit
{
     public class AgentCardDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public string? PhotoUrl { get; set; }
        public string FullName => $"{Name} {LastName}";
    }
}

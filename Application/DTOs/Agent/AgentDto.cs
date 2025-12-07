namespace RealState.Core.Application.DTOs.Agent
{
    public class AgentDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public bool IsActive { get; set; }

        public required int TotalProperties { get; set; } = 0;

    }
}

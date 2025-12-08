namespace RealState.Core.Application.ViewModels.Agent
{
    public class AgentCardViewModel
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public string? PhotoUrl { get; set; }
        public string FullName => $"{Name} {LastName}";
    }
}

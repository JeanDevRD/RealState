namespace RealState.Core.Application.ViewModels.Agent
{
    public class AgentViewModel
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required int TotalProperties { get; set; } = 0;

    }
}

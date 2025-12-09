namespace RealState.Core.Application.DTOs.Agent
{
    public class DeveloperDto
    {
        public required string Id { get; set; }
        public required string IdentityNumber { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public bool IsActive { get; set; }


    }
}

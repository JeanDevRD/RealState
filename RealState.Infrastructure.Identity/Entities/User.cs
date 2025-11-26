using Microsoft.AspNet.Identity.EntityFramework;

namespace RealState.Infrastructure.Identity.Entities
{
    public class User : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Phone { get; set; }
        public string? PhotoUrl { get; set; }
        public string? DocumentId { get; set; } // Cédula
        public bool IsActive { get; set; } = true;
    }
}

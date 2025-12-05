namespace RealState.Core.Application.ViewModels.User
{
<<<<<<< HEAD
    public class UserViewModel 
=======
    public class UserViewModel
>>>>>>> 1fe7c70e1328ffb96b77a35ed48f36d1d74a378c
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? DocumentId { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public string? Phone { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }
        public required string Role { get; set; }
    }
}

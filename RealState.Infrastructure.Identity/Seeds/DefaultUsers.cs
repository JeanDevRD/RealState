using Microsoft.AspNetCore.Identity;
using RealState.Core.Domain.Common.Enums;
using RealState.Infrastructure.Identity.Entities;

namespace RealState.Infrastructure.Identity.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedAsync(UserManager<User> userManager)
        {
            await CreateUserIfNotExists(userManager, new User
            {
                FirstName = "Admin",
                LastName = "System",
                DocumentId = "00000000001",
                Email = "admin@realestate.com",
                UserName = "admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            }, "Admin123!", UserRole.Admin.ToString());

            await CreateUserIfNotExists(userManager, new User
            {
                FirstName = "Carlos",
                LastName = "Agent",
                DocumentId = "00000000002",
                Email = "agent@realestate.com",
                UserName = "agent",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            }, "Agent123!", UserRole.Agent.ToString());

            await CreateUserIfNotExists(userManager, new User
            {
                FirstName = "Maria",
                LastName = "Client",
                DocumentId = "00000000003",
                Email = "client@realestate.com",
                UserName = "client",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            }, "Client123!", UserRole.Client.ToString());

            await CreateUserIfNotExists(userManager, new User
            {
                FirstName = "Dev",
                LastName = "Developer",
                DocumentId = "00000000004",
                Email = "dev@realestate.com",
                UserName = "developer",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            }, "Developer123!", UserRole.Developer.ToString());
        }

        private static async Task CreateUserIfNotExists(UserManager<User> userManager, User user, string password, string role)
        {
            var existingUser = await userManager.FindByEmailAsync(user.Email!);
            if (existingUser == null)
            {
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}

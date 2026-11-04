
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Constants;
using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Data
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(
            IServiceProvider services,
            IConfiguration configuration)
        {
            using var scope = services.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            var passwordHasher = scope.ServiceProvider
                .GetRequiredService<IPasswordHasher<User>>();

            var accounts = new[]
            {
                new
                {
                    Name = "System Admin",
                    Email = "admin@moviebooking.com",
                    Role = UserRoles.Admin,
                    Password = configuration["SeedUsers:AdminPassword"]
                },
                new
                {
                    Name = "Test Customer",
                    Email = "customer@moviebooking.com",
                    Role = UserRoles.Customer,
                    Password = configuration["SeedUsers:CustomerPassword"]
                }
            };

            foreach (var account in accounts)
            {
                if (string.IsNullOrWhiteSpace(account.Password))
                {
                    throw new InvalidOperationException(
                        $"Password configuration missing for {account.Email}");
                }

                var existingUser = await context.Users
                    .FirstOrDefaultAsync(x => x.Email == account.Email);

                if (existingUser != null)
                {
                    Console.WriteLine(
                        $"{account.Role} account already exists: {account.Email}");

                    continue;
                }

                var user = new User
                {
                    Name = account.Name,
                    Email = account.Email,
                    Role = account.Role
                };

                user.PasswordHash = passwordHasher.HashPassword(
                    user,
                    account.Password);

                context.Users.Add(user);

                await context.SaveChangesAsync();

                Console.WriteLine("--------------------------------");
                Console.WriteLine(
                    $"{account.Role} account created successfully!");
                Console.WriteLine($"Email: {account.Email}");
                Console.WriteLine("--------------------------------");
            }
        }
    }
}

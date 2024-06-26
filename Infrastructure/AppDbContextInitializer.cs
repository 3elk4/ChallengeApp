using ChallengeApp.Domain.Constants;
using Domain.Constants;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ChallengeApp.Infrastructure
{
    public static class InitialiserExtensions
    {
        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var initialiser = scope.ServiceProvider.GetRequiredService<AppDbContextInitializer>();

            //await initialiser.InitialiseAsync();

            await initialiser.SeedAsync();
        }
    }

    public class AppDbContextInitializer
    {
        private readonly ILogger<AppDbContextInitializer> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        // private readonly RoleManager<IdentityRole> _roleManager;

        public AppDbContextInitializer(ILogger<AppDbContextInitializer> logger, AppDbContext context, UserManager<User> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                await _context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            //// Default roles
            //var administratorRole = new IdentityRole(Roles.Administrator);

            //if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
            //{
            //    await _roleManager.CreateAsync(administratorRole);
            //}

            // Default users
            var testUsers = new List<User>() {
                new User { UserName = "test1@localhost", Email = "test1@localhost" },
                new User { UserName = "test2@localhost", Email = "test2@localhost" }
            };

            foreach (var user in testUsers)
            {
                if (_userManager.Users.All(u => u.UserName != user.UserName))
                {
                    await _userManager.CreateAsync(user, "Test1!");
                }

                _context.Challenges.Add(new Challenge
                {
                    Title = $"Test {user.UserName} Challenge",
                    Description = $"Description of {user.UserName} challenge",
                    Type = ChallengeType.PRIVATE_FINAL,
                    CreatedBy = user.Id,
                    Author = user.Id,
                    Chores =
                    {
                        new Chore { Title = $"Chore 1 {user.UserName}", Description = "xyzabcd", Difficulty = Difficulty.EASY, Points = 1, Completed = true, CreatedBy = user.Id, LastModifiedBy = user.Id },
                        new Chore { Title = $"Chore 2 {user.UserName}", Description = "xyzabcd", Difficulty = Difficulty.MEDIUM, Points = 2, CreatedBy = user.Id, LastModifiedBy = user.Id },
                        new Chore { Title = $"Chore 3 {user.UserName}", Description = "xyzabcd", Difficulty = Difficulty.HARD, Points = 5, CreatedBy = user.Id, LastModifiedBy = user.Id },
                    },
                });

                await _context.SaveChangesAsync();
            }
        }
    }
}

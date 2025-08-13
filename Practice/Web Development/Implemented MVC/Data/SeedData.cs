using Microsoft.AspNetCore.Identity;
using Implemented_MVC.Models;

namespace Implemented_MVC.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Create roles
            string[] roleNames = { "Admin", "User" };
            
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create admin user
            var adminEmail = "admin@todolist.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "System",
                    LastName = "Administrator",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    Console.WriteLine($"Admin user created successfully with email: {adminEmail}");
                    Console.WriteLine("Default password: Admin123!");
                }
            }

            // Create sample user
            var userEmail = "user@todolist.com";
            var sampleUser = await userManager.FindByEmailAsync(userEmail);
            
            if (sampleUser == null)
            {
                sampleUser = new ApplicationUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    FirstName = "Sample",
                    LastName = "User",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(sampleUser, "User123!");
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(sampleUser, "User");
                    Console.WriteLine($"Sample user created successfully with email: {userEmail}");
                    Console.WriteLine("Default password: User123!");
                }
            }
        }
    }
}

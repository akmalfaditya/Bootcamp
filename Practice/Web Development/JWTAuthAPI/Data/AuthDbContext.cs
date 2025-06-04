using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JWTAuthAPI.Models;

namespace JWTAuthAPI.Data
{
    /// <summary>
    /// Database context for our JWT Authentication API using Microsoft Identity
    /// This inherits from IdentityDbContext which gives us all the Identity tables automatically
    /// Think of this as getting a pre-built, enterprise-grade user management system
    /// </summary>
    public class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        /// <summary>
        /// OnModelCreating is where we configure our database relationships and constraints
        /// With Identity, most of the user/role configuration is handled automatically
        /// We just need to add our custom configurations and seed data
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure our custom ApplicationUser properties
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.FirstName).HasMaxLength(100);
                entity.Property(u => u.LastName).HasMaxLength(100);
            });

            // Seed default roles - every application needs some basic roles
            // Identity uses string IDs for roles, so we'll use GUIDs
            var adminRoleId = "2301D884-221A-4E7D-B509-0113DCC043E1";
            var userRoleId = "2301D884-221A-4E7D-B509-0113DCC043E2";
            var managerRoleId = "2301D884-221A-4E7D-B509-0113DCC043E3";

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = userRoleId
                },
                new IdentityRole
                {
                    Id = managerRoleId,
                    Name = "Manager",
                    NormalizedName = "MANAGER",
                    ConcurrencyStamp = managerRoleId
                }
            );

            // Create a default admin user
            var adminUserId = "2301D884-221A-4E7D-B509-0113DCC043A1";
            var hasher = new PasswordHasher<ApplicationUser>();

            var adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin@jwtauth.com",
                NormalizedUserName = "ADMIN@JWTAUTH.COM",
                Email = "admin@jwtauth.com",
                NormalizedEmail = "ADMIN@JWTAUTH.COM",
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Administrator",
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = adminUserId
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123!");

            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            // Assign admin role to the default admin user
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                }
                            );
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhoIsFaster.Domain.Entities;
using WhoIsFaster.Domain.Entities.RoomAggregate;

namespace WhoIsFaster.Infrastructure.Data.Persistance
{
    public class WhoIsFasterDbContext : IdentityDbContext<IdentityUser>
    {
        public WhoIsFasterDbContext(DbContextOptions<WhoIsFasterDbContext> options) : base(options) { }

        public DbSet<RegularUser> RegularUsers { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Room>().HasMany(r => r.RoomPlayers);
            modelBuilder.Entity<Room>().Ignore(r => r.WordList);
        }

        public static async Task CreateRoles(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    IdentityRole adminRole = new IdentityRole("Admin");
                    await roleManager.CreateAsync(adminRole);
                }

                if (!await roleManager.RoleExistsAsync("User"))
                {
                    IdentityRole userRole = new IdentityRole("User");
                    await roleManager.CreateAsync(userRole);
                }
            }
        }

        public static async Task CreateAdminAccount(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<IdentityUser>>();

                IdentityUser defaultAdmin;

                if ((defaultAdmin = await userManager.FindByNameAsync(configuration["Data:AdminUser:Name"])) == null)
                {
                    defaultAdmin = new IdentityUser
                    {
                        UserName = configuration["Data:AdminUser:Email"],
                        Email = configuration["Data:AdminUser:Email"]
                    };

                    // At this point we assume that the Identity DB is seeded with the "Admins" IdentityRole
                    await userManager.CreateAsync(defaultAdmin, configuration["Data:AdminUser:Password"]);
                    await userManager.AddToRoleAsync(defaultAdmin, "Admin");
                }
            }
        }
    }


}
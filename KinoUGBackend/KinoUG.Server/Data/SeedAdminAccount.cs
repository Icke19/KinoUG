using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KinoUG.Server.Models
{
    public class SeedAccountAdmin
    {
        public static async Task SeedAdminUser(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

           
            if (!await roleManager.RoleExistsAsync(Roles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }

            
            var adminUsers = await userManager.GetUsersInRoleAsync(Roles.Admin);
            if (adminUsers.Any())
            {
                return; 
            }

           
            var adminUser = new User
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
                Name = "Admin",
                Surname = "Admin"
            };
            
            var result = await userManager.CreateAsync(adminUser, "Admin123");
            
            var token = await userManager.GenerateEmailConfirmationTokenAsync(adminUser);
            var emailConfirmed = await userManager.ConfirmEmailAsync(adminUser,token);
            //admin@admin.com
            //Admin123

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, Roles.Admin);
            }
            else
            {
                throw new Exception("Failed to create the admin user");
            }

            
        }
    }
}

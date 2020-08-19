using BudgetIn.WebApi.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetIn.WebApi
{
    public static class IdentityDataInitializer
    {
        public static void SeedData (UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers(UserManager<User> userManager)
        {
            if (userManager.FindByNameAsync("testuser").Result == null)
            {
                User user = new User()
                {
                    UserName = "testuser",
                    Email = "testuser@localhost",
                    FirstName = "Test",
                    LastName = "Test",
                    Birthday = new DateTimeOffset(new DateTime(1996, 10, 6))
                };
                

                IdentityResult result = userManager.CreateAsync(user, "Pwd12345.").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "User").Wait();
                }
            }


            if (userManager.FindByNameAsync("budgetadmin").Result == null)
            {
                User user = new User()
                {
                    UserName = "budgetadmin",
                    Email = "budgetadmin@localhost",
                    FirstName = "Admin",
                    LastName = "Admin",
                    Birthday = new DateTimeOffset(new DateTime(1994, 6, 30))
                };

                IdentityResult result = userManager.CreateAsync(user, "Strong.Passw0rd#@112233").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = "User"
                };

                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = "Administrator"
                };

                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}

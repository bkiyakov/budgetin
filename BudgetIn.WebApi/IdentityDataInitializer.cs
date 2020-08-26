using BudgetIn.WebApi.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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
            string testUsername = Startup.staticConfiguration["TestUsername"];
            string testPassword = Startup.staticConfiguration["TestPassword"];
            string adminUsername = Startup.staticConfiguration["AdminUsername"];
            string adminPassword = Startup.staticConfiguration["AdminPassword"];

            if (userManager.FindByNameAsync(testUsername).Result == null)
            {
                User user = new User()
                {
                    UserName = testUsername,
                    Email = testUsername + "@budgetin.com",
                    FirstName = "Test",
                    LastName = "Test",
                    Birthday = new DateTimeOffset(new DateTime(1996, 10, 6))
                };
                

                IdentityResult result = userManager.CreateAsync(user, testPassword).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "User").Wait();
                }
            }


            if (userManager.FindByNameAsync(adminUsername).Result == null)
            {
                User user = new User()
                {
                    UserName = adminUsername,
                    Email = adminUsername + "@budgetin.com",
                    FirstName = "Admin",
                    LastName = "Admin",
                    Birthday = new DateTimeOffset(new DateTime(1994, 6, 30))
                };

                IdentityResult result = userManager.CreateAsync(user, adminPassword).Result;

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

using FamilyTreeNet.Core.Support;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyTreeNet.Auth
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new AuthDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AuthDbContext>>()))
            {
                // For sample purposes seed both with the same password.
                // Password is set with the following:
                // dotnet user-secrets set SeedUserPW <pw>
                // The admin user can do anything

                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@example.com").ConfigureAwait(false);
                await EnsureRole(serviceProvider, adminID, Constants.AdminRole).ConfigureAwait(false);

                //// allowed user can create and edit contacts that they create
                //var managerID = await EnsureUser(serviceProvider, testUserPw, "manager@contoso.com");
                //await EnsureRole(serviceProvider, managerID, Constants.ContactManagersRole);

                //SeedDB(context, adminID);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                                    string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(UserName).ConfigureAwait(false);
            if (user == null)
            {
                user = new IdentityUser { UserName = UserName };
                await userManager.CreateAsync(user, testUserPw).ConfigureAwait(false);
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string uid, string role)
        {
            IdentityResult IR;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new InvalidOperationException("roleManager cannot be null");
            }

            //if (!await roleManager.RoleExistsAsync(role))
            //{
            //    IR = await roleManager.CreateAsync(new IdentityRole(role));
            //}

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByIdAsync(uid).ConfigureAwait(false);

            if (user == null)
            {
                throw new InvalidOperationException("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role).ConfigureAwait(false);

            return IR;
        }

    }
}

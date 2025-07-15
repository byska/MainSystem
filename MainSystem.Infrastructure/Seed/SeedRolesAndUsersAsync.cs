using MainSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Infrastructure.Seed
{
    public static class SeedRolesAndUsersAsync
    {
        public static async Task SeedAsync(IServiceProvider sp)
        {
            var roleMgr = sp.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (var r in new[] { "Admin", "Viewer" })
                if (!await roleMgr.RoleExistsAsync(r))
                    await roleMgr.CreateAsync(new IdentityRole(r));

            var userMgr = sp.GetRequiredService<UserManager<ApplicationUser>>();

            async Task AddUser(string email, string role)
            {
                if (await userMgr.FindByEmailAsync(email) is null)
                {
                    var u = new ApplicationUser { UserName = email, Email = email };
                    await userMgr.CreateAsync(u, "Passw0rd!");
                    await userMgr.AddToRoleAsync(u, role);
                }
            }

            await AddUser("admin@demo.io", "Admin");
            await AddUser("viewer@demo.io", "Viewer");
        }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> _userManager)
        {
            if (_userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    DisplayName = "Ahmed Kebeir",
                    Email = "ahmed.kebeir@gmail.com",
                    UserName = "ahmed.kebeir",
                    PhoneNumber = "01111111111"
                };

                await _userManager.CreateAsync(user,"Ahmed.24112002");
            } 


        }
    }
}

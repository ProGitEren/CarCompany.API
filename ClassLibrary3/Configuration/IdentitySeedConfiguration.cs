using ClassLibrary2.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Configuration
{
    public class IdentitySeedConfiguration
    {

        public static async Task SeedUserAsync(UserManager<AppUsers> userManager) 
        {
            if (!userManager.Users.Any()) 
            {
                var InitialAdmin = new AppUsers
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = "admin@gmail.com",
                    UserName = "admin@gmail.com",
                    Phone = "90555555555",
                    birthtime = new DateTime(2000, 1, 1),
                    Address = new Address 
                    {
                        name = "Admin",
                        city = "Admin",
                        state = "Admin",
                        country = "TR",
                        zipcode = 1
                        
                    } 

                };

                var result = await userManager.CreateAsync(InitialAdmin,"Admin123*");

                if (result.Succeeded) 
                {
                    await userManager.AddToRoleAsync(InitialAdmin, "Admin");
                }


            }

                
        
        
        }
    }
}

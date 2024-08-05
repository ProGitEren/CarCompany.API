using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary2.Entities;

namespace Infrastucture.Data.Config
{
     public class IdentityConfiguration
    {

        public static async Task SeedUserAsync(UserManager<AppUsers> userManager)
        {
            //if (!userManager.Users.Any())
            //{
            //    var user = new AppUsers

            //    {
            //        FirstName = "Eren",
            //        LastName = "Gökmenler",
            //        Address = new Address

            //        {
            //            city = "Istanbul",
            //            name = "X Sokak, Y apartman, No:21",
            //            country = "Turkey",
            //            state = "Turkey",
            //            zipcode = 34406,
            //            AppUserId = Guid.NewGuid()



            //        },
            //        Email = "eren.gokmenler@gmail.com",
            //        UserName = "eren.gokmenler@gmail.com",



            //    };

            //    await userManager.CreateAsync(user, "Eren123*");
            //}
        }
    }


}
    


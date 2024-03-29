﻿using BataCMS.Data.Models;
using COHApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data
{
    public static class ExtensionMethods
    {
        public static async Task<ApplicationUser> FindByPhoneNumber(this UserManager<ApplicationUser> um, string number)
        {
            return await um?.Users?.SingleOrDefaultAsync(x => x.PhoneNumber == number);
        }

        public static async Task<MemberUser> FindByPhoneNumber(this UserManager<MemberUser> um, string number)
        {
            return await um?.Users?.SingleOrDefaultAsync(x => x.PhoneNumber == number);
        }

        public static async Task<String> GetFirstName(this UserManager<ApplicationUser> um, ApplicationUser applicationUser)
        {
            ApplicationUser user =  await um?.Users?.SingleOrDefaultAsync(x => x.FirstName == applicationUser.FirstName);

            return user.FirstName; 
        }

    }
}

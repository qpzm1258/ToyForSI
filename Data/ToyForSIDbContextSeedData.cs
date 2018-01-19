using System;
using ToyForSI.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ToyForSI.Data
{
    public class ToyForSIDbContextSeedData
    {
        private readonly ToyForSIContext _context;

        public ToyForSIDbContextSeedData(ToyForSIContext context)
        {
            _context = context;
        }

        public async void SeedAdminUser()
        {
            var user = new ApplicationUser
            {
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            if (!_context.ApplicationUser.AnyAsync(u => u.UserName == user.UserName).Result)
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "nnsiitd");
                user.PasswordHash = hashed;
                var userStore = new UserStore<ApplicationUser>(_context);
                await userStore.CreateAsync(user);
            }

            await _context.SaveChangesAsync();
        }

    }
}
using System;
using System.Linq;
using System.Security.Cryptography;
using ChaVoV1.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;

namespace ChaVoV1.Models
{
    public class Seed
    {
        static public void SeedData(DataContext context)
        {
            if (!context.Users.Any())
            {
                var role = context.Roles.FirstOrDefault(p => p.RoleText == "Admin");
                if (role == null)
                {
                    context.Roles.Add(new Role()
                    {
                        RoleText = "Admin"
                    });
                    context.SaveChanges();
                    role = context.Roles.FirstOrDefault(p => p.RoleText == "Admin");
                }
                string password = HashPassword("1234");
                context.Users.Add(new User()
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "Admin",
                    LastName = "Admin",
                    Login = "Admin",
                    Password = password,
                    Role = role
                });
                context.SaveChanges();
            }
        }
        static public string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}
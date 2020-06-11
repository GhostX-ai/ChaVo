using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
                string password = HashingPassword("1234");
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
        static public string HashingPassword(string password)
        {
            var data = Encoding.ASCII.GetBytes(password);
            var md5 = new MD5CryptoServiceProvider();
            var md5data = md5.ComputeHash(data);
            return Convert.ToBase64String(md5data);
        }
    }
}
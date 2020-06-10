using Microsoft.AspNetCore.Mvc;
using ChaVoV1.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ChaVo.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ChaVoV1.Models.Areas.Account.Controllers
{
    [Area("Account")]
    public class AccountController : Controller
    {
        DataContext _context;
        public AccountController(DataContext context)
        {
            this._context = context;
        }
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                model.Password = HashPassword(model.Password);
                User user = await _context.Users.Include(p => p.Role).FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);
                if (user != null)
                {
                    await Authentucate(user.Login, user.Role.RoleText);

                    return RedirectToAction("Index", "Home", new { @area = "" });
                }
                ModelState.AddModelError("", "Login and(or) password is not correct!");
            }
            return View(model);
        }
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(RegistModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
                if (user == null)
                {
                    model.Password = HashPassword(model.Password);
                    Guid id = Guid.NewGuid();
                    var role = await _context.Roles.FirstAsync(p => p.RoleText == "Client");
                    _context.Users.Add(new User()
                    {
                        Id = id.ToString(),
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Login = model.Login,
                        MiddleName = model.MiddleName,
                        Password = model.Password,
                        Role = role
                    });
                    await _context.SaveChangesAsync();
                    await Authentucate(model.Login, role.RoleText);

                    return RedirectToAction("Index", "Home", new { @area = "" });
                }
                ModelState.AddModelError("", "This login being using!");
            }
            return View(model);
        }
        private async Task Authentucate(string login, string role)
        {
            var claims = new List<Claim>{
                new Claim(ClaimsIdentity.DefaultNameClaimType,login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType,role)};
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        public string HashPassword(string password)
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
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public JsonResult GetInfo()
        {
            var model = _context.Users.Include(u => u.Role).SingleOrDefault(u => u.Login == User.Identity.Name);
            UserInfo ui = model == null ? new UserInfo() : new UserInfo()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Role = model.Role.RoleText
            };
            return new JsonResult(ui);
        }
        public IActionResult Manage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Manage(ChangePasswordModel model)
        {

            return RedirectToAction("Index", "Home");
        }
    }
}
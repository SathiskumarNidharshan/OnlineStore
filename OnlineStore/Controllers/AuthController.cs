using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OnlineStore.Entities;
using OnlineStore.Models;
using OnlineStore.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OnlineStore.Database;

namespace OnlineStore.Controllers
{
    public class AuthController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly AuthService authService;

        public AuthController()
        {
            authService = new AuthService();
        }

        // GET: Users
        public ActionResult Index()
        {
            return View(authService.getUserByRole("Customer"));
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await authService.Authenticate(model);
                if (result != null && result.IsSuccessful)
                {
                    if (result.IsSuccessful)
                    {
                        var loggedInUser = JsonConvert.DeserializeObject<UserModel>(JsonConvert.SerializeObject(result.Data));
                        string token = GenerateJwtToken(loggedInUser);

                        ViewBag.JwtToken = token;

                        Session["username"] = loggedInUser.Id;
                        Session["fullname"] = string.Format("{0}{1}", loggedInUser.FirstName, loggedInUser.LastName);
                        Session["role"] = loggedInUser.Role;
                        
                        return RedirectToAction("Admin", "Dashboard");
                        
                    }
                    else
                    {
                        ViewBag.ErrorMessage = result?.Message ?? "Invalid login attempt.";
                    }
                }
            }
            return View();
        }

        private string GenerateJwtToken(UserModel loggedInUser)
        {
            // Secret key for signing the token
            var secretKey = "KRVmyuHRyR7jtHNINgWGuVAWhF/DED9I7F0eB6uNA5Y="; // Replace with a secure key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            // Token descriptor with claims, expiration, and signing credentials
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, loggedInUser.Username),
                new Claim(ClaimTypes.NameIdentifier, loggedInUser.Id.ToString()),
                new Claim(ClaimTypes.Role, loggedInUser.Role),
            // Add additional claims if necessary
        }),
                Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            // Create the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            // Return the token as a string
            return tokenHandler.WriteToken(securityToken);
        }
        public ActionResult Logout()
        {
            Session["username"] = null;
            Session["fullName"] = null;
            return RedirectToAction("Login", "Auth");
        }

        public ActionResult Register()
        {
            var model = new RegisterModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await authService.Register(model);

                if (result.IsSuccessful)
                {
                    ViewBag.Message = result.Message;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = result.Message;
                    return View(model);
                }
            }
            return View(model);
        }
    }
}
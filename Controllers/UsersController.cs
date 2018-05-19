using System;
using System.Linq;
using System.Diagnostics;
using BeltExam.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// Other usings

namespace BeltExam.Controllers
{
    public class UsersController : Controller
    {
        private BeltContext _context;

        public UsersController(BeltContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            // List<Users> AllUsers = _context.Users.ToList();
            return View();
            // Other code
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(Users NewUser)
        {
            if (ModelState.IsValid)
            {
                Users RegisteredUser = _context.Users.SingleOrDefault(i => i.Email == NewUser.Email);
                if(RegisteredUser != null)
                {
                    // ViewBag.Message = "This email exists. Please use a different email.";
                    ModelState.AddModelError("Email","This email exists. Please use a different email.");

                    return View("Index");
                }
                PasswordHasher<Users> Hasher = new PasswordHasher<Users>();
                NewUser.Password = Hasher.HashPassword(NewUser, NewUser.Password);
                _context.Users.Add(NewUser);
                _context.SaveChanges();
                NewUser = _context.Users.SingleOrDefault(i => i.Email == NewUser.Email);
                HttpContext.Session.SetInt32("CurrentUser", NewUser.UserId);
                int? AccountId = HttpContext.Session.GetInt32("CurrentUser");
                ViewBag.CurrentUser = NewUser;
                // return RedirectToAction("Account", "Transactions", new{AccountId});
                return RedirectToAction("Dashboard" , "Ideas");
                //Handle success
            }
            // return RedirectToAction("Index");
            return View("Index");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
            // Other code
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(Login OldUser)
        {
            if (ModelState.IsValid)
            {
                Users RegisteredUser = _context.Users.SingleOrDefault(i => i.Email == OldUser.Email);
                if(RegisteredUser != null && OldUser.Password != null)
                {
                    var Hasher = new PasswordHasher<Users>();
                    // Pass the user object, the hashed password, and the PasswordToCheck
                    if(0 != Hasher.VerifyHashedPassword(RegisteredUser, RegisteredUser.Password,OldUser.Password))
                    {
                         HttpContext.Session.SetInt32("CurrentUser", RegisteredUser.UserId);
                         int? AccountId = HttpContext.Session.GetInt32("CurrentUser");
                         return RedirectToAction("Dashboard" , "Ideas");
                        //Handle success
                    }
                    else
                    {
                    ModelState.AddModelError("Password","Sorry! Wrong password.Did you forget your password?");    
                    }
                }
                else
                {
                    ModelState.AddModelError("Email","This email does not exist.Please register.");                        
                }
            }
            return View();
        }
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
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
    public class IdeasController : Controller
    {
        private BeltContext _context;

        public IdeasController(BeltContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ideas")]
        public IActionResult Dashboard()
        {
            int? CurrentUserId = HttpContext.Session.GetInt32("CurrentUser");
            if (CurrentUserId != null)
            {
                Users DashboardUser = _context.Users.SingleOrDefault(u => u.UserId == (int)CurrentUserId);
                List<Ideas> AllIdeas = _context.Ideas.Include(i => i.Users)
                                                   .Include(i => i.Likes)
                                                   .OrderByDescending(i => i.Likes.Count)
                                                   .ToList();
                ViewBag.CurrentUser = DashboardUser;
                ViewBag.Ideas = AllIdeas;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Users");
            }

        }
        [HttpPost]
        [Route("create")]
        public IActionResult CreateIdea(Ideas NewIdea)
        {
            int? CurrentUserId = HttpContext.Session.GetInt32("CurrentUser");
            if (CurrentUserId != null)
            {
                if (ModelState.IsValid)
                {
                    NewIdea.UserId = (int)CurrentUserId;
                    _context.Ideas.Add(NewIdea);
                    _context.SaveChanges();
                    return RedirectToAction("Dashboard");
                }
                Users DashboardUser = _context.Users.SingleOrDefault(u => u.UserId == (int)CurrentUserId);
                List<Ideas> AllIdeas = _context.Ideas.Include(i => i.Likes)
                                                   .OrderByDescending(i => i.Likes.Count)
                                                   .ToList();
                ViewBag.CurrentUser = DashboardUser;
                ViewBag.Ideas = AllIdeas;
                return View("Dashboard");
            }
            else
            {
                return RedirectToAction("Login", "Users");
            }
        }
        [HttpGet]
        [Route("like/{IdeaId}")]
        public IActionResult LikeIdea(Likes NewLike)
        {
            int? CurrentUserId = HttpContext.Session.GetInt32("CurrentUser");
            if (CurrentUserId != null)
            {
                NewLike.UserId = (int)CurrentUserId;
                _context.Likes.Add(NewLike);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else
            {
                return RedirectToAction("Login", "Users");
            }
        }
        [HttpGet]
        [Route("user/{NewUserId}")]
        public IActionResult UserPage(int NewUserId)
        {
            int? CurrentUserId = HttpContext.Session.GetInt32("CurrentUser");
            if (CurrentUserId != null)
            {
                Users NewUser = _context.Users.Where(u => u.UserId == NewUserId)
                                              .Include(i => i.Idea)
                                              .Include(l => l.Likes)
                                              .SingleOrDefault();
                ViewBag.UserToShow = NewUser;
                return View("ShowUser");
            }
            else
            {
                return RedirectToAction("Login", "Users");
            }
        }
        [HttpGet]
        [Route("idea/{NewIdeaId}")]
        public IActionResult IdeaPage(int NewIdeaId)
        {
            int? CurrentUserId = HttpContext.Session.GetInt32("CurrentUser");
            if (CurrentUserId != null)
            {
                Ideas NewIdea = _context.Ideas.Where(i => i.IdeaId == NewIdeaId)
                                              .Include(u => u.Users)
                                              .Include(l => l.Likes)
                                                .ThenInclude(u => u.Users)
                                              .SingleOrDefault();
                // List<Likes> NewLike = _context.Likes.Where(i => i.IdeaId == NewIdeaId).ToList();
                List<int> AllUserIds = _context.Likes.Include(l => l.Users)
                                                     .Where(l => l.IdeaId == NewIdeaId)
                                                     .Select(l => l.UserId)
                                                     .Distinct()
                                                     .ToList();
                List<Users> AllUsers = new List<Users>();
                foreach (int Id in AllUserIds)
                {
                    AllUsers.Add(_context.Users.Where(u => u.UserId == Id).FirstOrDefault());
                }
                ViewBag.Users = AllUsers;
                ViewBag.IdeaToShow = NewIdea;
                // ViewBag.Likes = NewLike;               
                return View("ShowIdea");
            }
            else
            {
                return RedirectToAction("Login", "Users");
            }
        }
        [HttpGet]
        [Route("delete/{NewIdeaId}")]
        public IActionResult Delete(int NewIdeaId)
        {
            Ideas IdeaToDelete = _context.Ideas.SingleOrDefault(i => i.IdeaId == NewIdeaId);
            _context.Remove(IdeaToDelete);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}
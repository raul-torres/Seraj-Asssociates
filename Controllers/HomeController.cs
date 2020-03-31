using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SerajAssociates.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;

namespace SerajAssociates.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;     
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("portfolio")]
        public IActionResult Portfolio()
        {
            return View();
        }

        [HttpGet("about")]
        public IActionResult About()
        {
            return View();
        }
        
        [HttpGet("contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpGet("adminsignin")]
        public IActionResult AdminSignIn()
        {
            return View();
        }
        [HttpGet("details")]
        public IActionResult Details()
        {
            return View();
        }
        [HttpGet("testing")]
        public IActionResult TESTING()
        {
            return View();
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet("dashboard/messages")]
        public IActionResult DashboardMessages()
        {
            List<Message> AllMessage= dbContext.Messages
            
            .OrderBy(m => m.CreatedAt)
            .ToList();
            return View(AllMessage);
        }

        [HttpGet("markasread/{IdMessage}")]
        public IActionResult MarkAsRead(int IdMessage)
        {
            Message MessageRead = dbContext.Messages.FirstOrDefault(m => m.MessageId == IdMessage);
            MessageRead.IsRead = true;
            dbContext.SaveChanges();
            return RedirectToAction("DashboardMessages");
        }

        [HttpGet("dashboard/readmessages")]
        public IActionResult DashboardReadMessages()
        {
            List<Message> AllMessage= dbContext.Messages
            
            .OrderBy(m => m.CreatedAt)
            .ToList();
            return View(AllMessage);
        }

        [HttpGet("dashboard/messages/{IdMessage}")]
        public IActionResult DashboardMessagesInfo(int IdMessage)
        {
            Message ThisMessage = dbContext.Messages
                .FirstOrDefault(M => M.MessageId == IdMessage);

            return View(ThisMessage);
        }

        [HttpGet("contact/success")]
        public IActionResult Success()
        {
            return View();
        }

    // HANDLING ADMIN-PROJECT ACTIONS~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        [HttpGet("dashboard/portfolio")]
        public IActionResult DashBoardPortfolio()
        {
            List<Project> AllProjects = dbContext.Project
            .OrderByDescending(x => x.CreatedAt)
            .ToList();
            return View(AllProjects);

        }

        [HttpGet("dashboard/editproj/{IdProject}")]
        public IActionResult DashboardEditProj(int IdProject)
        {
            ViewBag.ThisProject = dbContext.Project
                .FirstOrDefault(P => P.ProjectId == IdProject);
            return View();
        }

        [HttpGet("dashboard/newproj")]
        public IActionResult DashboardNewProj()
        {
            return View();
        }

    // HANDLING ADDING PHOTOS FORM ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        [HttpGet("dashboard/photos/{IdProject}")]
        public IActionResult DashboardPhoto(int IdProject)
        {
            ViewBag.ThisProject = dbContext.Project
                .Include(A => A.Album)
                .FirstOrDefault(P => P.ProjectId == IdProject);
            return View();
        }
    // HANDLING MESSAGE DELETE ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        [HttpGet("deletemessage/{IdMessage}")]
        public IActionResult deletemessage(int IdMessage)
        {
            Message MessageToDelete = dbContext.Messages
                .FirstOrDefault(M => M.MessageId == IdMessage);
            dbContext.Remove(MessageToDelete);
            dbContext.SaveChanges();
            return RedirectToAction("DashboardReadMessages");
        }
    // HANDLING PROJECT DELETION ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        [HttpGet("deleteproject/{IdProject}")]
        public IActionResult deleteproject(int IdProject)
        {
            Project ThisProject = dbContext.Project
                .FirstOrDefault(P => P.ProjectId == IdProject);
            dbContext.Remove(ThisProject);
            dbContext.SaveChanges();
            return RedirectToAction("DashboardPortfolio");
        }
    
    // HANDLING ONE TIME REGISTRATION FORM FOR ADMIN ~~~~~~~~~~~~~~~~~~~~~~~
        [HttpGet("mrof3172020adm1n2020noitartsiger314159")] 
        public IActionResult AdminForm()
        {
            if(dbContext.Admin.ToList().Count < 1)
            {
                return View();
            }
            return RedirectToAction("AdminSignIn");
        }


// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~POST~~~~~REQUESTS~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        [HttpPost("uploadimage/{IdProject}")]
        public async Task<IActionResult> UploadImage(IFormCollection form, int IdProject, Image NewImage)
        {
            string storePath = "wwwroot/Uploads/";
            if (form.Files == null || form.Files[0].Length == 0)
                return RedirectToAction("DashboardPhoto");
             
            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), storePath,
                        form.Files[0].FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await form.Files[0].CopyToAsync(stream);
            }

            string ImageName = Path.GetFileName(form.Files[0].FileName);
            string Physical = "~/Uploads/" + ImageName;
            NewImage.Path = Physical;
            NewImage.ProjectId = IdProject;

            dbContext.Add(NewImage);
            dbContext.SaveChanges();
            return Redirect("/dashboard/photos/" + IdProject);
        }

    // HANDLING PROJECT EDITING ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        [HttpPost("editproject/{IdProject}")]
        public IActionResult editproj(int IdProject, Project EditedProject)
        {
            if(ModelState.IsValid)
            {
                Project OldProject = dbContext.Project
                    .FirstOrDefault(P => P.ProjectId == IdProject);
                OldProject.Name = EditedProject.Name;
                OldProject.Description = EditedProject.Description;
                dbContext.SaveChanges();
                return RedirectToAction("DashBoardPortfolio");
            }
            return View("DashboardEditProj");
        }

    // HANDLING NEW PROJECT CREATION ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        [HttpPost("newproject")]
        public IActionResult newproject(Project NewProject)
        {
            if(ModelState.IsValid)
            {
                dbContext.Add(NewProject);
                dbContext.SaveChanges();
                return RedirectToAction("DashBoardPortfolio");
            }
            return View("DashboardNewProj");
        }

    // HANDLING NEW MESSAGE ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        [HttpPost("newmessage")]
        public IActionResult newMessage(Message NewMessage)
        {
            if(ModelState.IsValid)
            {
                dbContext.Add(NewMessage);
                dbContext.SaveChanges();
                return RedirectToAction("Success");
            }
            return View("Contact");
        }
    // HANDLING ADMIN CREATOR ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        [HttpPost("admincreator")]
        public IActionResult admincreator(Admin NewAdmin)
        {
            if(dbContext.Admin.ToList().Count < 1)
            {
                if(ModelState.IsValid)
                {
                    PasswordHasher<Admin> Hasher = new PasswordHasher<Admin>();
                    NewAdmin.Password = Hasher.HashPassword(NewAdmin, NewAdmin.Password);
                    dbContext.Add(NewAdmin);
                    dbContext.SaveChanges();
                    return RedirectToAction("AdminSignIn");
                }
                return View("AdminForm");
            }
            return RedirectToAction("AdminSignIn");
        }
     // HANDLING LOGIN ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        [HttpPost("login")]
        public IActionResult login(LoginAdmin AccessUser)
        {
            if(ModelState.IsValid)
            {
                var UserInDb = dbContext.Admin.FirstOrDefault(u => u.LoginName == AccessUser.LEmail); 
                if(UserInDb == null)
                {
                    ModelState.AddModelError("LEmail", "Invalid Email/Password");
                    return View("AdminSignIn");
                }
                var hasher = new PasswordHasher<LoginAdmin>();
                var result = hasher.VerifyHashedPassword(AccessUser, UserInDb.Password, AccessUser.LPassword);
                if(result == 0)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("AdminSignIn");
                }
                HttpContext.Session.SetInt32("UserInSession", UserInDb.AdminId);
                return RedirectToAction("Dashboard");
            }
            return View("AdminSignIn");
        }
            
            
    }
}

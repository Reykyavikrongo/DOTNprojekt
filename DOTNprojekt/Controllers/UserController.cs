using DOTNprojekt.Data;
using DOTNprojekt.Models;
using DOTNprojekt.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DOTNprojekt.Controllers
{
    public class UserController : Controller
    {
        //Class tasked with handling log-ins and user registration
        private readonly ApplicationDbContext _db;
        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User obj)
        {
            //Validation for user registration
            foreach (var user in _db.Users.ToList())
            {
                if (user.User_Name == obj.User_Name)
                {
                    ModelState.AddModelError("User_Name", "That name is already taken, please choose another");
                    break;
                }
            }

            if (ModelState.IsValid)
            {
                _db.Users.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Success");
            }
            return View();
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User obj)
        {
            // Login validation
            if (ModelState.IsValid)
            {
                foreach (var user in _db.Users.ToList())
                {
                    if (user.User_Name == obj.User_Name && user.Password == obj.Password)
                    {
                        //Add relevant information to the session to be used when needed (i.e. uploading files)
                        HttpContext.Session.SetString("login_name", obj.User_Name);
                        HttpContext.Session.SetInt32("logged_in", 1);
                        return RedirectToAction("MyProfile");
                    }
                    else 
                    { 
                        ModelState.AddModelError("Password", "Password is incorrect, please try again"); 
                    }
                }
            }

            return View();
        }

        public IActionResult LogOut()
        {
            // Logout and delete Session information
            HttpContext.Session.SetInt32("logged_in", 0);
            return View();
        }

        public IActionResult MyProfile()
        {
            return View();
        }
    }
}

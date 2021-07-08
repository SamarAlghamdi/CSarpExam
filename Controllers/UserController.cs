using System.Linq;
using CSharpExam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSharpExam.Controllers
{
    public class UserController : Controller
    {
        private MyContext _context;
        public UserController(MyContext context){
            _context=context;
        }
        [HttpGet("")]
        public IActionResult Index(){
            if(HttpContext.Session.GetInt32("UserId")==null){
                return View();
            } else {
                return RedirectToAction("Hobbies","Home");
            }
        }

        [HttpPost("create")]
        public IActionResult Create(User NewUser){
            if(ModelState.IsValid){
                if(_context.Users.Any(u=> u.Email==NewUser.Email)){
                    ModelState.AddModelError("Email","This Email is already registered");
                    return View("Index",NewUser);
                }
                if(_context.Users.Any(u=> u.UserName==NewUser.UserName)){
                    ModelState.AddModelError("UserName","This UserName is already existing");
                    return View("Index",NewUser);
                } else {
                    PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
                    NewUser.Password = passwordHasher.HashPassword(NewUser,NewUser.Password);
                    _context.Users.Add(NewUser);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("UserId",NewUser.UserId);
                    return RedirectToAction("Hobbies","Home");
                }
            } else {
                return View("Index");
            }
        }

        [HttpPost("check")]
        public IActionResult Login(LoginUser LoggedUser){
            if (ModelState.IsValid){
                User DbUser = _context.Users.FirstOrDefault(u => u.Email==LoggedUser.LoggedEmail);
                if (DbUser == null){
                    ModelState.AddModelError("LoggedEmail","Email or Password is Invalid");
                    return View("Index",LoggedUser);
                } else {
                    PasswordHasher<LoginUser> passwordHasher = new PasswordHasher<LoginUser>();
                    var result = passwordHasher.VerifyHashedPassword(LoggedUser,DbUser.Password,LoggedUser.LoggedPassword);
                    if (result==0){
                        ModelState.AddModelError("LoggedEmail","Email or Password is Invalid");
                        return View("Index",LoggedUser);
                    } else {
                        HttpContext.Session.SetInt32("UserId",DbUser.UserId);
                        return RedirectToAction("Hobbies","Home");
                    }
                }
            } else {
                return View("Index");
            }
        }


        [HttpGet("logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
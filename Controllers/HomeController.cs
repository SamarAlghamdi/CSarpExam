using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CSharpExam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CSharpExam.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;
        public HomeController(MyContext context){
            _context=context;
        }

        [HttpGet("Hobby")]
        public IActionResult Hobbies(){
            if(HttpContext.Session.GetInt32("UserId")!=null){
                int id = (int)HttpContext.Session.GetInt32("UserId");
                ViewBag.LoggedUser= _context.Users.SingleOrDefault(u => u.UserId==id);
                List<Hobby> AllHobbies = _context.Hobbies
                .Include(h=>h.Enthusiasts)
                .ToList();
                // ViewBag.ProHobbies=_context.Hobbies
                // .Include(h=>h.Enthusiasts)
                // .Where();
                return View(AllHobbies);
            } else {
                return RedirectToAction("Index","User");
            }
        }

        [HttpGet("Hobby/new")]
        public IActionResult HobbyForm() {
            return View();
        }

        [HttpPost("newHobby")]
        public IActionResult NewHobby(Hobby NewHobby) {
            if(HttpContext.Session.GetInt32("UserId")==null){
                return RedirectToAction("Index","User");
            } else {
                if (ModelState.IsValid){
                    if(_context.Hobbies.Any(h=> h.Name==NewHobby.Name)){
                        ModelState.AddModelError("Name","This Hobby name is already existing");
                        return View("HobbyForm", NewHobby);
                    } else{
                        _context.Hobbies.Add(NewHobby);
                        _context.SaveChanges();
                        return RedirectToAction("Hobbies");
                    }
                } else {
                    return View("HobbyForm");
                }
            }
        }

        [HttpGet("Hobby/{id}")]
        public ActionResult OneHobby(int id){
            if(HttpContext.Session.GetInt32("UserId") == null){
                return RedirectToAction("Index");
            }else{
                ViewBag.Uid = (int)HttpContext.Session.GetInt32("UserId");

                Hobby hobby = _context.Hobbies
                .Include(h=>h.Enthusiasts)
                .ThenInclude(h => h.User)
                .FirstOrDefault(h=>h.HobbyId==id);
                return View("OneHobby",hobby);
            }
        }

        [HttpPost("addToHobbies")]
        public IActionResult AddToHobbies(int hobbyId, string pro){
            int Uid = (int)HttpContext.Session.GetInt32("UserId");

            Association Asso=new Association();
            Asso.Proficiency=pro;
            Asso.HobbyId=hobbyId;
            Asso.UserId=Uid;
            _context.Associations.Add(Asso);
            _context.SaveChanges();
            Hobby hobby = _context.Hobbies
                .Include(h=>h.Enthusiasts)
                .ThenInclude(h => h.User)
                .FirstOrDefault(h=>h.HobbyId==hobbyId);
            return View("OneHobby",hobby);
        }

        [HttpGet("edit/{id}")]
        public IActionResult EditHobby(int id){
            if(HttpContext.Session.GetInt32("UserId") == null){
                return RedirectToAction("Index","User");
            }else{
                Hobby hobby = _context.Hobbies
                .FirstOrDefault(h=>h.HobbyId==id);
                return View(hobby);
            }
        }

        [HttpPost("edit/{id}")]
        public ActionResult Edit(int id, Hobby NewHobby){
            Hobby oldHobby = _context.Hobbies
            .FirstOrDefault(h=>h.HobbyId==id);
            if (ModelState.IsValid){
                oldHobby.Name = NewHobby.Name;
                oldHobby.Description=NewHobby.Description;
                oldHobby.UpdatedAt=DateTime.Now;
                _context.SaveChanges();
                return RedirectToAction("OneHobby",oldHobby);
            }
            else
            {
                return View("EditHobby"); 
            }
        }
        
    }
}

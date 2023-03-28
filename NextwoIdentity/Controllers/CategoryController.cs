using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NextwoIdentity.Data;
using NextwoIdentity.Models;
using System.Data;
using System.Diagnostics;

namespace NextwoIdentity.Controllers
{
    public class CategoryController : Controller
    {
        private NextwoDbContext db;
        public CategoryController(NextwoDbContext _db) {
            db = _db;
        }
        public IActionResult allCats()
        {
            
            return View(db.Categories);
        }
        [HttpGet]
        public IActionResult createproduct()
        {
            return View();
        }
        [HttpPost]

        public IActionResult createcat(Category cat)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(cat);
                db.SaveChanges();
                return RedirectToAction("allCats");
            }
            return View(cat);
        }
        public IActionResult catdetails(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("allCats");
            }
            var data = db.Categories.Find(id);//to check if the id exists
            if (data == null)
            {
                return RedirectToAction("allCats");
            }
            return View(data);
        }
        [HttpGet]
        public IActionResult editcat(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("allCats");
            }
            var data = db.Categories.Find(id);
            if (data == null)
            {
                return RedirectToAction("allCats");
            }
            return View(data);
        }
        [HttpPost]
        public IActionResult editpro(Category cat)
        {
            db.Categories.Update(cat);//update the obj
            db.SaveChanges();
            return RedirectToAction("allCats");
        }

        [HttpGet]
        public IActionResult deletecat(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("allCats");
            }
            var data = db.Categories.Find(id);//to check if the id exists
            if (data == null)
            {
                return RedirectToAction("allCats");
            }
            return View(data);
        }
        [HttpPost]
        public IActionResult deletecat(Category cat)
        {
            var data = db.Categories.Find(cat.CategoryId);
            if (data == null)
            {
                return RedirectToAction("allCats");
            }
            db.Categories.Remove(data); db.SaveChanges();
            return RedirectToAction("allCats");

        }











        public IActionResult Index()
        {
            return View();
        }
    }
}

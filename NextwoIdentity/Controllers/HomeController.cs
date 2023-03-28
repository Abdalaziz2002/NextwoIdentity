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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private NextwoDbContext db;
        public HomeController(ILogger<HomeController> logger, NextwoDbContext _db)
        {
            _logger = logger;
            db = _db;
        }
        public IActionResult allProducts()
        {
            return View(db.Products.Include(x=>x.Category));
        }
        [HttpGet]
        public IActionResult createproduct()
        {
            ViewBag.allDept = new SelectList(db.Categories, "CategoryId", "type");
            return View();
        }
        [HttpPost]
       
        public IActionResult createproduct(Product prd)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(prd);
                db.SaveChanges();
                return RedirectToAction("allProducts");
            }
            return View(prd);
        }
        public IActionResult details(int? id)
        {
            ViewBag.allDept = new SelectList(db.Categories, "CategoryId", "type");
            if (id == null)
            {
                return RedirectToAction("allProducts");
            }
            var data = db.Products.Find(id);//to check if the id exists
            if (data == null)
            {
                return RedirectToAction("allProducts");
            }
            return View(data);
        }
        [HttpGet]
        public IActionResult editpro(int? id)
        {
            ViewBag.allDept = new SelectList(db.Categories, "CategoryId", "type");
            if (id == null)
            {
                return RedirectToAction("allProducts");
            }
            var data = db.Products.Find(id);
            if (data == null)
            {
                return RedirectToAction("allProducts");
            }
            return View(data);
        }
        [HttpPost]
        public IActionResult editpro(Product prd)
        {
            db.Products.Update(prd);//update the obj
            db.SaveChanges();
            return RedirectToAction("allProducts");
        }

        [HttpGet]
        public IActionResult deletepro(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("allProducts");
            }
            var data = db.Products.Find(id);//to check if the id exists
            if (data == null)
            {
                return RedirectToAction("allProducts");
            }
            return View(data);
        }
        [HttpPost]
        public IActionResult deletepro(Product prd)
        {
            var data = db.Products.Find(prd.Id);
            if (data == null)
            {
                return RedirectToAction("allProducts");
            }
            db.Products.Remove(data); db.SaveChanges();
            return RedirectToAction("allProducts");

        }



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
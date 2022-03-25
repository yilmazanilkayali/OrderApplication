﻿using Microsoft.AspNetCore.Mvc;
using OrderApplication.Data;

namespace OrderApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
            db.Database.EnsureCreated();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}

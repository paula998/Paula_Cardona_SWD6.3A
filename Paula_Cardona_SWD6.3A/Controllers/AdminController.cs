using Common;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paula_Cardona_SWD6._3A.Controllers
{
    public class AdminController : Controller
    {
        private CacheDataAccess _cache;
        public AdminController(CacheDataAccess cache)
        {
            _cache = cache;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(MenuItem m)
        {
            _cache.AddMenuItem(m);
            return View();
        }
    }
}

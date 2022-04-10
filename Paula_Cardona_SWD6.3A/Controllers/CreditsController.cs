using Common;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paula_Cardona_SWD6._3A.Controllers
{
    public class CreditsController : Controller
    {
       private CacheDataAccess _cache;
       private readonly FireStoreDataAccess _fireStore;
 
        public CreditsController(CacheDataAccess cache, FireStoreDataAccess firestore)
            {
                _cache = cache;
                _fireStore = firestore;
            }

        public IActionResult Index()
           {
               return View();
           }

        [Authorize]
        public IActionResult List()
        {
            var existing = _cache.GetMenuItems();
            return View(existing);
        }

        [HttpGet]
        [Authorize]
        public IActionResult addcredit()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> addcredit(UserData credit)
        {
            credit.Id = Guid.NewGuid().ToString();
            await _fireStore.AddCredits(User.Claims.ElementAt(4).Value, credit);
            return RedirectToAction("List");
        }
    }

 }

  

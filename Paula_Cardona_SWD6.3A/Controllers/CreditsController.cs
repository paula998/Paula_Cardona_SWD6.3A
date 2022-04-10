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


            public CreditsController(CacheDataAccess cache)
            {
                _cache = cache;
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
    }

    }

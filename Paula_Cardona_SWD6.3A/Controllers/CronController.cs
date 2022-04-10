using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paula_Cardona_SWD6._3A.Controllers
{
    public class CronController : Controller
    {
        private ILogger<CronController> _logger; 

        public CronController(ILogger<CronController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            _logger.LogInformation("Writing a message every minute");
            return Ok("Method finsihed");
        }
    }
}

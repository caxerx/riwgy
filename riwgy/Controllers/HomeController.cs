using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using riwgy.Models;

namespace riwgy.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly RiwgyDbContext _context;

        private readonly string _homeUrl;

        private readonly string _portalUrl;


        public HomeController(RiwgyDbContext context, IConfiguration _config)
        {
            _context = context;
            _homeUrl = _config.GetValue<string>("HomeRedirect");
            _portalUrl = _config.GetValue<string>("Portal");
        }

        [HttpGet]
        public IActionResult HomePageRedirect()
        {
            return RedirectPermanent(_homeUrl);
        }

        [HttpGet("{rwigy}")]
        public IActionResult OriginalUrlRedirect(string rwigy)
        {
            string url = _context.UrlMapping.Where(r => r.Riwgy == rwigy).Select(r => r.OriginalUrl).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(url))
            {
                return Redirect(_portalUrl);
            }
            return Redirect(url);
        }
    }
}

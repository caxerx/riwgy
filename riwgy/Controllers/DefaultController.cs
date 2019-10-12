using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using riwgy.Dto;
using riwgy.Extension;
using riwgy.Model;
using riwgy.Models;
using riwgy.Response;

namespace riwgy.Controllers
{

    [Route("{*url}", Order = 999)]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly string _portalUrl;
        public DefaultController(IConfiguration config)
        {
            _portalUrl = config.GetValue<string>("Portal");
        }

        [HttpGet]
        public IActionResult RedirectToHome()
        {
            return Redirect(_portalUrl);
        }
    }
}

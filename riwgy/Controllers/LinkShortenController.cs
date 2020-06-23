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

    [Route("shorten")]
    [ApiController]
    public class LinkShortenController : ControllerBase
    {
        private const string RWIGY_REGEX = @"^[a-zA-Z0-9_-]+$";

        private readonly RiwgyDbContext _context;

        private readonly Regex _urlMatcher;
        private readonly Regex _riwgyMatcher;

        private readonly string _domain;


        public LinkShortenController(RiwgyDbContext context, IConfiguration _config)
        {
            _context = context;
            _urlMatcher = new Regex(URL_REGEX, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            _riwgyMatcher = new Regex(RWIGY_REGEX, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            _domain = _config.GetValue<string>("Domain");
        }

        [HttpPost]
        public async Task<IActionResult> ShortenLink(RiwgyLinkDto linkDto)
        {
            if(linkDto is null || string.IsNullOrWhiteSpace(linkDto.Url))
            {
                return BadRequest("Url is empty");
            }

            string riwgy = linkDto.Riwgy;

            if (string.IsNullOrWhiteSpace(riwgy) || !_riwgyMatcher.Match(riwgy).Success)
            {
                riwgy = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".GenerateCoupon(6);
            }

            _context.UrlMapping.Add(new UrlMapping
            {
                Riwgy = riwgy,
                OriginalUrl = linkDto.Url
            });

            await _context.SaveChangesAsync();

            return Ok(new LinkCreatedResponse { ShortenedLink = _domain + riwgy });
        }
    }
}

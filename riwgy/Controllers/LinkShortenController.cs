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
        private const string URL_REGEX = @"(https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,})";

        private readonly RiwgyDbContext _context;

        private readonly Regex _urlMatcher;

        private readonly string _domain;


        public LinkShortenController(RiwgyDbContext context, IConfiguration _config)
        {
            _context = context;
            _urlMatcher = new Regex(URL_REGEX, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            _domain = _config.GetValue<string>("Domain");
        }

        [HttpPost]
        public async Task<IActionResult> ShortenLink(RiwgyLinkDto linkDto)
        {
            if(linkDto is null || string.IsNullOrWhiteSpace(linkDto.Url))
            {
                return BadRequest("Url is empty");
            }
            if (!_urlMatcher.Match(linkDto.Url).Success)
            {
                return BadRequest("Invaild Url");
            }

            string riwgy = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".GenerateCoupon(6);

            _context.UrlMapping.Add(new UrlMapping
            {
                Riwgy = riwgy,
                OriginalUrl = linkDto.Url
            });

            await _context.SaveChangesAsync();

            return Ok(new LinkCreatedResponse { ShortenedLink = _domain + riwgy });
        }



        //// GET: api/UrlMappings
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<UrlMapping>>> GetUrlMapping()
        //{
        //    return await _context.UrlMapping.ToListAsync();
        //}

        //// GET: api/UrlMappings/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<UrlMapping>> GetUrlMapping(string id)
        //{
        //    var urlMapping = await _context.UrlMapping.FindAsync(id);

        //    if (urlMapping == null)
        //    {
        //        return NotFound();
        //    }

        //    return urlMapping;
        //}

        //// PUT: api/UrlMappings/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUrlMapping(string id, UrlMapping urlMapping)
        //{
        //    if (id != urlMapping.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(urlMapping).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UrlMappingExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/UrlMappings
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPost]
        //public async Task<ActionResult<UrlMapping>> PostUrlMapping(UrlMapping urlMapping)
        //{
        //    _context.UrlMapping.Add(urlMapping);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (UrlMappingExists(urlMapping.Id))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetUrlMapping", new { id = urlMapping.Id }, urlMapping);
        //}

        //// DELETE: api/UrlMappings/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<UrlMapping>> DeleteUrlMapping(string id)
        //{
        //    var urlMapping = await _context.UrlMapping.FindAsync(id);
        //    if (urlMapping == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.UrlMapping.Remove(urlMapping);
        //    await _context.SaveChangesAsync();

        //    return urlMapping;
        //}

        //private bool UrlMappingExists(string id)
        //{
        //    return _context.UrlMapping.Any(e => e.Id == id);
        //}
    }
}

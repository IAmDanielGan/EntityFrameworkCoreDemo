using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStoreWebAPI.Models;

namespace BookStoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : Controller
    {
        private readonly BookStoresDbContext _context;

        public PublishersController(BookStoresDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Publisher>>> GetPublishers()
        {
            return await _context.Publishers.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Publisher>> GetPublisher(int id)
        {
            //var publisher = await _context.Publishers.FindAsync(id);
            var publisher = _context.Publishers
                .Include(pub => pub.Books)
                    .ThenInclude(book => book.Sales)
                .Include(pub => pub.Users)
                .Where(pub => pub.PubId == id)
                .FirstOrDefault();
            if (publisher == null)
            {
                return NotFound();
            }
            return publisher;
        }

        [HttpGet()]
        [Route("GetReturnTypeObject")]
        public Publisher GetReturnTypeObject() => new Publisher()
        {
            PubId = 1,
            City = "Vancouver",
            Country = "Canada"
        };

        [HttpGet()]
        [Route("GetReturnTypeAsList")]
        public IEnumerable<Publisher> GetReturnTypeAsList() => new List<Publisher>
        {
            new Publisher(){PubId=1, City="Vancouver"},
            new Publisher(){PubId=2, City="Richmond" }
        };

        //[HttpPut]
        //public async Task<IActionResult> PutPublisher(int id, Publisher publisher)
        //{
        //    if(id!=publisher.PubId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(publisher).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();
        //}

        //public async Task<ActionResult<Publisher>> PostPublisher(Publisher publisher)
        //{
        //    _context.Publishers.Add(publisher);
        //    await _context.SaveChangesAsync();
        //    return CreatedAtAction("GetPublisher",new {id=publisher.PubId},publisher.)
        //}
    }
}

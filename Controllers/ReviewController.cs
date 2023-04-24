using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstAPI.Models;

namespace MyFirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly MyFirstAPIDBContext _context;

        public ReviewController(MyFirstAPIDBContext context)
        {
            _context = context;
        }

        // GET: api/Review
        // Modified to return the proper restaurant name alongside the reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetReviews()
        {
          var reviews = await _context.Reviews.Include(r => r.Restaurant).ToListAsync();

          if (reviews == null)
          {
            return NotFound();
          }

          var reviewsWithRestaurantNames = reviews.Select(r => new {
          r.reviewer_id,
          r.rating,
          r.comment,
          RestaurantName = r.Restaurant?.name ?? "Unknown"
        });

        return Ok(reviewsWithRestaurantNames);
    }

        // GET: api/Review/5
        // Modified this one as well to return the restaurant name for the given review
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {

            var reviews = await _context.Reviews.Include(r => r.Restaurant)
                .FirstOrDefaultAsync(r => r.reviewer_id == id);

            if (reviews == null)
            {
                return NotFound();
            }

            return reviews;
        }

        // PUT: api/Review/Repost/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Repost/{id}")]
        public async Task<IActionResult> PutReview(int id, Review review)
        {
            if (id != review.reviewer_id)
            {
                return BadRequest();
            }

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Review
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
          if (_context.Reviews == null)
          {
              return Problem("Entity set 'MyFirstAPIDBContext.Reviews'  is null.");
          }
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReview", new { id = review.reviewer_id }, review);
        }

        // DELETE: api/Review/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            if (_context.Reviews == null)
            {
                return NotFound();
            }
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewExists(int id)
        {
            return (_context.Reviews?.Any(e => e.reviewer_id == id)).GetValueOrDefault();
        }
    }
}

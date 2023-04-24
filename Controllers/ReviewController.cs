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
        public async Task<ActionResult<Response<IEnumerable<object>>>> GetReviews()
        {
            try
            {
                var reviews = await _context.Reviews.Include(r => r.Restaurant).ToListAsync();

                if (reviews == null)
                {
                    return NotFound(new Response<IEnumerable<object>>
                    {
                      StatusCode = 404,
                      StatusDescription = Microsoft.AspNetCore.WebUtilities
                        .ReasonPhrases.GetReasonPhrase(404),
                      Data = null
                    });
                }

                var reviewsWithRestaurantNames = reviews.Select(r => new {
                    r.reviewer_id,
                    r.rating,
                    r.comment,
                    RestaurantName = r.Restaurant?.name ?? "Unknown"
                });

                return Ok(new Response<IEnumerable<object>>
                {
                    StatusCode = 200,
                    StatusDescription = Microsoft.AspNetCore.WebUtilities
                      .ReasonPhrases.GetReasonPhrase(200),
                    Data = reviewsWithRestaurantNames
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response<IEnumerable<object>>
                {
                    StatusCode = 500,
                    StatusDescription = Microsoft.AspNetCore.WebUtilities
                      .ReasonPhrases.GetReasonPhrase(500)
                });
            }
        }

        // GET: api/Review/5
        // Modified this one as well to return the restaurant name for the given review
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            try
            {
                var review = await _context.Reviews.Include(r => r.Restaurant)
                    .FirstOrDefaultAsync(r => r.reviewer_id == id);

                if (review == null)
                {
                    return NotFound(new Response<IEnumerable<object>>
                    {
                      StatusCode = 404,
                      StatusDescription = Microsoft.AspNetCore.WebUtilities
                        .ReasonPhrases.GetReasonPhrase(404),
                      Data = null
                    });
                }

                var response = new Response<Review>
                {
                    StatusCode = 200,
                    StatusDescription = Microsoft.AspNetCore.WebUtilities
                        .ReasonPhrases.GetReasonPhrase(200),
                    Data = review
                };

                return Ok(response);
                }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response<IEnumerable<object>>
                {
                    StatusCode = 500,
                    StatusDescription = Microsoft.AspNetCore.WebUtilities
                      .ReasonPhrases.GetReasonPhrase(500)
                });
            }
        }

        // PUT: api/Review/Repost/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Repost/{id}")]
        public async Task<IActionResult> PutReview(int id, Review review)
        {
            try
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
                        return NotFound(new Response<IEnumerable<object>>
                        {
                        StatusCode = 404,
                        StatusDescription = Microsoft.AspNetCore.WebUtilities
                            .ReasonPhrases.GetReasonPhrase(404),
                        Data = null
                        });
                    }
                    else
                    {
                        throw;
                    }
                }

                var response = new Response<Review>
                {
                    StatusCode = 200,
                    StatusDescription = Microsoft.AspNetCore.WebUtilities
                        .ReasonPhrases.GetReasonPhrase(200),
                    Data = null
                };

                return Ok(response);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response<IEnumerable<object>>
                {
                    StatusCode = 500,
                    StatusDescription = Microsoft.AspNetCore.WebUtilities
                      .ReasonPhrases.GetReasonPhrase(500)
                });
            }
        }

        // DELETE: api/Review/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            try
            {
                var review = await _context.Reviews.FindAsync(id);

                if (review == null)
                {
                    return NotFound(new Response<IEnumerable<object>>
                    {
                      StatusCode = 404,
                      StatusDescription = Microsoft.AspNetCore.WebUtilities
                        .ReasonPhrases.GetReasonPhrase(404),
                      Data = null
                    });
                }

                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();

                var response = new Response<object>
                {
                    StatusCode = 200,
                    StatusDescription = Microsoft.AspNetCore.WebUtilities
                        .ReasonPhrases.GetReasonPhrase(200),
                    Data = null
                };

                return Ok(response);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response<IEnumerable<object>>
                {
                    StatusCode = 500,
                    StatusDescription = Microsoft.AspNetCore.WebUtilities
                      .ReasonPhrases.GetReasonPhrase(500)
                }); 
            }
        }

        private bool ReviewExists(int id)
        {
            return (_context.Reviews?.Any(e => e.reviewer_id == id)).GetValueOrDefault();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using back_end.DataModels;
using back_end.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ICrudService<Review> _crudService;
        private readonly IReviewService _reviewService;

        public ReviewController(ICrudService<Review> crudService, IReviewService reviewService)
        {
            _crudService = crudService;
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var reviews = await _reviewService.GetAll();
                return Ok(reviews);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Review newReview)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                int? userId = null;

                if(userIdClaim != null)
                {
                    // tenta di convertire il numero in un intero
                    if(int.TryParse(userIdClaim.Value, out int parsedUserId))
                    {
                        userId = parsedUserId;
                    }
                    else
                    {
                        return StatusCode(500, new
                        {
                            message = "Impossibile convertire il 'NameIdentifier' " +
                            "in un intero"
                        });
                    }
                }

                await _reviewService.Create(userId, newReview);
                return Ok(new { message = "Recensione aggiunta con successo." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Errore interno del server.", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Review reviewUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedReview = await _reviewService.Update(id, reviewUpdate);
                return Ok(updatedReview);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Recensione con ID: {id} non trovata" });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _crudService.Delete(id);
                return Ok(new { message = "Eliminazione avvenuta con successo" });
            }

            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Recensione con ID: {id} non trovata" });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}


using System.Security.Claims;
using back_end.DataModels;
using back_end.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
            var reviews = await _reviewService.GetAll();
            return Ok(reviews);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Review newReview)
        {
            await _reviewService.Create(newReview);
            return Ok(new { message = "Recensione aggiunta con successo." });
        }

        /*
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Review reviewUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedReview = await _reviewService.Update(id, reviewUpdate);
            return Ok(updatedReview);
        }*/


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _crudService.Delete(id);
            return Ok(new { message = "Eliminazione avvenuta con successo" });
        }
    }
}


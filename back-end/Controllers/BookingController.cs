using back_end.DataModels;
using back_end.Interfaces;
using back_end.Models;
using Microsoft.AspNetCore.Mvc;


namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ICrudService<Booking> _crudService;
        private readonly IBookingService _bookingService;

        public BookingController(ICrudService<Booking> crudService, IBookingService bookingService)
        {
            _crudService = crudService;
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookingRequest newBooking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdBooking = await _bookingService.Create(newBooking.Booking,
                    newBooking.UserId, newBooking.RoomIds, newBooking.ServiceIds);
                return Ok(createdBooking);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var bookings = await _bookingService.GetAll();
                return Ok(bookings);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] BookingRequest updateBooking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedBooking = await _bookingService.Update(id,
                    updateBooking.Booking, updateBooking.RoomIds, updateBooking.ServiceIds);
                return Ok(updatedBooking);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Prenotazione con ID: {id} non trovata" });
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
                return NotFound(new { message = $"Prenotazione con ID: {id} non trovato" });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}


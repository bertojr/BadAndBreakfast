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
        public async Task<IActionResult> Create([FromBody] BookingRequest bookingRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _bookingService.Create(
                bookingRequest,
                bookingRequest.RoomIds,
                bookingRequest.ServiceIds);
            return Ok(new { message = "Prenotazione effettuata con successo" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _bookingService.GetAll();
            return Ok(bookings);
        }

        /*
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] BookingUpdateRequest updateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedBooking = await _bookingService.Update(
                    id,
                    updateRequest,
                    updateRequest.RoomIds,
                    updateRequest.ServiceIds);
            return Ok(updatedBooking);
        }*/

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _crudService.Delete(id);
            return Ok(new { message = "Eliminazione avvenuta con successo" });
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchRooms(
            [FromBody] SearchAvailableRoomsRequest request)
        {
            var availableRooms = await _bookingService.GetAvailableRooms(
                    request.CheckInDate, request.CheckOutDate);

            return Ok(availableRooms);
        }
    }
}


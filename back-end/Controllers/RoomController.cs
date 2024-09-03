using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back_end.DataModels;
using back_end.Interfaces;
using back_end.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly ICrudService<Room> _crudService;
        private readonly IRoomService _roomService;

        public RoomController(ICrudService<Room> crudService, IRoomService roomService)
        {
            _crudService = crudService;
            _roomService = roomService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoomRequest roomRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newRoom = new Room
                {
                    Capacity = roomRequest.Capacity,
                    Description = roomRequest.Description,
                    IsAvailable = roomRequest.IsAvailable,
                    Price = roomRequest.Price,
                    RoomNumber = roomRequest.RoomNumber,
                    RoomType = roomRequest.RoomType,
                    size = roomRequest.size
                };

                var createdRoom = await _roomService.Create(
                    newRoom,
                    roomRequest.AmenitiesIds);
                return Ok(createdRoom);
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
                var rooms = await _roomService.GetAll();
                return Ok(rooms);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] RoomRequest updateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newRoom = new Room
                {
                    Capacity = updateRequest.Capacity,
                    Description = updateRequest.Description,
                    IsAvailable = updateRequest.IsAvailable,
                    Price = updateRequest.Price,
                    RoomNumber = updateRequest.RoomNumber,
                    RoomType = updateRequest.RoomType,
                    size = updateRequest.size
                };

                var updatedRoom = await _roomService.Update(
                    id,
                    newRoom,
                    updateRequest.AmenitiesIds);
                return Ok(updatedRoom);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Camera con ID: {id} non trovata" });
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
                return NotFound(new { message = $"Camera con ID: {id} non trovata" });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}


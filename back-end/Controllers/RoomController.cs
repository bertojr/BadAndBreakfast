using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back_end.DataModels;
using back_end.Interfaces;
using back_end.Models;
using back_end.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var room = await _roomService.GetById(id);
                return Ok(room);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Utente con ID {id} non trovato" });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception)
            {
                // Altre eccezzioni non previste
                return StatusCode(500, new { message = "Errore imprevisto." });
            }
        }


        [HttpPost("{roomId}/amenity/{amenityId}")]
        public async Task<IActionResult> AddAmenityToRoom(int roomId, int amenityId)
        {
            try
            {
                var amenity = await _roomService.AddAmenityToRoom(roomId, amenityId);
                return Ok(amenity);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Errore interno del server.", details = ex.Message });
            }
        }

        [HttpDelete("{roomId}/amenity/{amenityId}")]
        public async Task<IActionResult> RemoveAmenityFromRoom(int roomId, int amenityId)
        {
            try
            {
                await _roomService.RemoveAmenityFromRoom(roomId, amenityId);
                return Ok(new {message = "Rimozione avvenuta con successo"});
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Errore interno del server.", details = ex.Message });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Room updateRoom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

               var updatedRoom = await _roomService.Update(
                    id,
                    updateRoom);
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


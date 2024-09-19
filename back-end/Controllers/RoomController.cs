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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rooms = await _roomService.GetAll();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var room = await _roomService.GetById(id);
            return Ok(room);
        }


        [HttpPost("{roomId}/amenity/{amenityId}")]
        public async Task<IActionResult> AddAmenityToRoom(int roomId, int amenityId)
        {
            var amenity = await _roomService.AddAmenityToRoom(roomId, amenityId);
            return Ok(amenity);
        }

        [HttpDelete("{roomId}/amenity/{amenityId}")]
        public async Task<IActionResult> RemoveAmenityFromRoom(int roomId, int amenityId)
        {
            await _roomService.RemoveAmenityFromRoom(roomId, amenityId);
            return Ok(new { message = "Rimozione avvenuta con successo" });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Room updateRoom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedRoom = await _roomService.Update(
                    id,
                    updateRoom);
            return Ok(updatedRoom);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _crudService.Delete(id);
            return Ok(new { message = "Eliminazione avvenuta con successo" });
        }
    }
}


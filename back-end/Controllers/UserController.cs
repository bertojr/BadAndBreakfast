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
    public class UserController : ControllerBase
    {
        private readonly ICrudService<User> _crudService;
        private readonly IUserService _userService;

        public UserController(ICrudService<User> crudService, IUserService userService)
        {
            _crudService = crudService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetAll();
                return Ok(users);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("{userId}/role/{roleId}")]
        public async Task<IActionResult> AddUserToRole(int userId, int roleId)
        {
            try
            {
                await _userService.AddRoleToUser(userId, roleId);
                return Ok(new { message = "Ruolo aggiunto con successo." });
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
        public async Task<IActionResult> Update(int id, [FromBody] User updateUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedUser = await _crudService.Edit(id, updateUser);
                return Ok(updatedUser);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Utente con ID: {id} non trovata" });
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
                var user = await _userService.GetById(id);
                return Ok(user);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new {message = $"Utente con ID {id} non trovato"});
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
                return NotFound(new { message = $"Utente con ID: {id} non trovato" });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}


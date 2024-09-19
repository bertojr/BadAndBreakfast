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
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [HttpPost("{userId}/role/{roleId}")]
        public async Task<IActionResult> AddUserToRole(int userId, int roleId)
        {
            var role = await _userService.AddRoleToUser(userId, roleId);
            return Ok(role);
        }

        [HttpDelete("{userId}/role/{roleId}")]
        public async Task<IActionResult> RemoveRoleFromUser(int userId, int roleId)
        {
            await _userService.RemoveRoleFromUser(userId, roleId);
            return Ok(new { message = "Ruolo rimosso con successo." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] User updateUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedUser = await _crudService.Edit(id, updateUser);
            return Ok(updatedUser);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetById(id);
            return Ok(user);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _crudService.Delete(id);
            return Ok(new { message = "Eliminazione avvenuta con successo" });
        }
    }
}


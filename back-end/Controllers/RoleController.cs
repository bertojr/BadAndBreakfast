using back_end.DataModels;
using back_end.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ICrudService<Role> _crudService;

        public RoleController(ICrudService<Role> crudService)
        {
            _crudService = crudService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Role newRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdRole = await _crudService.Create(newRole);
            return Ok(createdRole);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _crudService.GetAll();
            return Ok(roles);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Role updateRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedRole = await _crudService.Edit(id, updateRole);
            return Ok(updatedRole);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _crudService.Delete(id);
            return Ok(new { message = "Eliminazione avvenuta con successo" });
        }
    }
}


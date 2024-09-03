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

            try
            {
                var createdRole = await _crudService.Create(newRole);
                return Ok(createdRole);
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
                var roles = await _crudService.GetAll();
                return Ok(roles);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Role updateRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedRole = await _crudService.Edit(id, updateRole);
                return Ok(updatedRole);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Ruolo con ID: {id} non trovata" });
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
                return NotFound(new { message = $"Ruolo con ID: {id} non trovato" });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}


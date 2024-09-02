using back_end.Interfaces;
using back_end.Services;
using Microsoft.AspNetCore.Mvc;


namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdditionalServiceController : ControllerBase
    {
        private readonly IAdditionalService _additionalService;

        public AdditionalServiceController(IAdditionalService additionalService)
        {
            _additionalService = additionalService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DataModels.AdditionalService newService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdService = await _additionalService.Create(newService);
                return Ok(createdService);
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
                var services = await _additionalService.GetAll();
                return Ok(services);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] DataModels.AdditionalService updateService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var editedService = await _additionalService.Edit(id, updateService);
                return Ok(editedService);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new {message = $"Servizio con ID: {id} non trovato" });
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
                await _additionalService.Delete(id);
                return Ok(new { message = "Eliminazione avvenuta con successo" });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Servizio con ID: {id} non trovato" });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}


using back_end.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdditionalServiceController : ControllerBase
    {
        private readonly ICrudService<DataModels.AdditionalService> _crudService;

        public AdditionalServiceController(ICrudService<DataModels.AdditionalService> crudService)
        {
            _crudService = crudService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DataModels.AdditionalService newService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdService = await _crudService.Create(newService);
            return Ok(createdService);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var services = await _crudService.GetAll();
            return Ok(services);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] DataModels.AdditionalService updateService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var editedService = await _crudService.Edit(id, updateService);
            return Ok(editedService);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var service = await _crudService.GetById(id);
            return Ok(service);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _crudService.Delete(id);
            return Ok(new { message = "Eliminazione avvenuta con successo" });
        }
    }
}


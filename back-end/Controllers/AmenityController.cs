using back_end.DataModels;
using back_end.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenityController : ControllerBase
    {
        private readonly ICrudService<Amenity> _crudService;

        public AmenityController(ICrudService<Amenity> crudService)
        {
            _crudService = crudService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]  Amenity newAmenity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdAmenity = await _crudService.Create(newAmenity);
            return Ok(createdAmenity);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var amenities = await _crudService.GetAll();
            return Ok(amenities);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Amenity amenityUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var editedAmenity = await _crudService.Edit(id, amenityUpdate);
            return Ok(editedAmenity);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var amenity = await _crudService.GetById(id);
            return Ok(amenity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _crudService.Delete(id);
            return Ok(new { message = "Eliminazione avvenuta con successo" });
        }
    }
}


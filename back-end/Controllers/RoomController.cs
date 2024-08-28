using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back_end.DataModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {

        private readonly ApplicationDbContext _dbContext;

        public RoomController(ApplicationDbContext dbCOntext)
        {
            _dbContext = dbCOntext;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<Room> Get()
        {
            return _dbContext.Rooms.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<Room>> Create(Room room)
        {
            await _dbContext.Rooms.AddAsync(room);
            await _dbContext.SaveChangesAsync();
            return room;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}


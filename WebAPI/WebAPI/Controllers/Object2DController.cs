using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Repositories;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Object2DController : ControllerBase
    {
        private readonly Object2DRepository _repository;

        public Object2DController(Object2DRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Object2D>>> GetAllObjects()
        {
            var objects = await _repository.GetAllObjectsAsync();
            return Ok(objects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Object2D>> GetObjectById(Guid id)
        {
            var objectData = await _repository.GetObjectByIdAsync(id);
            if (objectData == null)
                return NotFound($"Object with ID {id} not found.");

            return Ok(objectData);
        }

        [HttpPost]
        public async Task<ActionResult> CreateObject(Object2D object2D)
        {
            object2D.ObjectId = Guid.NewGuid();
            await _repository.CreateObjectAsync(object2D);
            return CreatedAtAction(nameof(GetObjectById), new { id = object2D.ObjectId }, object2D);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateObject(Guid id, Object2D object2D)
        {
            if (id != object2D.ObjectId)
                return BadRequest("Object ID mismatch.");

            var existingObject = await _repository.GetObjectByIdAsync(id);
            if (existingObject == null)
                return NotFound($"Object with ID {id} not found.");

            await _repository.UpdateObjectAsync(object2D);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteObject(Guid id)
        {
            var existingObject = await _repository.GetObjectByIdAsync(id);
            if (existingObject == null)
            {
                return NotFound($"Object with ID {id} not found.");
            }

            await _repository.DeleteObjectAsync(id);

            return NoContent();
        }

    }
}

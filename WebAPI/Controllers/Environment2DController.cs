using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Environment2DController : ControllerBase
    {
        private readonly Environment2DRepository _repository;

        public Environment2DController(Environment2DRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Environment2D>>> GetAllEnvironments()
        {
            var environments = await _repository.GetAllEnvironmentsAsync();
            return Ok(environments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Environment2D>> GetEnvironmentById(Guid environmentId)
        {
            var environment = await _repository.GetEnvironmentByIdAsync(environmentId);
            if (environment == null)
            {
                return NotFound($"Environment with ID {environmentId} not found.");
            }
            return Ok(environment);
        }

        [HttpPost]
        public async Task<ActionResult> CreateEnvironment(Environment2D environment)
        {
            await _repository.AddEnvironmentAsync(environment);
            return CreatedAtAction(nameof(GetEnvironmentById),
                                   new { environmentId = environment.EnvironmentId },
                                   environment);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEnvironment(Guid environmentId, Environment2D updatedEnvironment)
        {
            var existingEnvironment = await _repository.GetEnvironmentByIdAsync(environmentId);
            if (existingEnvironment == null)
            {
                return NotFound($"Environment with ID {environmentId} not found.");
            }

            updatedEnvironment.EnvironmentId = environmentId;
            await _repository.UpdateEnvironmentAsync(updatedEnvironment);
            return Ok(updatedEnvironment);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEnvironment(Guid environmentId)
        {
            var existingEnvironment = await _repository.GetEnvironmentByIdAsync(environmentId);
            if (existingEnvironment == null)
            {
                return NotFound($"Environment with ID {environmentId} not found.");
            }
            await _repository.DeleteEnvironmentAsync(environmentId);
            return Ok($"Environment with ID {environmentId} has been deleted.");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebAPI.Repositories;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("environments")]
    public class EnvironmentController : ControllerBase
    {
        private readonly EnvironmentRepository _repository;

        public EnvironmentController(EnvironmentRepository repository)
        {
            _repository = repository;
        }

        // POST: api/environments
        [HttpPost]
        public async Task<IActionResult> CreateEnvironment([FromBody] Environment environment)
        {
            if (environment == null)
            {
                return BadRequest("Environment data is required.");
            }

            try
            {
                var created = await _repository.CreateEnvironmentAsync(environment);

                if (created)
                {
                    return CreatedAtAction(nameof(CreateEnvironment), new { id = environment.id }, environment);
                }
                else
                {
                    return StatusCode(500, "There was an error creating the environment.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}

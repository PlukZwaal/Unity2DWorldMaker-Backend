using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces;

[ApiController]
[Route("environments")]
public class Environment2DController : ControllerBase
{
    private readonly IEnvironment2DRepository _repository; // Gebruik de interface in plaats van concrete repository
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<Environment2DController> _logger;

    // Pas de constructor aan om de interface te accepteren
    public Environment2DController(
        IEnvironment2DRepository repository,  // Gebruik de interface
        IAuthenticationService authenticationService,
        ILogger<Environment2DController> logger)
    {
        _repository = repository;
        _authenticationService = authenticationService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateEnvironment2D([FromBody] Environment2D environment)
    {
        if (environment == null)
        {
            return BadRequest("Environment data is required.");
        }

        var userId = _authenticationService.GetCurrentAuthenticatedUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User not authenticated.");
        }

        _logger.LogInformation("Creating environment for user ID: {UserId}", userId);

        var created = await _repository.CreateEnvironment2DAsync(environment, userId);
        if (created)
        {
            return CreatedAtAction(nameof(CreateEnvironment2D), environment);
        }
        else
        {
            return StatusCode(500, "Failed to create environment.");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetEnvironmentsForUser()
    {
        var userId = _authenticationService.GetCurrentAuthenticatedUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User not authenticated.");
        }

        _logger.LogInformation("Fetching environments for user ID: {UserId}", userId);

        var environments = await _repository.GetEnvironment2DsByUserIdAsync(userId);
        return Ok(environments);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEnvironment2D(string id)
    {
        var userId = _authenticationService.GetCurrentAuthenticatedUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User not authenticated.");
        }

        _logger.LogInformation("Deleting environment {EnvironmentId} for user ID: {UserId}", id, userId);

        var deleted = await _repository.DeleteEnvironment2DAsync(id, userId);
        if (deleted)
        {
            return NoContent();
        }
        else
        {
            return NotFound("Environment not found or you don't have permission to delete it.");
        }
    }
}
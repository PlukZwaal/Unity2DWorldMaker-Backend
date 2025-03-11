using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Interfaces;

[ApiController]
[Route("environments")]
public class Environment2DController : ControllerBase
{
    private readonly IEnvironment2DRepository _repository;  // Verander naar interface
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<Environment2DController> _logger;

    public Environment2DController(
        IEnvironment2DRepository repository,  // Verander naar interface
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

        if (string.IsNullOrWhiteSpace(environment.name) || environment.name.Length > 25)
        {
            return BadRequest("De naam moet tussen 1 en 25 karakters lang zijn.");
        }

        if (environment.maxHeight < 10 || environment.maxHeight > 100)
        {
            return BadRequest("MaxHeight moet tussen de 10 en 100 zijn.");
        }

        if (environment.maxLength < 20 || environment.maxLength > 200)
        {
            return BadRequest("MaxHeight moet tussen de 10 en 100 zijn.");
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

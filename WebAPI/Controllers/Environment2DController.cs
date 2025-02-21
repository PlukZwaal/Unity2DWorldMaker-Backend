using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("environments")]
public class Environment2DController : ControllerBase
{
    private readonly Environment2DRepository _repository;
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<Environment2DController> _logger;

    public Environment2DController(
        Environment2DRepository repository,
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
}

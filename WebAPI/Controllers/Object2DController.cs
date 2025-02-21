using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("environments/{environmentId}/objects")]
public class Object2DController : ControllerBase
{
    private readonly Object2DRepository _repository;
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<Object2DController> _logger;

    public Object2DController(
        Object2DRepository repository,
        IAuthenticationService authenticationService,
        ILogger<Object2DController> logger)
    {
        _repository = repository;
        _authenticationService = authenticationService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateObject2D(string environmentId, [FromBody] Object2D object2D)
    {
        if (object2D == null)
        {
            return BadRequest("Object data is required.");
        }

        var userId = _authenticationService.GetCurrentAuthenticatedUserId();
        _logger.LogInformation("Current authenticated user ID: {UserId}", userId);

        // Controleer of het environment bij de user hoort (extra beveiliging)
        bool environmentExists = await _repository.CheckEnvironmentOwnership(environmentId, userId);
        if (!environmentExists)
        {
            return Forbid("You do not have access to this environment.");
        }

        var created = await _repository.CreateObject2DAsync(object2D, environmentId);
        if (created)
        {
            return CreatedAtAction(nameof(CreateObject2D), object2D);
        }
        else
        {
            return StatusCode(500, "Failed to create object.");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetObjectsByEnvironment(string environmentId)
    {
        var objects = await _repository.GetObjectsByEnvironmentIdAsync(environmentId);
        if (objects == null || objects.Count == 0)
        {
            return NotFound("No objects found for this environment.");
        }
        return Ok(objects);
    }
}

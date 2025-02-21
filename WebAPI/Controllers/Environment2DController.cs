using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Voeg deze using toe
using System.Threading.Tasks;

[ApiController]
[Route("environments")]
public class Environment2DController : ControllerBase
{
    private readonly Environment2DRepository _repository;
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<Environment2DController> _logger; // Voeg een logger toe

    public Environment2DController(
        Environment2DRepository repository,
        IAuthenticationService authenticationService,
        ILogger<Environment2DController> logger) // Injecteer de logger
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

        // Haal de ID van de ingelogde gebruiker op
        var userId = _authenticationService.GetCurrentAuthenticatedUserId();
        _logger.LogInformation("Current authenticated user ID: {UserId}", userId); // Log de gebruikers-ID

        var created = await _repository.CreateEnvironment2DAsync(environment);
        if (created)
        {
            return CreatedAtAction(nameof(CreateEnvironment2D), environment);
        }
        else
        {
            return StatusCode(500, "Failed to create environment.");
        }
    }
}
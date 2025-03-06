using Microsoft.AspNetCore.Mvc;
using WebAPI.Interfaces;

[ApiController]
[Route("environments/{environmentId}/objects")]
public class Object2DController : ControllerBase
{
    private readonly IObject2DRepository _repository;  // ✅ Gebruik interface i.p.v. concrete klasse
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<Object2DController> _logger;

    public Object2DController(
        IObject2DRepository repository,  // ✅ Verander naar interface
        IAuthenticationService authenticationService,
        ILogger<Object2DController> logger)
    {
        _repository = repository;
        _authenticationService = authenticationService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateObject2D(string environmentId, [FromBody] Object2D request)
    {
        var userId = _authenticationService.GetCurrentAuthenticatedUserId();
        if (!await _repository.CheckEnvironmentOwnership(environmentId, userId))
            return Forbid();

        request.id = Guid.NewGuid().ToString();
        request.environmentId = environmentId;

        return await _repository.CreateObject2DAsync(request, environmentId)
            ? CreatedAtAction(nameof(CreateObject2D), request)
            : StatusCode(500);
    }

    [HttpGet]
    public async Task<IActionResult> GetObjectsByEnvironment(string environmentId)
    {
        _logger.LogInformation($"Getting objects for environment ID: {environmentId}");

        var objects = await _repository.GetObjectsByEnvironmentIdAsync(environmentId);
        if (objects == null || objects.Count == 0)
        {
            return NotFound("No objects found for this environment.");
        }
        return Ok(objects);
    }
}

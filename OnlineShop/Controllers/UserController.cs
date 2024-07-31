using Microsoft.AspNetCore.Mvc;
using OnlineShop.Contracts;
using OnlineShop.DataModels;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<UserController> _logger;

    public UserController(IAuthService authService, ILogger<UserController> logger)
    {
        _authService = authService;
        _logger = logger;
    }


    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid payload");
            var (status, message) = await _authService.Login(model);
            if (status == 0)
                return BadRequest(message);
            return Ok(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [Route("registeration")]
    public async Task<IActionResult> Register(RegistrationModel model, string role)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid payload");
            var (status, message) = await _authService.Registeration(model, role);
            if (status == 0)
            {   
                return BadRequest(message);
            }
            return CreatedAtAction(nameof(Register), model);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Contracts;
using OnlineShop.DataModels;
using OnlineShop.DTOs.Roles;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserRepository userRepository, ILogger<UserController> logger)
    {
        _userRepository = userRepository;
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
            var (status, message) = await _userRepository.Login(model);
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
    public async Task<IActionResult> CreateUser(RegistrationModel model, string role)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid payload");
            var (status, message) = await _userRepository.CreateUser(model, role);
            if (status == 0)
            {   
                return BadRequest(message);
            }
            return CreatedAtAction(nameof(CreateUser), model);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("Get User")]
    public async Task<ActionResult> GetUser(string userId)
    {
        if (!ModelState.IsValid)
            return BadRequest("invalid payload");
        var user = await this._userRepository.GetAsync(userId);
        if (user == null)
        {
            throw new Exception($"UserId{userId} is not Found");
        }
  
        return Ok(user);
    }

    [HttpGet("Get All User")]
    public async Task<ActionResult> GetAllUsers()
    {
        var users = await _userRepository.GetAllAsync();  
        return Ok(users);
    }
    [HttpDelete]
    [Route("DeleteUser")]
    public async Task<IActionResult> DeleteUser(string email)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid payload");
        var (status, message) = await _userRepository.DeleteUserAsync(email);
        if (status == 0) return BadRequest(message);
        return Ok(message);
    }

    [HttpPut]
    [Route("Update Password")]
    public async Task<ActionResult> UpdateUser(RegistrationModel model, string newPassword)
    {
        if (!ModelState.IsValid)
            return BadRequest("invalid");
        var (status, message) = await _userRepository.UpdatePasswordAsync(model, newPassword);
        if(status== 0) return BadRequest(message);
        return Ok(message);
    }


}
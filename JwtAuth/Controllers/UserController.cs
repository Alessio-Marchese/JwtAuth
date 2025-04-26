using JwtAuth.DTO;
using JwtAuth.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuth.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private readonly IRegistrationService _registrationService;

    public UserController(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterUserDTO dto)
    {
        var result = await _registrationService.RegisterAsync(dto);
        if (result.IsSuccessful)
            return Ok();
        else
            return BadRequest(result.Reason);
    }
}

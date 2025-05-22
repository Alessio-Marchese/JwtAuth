using JwtAuth.DTO;
using JwtAuth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuth.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private readonly IRegistrationService _registrationService;
    private readonly ILoginService _loginService;

    public UserController(IRegistrationService registrationService, ILoginService loginService)
    {
        _loginService = loginService;
        _registrationService = registrationService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterUserDTO dto)
    {
        var result = await _registrationService.RegisterAsync(dto);
        if (result.IsSuccessful)
            return Ok();
        else
            return BadRequest(result.ErrorMessage);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginUserDTO dto)
    {
        var result = await _loginService.Login(dto.email, dto.password);
        if (result.IsSuccessful)
            return Ok(result.Data);
        else
            return BadRequest(result.ErrorMessage);
    }
}

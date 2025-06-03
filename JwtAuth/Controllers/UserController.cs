using JwtAuth.Application.DTO;
using JwtAuth.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuth.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    private readonly IRegistrationService _registrationService;
    private readonly ILoginService _loginService;
    private readonly IJwtService _jwtService;

    public UserController(IRegistrationService registrationService, ILoginService loginService, IJwtService jwtService)
    {
        _loginService = loginService;
        _registrationService = registrationService;
        _jwtService = jwtService;
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

    [HttpGet("validate")]
    public ActionResult<bool> Validate(string token)
     => _jwtService.ValidateToken(token);
}

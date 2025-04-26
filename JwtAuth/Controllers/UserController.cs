using JwtAuth.DTO;
using JwtAuth.Services;
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
    public async Task Register(RegisterUserDTO dto)
        => await _registrationService.RegisterAsync(dto);
}

using Alessio.Marchese.Utils.Core;
using JwtAuth.DTO;
using JwtAuth.Tools;

namespace JwtAuth.Services;

public interface IRegistrationService
{
    Task<Result> RegisterAsync(RegisterUserDTO dto);
}

public class RegistrationService : IRegistrationService
{
    private readonly IRegistrationTool _registrationTool;

    public RegistrationService(IRegistrationTool registrationTool)
    {
        _registrationTool = registrationTool;
    }

    public async Task<Result> RegisterAsync(RegisterUserDTO dto)
    {
        if (_registrationTool.CheckEmailAvailability(dto.Email))
            return Result.Failure("The email is already used");

        await _registrationTool.RegisterAsync(dto);

        return Result.Success();
    }
}

using Alessio.Marchese.Utils.Core;
using JwtAuth.Application.DTO;
using JwtAuth.Application.Tools;

namespace JwtAuth.Application.Services;

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
        var checkAvailability = await _registrationTool.CheckEmailAvailabilityAsync(dto.Email);
        if (!checkAvailability.IsSuccessful)
            return checkAvailability.ToResult();

        await _registrationTool.RegisterAsync(dto);

        return Result.Success();
    }
}

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
        var checkAvailability = await _registrationTool.CheckEmailAvailabilityAsync(dto.Email);
        if (!checkAvailability.IsSuccessful)
            return checkAvailability;

        await _registrationTool.RegisterAsync(dto);

        return Result.Success();
    }
}

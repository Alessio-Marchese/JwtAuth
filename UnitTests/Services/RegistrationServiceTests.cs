using Alessio.Marchese.Utils.Core;
using JwtAuth.Application.DTO;
using JwtAuth.Application.Services;
using JwtAuth.Application.Tools;
using JwtAuth.Common.Costants;
using Moq;

namespace UnitTests.Services;

public class RegistrationServiceTests
{
    private readonly Mock<IRegistrationTool> _registrationTool;

    private readonly RegistrationService _sut;
    public RegistrationServiceTests()
    {
        _registrationTool = new Mock<IRegistrationTool>();

        _sut = new(_registrationTool.Object);
    }

    [Fact]
    public async Task RegisterAsync_should_execute_as_expected()
    {
        var userDto = new RegisterUserDTO
        {
            Email = "email@test.com"
        };

        _registrationTool.Setup(t => t.CheckEmailAvailabilityAsync(userDto.Email))
            .ReturnsAsync(Result.Success());
        
        var actual = await _sut.RegisterAsync(userDto);

        Assert.True(actual.IsSuccessful);
    }

    [Fact]
    public async Task RegisterAsync_should_fail_if_email_is_already_taken()
    {
        var userDto = new RegisterUserDTO
        {
            Email = "email@test.com"
        };

        _registrationTool.Setup(t => t.CheckEmailAvailabilityAsync(userDto.Email))
            .ReturnsAsync(Result.Failure(ErrorMessages.EmailAlreadyUsed));

        var actual = await _sut.RegisterAsync(userDto);

        Assert.False(actual.IsSuccessful);
    }
}

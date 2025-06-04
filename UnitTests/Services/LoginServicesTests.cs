using Alessio.Marchese.Utils.Core;
using JwtAuth.Application.Services;
using JwtAuth.Common.Costants;
using JwtAuth.Domain.Models.Entities;
using JwtAuth.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;

namespace UnitTests.Services;

public class LoginServicesTests
{
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IPasswordHasher<User>> _passwordHasher;
    private readonly Mock<IJwtService> _jwtService;

    private readonly LoginService _sut;

    public LoginServicesTests()
    {
        _userRepository = new Mock<IUserRepository>();
        _passwordHasher = new Mock<IPasswordHasher<User>>();
        _jwtService = new Mock<IJwtService>();

        _sut = new LoginService(
            _userRepository.Object,
            _passwordHasher.Object,
            _jwtService.Object);
    }

    [Fact]
    public async Task LoginAsync_should_execute_as_expected()
    {
        var email = "email@test.com";
        var password = "SuperSecretPassword";

        var loggedUser = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = "AQAAAAIAAYagAAAAEPVlrcjOuvCbcSwPHRAPNIwFseKE3aftxRX3iBzMJiKJpOAFe6oxAreiqkG4h0d02w==",
            Role = Roles.USER
        };

        _userRepository.Setup(r => r.GetByEmailAsync(email))
            .ReturnsAsync(Result<User>.Success(loggedUser));

        _passwordHasher.Setup(x => x.VerifyHashedPassword(loggedUser, loggedUser.PasswordHash, password))
            .Returns(PasswordVerificationResult.Success);

        var actual = await _sut.LoginAsync(email, password);

        Assert.True(actual.IsSuccessful);
    }

    [Fact]
    public async Task LoginAsync_should_fail_if_email_doesnt_exist()
    {
        var email = "email@test.com";
        var password = "SuperSecretPassword";

        _userRepository.Setup(r => r.GetByEmailAsync(email))
            .ReturnsAsync(Result<User>.Failure(ErrorMessages.UserNotFound));

        var actual = await _sut.LoginAsync(email, password);

        Assert.False(actual.IsSuccessful);    }

    [Fact]
    public async Task LoginAsync_should_fail_if_wrong_password()
    {
        var email = "email@test.com";
        var password = "SuperSecretPassword";

        var loggedUser = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = "AQAAAAIAAYagAAAAEPVlrcjOuvCbcSwPHRAPNIwFseKE3aftxRX3iBzMJiKJpOAFe6oxAreiqkG4h0d02w==",
            Role = Roles.USER
        };

        _userRepository.Setup(r => r.GetByEmailAsync(email))
            .ReturnsAsync(Result<User>.Success(loggedUser));

        _passwordHasher.Setup(x => x.VerifyHashedPassword(loggedUser, loggedUser.PasswordHash, password))
            .Returns(PasswordVerificationResult.Failed);

        var actual = await _sut.LoginAsync(email, password);

        Assert.False(actual.IsSuccessful);
        Assert.True(actual.ErrorMessage == ErrorMessages.WrongPassword);
    }

    [Fact]
    public async Task LoginAsync_should_fail_if_rehash_is_required()
    {
        var email = "email@test.com";
        var password = "SuperSecretPassword";

        var loggedUser = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = "AQAAAAIAAYagAAAAEPVlrcjOuvCbcSwPHRAPNIwFseKE3aftxRX3iBzMJiKJpOAFe6oxAreiqkG4h0d02w==",
            Role = Roles.USER
        };

        _userRepository.Setup(r => r.GetByEmailAsync(email))
            .ReturnsAsync(Result<User>.Success(loggedUser));

        _passwordHasher.Setup(x => x.VerifyHashedPassword(loggedUser, loggedUser.PasswordHash, password))
            .Returns(PasswordVerificationResult.SuccessRehashNeeded);

        var actual = await _sut.LoginAsync(email, password);

        Assert.False(actual.IsSuccessful);
        Assert.True(actual.ErrorMessage == ErrorMessages.RehashRequired);
    }
}

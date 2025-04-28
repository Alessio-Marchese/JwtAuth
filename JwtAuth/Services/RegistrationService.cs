using JwtAuth.DTO;
using JwtAuth.Models;
using JwtAuth.Repositories;
using JwtAuth.Utilities;
using Microsoft.AspNetCore.Identity;

namespace JwtAuth.Services;

public interface IRegistrationService
{
    Task<Result> RegisterAsync(RegisterUserDTO dto);
}

public class RegistrationService : IRegistrationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IEmailValidator _emailValidator;

    public RegistrationService(
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher,
        IEmailValidator emailValidator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _emailValidator = emailValidator;
    }

    public async Task<Result> RegisterAsync(RegisterUserDTO dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (!_emailValidator.IsValidEmail(dto.Email))
            return new Result() { IsSuccessful = false, Reason = "The email format is invalid" };

        if (user.IsSuccessful)
            return new Result() { IsSuccessful = false, Reason = "The email is already used"};

        var newUser = new User { Email = dto.Email };

        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, dto.Password);

        await _userRepository.CreateAsync(newUser);

        return new Result() { IsSuccessful = true };
    }
}

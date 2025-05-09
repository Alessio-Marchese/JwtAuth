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

    public RegistrationService(
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result> RegisterAsync(RegisterUserDTO dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user.IsSuccessful)
            return Result.Failure("The email is already used");

        var newUser = new User { Email = dto.Email };

        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, dto.Password);

        await _userRepository.CreateAsync(newUser);

        return Result.Success();
    }
}

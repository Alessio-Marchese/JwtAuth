using JwtAuth.DTO;
using JwtAuth.Models;
using JwtAuth.Repositories;
using Microsoft.AspNetCore.Identity;

namespace JwtAuth.Services;

public interface IRegistrationService
{
    Task Register(RegisterUserDTO dto);
}

public class RegistrationService
{
    private readonly UserRepository _userRepository;
    private readonly PasswordHasher<User> _passwordHasher;

    public RegistrationService(UserRepository userRepository, PasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task Register(RegisterUserDTO dto)
    {
        var user = _userRepository.GetByEmailAsync(dto.Email);
        if (user is not null)
            return;

        var newUser = new User { Email = dto.Email };

        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, dto.Password);

        await _userRepository.CreateAsync(newUser); 
    }
}

using JwtAuth.DTO;
using JwtAuth.Models;
using JwtAuth.Repositories;
using Microsoft.AspNetCore.Identity;

namespace JwtAuth.Services;

public interface IRegistrationService
{
    Task RegisterAsync(RegisterUserDTO dto);
}

public class RegistrationService : IRegistrationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public RegistrationService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task RegisterAsync(RegisterUserDTO dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user is not null)
            return;

        var newUser = new User { Email = dto.Email };

        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, dto.Password);

        await _userRepository.CreateAsync(newUser); 
    }
}

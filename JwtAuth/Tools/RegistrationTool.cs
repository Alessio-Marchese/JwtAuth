using JwtAuth.DTO;
using JwtAuth.Models.Entities;
using JwtAuth.Repositories;
using Microsoft.AspNetCore.Identity;

namespace JwtAuth.Tools;

public interface IRegistrationTool
{
    bool CheckEmailAvailability(string email);
    Task RegisterAsync(RegisterUserDTO dto);
}

public class RegistrationTool : IRegistrationTool
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public RegistrationTool(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public bool CheckEmailAvailability(string email)
        => _userRepository.GetByEmailAsync(email) is null;

    public async Task RegisterAsync(RegisterUserDTO dto)
    {
        var newUser = new User { Email = dto.Email };

        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, dto.Password);

        await _userRepository.CreateAsync(newUser);
    }
    
}

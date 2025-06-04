using Alessio.Marchese.Utils.Core;
using JwtAuth.Application.DTO;
using JwtAuth.Common.Costants;
using JwtAuth.Domain.Models.Entities;
using JwtAuth.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace JwtAuth.Application.Tools;

public interface IRegistrationTool
{
    Task<Result> CheckEmailAvailabilityAsync(string email);
    Task RegisterAsync(RegisterUserDTO dto);
}

public class RegistrationTool : IRegistrationTool
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public RegistrationTool(
        IPasswordHasher<User> passwordHasher,
        IUserRepository userRepository)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
    }

    public async Task<Result> CheckEmailAvailabilityAsync(string email)
    {
        var getUser = await _userRepository.GetByEmailAsync(email);
        if (getUser.IsSuccessful)
            return Result.Failure(ErrorMessages.EmailAlreadyUsed);

        return Result.Success();
    }

    public async Task RegisterAsync(RegisterUserDTO dto)
    {
        var newUser = new User { Email = dto.Email };

        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, dto.Password);

        await _userRepository.CreateAsync(newUser);
    }
    
}

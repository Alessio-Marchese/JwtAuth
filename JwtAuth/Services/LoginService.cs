using Alessio.Marchese.Utils.Core;
using JwtAuth.Models.Entities;
using JwtAuth.Repositories;
using Microsoft.AspNetCore.Identity;

namespace JwtAuth.Services;

public interface ILoginService
{
    Task<Result<string>> Login(string email, string password);
}

public class LoginService : ILoginService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    public LoginService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
    }
    public async Task<Result<string>> Login(string email, string password)
    {
        var getUser = await _userRepository.GetByEmailAsync(email);
        if (!getUser.IsSuccessful)
            return getUser.ToResult<string>();

        var entryPasswordHashed = _passwordHasher.VerifyHashedPassword(getUser.Data, getUser.Data.PasswordHash, password);

        if (entryPasswordHashed == PasswordVerificationResult.SuccessRehashNeeded)
            return Result<string>.Failure("Rehash needed :(");

        if (entryPasswordHashed == PasswordVerificationResult.Success)
            return Result<string>.Success("Logged In!");
        else
            return Result<string>.Failure("Wrong Password :(");
    }
}

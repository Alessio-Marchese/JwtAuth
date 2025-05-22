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
    private readonly IJwtService _jwtService;
    public LoginService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IJwtService jtwService)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _jwtService = jtwService;
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
        {
            var token = _jwtService.GenerateToken(getUser.Data.Id, getUser.Data.Role);
            return Result<string>.Success(token);
        }
        else
            return Result<string>.Failure("Wrong Password :(");
    }
}

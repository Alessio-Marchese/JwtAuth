using System.Threading.Tasks;
using JwtAuth.Models;
using JwtAuth.Repositories;
using JwtAuth.Utilities;
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
            return new Result<string> { IsSuccessful = false, Reason = getUser.Reason};

        var entryPasswordHashed = _passwordHasher.VerifyHashedPassword(getUser.Data, getUser.Data.PasswordHash, password);


        if (entryPasswordHashed == PasswordVerificationResult.SuccessRehashNeeded)
            return new Result<string> { IsSuccessful = false, Reason = "Rehashneeded :(" };

        if (entryPasswordHashed == PasswordVerificationResult.Success)
            return new Result<string> { IsSuccessful = true, Data = "Logged In!" };
        else
            return new Result<string> { IsSuccessful = false, Reason = "Wrong Password :(" };
    }
}

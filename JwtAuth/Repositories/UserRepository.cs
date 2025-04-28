using JwtAuth.Context;
using JwtAuth.Models;
using JwtAuth.Utilities;
using Microsoft.EntityFrameworkCore;

namespace JwtAuth.Repositories;


public interface IUserRepository
{
    Task<Result<User>> GetByEmailAsync(string email);
    Task CreateAsync(User user);

}
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<User>> GetByEmailAsync(string email)
    {
        var getUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if(getUser == null)
            return new Result<User>() { IsSuccessful = false, Reason = "User with this email not registered"};
        return new Result<User>() { IsSuccessful = true, Data = getUser };
    }
    public async Task CreateAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}

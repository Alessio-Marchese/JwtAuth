using Alessio.Marchese.Utils.Core;
using JwtAuth.Common.Costants;
using JwtAuth.Domain.Models.Entities;
using JwtAuth.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace JwtAuth.Infrastructure.Repositories;


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
        if (getUser == null)
            return Result<User>.Failure(ErrorMessages.UserNotFound);

        return Result<User>.Success(getUser);
    }
    public async Task CreateAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}

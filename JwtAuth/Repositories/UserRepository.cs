using JwtAuth.Context;
using JwtAuth.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtAuth.Repositories;


public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task CreateAsync(User user);

}
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    public async Task CreateAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}

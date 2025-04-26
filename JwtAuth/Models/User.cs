namespace JwtAuth.Models;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public Roles Role { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}

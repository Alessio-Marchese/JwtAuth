namespace JwtAuth.Models;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public Roles Role { get; set; }
    public string PasswordHash { get; set; } = string.Empty;

    public User()
    {
        Id = Guid.NewGuid();
        Role = Roles.USER;
    }
}

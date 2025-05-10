namespace Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public ICollection<Role> Roles { get; set; } = [];

    public static User Create(Guid id, string userName, string passwordHash, string email) {
        return new User { Id = id, Username = userName, PasswordHash = passwordHash, Email = email };
    }
}

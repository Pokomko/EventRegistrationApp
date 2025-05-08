namespace Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    //public string Email { get; set; } = string.Empty;
    //public ICollection<RoleEntity> Roles { get; set; } = [];

    public static User Create(Guid id, string userName, string passwordHash) {
        return new User { Id = id, Username = userName, PasswordHash = passwordHash};
    }
}

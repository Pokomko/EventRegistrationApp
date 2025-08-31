namespace Domain.Abstractions;

public interface IUserService1
{
    public Task RegisterAsync(string userName, string password, string email);
    public Task<string> LoginAsync(string email, string password);
}

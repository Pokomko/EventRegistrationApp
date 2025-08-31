namespace Application.Interfaces;

public interface IUserService
{
    public Task RegisterAsync(string userName, string password, string email);
    public Task<string> LoginAsync(string email, string password);
}

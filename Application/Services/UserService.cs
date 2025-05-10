using Domain.Abstractions;
using Domain.Entities;

namespace Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }
    public async Task Register(string userName, string password, string email) { 
        var hashedPassword = _passwordHasher.Generate(password); 

        var user = User.Create(Guid.NewGuid(), userName, hashedPassword, email);

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
    }

    public async Task<string> Login(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        var result = _passwordHasher.Verify(password, user.PasswordHash);

        if (!result) {
            throw new Exception("Failed to login");
        }

        var token = _jwtProvider.GenerateToken(user);

        return token;
    } 
}

using Domain.Abstractions;  // Подключение абстракций для работы с репозиториями и сервисами.
using Domain.Entities;      // Подключение сущностей (например, для пользователя).
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    // Сервис для работы с пользователями, например, регистрация и аутентификация.
    public class UserService
    {
        // Репозиторий для работы с пользователями, используется для добавления и получения данных.
        private readonly IUserRepository _userRepository;

        // Сервис для хэширования паролей, чтобы хранить их безопасным образом.
        private readonly IPasswordHasher _passwordHasher;

        // Сервис для генерации JWT-токенов (JSON Web Tokens) для аутентификации.
        private readonly IJwtProvider _jwtProvider;

        // Логгер для класса
        private readonly ILogger<UserService> _logger; 

        // Конструктор, через который внедряются зависимости от IUserRepository, IPasswordHasher и IJwtProvider.
        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider, ILogger<UserService> logger)
        {
            _userRepository = userRepository;  // Инициализация репозитория пользователей.
            _passwordHasher = passwordHasher;  // Инициализация сервиса хэширования паролей.
            _jwtProvider = jwtProvider;  // Инициализация сервиса генерации токенов.
            _logger = logger; // Инициализация логгера.
        }

        // Метод для регистрации пользователя. Хэширует пароль и сохраняет нового пользователя.
        public async Task Register(string userName, string password, string email)
        {
            try
            {
                _logger.LogInformation("Начало регистрации пользователя {UserName}", userName);

                // Генерация хэшированного пароля.
                var hashedPassword = _passwordHasher.Generate(password);

                // Создание нового пользователя с уникальным идентификатором, хэшированным паролем и email.
                var user = User.Create(Guid.NewGuid(), userName, hashedPassword, email);

                // Добавление пользователя в репозиторий.
                await _userRepository.AddAsync(user);

                // Сохранение изменений в базе данных.
                await _userRepository.SaveChangesAsync();

                _logger.LogInformation("Пользователь {UserName} успешно зарегистрирован", userName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при регистрации пользователя {UserName}", userName);
                throw; // Перебрасываем исключение дальше.
            }
        }

        // Метод для аутентификации пользователя. Проверяет пароль и генерирует JWT-токен.
        public async Task<string> Login(string email, string password)
        {
            try
            {
                _logger.LogInformation("Попытка аутентификации пользователя с email: {Email}", email);

                // Получение пользователя по email из репозитория.
                var user = await _userRepository.GetByEmailAsync(email);

                if (user == null)
                {
                    _logger.LogWarning("Пользователь с email {Email} не найден.", email);
                    throw new InvalidOperationException("Пользователь не найден.");
                }

                // Проверка, совпадает ли введённый пароль с сохранённым хэшом.
                var result = _passwordHasher.Verify(password, user.PasswordHash);

                if (!result)
                {
                    _logger.LogWarning("Неверный пароль для пользователя с email: {Email}", email);
                    throw new InvalidOperationException("Неверный пароль.");
                }

                // Генерация JWT-токена для аутентифицированного пользователя.
                var token = _jwtProvider.GenerateToken(user);

                _logger.LogInformation("Пользователь с email {Email} успешно аутентифицирован.", email);

                // Возвращаем сгенерированный токен.
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при аутентификации пользователя с email: {Email}", email);
                throw; // Перебрасываем исключение дальше.
            }
        }
    }
    
}

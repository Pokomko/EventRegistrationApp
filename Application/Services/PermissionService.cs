using Domain.Abstractions;  // Подключение абстракций для работы с репозиториями.
using Domain.Enum;          // Подключение перечислений прав пользователя.
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    // Сервис для работы с правами доступа пользователей.
    public class PermissionService : IPermissionService
    {
        // Репозиторий для работы с пользователями, используется для получения данных о правах.
        private readonly IUserRepository _userRepository;

        private readonly ILogger<PermissionService> _logger;

        // Конструктор, через который внедряется зависимость от IUserRepository.
        public PermissionService(IUserRepository userRepository, ILogger<PermissionService> logger)
        {
            _userRepository = userRepository; // Сохраняем репозиторий в поле класса.
            _logger = logger;
        }

        // Метод для получения прав пользователя по его идентификатору.
        public Task<HashSet<PermissionsEnum>> GetPermissionsAsync(Guid id)
        {
            try
            {
                _logger.LogInformation($"Запрос прав пользователя с ID: {id}");

                // Вызов репозитория для получения прав пользователя.
                var permissions = _userRepository.GetUserPermissions(id);

                if (permissions == null)
                {
                    _logger.LogWarning($"Пользователь с ID {id} не имеет прав или данные не найдены.");
                }

                return permissions; // Возвращаем пустой набор, если данных нет.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении прав для пользователя с ID: {id}");
                throw; // Перебрасываем исключение дальше, чтобы оно могло быть обработано на более высоком уровне.
            }
        }
    }
}

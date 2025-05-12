using Domain.Entities;  // Подключение сущности User для использования в методах.

namespace Domain.Abstractions
{
    // Интерфейс для работы с JWT-токенами.
    public interface IJwtProvider
    {
        // Метод для генерации JWT-токена для указанного пользователя.
        string GenerateToken(User user);
    }
}

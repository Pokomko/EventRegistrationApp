namespace Domain.Abstractions
{
    // Интерфейс для хэширования паролей.
    public interface IPasswordHasher
    {
        // Метод для генерации хэшированного пароля из обычного.
        string Generate(string password);

        // Метод для проверки пароля: совпадает ли введённый пароль с сохранённым хэшом.
        bool Verify(string password, string hashedPassword);
    }
}

// Функция для отправки данных регистрации
async function registerUser(event) {
    event.preventDefault(); // Отменяем стандартное поведение формы

    // Собираем данные из формы
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    const userData = {
        username: username,
        password: password
    };

    try {
        // Отправляем запрос на сервер для регистрации
        const response = await fetch('/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(userData),
        });

        // Проверяем успешность ответа
        if (response.ok) {
            alert('Регистрация прошла успешно!');
            window.location.href = "/index.html";
        } else {
            alert('Ошибка при регистрации. Попробуйте снова.');
        }
    } catch (error) {
        console.error('Ошибка при отправке данных на сервер:', error);
        alert('Произошла ошибка. Попробуйте снова.');
    }
}

// Функция для отправки данных авторизации
async function loginUser(event) {
    event.preventDefault(); // Отменяем стандартное поведение формы

    // Собираем данные из формы
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    const userData = {
        username: username,
        password: password
    };

    console.log(userData);

    try {
        // Отправляем запрос на сервер для авторизации
        const response = await fetch('/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(userData),
        });

        // Проверяем успешность ответа
        if (response.ok) {
            alert('Вход выполнен успешно!');
            window.location.href = "/index.html";
        } else {
            alert('Ошибка при входе. Проверьте данные и попробуйте снова.');
        }
    } catch (error) {
        console.error('Ошибка при отправке данных на сервер:', error);
        alert('Произошла ошибка. Попробуйте снова.');
    }
}

// Добавление обработчиков событий для форм
document.getElementById('registration-form')?.addEventListener('submit', registerUser);
document.getElementById('login-form')?.addEventListener('submit', loginUser);
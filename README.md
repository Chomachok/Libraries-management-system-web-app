# 📚 Library Management System

Полноценное веб-приложение для управления библиотеками с разделением ролей (`Librarian`, `Reader`), поддержкой нескольких филиалов, каталогом книг, выдачей и возвратом, а также расчётом штрафов за просрочку.

**Стек**: .NET 8 Web API, PostgreSQL, React (Vite), Docker.

---

## 🚀 Возможности

- 📖 Публичный каталог книг с поиском и фильтрацией по библиотекам
- 🔐 Регистрация и аутентификация (JWT)
- 👥 Роли:
  - **Гость** — просмотр каталога, регистрация
  - **Читатель** — взятие/возврат книг, история выдач, профиль
  - **Библиотекарь** — полное управление книгами, читателями, выдачами, дашборд статистики
- ⚖️ Бизнес-правила:
  - Контроль доступных экземпляров
  - Автоматический расчёт штрафа при просрочке (0.5 ₽ / день)
  - Запрет удаления книги с активными выдачами
- 🎨 Современный пастельный дизайн (персиковый акцент)
- 📱 Адаптивная вёрстка (мобильные, планшеты, десктоп)
- 🐳 Docker Compose для быстрого развёртывания

---

## 📐 Архитектура
    project-root/
    ├── backend/ # .NET 8 Web API
    │ ├── Controllers/ # Auth, Books, Checkouts, Dashboard...
    │ ├── Services/ # Бизнес-логика
    │ ├── Models/ # Сущности БД
    │ ├── DTOs/ # Объекты передачи данных
    │ ├── Middleware/ # Глобальная обработка ошибок
    │ └── Data/ # DbContext, сиды
    ├── frontend/ # React (Vite)
    │ ├── src/
    │ │ ├── components/ # Header, Modal, Pagination, ProtectedRoute
    │ │ ├── pages/ # Страницы (Login, Books, Dashboard...)
    │ │ ├── hooks/ # useApi для безопасных запросов
    │ │ └── styles/ # Глобальные CSS-переменные и утилиты
    │ └── ...
    ├── docker-compose.yml # DB, Backend, Frontend
    └── .env.example # Шаблон переменных окружения

    
---

## ⚙️ Переменные окружения

Создайте в корне файл `.env` на основе `.env.example` и заполните реальными значениями.

## 🐳 Запустите через Docker
Убедитесь, что установлены Docker и Docker Compose.

```bash
# 1. Клонировать репозиторий
git clone <repo-url>
cd <project-folder>

# 2. Скопировать .env.example в .env и заполнить (см. выше)
cp .env.example .env

# 3. Запустить все сервисы
docker-compose up --build
```

После запуска:
- API + Swagger: http://localhost:5000/swagger
- Frontend: http://localhost:5173

Для остановки напишите docker-compose down.

## 💻 Локальный запуск (без Docker)

### Бэкенд

1. Установите .NET 8 SDK и PostgreSQL.

2. Создайте базу данных libraries_db.

3. копируйте backend/.env.example → backend/.env, заполните правильные настройки подключения (хост, порт, пользователь, пароль, имя БД).

4. Установите инструмент EF Core:

```bash
dotnet tool install --global dotnet-ef
```

5. Примените миграции:

```bash
cd backend/LibrariesManagementSystem.Api
dotnet ef database update
```

6. Запустите приложение:

```bash
dotnet run
```

### Frontend

1. Установите Node.js 20+.

2. Скопируйте `frontend/.env.example` → `frontend/.env`, задайте `VITE_API_URL=http://localhost:5000/api`.

3. Установите зависимости и запустите:

```bash
cd frontend
npm install
nom run dev
```

## 👤 Тестовые учётные записи (создаются при первой миграции)
| Роль | Email | Пароль |
| :--: | :---: | :----: |
| Библиотекарь | librarian@lib.ru | Librarian1! |
| Читатель | reader@lib.ru | Reader1! |

## 📄 Лицензия

Этот проект является частной собственностью. Все права защищены.  
Использование, копирование, модификация или распространение кода без явного письменного разрешения автора запрещены.  
Подробнее см. файл [LICENSE](./LICENSE).
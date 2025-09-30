To-Do List (ASP.NET Core Web API + EF Core + SQLite)

Простое веб‑приложение для управления задачами (CRUD, фильтрация) с хранением в SQLite.

## Стек
- .NET 9, ASP.NET Core Web API
- Entity Framework Core (SQLite)
- Статический фронтенд (wwwroot/index.html)

## Быстрый старт
1) Клонируйте репозиторий
```bash
git clone <your-repo-url>
cd <repo-folder>
```
2) Перейдите в проект и создайте папку для базы данных
```bash
cd odo.ebpi
mkdir AppData
```
3) Запуск API (по умолчанию http://localhost:5080)
```bash
dotnet run --urls=http://localhost:5080
```
4) Открывайте:
- Фронтенд: http://localhost:5080/index.html
- Swagger (Dev): http://localhost:5080/swagger

## API (основные эндпоинты)
- GET /api/tasks — список задач (фильтр: ?completed=true|false)
- GET /api/tasks/{id} — получить задачу по Id
- POST /api/tasks — создать задачу
- PUT /api/tasks/{id} — обновить задачу
- PATCH /api/tasks/{id}/status — изменить статус (тело: true/false)
- DELETE /api/tasks/{id} — удалить задачу

## Конфигурация
Строка подключения задаётся в appsettings.json:
```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=AppData/todo.db"
}
```
База создаётся автоматически при старте через EnsureCreated().

## Скрипт SQL (эквивалент)
```sql
CREATE TABLE IF NOT EXISTS Tasks (
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  Title TEXT NOT NULL,
  Description TEXT,
  IsCompleted INTEGER NOT NULL DEFAULT 0,
  CreatedAt TEXT NOT NULL,
  DueDate TEXT
);
```

## Сборка релиза
```bash
dotnet build odo.ebpi/odo.ebpi.csproj -c Release
```

## Лицензия
MIT



=== backend/overview.md ===
📦 backend/Overview
📂 Назначение backend
Backend реализует REST API для системы управления задачами (TaskManager).
Система покрывает:


управление пользователями и ролями (RBAC)


управление задачами (CRUD + статусы + история)


комментарии


события (календарь)


отчёты и аналитика


аудит действий пользователей



🧱 Общая архитектура
Архитектурный стиль
Проект реализует упрощённую Clean Architecture:
Controllers (Presentation)        ↓Services (Application / Business Logic)        ↓Data (EF Core / Infrastructure)        ↓Database (PostgreSQL)
Дополнительно:
Models (Domain)DTOs (Contracts)Extensions (Mapping Layer)

🔄 End-to-End Flow
Пример: создание задачи
Client (frontend)   ↓ HTTP POST /api/tasksController (TasksController)   ↓TaskService.CreateAsync()   ↓AppDbContext (EF Core)   ↓PostgreSQLTaskService   ↓AuditService.LogAsync()TaskService   ↓MappingExtensions.ToDto()Controller   ↓HTTP 201 Created (TaskDto)

⚙️ Конфигурация приложения
📄 Program.cs
Главная точка входа приложения.

1. Подключение базы данных
builder.Services.AddDbContext<AppDbContext>(opt =>    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


используется PostgreSQL


ORM: Entity Framework Core



2. Dependency Injection
builder.Services.AddScoped<IUserService, UserService>();
Регистрируются сервисы:


IAuditService


IAuthService


IUserService


ITaskService


IReportService


IEventService


👉 lifetime: Scoped (на запрос)

3. JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
Параметры:


Issuer


Audience


SigningKey


👉 используется Symmetric Key (HMAC)

4. Authorization
builder.Services.AddAuthorization();
Используется:


[Authorize]


[Authorize(Roles = "...")]



5. Swagger (OpenAPI)
builder.Services.AddSwaggerGen(...)
Особенности:


поддержка JWT


UI доступен по /swagger



6. CORS
.WithOrigins(    "http://localhost:3000",    "http://localhost:5173",    "http://frontend")
👉 разрешён доступ frontend-приложениям

⚠️ Критическое поведение при старте
❗ Полная пересоздача БД
await db.Database.EnsureDeletedAsync();await db.Database.EnsureCreatedAsync();

Последствия:


❌ ВСЕ данные удаляются при каждом запуске


❌ не используется миграции


❌ невозможно использовать в production



Затем:
await DbSeeder.SeedAsync(db);
👉 база заполняется начальными данными

📌 Вывод:
Это:


подходит для разработки / демо


НЕ подходит для production



🐳 Docker
Dockerfile
Многоступенчатая сборка:


build stage


restore


build


publish




runtime stage


ASP.NET runtime


запуск приложения




ENTRYPOINT ["dotnet", "TaskManager.API.dll"]

Особенности:


порт: 8080


production-ready image


минимальный размер (без SDK)



⚙️ Конфигурация (appsettings.json)

1. Connection String
"Host=localhost;Port=5432;Database=taskmanager"


PostgreSQL


user: postgres



2. JWT
"Key": "TaskManager_SuperSecret_Key..."
👉 используется для подписи токенов

3. Logging
"Microsoft.EntityFrameworkCore.Database.Command": "Warning"
👉 SQL-запросы не логируются подробно

📦 Зависимости (.csproj)

Основные библиотеки:
EF Core


Microsoft.EntityFrameworkCore


Npgsql.EntityFrameworkCore.PostgreSQL



Auth


Microsoft.AspNetCore.Authentication.JwtBearer


System.IdentityModel.Tokens.Jwt



Security


BCrypt.Net-Next


👉 используется для хеширования паролей

Swagger


Swashbuckle.AspNetCore



🔗 Связи между слоями
Controllers   ↓Services   ↓DbContext   ↓DatabaseServices   ↓MappingExtensions   ↓DTOs

💡 Ключевые архитектурные особенности

1. Без Repository слоя
Service → DbContext
👉 упрощённая архитектура

2. Mapping через Extension методы
Entity → ToDto()
👉 альтернатива AutoMapper

3. Явный AuditService
Service → AuditService
👉 аудит встроен в бизнес-логику

4. Role-based security


JWT + Roles


проверка:


на уровне контроллеров


внутри сервисов





5. DTO-first API


чёткое разделение:


Request


Response


Brief


Detail





⚠️ Архитектурные проблемы

1. ❗ Пересоздание БД при старте
EnsureDeleted + EnsureCreated
👉 критично для production

2. Нет миграций
👉 нельзя управлять схемой БД

3. Нет Repository слоя
👉 сильная связность сервисов с EF

4. Бизнес-логика частично в Mapping
IsOverdueStatusName comparisons

5. Строковые статусы
"Выполнена"
👉 риск ошибок

6. Нет централизованного error handling
👉 отсутствует middleware

7. Нет кэширования
👉 отчёты пересчитываются каждый раз

📌 Итог
Backend представляет собой:


хорошо структурированную систему


с понятной логикой


с упрощённой архитектурой


Подходит для:


MVP


корпоративного внутреннего инструмента


Требует доработки для:


production


масштабирования




=== PROJECT_STRUCTURE.md ===
📦 PROJECT STRUCTURE
📁 Полное дерево проекта (backend)
backend/│├── Controllers/│   ├── AuthController.cs│   ├── EventsController.cs│   ├── ReportsController.cs│   ├── TasksController.cs│   └── UsersController.cs│├── Models/│   ├── User.cs│   ├── Role.cs│   ├── Department.cs│   ├── TaskItem.cs│   ├── Status.cs│   ├── Comment.cs│   ├── TaskHistory.cs│   ├── Event.cs│   ├── Report.cs│   └── AuditLog.cs│├── DTOs/│   └── (все DTO-контракты)│├── Services/│   ├── AuthService.cs│   ├── UserService.cs│   ├── TaskService.cs│   ├── EventService.cs│   ├── ReportService.cs│   └── AuditService.cs│├── Data/│   ├── AppDbContext.cs│   └── DbSeeder.cs│├── Extensions/│   └── MappingExtensions.cs│├── Program.cs├── appsettings.json├── Dockerfile└── TaskManager.API.csproj

📂 Уровни архитектуры

1. Domain Layer (ядро)
Models/
Содержит:


сущности системы


связи между ними



2. Contract Layer
DTOs/
Содержит:


входные и выходные модели API



3. Application Layer
Services/
Содержит:


бизнес-логику


use-case'ы



4. Mapping Layer
Extensions/
Содержит:


преобразование Entity → DTO



5. Infrastructure Layer
Data/
Содержит:


DbContext


доступ к БД


seed



6. Presentation Layer
Controllers/
Содержит:


API endpoints



7. Composition Root
Program.cs
Содержит:


DI


конфигурацию


middleware



🔄 Полный поток данных
[ Client (Frontend) ]        ↓ HTTP[ Controllers ]        ↓[ Services ]        ↓[ DbContext (EF Core) ]        ↓[ PostgreSQL ]        ↑[ Services ]        ↓[ MappingExtensions ]        ↓[ DTO ]        ↓[ Controllers ]        ↓ JSON[ Client ]

📌 Как всё связано

1. Controllers


принимают HTTP


вызывают Services



2. Services


реализуют бизнес-логику


работают с DbContext


вызывают AuditService


используют MappingExtensions



3. DbContext


управляет БД


хранит DbSet



4. MappingExtensions


преобразуют Entities → DTO



5. DTOs


формируют API ответы



📌 Ключевые особенности проекта


EF Core без Repository


ручной mapping через extension


JWT авторизация


RBAC (roles)


аудит действий


аналитика (reports)
# Руководство по добавлению системы виртуализации

## Изменение исходных проектов

### Moxcontrol.Models

В данном проекте требуется:

- Добавить новую систему виртуализации в перечисление: `Entities/Enums/VirtualizationSystem.cs`

### MoxControl.Factory

В данном проекте требуется:

- Зарегистрировать в `ConnectServiceFactory.cs` новый сервис с соотвествующей системой виртуализации

### MoxControl.DependencyInjection

В данном проекте требуется:

- Зарегистрировать контекст базы данных для новой системы виртуализации. Пример: `serviceCollection.RegisterConnectProxmoxContext(connectionString);`
- Зарегистрировать сервисы для новой системы виртуализации. Пример: `serviceCollection.RegisterConnectProxmoxServices();`

### MoxControl.Data

Вносить изменения в проект не требуется.

### MoxControl.Connect

Вносить изменения в проект не требуется.

## Добавление новых проектов

### MoxControl.Connect."VirtualizationSystem"

В данном проекте потребуется:

- Реализовать интерфейсы: `IConnectService`, `IMachineService`, `IServerService`
- Реализовать опциональный контроллер и представления для настройки сервера

### MoxControl.Connect."VirtualizationSystem".Data

В данном проекте потребуется:

- Реализовать контекст базы данных для новой системы виртуализации

### MoxControl.Connect."VirtualizationSystem".Models

В данном проекте потребуется:

- Реализовать модель таблицы серверов путем наследования от модели `BaseServer`
- Реализовать модель таблицы виртуальных машин путем наследования от модели `BaseMachine`
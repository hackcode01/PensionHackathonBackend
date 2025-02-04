# Команда рИИбки. Backend-часть

## Инструкция по развертыванию ASP.NET приложения

API развернуто по [адресу](http://147.45.110.199/swagger/index.html)

### Предварительные требования

- **.NET SDK** (версии 8.0)
- **База данных SQLite**, расположенная в папке ~/AppData

### Шаг 1: Сборка приложения

1. Откройте терминал в корневой директории вашего проекта.
2. Выполните команду для сборки приложения в конфигурации `Release`:
```bash
   dotnet build --configuration Release
```
3. Опубликуйте приложение в папку publish, чтобы подготовить файлы для деплоя
```bash
    dotnet publish --configuration Release --output ./publish
```

### Шаг 2: Запуск приложения
1. Запустите приложение
```bash
    dotnet ./publish/PensionHackathonBackend.dll
```


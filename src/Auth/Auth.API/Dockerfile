# Базовый образ для запуска ASP.NET приложений
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001
EXPOSE 5002

# Установка supervisord
RUN apt-get update && apt-get install -y supervisor && apt-get clean

# Сборка всех сервисов
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src

# Копируем проекты
COPY ["src/Auth/Auth.API/Auth.API.csproj", "src/Auth/Auth.API/"]
COPY ["src/Events/Events.API/Events.API.csproj", "src/Events/Events.API/"]
COPY ["src/IntermediateService/EventIntermediate/EventIntermediate.API/EventIntermediate.API.csproj", "src/IntermediateService/EventIntermediate/EventIntermediate.API/"]

# Восстанавливаем зависимости
RUN dotnet restore "src/Auth/Auth.API/Auth.API.csproj"

# Копируем остальной код
COPY . .

# Сборка и публикация Auth.API
WORKDIR "/src/src/Auth/Auth.API"
RUN dotnet publish "Auth.API.csproj" -c $BUILD_CONFIGURATION -o /app/auth /p:UseAppHost=false

# Сборка и публикация Events.API
WORKDIR "/src/src/Events/Events.API"
RUN dotnet publish "Events.API.csproj" -c $BUILD_CONFIGURATION -o /app/events /p:UseAppHost=false

# Сборка и публикация EventIntermediate.API
WORKDIR "/src/src/IntermediateService/EventIntermediate/EventIntermediate.API"
RUN dotnet publish "EventIntermediate.API.csproj" -c $BUILD_CONFIGURATION -o /app/eventintermediate /p:UseAppHost=false

# Финальный образ
FROM base AS final
WORKDIR /app

# Копируем опубликованные приложения
COPY --from=build /app/auth ./auth
COPY --from=build /app/events ./events
COPY --from=build /app/eventintermediate ./eventintermediate

# Настройка supervisord
COPY supervisord.conf /etc/supervisor/conf.d/supervisord.conf

# Запуск supervisord
CMD ["supervisord", "-c", "/etc/supervisor/conf.d/supervisord.conf"]

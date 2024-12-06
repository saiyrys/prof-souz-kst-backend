# Этап для базового ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Этап для сборки
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src

# Сборка первого микросервиса (Events.API)
COPY ["src/Events/Events.API/Events.API.csproj", "src/Events/Events.API/"]
RUN dotnet restore "./src/Events/Events.API/Events.API.csproj"

# Сборка второго микросервиса (EventIntermediate.API)
COPY ["src/IntermediateService/EventIntermediate/EventIntermediate.API/EventIntermediate.API.csproj", "src/IntermediateService/EventIntermediate/EventIntermediate.API/"]
RUN dotnet restore "./src/IntermediateService/EventIntermediate/EventIntermediate.API/EventIntermediate.API.csproj"

# Копирование исходников и сборка
COPY . .
WORKDIR "/src/src/Events/Events.API"
RUN dotnet build "./Events.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

WORKDIR "/src/src/IntermediateService/EventIntermediate/EventIntermediate.API"
RUN dotnet build "./EventIntermediate.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этап публикации
FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
WORKDIR "/src/src/Events/Events.API"
RUN dotnet publish "./Events.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

WORKDIR "/src/src/IntermediateService/EventIntermediate/EventIntermediate.API"
RUN dotnet publish "./EventIntermediate.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этап запуска
FROM base AS final
WORKDIR /app

# Для Events.API
COPY --from=publish /app/publish .

# Для EventIntermediate.API (если потребуется раздельный запуск)
# COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Events.API.dll"]

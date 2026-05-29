FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем csproj и восстанавливаем зависимости
COPY ["LibrariesManagementSystem.Api/LibrariesManagementSystem.Api.csproj", "LibrariesManagementSystem.Api/"]
RUN dotnet restore "LibrariesManagementSystem.Api/LibrariesManagementSystem.Api.csproj"

# Копируем остальные файлы и собираем
COPY . .
WORKDIR "/src/LibrariesManagementSystem.Api"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 5000

# Копируем опубликованное приложение
COPY --from=build /app/publish .

# Запускаем (миграции применятся автоматически при старте)
ENTRYPOINT ["dotnet", "LibrariesManagementSystem.Api.dll"]
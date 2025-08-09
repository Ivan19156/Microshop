# Вибираємо базовий SDK-образ для збірки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо csproj та відновлюємо залежності
COPY ["FileService.csproj", "./"]
RUN dotnet restore "./FileService.csproj"

# Копіюємо решту коду і збираємо реліз
COPY . .
RUN dotnet publish "FileService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Фінальний образ для запуску
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Виставляємо порт, якщо треба (звичайно 80)
EXPOSE 5005

# Команда запуску
ENTRYPOINT ["dotnet", "FileService.dll"]

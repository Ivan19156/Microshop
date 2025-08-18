# Етап збірки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо csproj-файли
COPY ["NotificationService/NotificationService.csproj", "NotificationService/"]
COPY ["Microshop.Contracts/Microshop.Contracts.csproj", "Microshop.Contracts/"]

# Відновлюємо залежності
RUN dotnet restore "NotificationService/NotificationService.csproj"

# Копіюємо весь код
COPY . .

# Публікуємо
RUN dotnet publish "NotificationService/NotificationService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Етап рантайму
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "NotificationService.dll"]

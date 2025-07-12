# === Build stage ===
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо csproj та restore
COPY Gateway.Api.csproj .
RUN dotnet restore

# Копіюємо решту файлів і публікуємо
COPY . .
RUN dotnet publish -c Release -o /app/publish

# === Runtime stage ===
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

# Вказуємо порт за замовчуванням
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "Gateway.Api.dll"]

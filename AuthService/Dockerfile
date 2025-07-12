# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо csproj і відновлюємо залежності
COPY AuthService.csproj ./
RUN dotnet restore AuthService.csproj

# Копіюємо увесь код
COPY . ./

# Публікуємо релізну збірку (Production-ready)
RUN dotnet publish AuthService.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Копіюємо зібраний додаток з попереднього кроку
COPY --from=build /app/publish ./

# Виставляємо порт (той, що слухає додаток)
EXPOSE 5000

# Запускаємо додаток
ENTRYPOINT ["dotnet", "AuthService.dll"]

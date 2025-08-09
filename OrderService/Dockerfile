FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


COPY ["OrderService.csproj", "./"]
RUN dotnet restore "./OrderService.csproj"


COPY . .

RUN dotnet publish "OrderService.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5010
EXPOSE 5010

ENTRYPOINT ["dotnet", "OrderService.dll"]


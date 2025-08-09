FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["OrchestratorService.csproj", "./"]
RUN dotnet restore "./OrchestratorService.csproj"


COPY . .

RUN dotnet publish "OrchestratorService.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5015
EXPOSE 5015

ENTRYPOINT ["dotnet", "OrchestratorService.dll"]


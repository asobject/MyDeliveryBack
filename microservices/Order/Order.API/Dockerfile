# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["microservices/Order/Order.API/Order.API.csproj", "microservices/Order/Order.API/"]
COPY ["microservices/Order/Infrastructure/Infrastructure.csproj", "microservices/Order/Infrastructure/"]
COPY ["microservices/Order/Application/Application.csproj", "microservices/Order/Application/"]
COPY ["buildingBlocks/BuildingBlocks/BuildingBlocks.csproj", "buildingBlocks/BuildingBlocks/"]
COPY ["microservices/Order/Domain/Domain.csproj", "microservices/Order/Domain/"]
RUN dotnet restore "./microservices/Order/Order.API/Order.API.csproj"
COPY . .
WORKDIR "/src/microservices/Order/Order.API"
RUN dotnet build "./Order.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Order.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.API.dll"]
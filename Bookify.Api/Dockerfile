FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /
COPY ["Bookify.Api/Bookify.Api.csproj", "Bookify.Api/"]
COPY ["Bookify.Application/Bookify.Application.csproj", "Bookify.Application/"]
COPY ["Bookify.Domain/Bookify.Domain.csproj", "Bookify.Domain/"]
COPY ["Bookify.Infrastructure/Bookify.Infrastructure.csproj", "Bookify.Infrastructure/"]
RUN dotnet restore "./Bookify.Api/./Bookify.Api.csproj"
COPY . .
WORKDIR "Bookify.Api"
RUN dotnet build "./Bookify.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Bookify.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bookify.Api.dll"]
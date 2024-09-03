FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AuthenticationServer.Api/AuthenticationServer.Api.csproj", "AuthenticationServer.Api/"]
COPY ["gp-backend.Api/gp-backend.Api.csproj", "gp-backend.Api/"]
COPY ["gp-backend.Core/gp-backend.Core.csproj", "gp-backend.Core/"]
COPY ["gp-backend.EF/gp-backend.EF.csproj", "gp-backend.EF/"]
RUN dotnet restore "AuthenticationServer.Api/AuthenticationServer.Api.csproj"
RUN dotnet restore "gp-backend.Api/gp-backend.Api.csproj"
COPY . .
WORKDIR "/src/gp-backend.Api"
RUN dotnet build "gp-backend.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "gp-backend.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "gp-backend.Api.dll"]
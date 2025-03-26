FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ENV ASPNETCORE_URLS=http://+:8080;http://+:8081
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

ARG CONNECTION_STRING
ARG JWT_SECRET
ARG JWT_ISSUER
ARG JWT_AUDIENCE

# Copy solution and project files
COPY ["SriSaiAPI.sln", "."]
COPY ["SriSai.API/SriSai.API.csproj", "SriSai.API/"]
COPY ["SriSai.Application/SriSai.Application.csproj", "SriSai.Application/"]
COPY ["SriSai.Domain/SriSai.Domain.csproj", "SriSai.Domain/"]
COPY ["SriSai.infrastructure/SriSai.infrastructure.csproj", "SriSai.infrastructure/"]

# Restore dependencies
RUN dotnet restore "SriSai.API/SriSai.API.csproj"

# Copy the rest of the code
COPY ["SriSai.API/", "SriSai.API/"]
COPY ["SriSai.Application/", "SriSai.Application/"]
COPY ["SriSai.Domain/", "SriSai.Domain/"]
COPY ["SriSai.infrastructure/", "SriSai.infrastructure/"]

WORKDIR "/src/SriSai.API"

# Build the application
RUN dotnet build "SriSai.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SriSai.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

# Pass build arguments to final stage
ARG CONNECTION_STRING
ARG JWT_SECRET
ARG JWT_ISSUER
ARG JWT_AUDIENCE

# Copy published files except appsettings.json
COPY --from=publish /app/publish .

# Handle appsettings.json configuration
COPY ["SriSai.API/appsettings.json", "./appsettings.json"]

# Configure appsettings.json
RUN set -e && \
    # Install jq
    apt-get update && \
    apt-get install -y jq && \
    # Debug: Show current directory and files
    pwd && ls -la && \
    # Debug: Show original appsettings.json content
    echo "Original appsettings.json:" && \
    cat appsettings.json && \
    # Create config with jq
    echo '{ \
      "Jwt": { \
        "Secret": "'$JWT_SECRET'", \
        "Issuer": "'$JWT_ISSUER'", \
        "Audience": "'$JWT_AUDIENCE'", \
        "ExpiryHours": 1 \
      }, \
      "Logging": { \
        "LogLevel": { \
          "Default": "Information", \
          "Microsoft.AspNetCore": "Warning" \
        } \
      }, \
      "ConnectionStrings": { \
        "DefaultConnection": "'$CONNECTION_STRING'" \
      }, \
      "AllowedHosts": "*", \
      "DatabaseSettings": { \
        "MaxRetryCount": 3, \
        "EnableDetailedErrors": false, \
        "EnableSensitiveDataLogging": false \
      } \
    }' > appsettings.json && \
    # Verify the JSON is valid
    echo "Verifying JSON validity..." && \
    jq empty appsettings.json && \
    # Show final config (excluding sensitive data)
    echo "Final configuration (excluding sensitive data):" && \
    jq 'del(.Jwt.Secret) | del(.ConnectionStrings)' appsettings.json

ENTRYPOINT ["dotnet", "SriSai.API.dll"]

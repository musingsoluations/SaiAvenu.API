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
RUN if [ ! -f "appsettings.json" ]; then \
    echo "Error: appsettings.json not found!" && exit 1; \
    fi

# Install jq and configure appsettings.json
RUN apt-get update && apt-get install -y jq && \
    # Create a backup of original appsettings.json
    cp appsettings.json appsettings.json.bak && \
    # Use jq to replace placeholders while maintaining JSON structure
    jq --arg conn "$CONNECTION_STRING" \
       --arg secret "$JWT_SECRET" \
       --arg issuer "$JWT_ISSUER" \
       --arg audience "$JWT_AUDIENCE" \
       '.Jwt.Secret = $secret | .Jwt.Issuer = $issuer | .Jwt.Audience = $audience | .ConnectionStrings.DefaultConnection = $conn' \
       appsettings.json.bak > temp.json && \
    mv temp.json appsettings.json && \
    # Verify JSON is valid and show non-sensitive parts
    jq empty appsettings.json && \
    echo "Final configuration (excluding sensitive data):" && \
    jq 'del(.Jwt.Secret) | del(.ConnectionStrings)' appsettings.json

ENTRYPOINT ["dotnet", "SriSai.API.dll"]

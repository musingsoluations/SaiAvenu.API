FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
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
ARG WHATAAPP_BASEURL
ARG WHATSAPP_APIKEY
ARG WHATSAPP_SENDERID

# Install jq in build stage
RUN apt-get update && \
    apt-get install -y --no-install-recommends jq && \
    rm -rf /var/lib/apt/lists/*

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
ARG CONNECTION_STRING
ARG JWT_SECRET
ARG JWT_ISSUER
ARG JWT_AUDIENCE
ARG WHATAAPP_BASEURL
ARG WHATSAPP_APIKEY
ARG WHATSAPP_SENDERID

# Install jq in publish stage
RUN apt-get update && \
    apt-get install -y --no-install-recommends jq && \
    rm -rf /var/lib/apt/lists/*

# Configure appsettings.json before publishing
COPY ["SriSai.API/appsettings.json", "./appsettings.json"]
RUN jq -n \
    --arg secret "$JWT_SECRET" \
    --arg issuer "$JWT_ISSUER" \
    --arg audience "$JWT_AUDIENCE" \
    --arg connStr "$CONNECTION_STRING" \
    --arg baseUrl "$WHATAAPP_BASEURL" \
    --arg apiKey "$WHATSAPP_APIKEY" \
    --arg senderId "$WHATSAPP_SENDERID" \
    '{"Jwt":{"Secret":$secret,"Issuer":$issuer,"Audience":$audience,"ExpiryHours":1},"Logging":{"LogLevel":{"Default":"Information","Microsoft.AspNetCore":"Warning"}},"ConnectionStrings":{"DefaultConnection":$connStr},"AllowedHosts":"*","DatabaseSettings":{"MaxRetryCount":3,"EnableDetailedErrors":false,"EnableSensitiveDataLogging":false},"WhatsApp":{"BaseUrl":$baseUrl,"SenderId":$senderId,"ApiKey":$apiKey}}' > appsettings.json && \
    echo "Verifying JSON validity..." && \
    jq empty appsettings.json && \
    echo "Final configuration (excluding sensitive data):" && \
    jq 'del(.ConnectionStrings, .Jwt.Secret)' appsettings.json

# Publish the application
RUN dotnet publish "SriSai.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "SriSai.API.dll"]

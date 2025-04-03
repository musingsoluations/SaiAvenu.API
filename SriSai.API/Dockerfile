FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Install ICU libraries for globalization support
RUN apk add --no-cache icu-libs

# Set globalization invariant mode to false to use ICU
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

ARG CONNECTION_STRING
ARG JWT_SECRET
ARG JWT_ISSUER
ARG JWT_AUDIENCE
ARG WHATAAPP_BASEURL
ARG WHATSAPP_APIKEY
ARG WHATSAPP_SENDERID
ARG MW_SERVER_URL
ARG MW_APIKEY
ARG MV_APP_NAME

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
ARG MW_SERVER_URL
ARG MW_APIKEY
ARG MV_APP_NAME

# Install gettext for envsubst (Alpine version)
RUN apk add --no-cache gettext

# Set environment variables for substitution
ENV JWT_SECRET=$JWT_SECRET
ENV JWT_ISSUER=$JWT_ISSUER
ENV JWT_AUDIENCE=$JWT_AUDIENCE
ENV CONNECTION_STRING=$CONNECTION_STRING
ENV WHATAAPP_BASEURL=$WHATAAPP_BASEURL
ENV WHATSAPP_APIKEY=$WHATSAPP_APIKEY
ENV WHATSAPP_SENDERID=$WHATSAPP_SENDERID
ENV MW_SERVER_URL=$MW_SERVER_URL
ENV MW_APIKEY=$MW_APIKEY
ENV MV_APP_NAME=$MV_APP_NAME

# Replace placeholders in appsettings.json
RUN envsubst < appsettings.json > appsettings.tmp.json && \
    mv appsettings.tmp.json appsettings.json && \
    echo "Updated appsettings.json (excluding sensitive data):" && \
    cat appsettings.json | grep -v "Secret" | grep -v "ApiKey" | grep -v "ConnectionString"

# Publish the application
RUN dotnet publish "SriSai.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Install SQL Server client dependencies for Alpine
RUN apk add --no-cache \
    libc6-compat \
    libstdc++ \
    libintl

# Create a non-root user
RUN adduser -D -H -u 1000 appuser && chown -R appuser /app
USER appuser
ENTRYPOINT ["dotnet", "SriSai.API.dll"]
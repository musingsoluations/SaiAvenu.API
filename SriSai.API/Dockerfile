# Use lightweight .NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0-chiseled AS base
WORKDIR /app
EXPOSE 8080 8081
ENV ASPNETCORE_URLS=http://+:8080;http://+:8081
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_HTTP_PORT=8080

# Build stage using Alpine-based .NET SDK
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
COPY . .

WORKDIR "/src/SriSai.API"

# Build the application (no unnecessary debugging symbols)
RUN dotnet build "SriSai.API.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release

# Publish the app with trimming and without apphost to reduce size
RUN dotnet publish "SriSai.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore /p:PublishTrimmed=true /p:PublishSingleFile=false /p:EnableCompressionInSingleFile=true /p:SelfContained=false

# Final runtime stage with minimal image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SriSai.API.dll"]
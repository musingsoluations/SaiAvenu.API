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

# Ensure appsettings.json exists and update configuration
COPY ["SriSai.API/appsettings.json", "/src/SriSai.API/appsettings.json"]
RUN if [ ! -f "appsettings.json" ]; then \
    echo "Error: appsettings.json not found!" && exit 1; \
    fi && \
    sed -i \
    -e "s|#{CONNECTION_STRING}#|${CONNECTION_STRING}|g" \
    -e "s|#{JWT_SECRET}#|${JWT_SECRET}|g" \
    -e "s|#{JWT_ISSUER}#|${JWT_ISSUER}|g" \
    -e "s|#{JWT_AUDIENCE}#|${JWT_AUDIENCE}|g" \
    appsettings.json && \
    cat appsettings.json | grep -v "Secret\|Connection" # Log config without sensitive data

# Build the application
RUN dotnet build "SriSai.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SriSai.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
# Ensure all files including appsettings.json are copied from publish
COPY --from=publish /app/publish .
# Verify appsettings.json exists in final image
RUN if [ ! -f "appsettings.json" ]; then \
    echo "Error: appsettings.json not found in final image!" && exit 1; \
    fi
ENTRYPOINT ["dotnet", "SriSai.API.dll"]

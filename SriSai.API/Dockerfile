FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ENV ASPNETCORE_URLS=http://+:8080;http://+:8081
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG CONNECTION_STRING
ARG JWT_SECRET
ARG JWT_ISSUER
ARG JWT_AUDIENCE
WORKDIR /src
ENV ConnectionStrings__DefaultConnection=$CONNECTION_STRING
ENV Jwt__Secret=$JWT_SECRET
ENV Jwt__Issuer=$JWT_ISSUER
ENV Jwt__Audience=$JWT_AUDIENCE
COPY ["SriSai.API/SriSai.API.csproj", "SriSai.API/"]
COPY ["SriSai.Application/SriSai.Application.csproj", "SriSai.Application/"]
COPY ["SriSai.Domain/SriSai.Domain.csproj", "SriSai.Domain/"]
COPY ["SriSai.infrastructure/SriSai.infrastructure.csproj", "SriSai.Infrastructure/"]
RUN dotnet restore "SriSai.API/SriSai.API.csproj"
COPY ../.. .
WORKDIR "/src/SriSai.API"

# Safely replace placeholders in appsettings.json (handles @ symbols)
RUN test -f /src/SriSai.API/appsettings.json && \
    sed -i \
    -e "s|#{CONNECTION_STRING}#|$(printf '%s' "$CONNECTION_STRING" | sed -e 's/[\/&\\|]/\\&/g')|g" \
    -e "s|#{JWT_SECRET}#|$(printf '%s' "$JWT_SECRET" | sed -e 's/[\/&\\|]/\\&/g')|g" \
    -e "s|#{JWT_ISSUER}#|$(printf '%s' "$JWT_ISSUER" | sed -e 's/[\/&\\|]/\\&/g')|g" \
    -e "s|#{JWT_AUDIENCE}#|$(printf '%s' "$JWT_AUDIENCE" | sed -e 's/[\/&\\|]/\\&/g')|g" \
    /src/SriSai.API/appsettings.json || \
    echo "appsettings.json NOT FOUND - Skipping sed replacements"

RUN dotnet build "SriSai.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SriSai.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SriSai.API.dll"]

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY SriSai.API/SriSai.API.csproj SriSai.API/
COPY SriSai.Application/SriSai.Application.csproj SriSai.Application/
COPY SriSai.Domain/SriSai.Domain.csproj SriSai.Domain/
COPY SriSai.infrastructure/SriSai.infrastructure.csproj SriSai.infrastructure/
RUN dotnet restore "SriSai.API/SriSai.API.csproj"

COPY . .
WORKDIR "/src/SriSai.API"
RUN dotnet build "SriSai.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "SriSai.API.csproj" -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SriSai.API.dll"]
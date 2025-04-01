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
ARG MW_SERVER_URL
ARG MW_APIKEY
ARG MV_APP_NAME

# Install jq in build stage
RUN apk add --no-cache jq

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

# Install jq in publish stage
RUN apk add --no-cache jq

# Replace placeholders in appsettings.json
RUN sed -i \
    -e "s/#{JWT_SECRET}#/$JWT_SECRET/g" \
    -e "s/#{JWT_ISSUER}#/$JWT_ISSUER/g" \
    -e "s/#{JWT_AUDIENCE}#/$JWT_AUDIENCE/g" \
    -e "s/#{CONNECTION_STRING}#/$CONNECTION_STRING/g" \
    -e "s/#{WHATAAPP_BASEURL}#/$WHATAAPP_BASEURL/g" \
    -e "s/#{WHATSAPP_APIKEY}#/$WHATSAPP_APIKEY/g" \
    -e "s/#{WHATSAPP_SENDERID}#/$WHATSAPP_SENDERID/g" \
    -e "s/#{MW_SERVER_URL}#/$MW_SERVER_URL/g" \
    -e "s/#{MW_APIKEY}#/$MW_APIKEY/g" \
    -e "s/#{MV_APP_NAME}#/$MV_APP_NAME/g" \
    appsettings.json

# Publish the application
RUN dotnet publish "SriSai.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "SriSai.API.dll"]
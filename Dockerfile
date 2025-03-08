# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the necessary project files for dependency resolution
COPY SriSai.API/SriSai.API.csproj SriSai.API/
COPY SriSai.Application/SriSai.Application.csproj SriSai.Application/
COPY SriSai.Domain/SriSai.Domain.csproj SriSai.Domain/
COPY SriSai.infrastructure/SriSai.infrastructure.csproj SriSai.infrastructure/
RUN dotnet restore "SriSai.API/SriSai.API.csproj"

# **Explicitly copy appsettings.json before changing WORKDIR**
# Ensure appsettings.json is copied to the right place
COPY SriSai.API/appsettings.json /src/SriSai.API/appsettings.json

# Copy the entire project source code
COPY . .

# Ensure appsettings.json is copied before modifying it
WORKDIR "/src/SriSai.API"

# Verify if the file exists (for debugging)
RUN ls -l /src/SriSai.API/appsettings.json || echo "appsettings.json NOT FOUND"

# Define all environment variables
ARG CONNECTION_STRING
ARG JWT_SECRET
ARG JWT_ISSUER
ARG JWT_AUDIENCE

# Debugging step - Print environment variables (optional)
RUN echo "CONNECTION_STRING=$CONNECTION_STRING" && \
    echo "JWT_SECRET=$JWT_SECRET" && \
    echo "JWT_ISSUER=$JWT_ISSUER" && \
    echo "JWT_AUDIENCE=$JWT_AUDIENCE"

# Safely replace placeholders in appsettings.json
RUN test -f appsettings.json && \
    sed -i "s@#{CONNECTION_STRING}#@$CONNECTION_STRING@g" appsettings.json && \
    sed -i "s@#{JWT_SECRET}#@$JWT_SECRET@g" appsettings.json && \
    sed -i "s@#{JWT_ISSUER}#@$JWT_ISSUER@g" appsettings.json && \
    sed -i "s@#{JWT_AUDIENCE}#@$JWT_AUDIENCE@g" appsettings.json || \
    echo "appsettings.json NOT FOUND - Skipping sed replacements"

# Build the application
RUN dotnet build "SriSai.API.csproj" -c Release -o /app/build
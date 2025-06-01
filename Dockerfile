# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY RenzGrandWeddingblazor.ph.csproj ./
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Install Node.js (for npm dependencies)
RUN apt-get update && apt-get install -y curl && \
    curl -fsSL https://deb.nodesource.com/setup_20.x | bash - && \
    apt-get install -y nodejs && \
    npm install

# Build the .NET app
RUN dotnet publish -c Release -o out

# Stage 2: Serve
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Expose port 80
EXPOSE 80

# Set the entrypoint
ENTRYPOINT ["dotnet", "RenzGrandWeddingblazor.ph.dll"] 
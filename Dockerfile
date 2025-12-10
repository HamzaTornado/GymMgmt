# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the csproj files first to cache the restore (optimization step)
# Note: We maintain the folder structure 'src/ProjectName'
COPY ["src/GymMgmt.Api/GymMgmt.Api.csproj", "src/GymMgmt.Api/"]
COPY ["src/GymMgmt.Application/GymMgmt.Application.csproj", "src/GymMgmt.Application/"]
COPY ["src/GymMgmt.Domain/GymMgmt.Domain.csproj", "src/GymMgmt.Domain/"]
COPY ["src/GymMgmt.Infrastructure/GymMgmt.Infrastructure.csproj", "src/GymMgmt.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/GymMgmt.Api/GymMgmt.Api.csproj"

# Copy the rest of the source code
COPY . .

# Build and Publish the API
WORKDIR "/src/src/GymMgmt.Api"
RUN dotnet publish "GymMgmt.Api.csproj" -c Release -o /app/publish

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GymMgmt.Api.dll"]
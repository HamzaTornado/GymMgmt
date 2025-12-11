# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# FIX: Removed "src/" prefix because the folders are at the root
COPY ["GymMgmt.Api/GymMgmt.Api.csproj", "GymMgmt.Api/"]
COPY ["GymMgmt.Application/GymMgmt.Application.csproj", "GymMgmt.Application/"]
COPY ["GymMgmt.Domain/GymMgmt.Domain.csproj", "GymMgmt.Domain/"]
COPY ["GymMgmt.Infrastructure/GymMgmt.Infrastructure.csproj", "GymMgmt.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "GymMgmt.Api/GymMgmt.Api.csproj"

# Copy the rest of the source code
COPY . .

# Build and Publish
WORKDIR "/src/GymMgmt.Api"
RUN dotnet publish "GymMgmt.Api.csproj" -c Release -o /app/publish

# Stage 2: Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GymMgmt.Api.dll"]
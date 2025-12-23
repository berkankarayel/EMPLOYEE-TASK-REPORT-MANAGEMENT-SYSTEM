# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Solution ve tüm proje dosyalarını kopyala
COPY ["EMPLOYEE TASK & REPORT MANAGEMENT SYSTEM.sln", "./"]
COPY ["EmployeeTaskManagement.API/EmployeeTaskManagement.API.csproj", "EmployeeTaskManagement.API/"]
COPY ["EmployeeTaskManagement.Application/EmployeeTaskManagement.Application.csproj", "EmployeeTaskManagement.Application/"]
COPY ["EmployeeTaskManagement.Infrastructure/EmployeeTaskManagement.Infrastructure.csproj", "EmployeeTaskManagement.Infrastructure/"]
COPY ["EmployeeTaskManagement.Domain/EmployeeTaskManagement.Domain.csproj", "EmployeeTaskManagement.Domain/"]

# NuGet paketlerini restore et
RUN dotnet restore "EmployeeTaskManagement.API/EmployeeTaskManagement.API.csproj"

# Tüm kaynak kodları kopyala
COPY . .

# API projesini build et ve publish et
WORKDIR /src/EmployeeTaskManagement.API
RUN dotnet publish "EmployeeTaskManagement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Build stage'den publish edilen dosyaları kopyala
COPY --from=build /app/publish .

# Port açıklama
EXPOSE 8080
EXPOSE 8081

# Uygulamayı başlat
ENTRYPOINT ["dotnet", "EmployeeTaskManagement.API.dll"]
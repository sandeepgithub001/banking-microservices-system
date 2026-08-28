# ============================================================
# MicroBank - Start All Backend Services
# Opens each service in its own terminal window.
# Startup order matters: ServiceRegistry (Consul) first,
# then ConfigService, then the business services, then the gateway.
# ============================================================

$root = $PSScriptRoot

Write-Host "Starting MicroBank backend services..." -ForegroundColor Cyan

# 1. Service Registry (Consul-like) - port 8500 (start first, others register with it)
Start-Process powershell -ArgumentList "-NoExit", "-Command", "Set-Location '$root\src\MicroBank.ServiceRegistry'; dotnet run" -WindowStyle Normal
Write-Host "  [1/5] ServiceRegistry   -> http://localhost:8500" -ForegroundColor Green

# 2. Config Service - port 5000
Start-Process powershell -ArgumentList "-NoExit", "-Command", "Set-Location '$root\src\MicroBank.ConfigService'; dotnet run" -WindowStyle Normal
Write-Host "  [2/5] ConfigService     -> http://localhost:5000" -ForegroundColor Green

# 3. Customer Service - port 6000
Start-Process powershell -ArgumentList "-NoExit", "-Command", "Set-Location '$root\src\MicroBank.CustomerService'; dotnet run" -WindowStyle Normal
Write-Host "  [3/5] CustomerService   -> http://localhost:6000" -ForegroundColor Green

# 4. Account Service - port 6001
Start-Process powershell -ArgumentList "-NoExit", "-Command", "Set-Location '$root\src\MicroBank.AccountService'; dotnet run" -WindowStyle Normal
Write-Host "  [4/5] AccountService    -> http://localhost:6001" -ForegroundColor Green

# 5. API Gateway (Ocelot) - port 7000 (frontend talks to this)
Start-Process powershell -ArgumentList "-NoExit", "-Command", "Set-Location '$root\src\MicroBank.ApiGateway'; dotnet run" -WindowStyle Normal
Write-Host "  [5/5] ApiGateway        -> http://localhost:7000" -ForegroundColor Green

Write-Host ""
Write-Host "All backend services are starting..." -ForegroundColor Cyan
Write-Host "Wait ~15-20 seconds, then run: .\start-frontend.ps1" -ForegroundColor Yellow
Write-Host "Frontend will be available at:  http://localhost:4200" -ForegroundColor Yellow
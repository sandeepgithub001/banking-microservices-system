# ============================================================
# MicroBank - Start Angular Frontend
# Serves the frontend at http://localhost:4200
# Make sure the backend is running first (start-backend.ps1)
# ============================================================

$root = $PSScriptRoot

Write-Host "Starting MicroBank frontend..." -ForegroundColor Cyan

Set-Location "$root\frontend"

# Install dependencies if node_modules is missing
if (-not (Test-Path "$root\frontend\node_modules")) {
    Write-Host "node_modules not found - installing dependencies (npm install)..." -ForegroundColor Yellow
    npm install
}

Write-Host "Frontend starting at http://localhost:4200" -ForegroundColor Green
Write-Host "(API calls go through the gateway at http://localhost:7000)" -ForegroundColor Gray

npm start
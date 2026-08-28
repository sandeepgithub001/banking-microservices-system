# ============================================================
# MicroBank - Stop All Running Services
# Kills all dotnet processes and the Angular dev server.
# ============================================================

Write-Host "Stopping MicroBank services..." -ForegroundColor Cyan

# Stop all dotnet processes (backend services)
$dotnet = Get-Process -Name "dotnet" -ErrorAction SilentlyContinue
if ($dotnet) {
    $dotnet | Stop-Process -Force
    Write-Host "  Stopped $($dotnet.Count) dotnet process(es)." -ForegroundColor Green
} else {
    Write-Host "  No dotnet processes running." -ForegroundColor Gray
}

# Stop node processes running the Angular dev server (ng serve)
$node = Get-Process -Name "node" -ErrorAction SilentlyContinue
if ($node) {
    $node | Stop-Process -Force
    Write-Host "  Stopped $($node.Count) node process(es)." -ForegroundColor Green
} else {
    Write-Host "  No node processes running." -ForegroundColor Gray
}

Write-Host "All services stopped." -ForegroundColor Cyan
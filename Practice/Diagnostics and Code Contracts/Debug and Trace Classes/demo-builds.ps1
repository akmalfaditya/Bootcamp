# Debug and Trace Build Configurations
# This script demonstrates how Debug and Trace behave differently in Debug vs Release builds

Write-Host "=== Debug and Trace Classes Build Demo ===" -ForegroundColor Green
Write-Host

# Clean any previous builds
Write-Host "Cleaning previous builds..." -ForegroundColor Yellow
dotnet clean --nologo

# Build and run in Debug mode
Write-Host "`n1. Building and running in DEBUG mode..." -ForegroundColor Cyan
Write-Host "   (Both Debug and Trace messages will appear)" -ForegroundColor Gray
dotnet build -c Debug --nologo
if ($LASTEXITCODE -eq 0) {
    Write-Host "   Running Debug build..." -ForegroundColor Green
    dotnet run -c Debug --no-build
} else {
    Write-Host "   Debug build failed!" -ForegroundColor Red
}

Write-Host "`n" + "="*60 -ForegroundColor Yellow
Write-Host

# Build and run in Release mode
Write-Host "2. Building and running in RELEASE mode..." -ForegroundColor Cyan
Write-Host "   (Only Trace messages will appear - Debug calls are removed)" -ForegroundColor Gray
dotnet build -c Release --nologo
if ($LASTEXITCODE -eq 0) {
    Write-Host "   Running Release build..." -ForegroundColor Green
    dotnet run -c Release --no-build
} else {
    Write-Host "   Release build failed!" -ForegroundColor Red
}

Write-Host "`n=== Build Demo Complete ===" -ForegroundColor Green
Write-Host "Key Observations:" -ForegroundColor Yellow
Write-Host "• Debug messages only appear in Debug builds" -ForegroundColor White
Write-Host "• Trace messages appear in both Debug and Release builds" -ForegroundColor White
Write-Host "• Release builds are optimized and exclude Debug code entirely" -ForegroundColor White
Write-Host
Write-Host "Check the generated log files to see the output differences!" -ForegroundColor Cyan

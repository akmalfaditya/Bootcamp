# JWT Authentication API Test Script
# This PowerShell script demonstrates the complete authentication flow

$baseUrl = "http://localhost:5195/api/auth"

Write-Host "🚀 JWT Authentication API Test Script" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green

# Test 1: Register a new user
Write-Host "`n1. Testing User Registration..." -ForegroundColor Yellow

$registerData = @{
    email = "testuser@example.com"
    password = "TestPassword123!"
    firstName = "Test"
    lastName = "User"
} | ConvertTo-Json

try {
    $registerResponse = Invoke-RestMethod -Uri "$baseUrl/register" -Method Post -Body $registerData -ContentType "application/json"
    Write-Host "✅ Registration successful!" -ForegroundColor Green
    Write-Host "User created: $($registerResponse.email)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ Registration failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 2: Login with the registered user
Write-Host "`n2. Testing User Login..." -ForegroundColor Yellow

$loginData = @{
    email = "testuser@example.com"
    password = "TestPassword123!"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$baseUrl/login" -Method Post -Body $loginData -ContentType "application/json"
    Write-Host "✅ Login successful!" -ForegroundColor Green
    Write-Host "Token received for: $($loginResponse.fullName)" -ForegroundColor Cyan
    Write-Host "Roles: $($loginResponse.roles -join ', ')" -ForegroundColor Cyan
    
    $userToken = $loginResponse.token
} catch {
    Write-Host "❌ Login failed: $($_.Exception.Message)" -ForegroundColor Red
    return
}

# Test 3: Access protected endpoint
Write-Host "`n3. Testing Protected Endpoint (Profile)..." -ForegroundColor Yellow

$headers = @{
    'Authorization' = "Bearer $userToken"
}

try {
    $profileResponse = Invoke-RestMethod -Uri "$baseUrl/profile" -Method Get -Headers $headers
    Write-Host "✅ Profile access successful!" -ForegroundColor Green
    Write-Host "Profile: $($profileResponse.fullName) ($($profileResponse.email))" -ForegroundColor Cyan
} catch {
    Write-Host "❌ Profile access failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 4: Test with admin account
Write-Host "`n4. Testing Admin Login..." -ForegroundColor Yellow

$adminLoginData = @{
    email = "admin@jwtauth.com"
    password = "Admin123!"
} | ConvertTo-Json

try {
    $adminLoginResponse = Invoke-RestMethod -Uri "$baseUrl/login" -Method Post -Body $adminLoginData -ContentType "application/json"
    Write-Host "✅ Admin login successful!" -ForegroundColor Green
    Write-Host "Admin: $($adminLoginResponse.fullName)" -ForegroundColor Cyan
    Write-Host "Roles: $($adminLoginResponse.roles -join ', ')" -ForegroundColor Cyan
    
    $adminToken = $adminLoginResponse.token
} catch {
    Write-Host "❌ Admin login failed: $($_.Exception.Message)" -ForegroundColor Red
    return
}

# Test 5: Access admin-only endpoint
Write-Host "`n5. Testing Admin-Only Endpoint..." -ForegroundColor Yellow

$adminHeaders = @{
    'Authorization' = "Bearer $adminToken"
}

try {
    $adminResponse = Invoke-RestMethod -Uri "$baseUrl/users" -Method Get -Headers $adminHeaders
    Write-Host "✅ Admin endpoint access successful!" -ForegroundColor Green
    Write-Host "Total users found: $($adminResponse.Count)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ Admin endpoint access failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n🎉 Testing Complete!" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green
Write-Host "API is running at: $baseUrl" -ForegroundColor Cyan
Write-Host "Swagger UI: http://localhost:5195/swagger" -ForegroundColor Cyan

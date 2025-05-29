# ========================================
# BATCH SCRIPTS FOR DIFFERENT BUILD CONFIGURATIONS
# ========================================
# These scripts demonstrate how to build the project with different conditional compilation symbols
# Run these from the project directory

# Development Build (Full features)
echo "Building Development Configuration..."
dotnet build -c Debug -p:DefineConstants="DEVELOPMENT;DEBUG_MODE;LOGGING;PERFORMANCE_METRICS;TESTING_ENABLED"

# Production Build (Minimal features for performance)
echo "Building Production Configuration..."
dotnet build -c Release -p:DefineConstants="LOGGING"

# Testing Build (Development + Testing features)
echo "Building Testing Configuration..."
dotnet build -c Debug -p:DefineConstants="DEVELOPMENT;LOGGING;TESTING_ENABLED;PERFORMANCE_METRICS"

# Legacy Support Build (For older systems)
echo "Building Legacy Support Configuration..."
dotnet build -c Debug -p:DefineConstants="DEVELOPMENT;LOGGING;LEGACY_SUPPORT"

# Minimal Build (No conditional features)
echo "Building Minimal Configuration..."
dotnet build -c Release -p:DefineConstants=""

echo "All builds completed!"

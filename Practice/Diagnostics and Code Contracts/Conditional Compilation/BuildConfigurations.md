# Build Configurations Guide

This project demonstrates different ways to configure conditional compilation symbols for various build scenarios.

## Current Configuration (Development Build)
The project is currently configured with these symbols in the `.csproj` file:
- `DEVELOPMENT` - Enables development-specific logging and features
- `LOGGING` - Enables general logging capabilities
- `PERFORMANCE_METRICS` - Includes performance measurement code

## How to Change Build Configurations

### 1. Production Build
To create a production build, modify the `.csproj` file:
```xml
<DefineConstants>LOGGING</DefineConstants>
```
This removes development features and performance metrics while keeping basic logging.

### 2. Testing Build
For testing environments:
```xml
<DefineConstants>DEVELOPMENT;LOGGING;TESTING_ENABLED</DefineConstants>
```

### 3. Legacy Support Build
If you need to support legacy systems:
```xml
<DefineConstants>DEVELOPMENT;LOGGING;LEGACY_SUPPORT</DefineConstants>
```

### 4. Minimal Production Build
For maximum performance in production:
```xml
<!-- Remove DefineConstants entirely or set to empty -->
<DefineConstants></DefineConstants>
```

## Using Different Configurations

### Command Line Build
You can also define symbols via command line:
```bash
dotnet build -p:DefineConstants="DEVELOPMENT;CUSTOM_FEATURE"
```

### Visual Studio Configurations
In Visual Studio, you can create different build configurations:
1. Go to Build → Configuration Manager
2. Create new configurations (e.g., "Development", "Testing", "Production")
3. Set different conditional compilation symbols for each

## Symbol Effects

| Symbol | Effect |
|--------|--------|
| `DEVELOPMENT` | Enables development logging and features |
| `DEBUG_MODE` | Enables detailed debug output and file logging |
| `LOGGING` | Enables general application logging |
| `PERFORMANCE_METRICS` | Includes performance measurement overhead |
| `TESTING_ENABLED` | Enables testing-specific features |
| `LEGACY_SUPPORT` | Uses older, compatible API implementations |

## Best Practices

1. **Use symbols sparingly** - Too many can make code hard to follow
2. **Document your symbols** - Always explain what each symbol does
3. **Test all configurations** - Make sure your app works with different symbol combinations
4. **Consider runtime flags** - For features that need to be toggled without recompilation
5. **Performance considerations** - Conditional attributes have zero runtime cost when disabled

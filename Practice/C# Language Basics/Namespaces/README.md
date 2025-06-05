# Namespaces in C#

## Learning Objectives
By the end of this module, you will learn:
- **Namespace fundamentals** and their role in code organization
- **Using directives** and their various forms (global, static, aliases)
- **Nested namespaces** for hierarchical code structure
- **Namespace aliases** to resolve naming conflicts
- **Type aliases** (C# 12) for improved code readability
- **Global namespace** and conflict resolution strategies
- **Best practices** for professional namespace organization

## Core Concepts Covered

### 1. Namespace Fundamentals
- **Logical grouping**: Organize related types and functionality
- **Naming conflicts prevention**: Avoid type name collisions
- **Assembly boundaries**: Namespaces can span multiple assemblies
- **Fully qualified names**: Complete type identification path

### 2. Using Directives
- **Standard using**: Import entire namespaces
- **Global using** (C# 10): Project-wide imports
- **Using static**: Import static members directly
- **Using aliases**: Create shortcuts for long namespace names
- **Type aliases** (C# 12): Simplify complex type declarations

### 3. Namespace Organization
- **Hierarchical structure**: Multi-level namespace nesting
- **Domain-driven design**: Organize by business domains
- **Layer separation**: Separate concerns (Data, Business, UI)
- **Assembly alignment**: Match namespaces to assembly structure

## Key Features & Examples

### Basic Namespace Usage
```csharp
// Declaring a namespace
namespace Company.ProjectName.Module
{
    public class BusinessLogic
    {
        public void ProcessData() 
        {
            // Implementation
        }
    }
}

// Using fully qualified names
var processor = new Company.ProjectName.Module.BusinessLogic();

// With using directive
using Company.ProjectName.Module;
var processor = new BusinessLogic();
```

### Global Using Directives (C# 10)
```csharp
// In GlobalUsings.cs or any .cs file
global using System;
global using System.Collections.Generic;
global using System.Linq;

// Available in all files in the project
public class AnyClass
{
    public void Method()
    {
        Console.WriteLine("No need for using System!");
        var list = new List<string>(); // No need for using statement
    }
}
```

### Using Static for Direct Member Access
```csharp
using static System.Console;
using static System.Math;
using static System.DateTime;

public class Calculator
{
    public void DisplayResult(double value)
    {
        // Direct access to static members
        WriteLine($"Result: {Round(value, 2)}");
        WriteLine($"Calculated at: {Now}");
        
        // Instead of:
        // System.Console.WriteLine($"Result: {System.Math.Round(value, 2)}");
    }
}
```

### Namespace and Type Aliases
```csharp
// Namespace aliases
using MyCollections = System.Collections.Generic;
using WinForms = System.Windows.Forms;

// Type aliases (C# 12)
using StringList = List<string>;
using UserData = Dictionary<string, object>;
using Coordinates = (double X, double Y);

public class Example
{
    public void UseAliases()
    {
        // Using namespace alias
        var list = new MyCollections.List<int>();
        
        // Using type aliases
        StringList names = new StringList();
        UserData userData = new UserData();
        Coordinates point = (10.5, 20.3);
    }
}
```

### Nested Namespace Organization
```csharp
namespace Company.ECommerce
{
    namespace Data
    {
        namespace Models
        {
            public class Product { }
            public class Order { }
        }
        
        namespace Repositories
        {
            public class ProductRepository { }
        }
    }
    
    namespace Business
    {
        namespace Services
        {
            public class OrderService { }
        }
    }
    
    namespace Web.Controllers
    {
        public class ProductController { }
    }
}
```

## Tips

### Performance Considerations
- **Using directives** have no runtime performance impact
- **Global using** reduces compilation overhead across files
- **Type aliases** can improve IntelliSense performance with complex generic types
- **Nested namespaces** don't affect runtime performance

### Memory Management
```csharp
// Efficient: Use type aliases for complex generics
using ComplexDictionary = Dictionary<string, List<KeyValuePair<int, object>>>;

// Instead of repeatedly writing:
Dictionary<string, List<KeyValuePair<int, object>>> data = new();

// Use:
ComplexDictionary data = new();
```

### Common Pitfalls
```csharp
// Avoid: Overly deep nesting
namespace Company.Department.Team.Project.Module.SubModule.Feature
{
    // Too deep - hard to navigate
}

// Better: Balanced hierarchy
namespace Company.ProjectName.Features
{
    // Clear and manageable
}

// Avoid: Generic namespace names
namespace Utilities
{
    // Too vague
}

// Better: Specific purpose
namespace Company.StringHelpers
{
    // Clear intent
}
```

### Conflict Resolution
```csharp
// When you have naming conflicts
using System.Windows.Forms;
using MyApp.Controls;

public class Example
{
    public void HandleConflict()
    {
        // Explicit disambiguation
        var winButton = new System.Windows.Forms.Button();
        var customButton = new MyApp.Controls.Button();
        
        // Or use aliases
        using WinForms = System.Windows.Forms;
        var button = new WinForms.Button();
    }
}
```

## Best Practices & Guidelines

### 1. Namespace Naming Conventions
```csharp
// Follow Microsoft naming guidelines
namespace CompanyName.ProductName.FeatureName
{
    // Clear hierarchy and ownership
}

// Use PascalCase for all namespace segments
namespace DataAccess.EntityFramework.Repositories
{
    // Consistent casing
}

// Avoid abbreviations and acronyms
namespace DA.EF.Repos  // Unclear

// Use full, descriptive names
namespace DataAccess.EntityFramework.Repositories
```

### 2. File Organization Strategy
```csharp
// Organize files to match namespace structure
Company.ECommerce.Data/
   Models/
      Product.cs
      Order.cs
   Repositories/
      ProductRepository.cs
      OrderRepository.cs

// Each file should contain types in matching namespace
namespace Company.ECommerce.Data.Models
{
    public class Product { }
}
```

### 3. Global Using Strategy
```csharp
// Create a dedicated GlobalUsings.cs file
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading.Tasks;

// Project-specific global usings
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;

// Domain-specific aliases
global using UserId = System.Guid;
global using Timestamp = System.DateTimeOffset;
```

### 4. Layered Architecture Namespaces
```csharp
// Clear separation of concerns
namespace Company.ProjectName.Domain        // Business entities
namespace Company.ProjectName.Application  // Use cases, services
namespace Company.ProjectName.Infrastructure // Data access, external services
namespace Company.ProjectName.Presentation // Controllers, views
namespace Company.ProjectName.Tests        // Test projects
```

## Real-World Applications

### 1. Enterprise Application Structure
```csharp
namespace ContosoBank
{
    namespace Core
    {
        namespace Entities
        {
            public class Account { }
            public class Transaction { }
        }
        
        namespace Interfaces
        {
            public interface IAccountRepository { }
        }
    }
    
    namespace Infrastructure
    {
        namespace Data
        {
            public class SqlAccountRepository : Core.Interfaces.IAccountRepository { }
        }
        
        namespace External
        {
            public class PaymentGateway { }
        }
    }
    
    namespace Application
    {
        namespace Services
        {
            public class AccountService { }
        }
        
        namespace DTOs
        {
            public class AccountDto { }
        }
    }
}
```

### 2. Microservices Architecture
```csharp
// Each microservice has its own namespace hierarchy
namespace UserService.Api
namespace UserService.Domain
namespace UserService.Infrastructure

namespace OrderService.Api
namespace OrderService.Domain
namespace OrderService.Infrastructure

namespace PaymentService.Api
namespace PaymentService.Domain
namespace PaymentService.Infrastructure
```

### 3. Plugin Architecture
```csharp
namespace PluginSystem
{
    namespace Core
    {
        public interface IPlugin { }
        public abstract class PluginBase { }
    }
    
    namespace Plugins.Authentication
    {
        public class GoogleAuthPlugin : Core.PluginBase { }
        public class FacebookAuthPlugin : Core.PluginBase { }
    }
    
    namespace Plugins.Storage
    {
        public class AzureStoragePlugin : Core.PluginBase { }
        public class AmazonS3Plugin : Core.PluginBase { }
    }
}
```

## Industry Applications

### Software Architecture
- **Clean Architecture**: Namespace-based layer separation
- **Domain-Driven Design**: Business domain organization
- **Microservices**: Service boundary definition
- **Plugin Systems**: Extensible application architecture

### Team Collaboration
- **Code Organization**: Clear ownership and responsibility
- **Merge Conflict Reduction**: Logical file separation
- **Code Reviews**: Easier navigation and understanding
- **Onboarding**: Clear project structure for new developers

### Maintenance & Scalability
- **Refactoring**: Safe code reorganization
- **Feature Toggles**: Namespace-based feature organization
- **Testing**: Parallel test namespace structures
- **Documentation**: Self-documenting code organization


#nullable disable // Disable nullable warnings for this educational demo

// Global using directives (C# 10) - Apply to entire project
global using System;
global using System.Collections.Generic;

// Regular using directives
using System.Text;
using System.Reflection;

// Using static - Import static members directly
using static System.Console;
using static System.Math;

// Namespace aliases to avoid conflicts
using MyReflection = System.Reflection;
using WinVisibility = System.ComponentModel;

// Type aliases (C# 12 feature)
using NumberList = double[];
using StringPair = (string first, string second);
using PersonInfo = System.ValueTuple<string, int, string>;

namespace NamespacesDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("=== C# NAMESPACES MASTERCLASS ===");
            WriteLine("Your complete guide to organizing code like a professional\n");
            
            // Section 1: Basic Namespace Concepts
            DemonstrateBasicNamespaces();
            
            // Section 2: Nested Namespaces
            DemonstrateNestedNamespaces();
            
            // Section 3: Using Directives
            DemonstrateUsingDirectives();
            
            // Section 4: Using Static
            DemonstrateUsingStatic();
            
            // Section 5: Aliases and Type Aliases
            DemonstrateAliases();
            
            // Section 6: Global Namespace and Conflicts
            DemonstrateGlobalNamespace();
            
            // Section 7: Real-World Organization
            DemonstrateRealWorldOrganization();
            
            WriteLine("\n=== NAMESPACE MASTERY COMPLETE ===");
            WriteLine("You now know how to organize code like a seasoned developer!");
        }
        
        #region Basic Namespace Concepts
        
        static void DemonstrateBasicNamespaces()
        {
            WriteLine("=== BASIC NAMESPACE CONCEPTS ===");
            WriteLine("Understanding the foundation of code organization\n");
            
            // Working with classes from our custom namespaces
            WriteLine("--- Creating Objects from Different Namespaces ---");
            
            // Fully qualified names - the explicit way
            var basicUser = new BasicTypes.User("Alice", "alice@example.com");
            WriteLine($"Created user with fully qualified name: {basicUser.GetInfo()}");
            
            // Using imported namespace (via using directive at top)
            var advancedUser = new AdvancedTypes.EnhancedUser("Bob", "bob@example.com", "Premium");
            WriteLine($"Created enhanced user: {advancedUser.GetFullInfo()}");
            
            // Demonstrating namespace hierarchy
            WriteLine("\n--- Namespace Hierarchy ---");
            WriteLine("BasicTypes.User lives in: BasicTypes namespace");
            WriteLine("AdvancedTypes.EnhancedUser lives in: AdvancedTypes namespace");
            WriteLine("Both are organized logically based on their complexity level");
            
            WriteLine("\nNamespaces prevent naming conflicts and provide logical organization\n");
        }
        
        #endregion
        
        #region Nested Namespaces
        
        static void DemonstrateNestedNamespaces()
        {
            WriteLine("=== NESTED NAMESPACES ===");
            WriteLine("Creating hierarchical code organization\n");
            
            WriteLine("--- Multi-Level Namespace Structure ---");
            
            // Working with deeply nested namespaces
            var dbUser = new Company.Data.Models.DatabaseUser(1, "Charlie");
            var apiUser = new Company.Api.Controllers.UserController();
            var webComponent = new Company.Web.UI.Components.UserCard();
            
            WriteLine($"Database user: {dbUser.GetInfo()}");
            WriteLine($"API controller: {apiUser.GetControllerInfo()}");
            WriteLine($"Web component: {webComponent.GetComponentInfo()}");
            
            WriteLine("\n--- Namespace Hierarchy Explanation ---");
            WriteLine("Company.Data.Models.DatabaseUser");
            WriteLine("  └── Company (root namespace)");
            WriteLine("      └── Data (data layer)");
            WriteLine("          └── Models (data models)");
            WriteLine("              └── DatabaseUser (specific class)");
            
            WriteLine("\n--- Benefits of Nested Organization ---");
            WriteLine("• Clear separation of concerns");
            WriteLine("• Easy to locate related functionality");
            WriteLine("• Prevents naming conflicts across layers");
            WriteLine("• Mirrors your project's architecture");
            
            WriteLine("\nNested namespaces reflect your application's logical structure\n");
        }
        
        #endregion
        
        #region Using Directives
        
        static void DemonstrateUsingDirectives()
        {
            WriteLine("=== USING DIRECTIVES ===");
            WriteLine("Simplifying code by importing namespaces\n");
            
            WriteLine("--- Global Using Directives ---");
            WriteLine("Thanks to 'global using System;' at the top:");
            WriteLine("• We can use Console.WriteLine without System prefix");
            WriteLine("• DateTime, String, and other System types are directly accessible");
            WriteLine("• Applied to entire project automatically");
            
            // Demonstrating global using benefits
            var now = DateTime.Now;  // No need for System.DateTime
            var numbers = new List<int> { 1, 2, 3 };  // No need for System.Collections.Generic.List
            
            WriteLine($"Current time (using global using): {now:HH:mm:ss}");
            WriteLine($"Numbers list (using global using): [{string.Join(", ", numbers)}]");
            
            WriteLine("\n--- Regular Using Directives ---");
            WriteLine("Regular using directives are file-scoped:");
            
            // Using System.Text (imported at top)
            var builder = new StringBuilder();  // No need for System.Text.StringBuilder
            builder.Append("Built with ");
            builder.Append("StringBuilder");
            WriteLine($"String building result: {builder}");
            
            // Using System.Reflection (imported with alias)
            var type = typeof(Program);
            var methods = type.GetMethods();
            WriteLine($"This class has {methods.Length} methods (using Reflection)");
            
            WriteLine("\n--- Without Using Directives ---");
            WriteLine("Without using directives, you'd need fully qualified names:");
            WriteLine("System.Console.WriteLine(\"Hello\");");
            WriteLine("System.Collections.Generic.List<int> list = new();");
            WriteLine("System.Text.StringBuilder sb = new();");
            
            WriteLine("\nUsing directives make your code cleaner and more readable\n");
        }
        
        #endregion
        
        #region Using Static
        
        static void DemonstrateUsingStatic()
        {
            WriteLine("=== USING STATIC ===");
            WriteLine("Importing static members for cleaner code\n");
            
            WriteLine("--- Console Methods ---");
            WriteLine("Thanks to 'using static System.Console;':");
            WriteLine("• WriteLine() instead of Console.WriteLine()");
            WriteLine("• Write() instead of Console.Write()");
            Write("• This message uses Write() directly");
            WriteLine(" - and this continues with WriteLine()");
            
            WriteLine("\n--- Math Functions ---");
            WriteLine("Thanks to 'using static System.Math;':");
            
            // Using Math functions without Math prefix
            double radius = 5.0;
            double area = PI * Pow(radius, 2);  // No need for Math.PI or Math.Pow
            double circumference = 2 * PI * radius;
            
            WriteLine($"Circle with radius {radius}:");
            WriteLine($"• Area = π × r² = {area:F2}");
            WriteLine($"• Circumference = 2π × r = {circumference:F2}");
            WriteLine($"• Square root of area = {Sqrt(area):F2}");
            
            WriteLine("\n--- Comparison: With vs Without Using Static ---");
            WriteLine("WITH using static System.Math:");
            WriteLine("  double result = Sqrt(Pow(x, 2) + Pow(y, 2));");
            WriteLine();
            WriteLine("WITHOUT using static:");
            WriteLine("  double result = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));");
            
            WriteLine("\n--- Using Static with Enums ---");
            // Demonstrating enum static import concept
            WriteLine("You can also use 'using static' with enums for direct value access:");
            WriteLine("Example: using static DayOfWeek;");
            WriteLine("Then: DayOfWeek today = Monday; (instead of DayOfWeek.Monday)");
            
            WriteLine("\nUsing static reduces verbosity for frequently used static members\n");
        }
        
        #endregion
        
        #region Aliases and Type Aliases
        
        static void DemonstrateAliases()
        {
            WriteLine("=== ALIASES AND TYPE ALIASES ===");
            WriteLine("Resolving conflicts and simplifying complex types\n");
            
            WriteLine("--- Namespace Aliases ---");
            WriteLine("Using 'using MyReflection = System.Reflection;':");
            
            // Using namespace alias
            MyReflection.PropertyInfo prop = typeof(Program).GetProperty("SampleProperty");
            WriteLine($"Found property using alias: {prop?.Name ?? "None found"}");
            
            WriteLine("\n--- Type Aliases (C# 12) ---");
            WriteLine("Modern C# allows aliasing complex types:");
            
            // Using type aliases defined at the top
            NumberList prices = { 10.99, 25.50, 5.75, 99.99 };
            WriteLine($"Product prices: [{string.Join(", ", prices)}]");
            
            StringPair nameAndRole = ("John Doe", "Developer");
            WriteLine($"Employee: {nameAndRole.first} - {nameAndRole.second}");
            
            PersonInfo person = ("Alice Smith", 30, "Manager");
            WriteLine($"Person info: {person.Item1}, Age: {person.Item2}, Role: {person.Item3}");
            
            WriteLine("\n--- Benefits of Type Aliases ---");
            WriteLine("• Make complex generic types readable");
            WriteLine("• Provide domain-specific names");
            WriteLine("• Reduce repetition in code");
            WriteLine("• Easy to change underlying type later");
            
            WriteLine("\n--- Before and After Type Aliases ---");
            WriteLine("BEFORE:");
            WriteLine("Dictionary<string, List<(int id, string name, DateTime created)>> data;");
            WriteLine();
            WriteLine("AFTER (with aliases):");
            WriteLine("using UserRecord = (int id, string name, DateTime created);");
            WriteLine("using UserDatabase = Dictionary<string, List<UserRecord>>;");
            WriteLine("UserDatabase data;");
            
            WriteLine("\nAliases make your code more readable and maintainable\n");
        }
        
        #endregion
        
        #region Global Namespace and Conflicts
        
        static void DemonstrateGlobalNamespace()
        {
            WriteLine("=== GLOBAL NAMESPACE AND CONFLICTS ===");
            WriteLine("Understanding scope and resolving naming conflicts\n");
            
            WriteLine("--- Global Namespace Concept ---");
            WriteLine("Types not in any namespace live in the global namespace");
            WriteLine("Our Program class is in NamespacesDemo namespace");
            
            // Demonstrating global namespace access
            WriteLine("\n--- Accessing Global Types ---");
            WriteLine("You can use global:: to explicitly access global namespace:");
            WriteLine("Example: global::System.Console.WriteLine()");
            WriteLine("This ensures you're using the global System namespace");
            
            // Demonstrate global qualifier usage
            global::System.Console.WriteLine("This line uses global::System.Console.WriteLine()");
            
            WriteLine("\n--- Name Conflict Resolution ---");
            WriteLine("When you have naming conflicts, you have several options:");
            
            WriteLine("\n1. Fully Qualified Names:");
            WriteLine("   BasicTypes.User vs AdvancedTypes.User");
            
            WriteLine("\n2. Namespace Aliases:");
            WriteLine("   using BT = BasicTypes;");
            WriteLine("   using AT = AdvancedTypes;");
            WriteLine("   BT.User basicUser = new();");
            WriteLine("   AT.User advancedUser = new();");
            
            WriteLine("\n3. Global Qualifier:");
            WriteLine("   global::MyClass vs SomeNamespace.MyClass");
            
            WriteLine("\n--- Practical Conflict Example ---");
            WriteLine("Imagine you have two libraries with same class names:");
            WriteLine("• Library1.Data.User");
            WriteLine("• Library2.Models.User");
            WriteLine();
            WriteLine("Solution with aliases:");
            WriteLine("using DataUser = Library1.Data.User;");
            WriteLine("using ModelUser = Library2.Models.User;");
            
            WriteLine("\nProper namespace usage prevents and resolves naming conflicts\n");
        }
        
        #endregion
        
        #region Real-World Organization
        
        static void DemonstrateRealWorldOrganization()
        {
            WriteLine("=== REAL-WORLD ORGANIZATION ===");
            WriteLine("How professionals structure namespaces in real projects\n");
            
            WriteLine("--- Typical Enterprise Application Structure ---");
            WriteLine("MyCompany.ProjectName.Domain.Models");
            WriteLine("MyCompany.ProjectName.Domain.Services");
            WriteLine("MyCompany.ProjectName.Infrastructure.Database");
            WriteLine("MyCompany.ProjectName.Infrastructure.External");
            WriteLine("MyCompany.ProjectName.Application.Commands");
            WriteLine("MyCompany.ProjectName.Application.Queries");
            WriteLine("MyCompany.ProjectName.Web.Controllers");
            WriteLine("MyCompany.ProjectName.Web.Models");
            
            WriteLine("\n--- Creating Instances from Organized Namespaces ---");
            
            // Demonstrate using our organized namespace structure
            var orderService = new Company.Services.Business.OrderService();
            var orderRepo = new Company.Data.Repositories.OrderRepository();
            var orderModel = new Company.Web.Models.OrderViewModel();
            
            WriteLine($"Business service: {orderService.GetServiceInfo()}");
            WriteLine($"Data repository: {orderRepo.GetRepositoryInfo()}");
            WriteLine($"Web model: {orderModel.GetModelInfo()}");
            
            WriteLine("\n--- Best Practices for Namespace Organization ---");
            WriteLine("1. Company/Organization root namespace");
            WriteLine("2. Project or product name");
            WriteLine("3. Layer or feature area");
            WriteLine("4. Specific component type");
            WriteLine();
            WriteLine("Example breakdown:");
            WriteLine("Microsoft.AspNetCore.Mvc.Controllers");
            WriteLine("  └── Microsoft (company)");
            WriteLine("      └── AspNetCore (product)");
            WriteLine("          └── Mvc (feature area)");
            WriteLine("              └── Controllers (component type)");
            
            WriteLine("\n--- File Organization Tips ---");
            WriteLine("• One namespace per folder (usually)");
            WriteLine("• Folder structure mirrors namespace structure");
            WriteLine("• Use file-scoped namespaces in C# 10+ for cleaner code");
            WriteLine("• Group related classes in same namespace");
            WriteLine("• Avoid deep nesting unless necessary");
            
            WriteLine("\n--- Using Directives Strategy ---");
            WriteLine("• Global using for framework types (System, System.Collections.Generic)");
            WriteLine("• Regular using for project-specific namespaces");
            WriteLine("• Using static for frequently used utility classes");
            WriteLine("• Aliases for conflict resolution and complex types");
            
            WriteLine("\nGood namespace organization is the foundation of maintainable code!");
        }
        
        #endregion
        
        // Sample property for reflection demo
        public static string SampleProperty { get; set; } = "Demo Property";
    }

    #region Supporting Namespaces and Classes

    // Basic namespace example
    namespace BasicTypes
    {
        public class User
        {
            public string Name { get; set; }
            public string Email { get; set; }
            
            public User(string name, string email)
            {
                Name = name;
                Email = email;
            }
            
            public string GetInfo()
            {
                return $"{Name} ({Email})";
            }
        }
        
        public class Product
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
        }
    }

    // Advanced namespace example  
    namespace AdvancedTypes
    {
        public class EnhancedUser
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string MembershipLevel { get; set; }
            public DateTime CreatedAt { get; set; }
            
            public EnhancedUser(string name, string email, string membershipLevel)
            {
                Name = name;
                Email = email;
                MembershipLevel = membershipLevel;
                CreatedAt = DateTime.Now;
            }
            
            public string GetFullInfo()
            {
                return $"{Name} ({Email}) - {MembershipLevel} member since {CreatedAt:yyyy-MM-dd}";
            }
        }
    }

    // Nested namespace examples - Company structure
    namespace Company
    {
        namespace Data
        {
            namespace Models
            {
                public class DatabaseUser
                {
                    public int Id { get; set; }
                    public string Username { get; set; }
                    
                    public DatabaseUser(int id, string username)
                    {
                        Id = id;
                        Username = username;
                    }
                    
                    public string GetInfo()
                    {
                        return $"DB User: {Username} (ID: {Id})";
                    }
                }
            }
            
            namespace Repositories
            {
                public class OrderRepository
                {
                    public string GetRepositoryInfo()
                    {
                        return "Order Repository - handles order data persistence";
                    }
                }
            }
        }
        
        namespace Api
        {
            namespace Controllers
            {
                public class UserController
                {
                    public string GetControllerInfo()
                    {
                        return "User API Controller - handles HTTP requests";
                    }
                }
            }
        }
        
        namespace Web
        {
            namespace UI
            {
                namespace Components
                {
                    public class UserCard
                    {
                        public string GetComponentInfo()
                        {
                            return "User Card UI Component - displays user information";
                        }
                    }
                }
            }
            
            namespace Models
            {
                public class OrderViewModel
                {
                    public string GetModelInfo()
                    {
                        return "Order View Model - shapes data for web presentation";
                    }
                }
            }
        }
        
        namespace Services
        {
            namespace Business
            {
                public class OrderService
                {
                    public string GetServiceInfo()
                    {
                        return "Order Business Service - implements order processing logic";
                    }
                }
            }
        }
    }

    // Compact nested namespace syntax (alternative to above)
    namespace Company.Utils.Helpers
    {
        public class StringHelper
        {
            public static string FormatUserName(string firstName, string lastName)
            {
                return $"{firstName} {lastName}".Trim();
            }
        }
    }

    namespace Company.Utils.Extensions
    {
        public static class DateTimeExtensions
        {
            public static string ToFriendlyString(this DateTime dateTime)
            {
                return dateTime.ToString("MMM dd, yyyy");
            }
        }
    }

    #endregion
}

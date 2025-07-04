using System;
using System.Diagnostics;

namespace StructDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== STRUCTS: Value Types with Class-like Capabilities ===");
            Console.WriteLine("Understanding the fundamental differences and when to use structs\n");

            // Step through each core concept systematically
            Console.WriteLine("Press any key to start exploring...");
            Console.ReadKey();
            Console.Clear();

            DemonstrateValueTypeSemantics();
            Console.WriteLine("\nPress any key for Constructor Behavior...");
            Console.ReadKey();
            Console.Clear();

            DemonstrateConstructorBehavior();
            Console.WriteLine("\nPress any key for Readonly Features...");
            Console.ReadKey();
            Console.Clear();

            DemonstrateReadOnlyFeatures();
            Console.WriteLine("\nPress any key for Ref Structs...");
            Console.ReadKey();
            Console.Clear();

            DemonstrateRefStructs();
            Console.WriteLine("\nPress any key for Inheritance Limitations...");
            Console.ReadKey();
            Console.Clear();

            DemonstrateInheritanceLimitations();
            Console.WriteLine("\nPress any key for Performance Analysis...");
            Console.ReadKey();
            Console.Clear();

            DemonstratePerformanceAnalysis();
            Console.WriteLine("\nPress any key for Practical Use Cases...");
            Console.ReadKey();
            Console.Clear();

            DemonstratePracticalUseCases();

            Console.WriteLine("\n=== TRAINING COMPLETE ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Core Concept 1: Value Type Semantics
        /// Demonstrates how structs behave as value types vs reference types
        /// </summary>
        static void DemonstrateValueTypeSemantics()
        {
            Console.WriteLine("1. VALUE TYPE SEMANTICS");
            Console.WriteLine("========================");
            Console.WriteLine("Structs are value types - they store data directly, not references\n");

            // Create basic struct and class instances
            Console.WriteLine("Creating Point (struct) and PointClass (reference type):");
            var structPoint = new BasicPoint(10, 20);
            var classPoint = new PointClass(10, 20);

            Console.WriteLine($"Original struct: {structPoint}");
            Console.WriteLine($"Original class: {classPoint}");

            // Demonstrate value semantics with copying
            Console.WriteLine("\nCopying behavior:");
            var copiedStruct = structPoint;    // Copies all data
            var copiedClass = classPoint;      // Copies reference only

            // Modify the copies
            copiedStruct.X = 100;
            copiedClass.X = 100;

            Console.WriteLine($"After modifying copies:");
            Console.WriteLine($"Original struct: {structPoint} (unchanged - value copied)");
            Console.WriteLine($"Original class: {classPoint} (changed - reference copied)");

            // Assignment behavior
            Console.WriteLine("\nDemonstrating assignment:");
            var point1 = new BasicPoint(5, 15);
            var point2 = point1;  // Full copy of data
            point2.X = 999;

            Console.WriteLine($"Point1: {point1} (original unchanged)");
            Console.WriteLine($"Point2: {point2} (copy was modified)");

            // Equality behavior
            Console.WriteLine("\nEquality comparison:");
            var pointA = new BasicPoint(10, 20);
            var pointB = new BasicPoint(10, 20);
            Console.WriteLine($"Two structs with same values: pointA == pointB = {pointA.Equals(pointB)}");
            Console.WriteLine("Structs compare by value, not reference");

            // Memory allocation differences
            Console.WriteLine("\nMemory allocation:");
            Console.WriteLine("✓ Struct: Lives on the stack (faster allocation/deallocation)");
            Console.WriteLine("✓ Class: Lives on the heap (garbage collected)");
            Console.WriteLine("✓ Struct members inline, no pointer dereferencing");
        }

        /// <summary>
        /// Core Concept 2: Constructor Behavior and Default Values
        /// Explains how struct constructors work differently from classes
        /// </summary>
        static void DemonstrateConstructorBehavior()
        {
            Console.WriteLine("2. CONSTRUCTOR BEHAVIOR & DEFAULT VALUES");
            Console.WriteLine("=======================================");
            Console.WriteLine("Structs have special constructor rules and automatic defaults\n");

            // Default constructor behavior
            Console.WriteLine("Default constructor (always available):");
            var defaultPoint = new BasicPoint();  // Calls implicit parameterless constructor
            Console.WriteLine($"Default BasicPoint: {defaultPoint}");
            Console.WriteLine("All fields automatically initialized to their default values");

            // Using default keyword
            Console.WriteLine("\nUsing 'default' keyword:");
            var defaultKeywordPoint = default(BasicPoint);
            Console.WriteLine($"default(BasicPoint): {defaultKeywordPoint}");
            Console.WriteLine("Equivalent to parameterless constructor");

            // Custom constructor with modern features
            Console.WriteLine("\nModern struct with custom defaults:");
            var modernPoint1 = new ModernPoint();  // Uses custom constructor
            var modernPoint2 = default(ModernPoint);  // Uses default values
            Console.WriteLine($"Custom constructor: {modernPoint1}");
            Console.WriteLine($"Default values: {modernPoint2}");

            // Constructor requirement: must initialize ALL fields
            Console.WriteLine("\nCustom constructor behavior:");
            var customPoint = new BasicPoint(100, 200);
            Console.WriteLine($"Custom constructor: {customPoint}");
            Console.WriteLine("Custom constructors must initialize ALL fields");

            // Field initialization in newer C# versions
            Console.WriteLine("\nField initializers (C# 10+):");
            var initializedPoint = new AdvancedPoint(42, 84);
            Console.WriteLine($"Advanced point with initializers: {initializedPoint}");
        }

        /// <summary>
        /// Core Concept 3: Readonly Structs and Methods
        /// Shows immutability features and performance benefits
        /// </summary>
        static void DemonstrateReadOnlyFeatures()
        {
            Console.WriteLine("3. READONLY STRUCTS & METHODS");
            Console.WriteLine("============================");
            Console.WriteLine("Immutability features for safety and performance\n");

            // Readonly struct demonstration
            Console.WriteLine("Readonly struct (immutable):");
            var immutablePoint = new ReadOnlyPoint(15, 25);
            Console.WriteLine($"Immutable point: {immutablePoint}");
            Console.WriteLine("Cannot modify fields after construction");

            // Readonly methods on mutable structs
            Console.WriteLine("\nReadonly methods on mutable structs:");
            var mutablePoint = new MutablePoint(30, 40);
            Console.WriteLine($"Original: {mutablePoint}");

            // Readonly method doesn't create defensive copy
            double distance = mutablePoint.CalculateDistance();
            Console.WriteLine($"Distance from origin: {distance:F2}");
            Console.WriteLine("Readonly methods guarantee no modification");

            // Demonstrating 'in' parameter (readonly reference)
            Console.WriteLine("\nReadonly reference parameter ('in'):");
            var rectangle = new Rectangle { Width = 10, Height = 5 };
            ProcessReadOnlyRectangle(in rectangle);
            Console.WriteLine("'in' parameters avoid copying while preventing modification");

            // Performance benefits
            Console.WriteLine("\nPerformance benefits:");
            Console.WriteLine("✓ No defensive copying for readonly methods");
            Console.WriteLine("✓ Compiler optimizations for readonly structs");
            Console.WriteLine("✓ Clear intent - immutability enforced at compile time");
        }

        /// <summary>
        /// Core Concept 4: Ref Structs (Stack-only types)
        /// Demonstrates advanced memory management features
        /// </summary>
        static void DemonstrateRefStructs()
        {
            Console.WriteLine("4. REF STRUCTS (Stack-Only Types)");
            Console.WriteLine("=================================");
            Console.WriteLine("Advanced structs that can only live on the stack\n");

            // Basic ref struct usage
            Console.WriteLine("Creating ref struct (stack-only):");
            var stackPoint = new StackOnlyPoint { X = 42, Y = 84 };
            Console.WriteLine($"Stack-only point: X={stackPoint.X}, Y={stackPoint.Y}");

            // Demonstrate stack-only restrictions
            Console.WriteLine("\nRef struct restrictions:");
            Console.WriteLine("✗ Cannot be boxed to object");
            Console.WriteLine("✗ Cannot be assigned to Object, dynamic, or interface variables");
            Console.WriteLine("✗ Cannot be field of non-ref struct or class");
            Console.WriteLine("✗ Cannot implement interfaces");
            Console.WriteLine("✗ Cannot be used in async methods");
            Console.WriteLine("✗ Cannot be captured by lambda expressions");

            // Demonstrate use case - high-performance scenarios
            Console.WriteLine("\nTypical use cases:");
            Console.WriteLine("✓ High-performance data processing");
            Console.WriteLine("✓ Working with unmanaged memory");
            Console.WriteLine("✓ Avoiding heap allocations entirely");

            // Process data without heap allocation
            ProcessStackData(stackPoint);
        }

        /// <summary>
        /// Core Concept 5: Inheritance Limitations
        /// Explains what structs cannot do regarding inheritance
        /// </summary>
        static void DemonstrateInheritanceLimitations()
        {
            Console.WriteLine("5. INHERITANCE LIMITATIONS");
            Console.WriteLine("==========================");
            Console.WriteLine("Understanding what structs cannot inherit\n");

            // No class inheritance
            Console.WriteLine("Inheritance restrictions:");
            Console.WriteLine("✗ Cannot inherit from classes");
            Console.WriteLine("✗ Cannot inherit from other structs");
            Console.WriteLine("✗ Cannot be inherited by other types");
            Console.WriteLine("✗ Cannot have virtual methods");
            Console.WriteLine("✗ Cannot be abstract");

            // What they CAN do - interface implementation
            Console.WriteLine("\nWhat structs CAN do:");
            Console.WriteLine("✓ Implement interfaces");
            Console.WriteLine("✓ Override Object methods (ToString, Equals, GetHashCode)");

            // Interface implementation example
            var drawablePoint = new DrawablePoint(50, 75, "Red");
            drawablePoint.Draw();
            Console.WriteLine("Structs can implement interfaces for polymorphic behavior");

            // No virtual members
            Console.WriteLine("\nNo virtual members demonstration:");
            Console.WriteLine("✗ All struct methods are effectively 'sealed'");
            Console.WriteLine("✗ Cannot override methods in derived types (no derivation allowed)");
            Console.WriteLine("✓ Performance benefit - no virtual method table lookups");
        }

        /// <summary>
        /// Core Concept 6: Performance Analysis
        /// Compares struct vs class performance characteristics
        /// </summary>
        static void DemonstratePerformanceAnalysis()
        {
            Console.WriteLine("6. PERFORMANCE ANALYSIS");
            Console.WriteLine("=======================");
            Console.WriteLine("Measuring struct vs class performance\n");

            const int iterations = 1000000;
            Stopwatch sw = new Stopwatch();

            // Struct performance test
            Console.WriteLine($"Creating {iterations:N0} struct instances:");
            sw.Start();
            for (int i = 0; i < iterations; i++)
            {
                var point = new BasicPoint { X = i, Y = i * 2 };
                // Struct creation is very fast - stack allocation
            }
            sw.Stop();
            long structTime = sw.ElapsedMilliseconds;
            Console.WriteLine($"Struct creation time: {structTime} ms");

            // Class performance test
            sw.Restart();
            Console.WriteLine($"\nCreating {iterations:N0} class instances:");
            for (int i = 0; i < iterations; i++)
            {
                var point = new PointClass(i, i * 2);
                // Class creation involves heap allocation
            }
            sw.Stop();
            long classTime = sw.ElapsedMilliseconds;
            Console.WriteLine($"Class creation time: {classTime} ms");

            // Performance summary
            Console.WriteLine("\nPerformance characteristics:");
            Console.WriteLine("STRUCTS:");
            Console.WriteLine("✓ Stack allocation (very fast)");
            Console.WriteLine("✓ No garbage collection overhead");
            Console.WriteLine("✓ Better memory locality");
            Console.WriteLine("✓ No null reference exceptions");

            Console.WriteLine("\nCLASSES:");
            Console.WriteLine("✓ Heap allocation (more flexible)");
            Console.WriteLine("✓ Reference semantics");
            Console.WriteLine("✗ Garbage collection overhead");
            Console.WriteLine("✗ Potential null references");

            Console.WriteLine($"\nIn this test: Structs were {((float)classTime / structTime):F1}x faster for creation");
        }

        /// <summary>
        /// Core Concept 7: Practical Use Cases
        /// Real-world examples of when to use structs
        /// </summary>
        static void DemonstratePracticalUseCases()
        {
            Console.WriteLine("7. PRACTICAL USE CASES");
            Console.WriteLine("=====================");
            Console.WriteLine("Real-world scenarios where structs excel\n");

            // Coordinate systems
            Console.WriteLine("1. Coordinate Systems:");
            var point = new BasicPoint(10, 20);
            var vector = new Vector2D(3.5f, 4.2f);
            Console.WriteLine($"Point: {point}");
            Console.WriteLine($"Vector: {vector}");

            // Color values
            Console.WriteLine("\n2. Color Values:");
            var red = Color.FromRgb(255, 0, 0);
            var blue = Color.FromRgb(0, 0, 255);
            Console.WriteLine($"Red: {red}");
            Console.WriteLine($"Blue: {blue}");

            // Date/Time values
            Console.WriteLine("\n3. Date/Time Structures:");
            var timestamp = new SimpleDateTime(2024, 12, 25, 10, 30);
            Console.WriteLine($"Christmas 2024: {timestamp}");

            // Financial values
            Console.WriteLine("\n4. Financial Values:");
            var price = new Money(99.99m, "USD");
            var discount = new Money(10.00m, "USD");
            var finalPrice = price - discount;
            Console.WriteLine($"Price: {price}");
            Console.WriteLine($"Discount: {discount}");
            Console.WriteLine($"Final: {finalPrice}");

            // Guidelines
            Console.WriteLine("\nWhen to use structs:");
            Console.WriteLine("✓ Small, simple data (< 16 bytes recommended)");
            Console.WriteLine("✓ Immutable value types");
            Console.WriteLine("✓ No need for reference semantics");
            Console.WriteLine("✓ Mathematical concepts (points, vectors, colors)");
            Console.WriteLine("✓ High-performance scenarios");

            Console.WriteLine("\nWhen to avoid structs:");
            Console.WriteLine("✗ Large, complex data structures");
            Console.WriteLine("✗ Need for inheritance");
            Console.WriteLine("✗ Mutable types that get passed around");
            Console.WriteLine("✗ Reference semantics required");
        }

        /// <summary>
        /// Helper method for readonly reference demonstration
        /// </summary>
        static void ProcessReadOnlyRectangle(in Rectangle rect)
        {
            // 'in' parameter prevents copying and modification
            Console.WriteLine($"Processing rectangle: {rect.Width}x{rect.Height}, Area: {rect.Area}");
            // rect.Width = 100; // Would cause compile error
        }

        /// <summary>
        /// Helper method for ref struct demonstration
        /// </summary>
        static void ProcessStackData(StackOnlyPoint point)
        {
            Console.WriteLine($"Processing stack data: X={point.X}, Y={point.Y}");
            // This method receives the ref struct by value (copied)
            // But ref structs cannot escape to heap, so they're still stack-only
        }
    }
}

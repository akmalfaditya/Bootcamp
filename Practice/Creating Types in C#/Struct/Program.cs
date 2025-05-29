namespace StructDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Structs: Value Types in Action ===\n");

            // Let's explore each concept with hands-on examples
            DemonstrateValueTypeVsReferenceType();
            DemonstrateNoInheritance();
            DemonstrateNoVirtualMembers();
            DemonstrateConstructorBehavior();
            DemonstrateReadOnlyStructs();
            DemonstrateReadOnlyMethods();
            DemonstrateRefStructs();
            DemonstratePerformanceComparison();
            DemonstrateCommonUseCases();

            Console.WriteLine("\n=== Structs Demo Complete! ===");
            Console.ReadKey();
        }

        /// <summary>
        /// Shows the fundamental difference: structs copy values, classes copy references
        /// This is the #1 thing to understand about structs!
        /// </summary>
        static void DemonstrateValueTypeVsReferenceType()
        {
            Console.WriteLine("1. Value Type vs Reference Type:");
            
            // Struct behavior - VALUE SEMANTICS
            Point structPoint1 = new Point { X = 10, Y = 20 };
            Point structPoint2 = structPoint1;  // COPY the values
            structPoint2.X = 99;  // Modify the copy
            
            Console.WriteLine("Struct (Value Type) behavior:");
            Console.WriteLine($"Original point: X={structPoint1.X}, Y={structPoint1.Y}");  // X=10 (unchanged!)
            Console.WriteLine($"Copied point: X={structPoint2.X}, Y={structPoint2.Y}");    // X=99 (changed)
            Console.WriteLine("Notice: changing the copy doesn't affect the original!\n");
            
            // Class behavior - REFERENCE SEMANTICS
            PointClass classPoint1 = new PointClass { X = 10, Y = 20 };
            PointClass classPoint2 = classPoint1;  // COPY the reference
            classPoint2.X = 99;  // Modify through the reference
            
            Console.WriteLine("Class (Reference Type) behavior:");
            Console.WriteLine($"Original point: X={classPoint1.X}, Y={classPoint1.Y}");  // X=99 (changed!)
            Console.WriteLine($"Referenced point: X={classPoint2.X}, Y={classPoint2.Y}"); // X=99 (same object)
            Console.WriteLine("Notice: both variables point to the same object!\n");
            
            // Memory allocation difference
            Console.WriteLine("Memory allocation:");
            Console.WriteLine("Struct: Lives on the stack (faster allocation/deallocation)");
            Console.WriteLine("Class: Lives on the heap (garbage collected)");
            Console.WriteLine();
        }

        /// <summary>
        /// Demonstrates that structs cannot inherit from other structs or classes
        /// They can only implement interfaces - that's their only "inheritance"
        /// </summary>
        static void DemonstrateNoInheritance()
        {
            Console.WriteLine("2. No Inheritance in Structs:");
            
            Console.WriteLine("✅ Structs can implement interfaces:");
            var movablePoint = new MovablePoint { X = 5, Y = 10 };
            Console.WriteLine($"Before move: {movablePoint}");
            
            // Using interface method
            movablePoint.Move(3, 4);
            Console.WriteLine($"After move: {movablePoint}");
            
            Console.WriteLine("\n❌ But structs CANNOT inherit from other structs or classes:");
            Console.WriteLine("// struct ColoredPoint : Point { } // COMPILER ERROR!");
            Console.WriteLine("// struct MyStruct : SomeClass { } // COMPILER ERROR!");
            
            Console.WriteLine("\nWhy? Structs are meant to be simple value containers.");
            Console.WriteLine("If you need inheritance, use classes instead!");
            Console.WriteLine();
        }

        /// <summary>
        /// Shows that structs cannot have virtual, abstract, or protected members
        /// This keeps them simple and prevents the complexity of polymorphism
        /// </summary>
        static void DemonstrateNoVirtualMembers()
        {
            Console.WriteLine("3. No Virtual Members:");
            
            var shape = new SimpleShape { Width = 10, Height = 5 };
            Console.WriteLine($"Shape area: {shape.CalculateArea()}");
            
            Console.WriteLine("\n❌ Structs cannot have:");
            Console.WriteLine("• virtual methods (no polymorphism)");
            Console.WriteLine("• abstract methods (can't be inherited anyway)");
            Console.WriteLine("• protected members (no inheritance, so no point)");
            Console.WriteLine("• finalizers (they're value types, no cleanup needed)");
            
            Console.WriteLine("\n✅ Structs CAN have:");
            Console.WriteLine("• public methods and properties");
            Console.WriteLine("• private methods and fields");
            Console.WriteLine("• static members");
            Console.WriteLine("• constructors (with some rules)");
            Console.WriteLine();
        }

        /// <summary>
        /// Explores the tricky world of struct constructors
        /// Default behavior vs custom constructors vs field initializers
        /// </summary>
        static void DemonstrateConstructorBehavior()
        {
            Console.WriteLine("4. Constructor Behavior (This Gets Tricky!):");
            
            // Default constructor behavior
            Console.WriteLine("Default constructor (always available):");
            var defaultPoint = new Point();  // Calls implicit parameterless constructor
            Console.WriteLine($"Default Point: X={defaultPoint.X}, Y={defaultPoint.Y}");  // Both 0
            
            // Using default keyword
            var defaultKeywordPoint = default(Point);
            Console.WriteLine($"Default keyword Point: X={defaultKeywordPoint.X}, Y={defaultKeywordPoint.Y}");  // Both 0
            
            // Modern struct with field initializers and custom constructor
            Console.WriteLine("\nModern struct with field initializers (C# 10+):");
            var modernPoint1 = new ModernPoint();  // Uses custom constructor
            var modernPoint2 = default(ModernPoint);  // Uses default values
            
            Console.WriteLine($"new ModernPoint(): X={modernPoint1.X}, Y={modernPoint1.Y}");  // X=1, Y=1
            Console.WriteLine($"default(ModernPoint): X={modernPoint2.X}, Y={modernPoint2.Y}");  // X=0, Y=0
            
            Console.WriteLine("\nKey lesson: 'new' and 'default' can give different results!");
            Console.WriteLine("'new' calls your constructor, 'default' gives zero-initialized values");
            
            // Custom constructor
            var customPoint = new Point(100, 200);
            Console.WriteLine($"Custom constructor: X={customPoint.X}, Y={customPoint.Y}");
            Console.WriteLine();
        }

        /// <summary>
        /// Demonstrates readonly structs for immutability
        /// Once created, these structs cannot be modified - great for thread safety!
        /// </summary>
        static void DemonstrateReadOnlyStructs()
        {
            Console.WriteLine("5. ReadOnly Structs (Immutable Value Types):");
            
            var immutablePoint = new ImmutablePoint(15, 25);
            Console.WriteLine($"Immutable point: {immutablePoint}");
            
            // This would cause a compiler error:
            // immutablePoint.X = 50;  // Error: cannot modify readonly field
            
            Console.WriteLine("Benefits of readonly structs:");
            Console.WriteLine("✅ Thread-safe (can't be modified)");
            Console.WriteLine("✅ Compiler optimizations (no defensive copying)");
            Console.WriteLine("✅ Clear intent (this value won't change)");
            Console.WriteLine("✅ Prevents accidental mutations");
            
            // Demonstrate immutable operations
            var movedPoint = immutablePoint.Move(5, 5);  // Returns new instance
            Console.WriteLine($"Original: {immutablePoint}");  // Unchanged
            Console.WriteLine($"Moved: {movedPoint}");          // New instance
            
            Console.WriteLine("\nImmutable pattern: operations return NEW instances instead of modifying existing ones");
            Console.WriteLine();
        }

        /// <summary>
        /// Shows readonly methods that promise not to modify the struct
        /// Useful for large structs to avoid unnecessary copying
        /// </summary>
        static void DemonstrateReadOnlyMethods()
        {
            Console.WriteLine("6. ReadOnly Methods:");
            
            var rectangle = new Rectangle { Width = 10, Height = 5 };
            
            // Readonly methods don't modify the struct
            double area = rectangle.CalculateArea();        // readonly method
            double perimeter = rectangle.CalculatePerimeter(); // readonly method
            string info = rectangle.GetInfo();              // readonly method
            
            Console.WriteLine($"Rectangle: {rectangle}");
            Console.WriteLine($"Area: {area}");
            Console.WriteLine($"Perimeter: {perimeter}");
            Console.WriteLine($"Info: {info}");
            
            Console.WriteLine("\nReadonly methods guarantee:");
            Console.WriteLine("✅ Won't modify any fields");
            Console.WriteLine("✅ Can be called on readonly references");
            Console.WriteLine("✅ Prevent defensive copying in some scenarios");
            Console.WriteLine("✅ Make your intent clear to other developers");
            
            // You can call readonly methods on readonly references
            ReadOnlyMethodsDemo(rectangle);
            Console.WriteLine();
        }

        static void ReadOnlyMethodsDemo(in Rectangle rect)  // 'in' = readonly reference
        {
            // Can call readonly methods on 'in' parameters
            Console.WriteLine($"Readonly reference area: {rect.CalculateArea()}");
        }

        /// <summary>
        /// Explores ref structs - stack-only structs for high-performance scenarios
        /// These are special structs that MUST live on the stack
        /// </summary>
        static void DemonstrateRefStructs()
        {
            Console.WriteLine("7. Ref Structs (Stack-Only Structs):");
            
            var stackPoint = new StackOnlyPoint { X = 42, Y = 84 };
            Console.WriteLine($"Stack-only point: {stackPoint.X}, {stackPoint.Y}");
            
            // Process some data with ref struct
            ProcessStackData(stackPoint);
            
            Console.WriteLine("\nRef struct characteristics:");
            Console.WriteLine("✅ MUST live on the stack (never heap)");
            Console.WriteLine("✅ Cannot be boxed to object");
            Console.WriteLine("✅ Cannot be array elements (arrays live on heap)");
            Console.WriteLine("✅ Cannot be fields in classes");
            Console.WriteLine("✅ Perfect for high-performance scenarios");
            
            Console.WriteLine("\nReal-world examples:");
            Console.WriteLine("• Span<T> and ReadOnlySpan<T>");
            Console.WriteLine("• Memory manipulation utilities");
            Console.WriteLine("• High-frequency trading systems");
            Console.WriteLine("• Game engines (tight memory control)");
            
            // These would cause compiler errors:
            // object boxed = stackPoint;  // Error: cannot box ref struct
            // var array = new StackOnlyPoint[10];  // Error: cannot create arrays
            Console.WriteLine();
        }

        static void ProcessStackData(StackOnlyPoint point)
        {
            Console.WriteLine($"Processing stack data: X={point.X}, Y={point.Y}");
            // Imagine high-performance calculations here
        }

        /// <summary>
        /// Compares performance between structs and classes
        /// Shows when to choose one over the other
        /// </summary>
        static void DemonstratePerformanceComparison()
        {
            Console.WriteLine("8. Performance Comparison:");
            
            const int iterations = 1000;
            
            Console.WriteLine($"Creating {iterations} instances...");
            
            // Struct performance test
            var start = DateTime.Now;
            for (int i = 0; i < iterations; i++)
            {
                var point = new Point { X = i, Y = i * 2 };
                // Structs live on stack - very fast allocation
            }
            var structTime = DateTime.Now - start;
            
            // Class performance test
            start = DateTime.Now;
            for (int i = 0; i < iterations; i++)
            {
                var point = new PointClass { X = i, Y = i * 2 };
                // Classes need heap allocation and GC tracking
            }
            var classTime = DateTime.Now - start;
            
            Console.WriteLine($"Struct creation time: {structTime.TotalMilliseconds:F2}ms");
            Console.WriteLine($"Class creation time: {classTime.TotalMilliseconds:F2}ms");
            
            Console.WriteLine("\nPerformance guidelines:");
            Console.WriteLine("✅ Use structs for: small, simple data (like Point, Color, DateTime)");
            Console.WriteLine("✅ Use classes for: complex objects, inheritance, large data structures");
            Console.WriteLine("⚠️  Avoid: large structs (copying becomes expensive)");
            Console.WriteLine("⚠️  Avoid: structs with reference type fields (defeats the purpose)");
            Console.WriteLine();
        }

        /// <summary>
        /// Shows common real-world use cases for structs
        /// When structs shine vs when classes are better
        /// </summary>
        static void DemonstrateCommonUseCases()
        {
            Console.WriteLine("9. Common Use Cases for Structs:");
            
            // Mathematical coordinates
            var point = new Point(10, 20);
            var vector = new Vector2D(3.5f, 4.2f);
            Console.WriteLine($"Point: {point}");
            Console.WriteLine($"Vector: {vector}");
            
            // Color representation
            var red = new Color(255, 0, 0);
            var blue = new Color(0, 0, 255);
            Console.WriteLine($"Red: {red}");
            Console.WriteLine($"Blue: {blue}");
            
            // Date/time values (like built-in DateTime)
            var timestamp = new SimpleDateTime(2024, 12, 25, 10, 30);
            Console.WriteLine($"Timestamp: {timestamp}");
            
            // Money/currency (precision matters)
            var price = new Money(99.99m, "USD");
            var discount = new Money(10.00m, "USD");
            var final = price.Subtract(discount);
            Console.WriteLine($"Price: {price}");
            Console.WriteLine($"Final after discount: {final}");
            
            Console.WriteLine("\nPerfect struct candidates:");
            Console.WriteLine("✅ Coordinates (Point, Vector)");
            Console.WriteLine("✅ Colors (RGB, RGBA)");
            Console.WriteLine("✅ Dates and times");
            Console.WriteLine("✅ Money and currency");
            Console.WriteLine("✅ Complex numbers");
            Console.WriteLine("✅ Ranges and spans");
            
            Console.WriteLine("\nAvoid structs for:");
            Console.WriteLine("❌ Objects with identity");
            Console.WriteLine("❌ Large, complex data");
            Console.WriteLine("❌ Mutable collections");
            Console.WriteLine("❌ Objects needing inheritance");
            Console.WriteLine();
        }
    }
}

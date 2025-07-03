namespace Inheritance
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== COMPREHENSIVE C# INHERITANCE CONCEPTS DEMONSTRATION ===");
            Console.WriteLine("This program demonstrates all major inheritance concepts in C#");
            Console.WriteLine("Based on professional training material and real-world examples\n");

            try
            {
                // 1. Basic Inheritance - Foundation concepts
                Console.WriteLine("Press any key to start with BASIC INHERITANCE...");
                Console.ReadKey();
                Console.Clear();
                BasicInheritanceDemo.RunDemo();
                
                Console.WriteLine("\nPress any key to continue to POLYMORPHISM...");
                Console.ReadKey();
                Console.Clear();
                
                // 2. Polymorphism - Many forms, one interface
                PolymorphismDemoClass.RunDemo();
                
                Console.WriteLine("\nPress any key to continue to CASTING AND CONVERSIONS...");
                Console.ReadKey();
                Console.Clear();
                
                // 3. Casting and Reference Conversions
                CastingAndConversionsDemo.RunDemo();
                
                Console.WriteLine("\nPress any key to continue to VIRTUAL METHODS AND OVERRIDING...");
                Console.ReadKey();
                Console.Clear();
                
                // 4. Virtual Methods and Overriding
                VirtualOverrideDemo.RunDemo();
                
                Console.WriteLine("\nPress any key to continue to MEMBER HIDING...");
                Console.ReadKey();
                Console.Clear();
                
                // 5. Member Hiding with 'new' keyword
                MemberHidingDemo.RunDemo();
                
                Console.WriteLine("\nPress any key to continue to BASE KEYWORD...");
                Console.ReadKey();
                Console.Clear();
                
                // 6. Base keyword usage
                BaseKeywordDemo.RunDemo();
                
                Console.WriteLine("\nPress any key to continue to CONSTRUCTOR INHERITANCE...");
                Console.ReadKey();
                Console.Clear();
                
                // 7. Constructor Inheritance and Required Members
                ConstructorInheritanceDemo.RunDemo();
                
                Console.WriteLine("\nPress any key to continue to SEALED CONCEPTS...");
                Console.ReadKey();
                Console.Clear();
                
                // 8. Sealed Classes and Methods
                SealedDemo.RunDemo();
                
                Console.WriteLine("\nPress any key to continue to OVERLOAD RESOLUTION...");
                Console.ReadKey();
                Console.Clear();
                
                // 9. Overload Resolution with Inheritance
                OverloadResolutionDemo.RunDemo();
                
                Console.Clear();
                Console.WriteLine("=== ALL INHERITANCE DEMONSTRATIONS COMPLETED ===");
                Console.WriteLine();
                Console.WriteLine("CONCEPTS COVERED:");
                Console.WriteLine("✓ Basic Inheritance - Creating class hierarchies");
                Console.WriteLine("✓ Polymorphism - One interface, many implementations");
                Console.WriteLine("✓ Casting & Conversions - Upcasting, downcasting, 'as', 'is' operators");
                Console.WriteLine("✓ Virtual Methods - Overriding behavior in derived classes");
                Console.WriteLine("✓ Member Hiding - Using 'new' vs 'override'");
                Console.WriteLine("✓ Base Keyword - Accessing base class members");
                Console.WriteLine("✓ Constructor Inheritance - Chaining and required members");
                Console.WriteLine("✓ Sealed Classes/Methods - Preventing further inheritance");
                Console.WriteLine("✓ Overload Resolution - How compiler chooses methods");
                Console.WriteLine();
                Console.WriteLine("These demonstrations show real-world usage patterns and best practices");
                Console.WriteLine("for object-oriented programming with C# inheritance.");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during demonstration: {ex.Message}");
                Console.WriteLine("This might be due to missing dependencies or runtime issues.");
            }
            
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}

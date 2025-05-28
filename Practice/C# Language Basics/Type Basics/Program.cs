

// Using directive: Imports the Animals namespace so we can use the Dog class
// This demonstrates how namespaces help organize code and avoid naming conflicts
// Without this using statement, we would need to write "Animals.Dog" instead of just "Dog"
using Animals;

// Namespace declaration: Groups related classes and types together
// Namespaces provide a hierarchical organization system for code
// They help prevent naming conflicts when multiple libraries have similar class names
namespace TypesDemo
{
    // Custom Class Definition: UnitConverter
    // This is an example of a user-defined class that demonstrates several key concepts:
    // 1. Encapsulation: Private field with public methods to access it
    // 2. Constructor: Special method to initialize object state
    // 3. Instance methods: Methods that operate on specific object instances
    // 4. Data hiding: Internal implementation (ratio field) is hidden from external code
    public class UnitConverter
    {
        // Private field: Stores the conversion ratio for this converter instance
        // Fields are variables that belong to each instance of the class
        // Private access modifier means this field can only be accessed within this class
        // This demonstrates encapsulation - hiding internal implementation details
        int ratio;

        // Constructor: Special method that runs when an object is created
        // Constructors are used to initialize the object's state with starting values
        // The constructor name must match the class name exactly
        // Parameters allow customization of how each object is initialized
        public UnitConverter(int unitRatio)  
        {
            // Initialize the private field with the provided parameter value
            // This sets up the conversion ratio that will be used by this specific instance
            ratio = unitRatio;
        }

        // Instance method: A method that operates on a specific object instance
        // This method can access the instance's private fields (like 'ratio')
        // Instance methods are called on objects using dot notation: object.Method()
        // The method performs the actual unit conversion using the stored ratio
        public int Convert(int unit)  
        {
            // Return the converted value by multiplying input by the stored ratio
            // This demonstrates how encapsulated data (ratio) is used internally
            return unit * ratio;
        }
    }    // Custom Class: Panda (demonstrating instance vs static members)
    // This class demonstrates the key difference between instance and static members
    // Instance members belong to each individual object, while static members belong to the class itself
    public class Panda
    {
        // Instance field: Each Panda object has its own Name property
        // This field is unique to each instance and stores data specific to that object
        // When you create multiple Panda objects, each one has its own separate Name value
        public string Name;
        
        // Static field: Shared by all instances of the Panda class
        // This field belongs to the class itself, not to any specific instance
        // All Panda objects share the same Population value - it's a class-level counter
        // Static members exist even before any objects are created
        public static int Population;

        // Constructor: Special method called when creating a new Panda object
        // This method initializes the new object and performs any setup required
        // Notice how it affects both instance data (Name) and static data (Population)
        public Panda(string n)
        {
            // Set the instance-specific name for this particular Panda object
            // Each Panda object will have its own unique name stored in this field
            Name = n;
            
            // Increment the static population counter shared by ALL Panda instances
            // This demonstrates how static fields maintain state across all objects of the class
            // Every time a new Panda is created, the total population increases
            Population++;
        }

        // Instance method: Called on a specific Panda object
        // This method can access both instance fields (Name) and static fields (Population)
        // Instance methods have access to the current object's data via 'this' (implicit)
        public void DisplayInfo()
        {
            // This method demonstrates accessing both types of members:
            // - Name: instance field (specific to this Panda object)
            // - Population: static field (shared across all Panda objects)
            Console.WriteLine($"Panda Name: {Name}, Total Population: {Population}");
        }
    }

    // Value Type: Struct - demonstrates value type behavior
    // Structs are value types, meaning they store data directly and are copied when assigned
    // When you assign one struct to another, you get a complete copy of the data
    // Structs are typically used for small, simple data structures
    public struct Point
    {
        // Public fields to store coordinate values
        // In structs, fields are often public for simplicity
        // These represent the X and Y coordinates of a point in 2D space
        public int X, Y;

        // Struct constructor: Initializes the coordinate values
        // Struct constructors must initialize all fields
        // Unlike classes, structs cannot have parameterless constructors in older C# versions
        public Point(int x, int y)
        {
            // Initialize both coordinate fields with the provided values
            // All fields in a struct must be assigned before the constructor completes
            X = x;
            Y = y;
        }

        // Instance method to display the point's coordinates
        // Even though this is a struct, it can still have methods like classes
        // This method provides a convenient way to output the point's position
        public void DisplayPoint()
        {
            Console.WriteLine($"Point coordinates: ({X}, {Y})");
        }
    }

    // Reference Type: Class version of Point - demonstrates reference type behavior
    // Classes are reference types, meaning variables store references (addresses) to objects
    // When you assign one class variable to another, both point to the same object
    // Classes are used for more complex objects and when you need reference semantics
    public class PointClass
    {
        // Public fields to store coordinate values (same as struct version)
        // The behavior difference comes from the type system, not the field declarations
        public int X, Y;

        // Class constructor: Initializes the coordinate values
        // Class constructors work similarly to struct constructors
        // However, classes support parameterless constructors and inheritance
        public PointClass(int x, int y)
        {
            // Initialize the coordinate fields
            // Classes allow more flexibility in initialization order and patterns
            X = x;
            Y = y;
        }

        // Instance method identical to the struct version
        // The method implementation is the same, but the calling semantics differ
        // When called on a class instance, 'this' refers to the object reference
        public void DisplayPoint()
        {
            Console.WriteLine($"Point coordinates: ({X}, {Y})");
        }
    }    class Program
    {
        // Main method: Entry point of the application
        // This is where program execution begins
        // The static keyword means this method belongs to the class, not to instances
        static void Main()
        {
            Console.WriteLine("=== C# Types Demonstration ===\n");

            // Call demonstration methods to show different aspects of C# types
            // Each method focuses on a specific concept from the educational material

            // 1. Predefined Types and Variables - showing built-in C# types
            DemonstratePredefinedTypes();

            // 2. Custom Types - showing user-defined classes and their usage
            DemonstrateCustomTypes();

            // 3. Instance vs Static Members - showing the difference between instance and class-level members
            DemonstrateInstanceVsStatic();

            // 4. Type Conversions - showing implicit and explicit type conversions
            DemonstrateTypeConversions();

            // 5. Value Types vs Reference Types - showing the fundamental difference in behavior
            DemonstrateValueVsReferenceTypes();

            // 6. Namespaces - showing how namespaces organize code and prevent naming conflicts
            DemonstrateNamespaces();

            Console.WriteLine("\n=== End of Demonstration ===");
        }

        // Method to demonstrate C#'s predefined (built-in) types
        // These are types that come with the .NET framework and don't need to be defined by the user
        static void DemonstratePredefinedTypes()
        {
            Console.WriteLine("1. PREDEFINED TYPES AND VARIABLES");
            Console.WriteLine("----------------------------------");

            // Integer type: Stores whole numbers (32-bit signed integer)
            // Range: -2,147,483,648 to 2,147,483,647
            int x = 12 * 30;
            Console.WriteLine($"Integer calculation: 12 * 30 = {x}");

            // Constant: A value that cannot be changed after initialization
            // Constants are evaluated at compile time and are implicitly static
            const int y = 360;
            Console.WriteLine($"Constant value: {y}");

            // String type: Stores text data (sequence of characters)
            // Strings are reference types but behave like value types due to immutability
            string message = "Hello, world";
            string upperMessage = message.ToUpper(); // ToUpper() creates a new string
            Console.WriteLine($"Original message: {message}");
            Console.WriteLine($"Uppercase message: {upperMessage}");

            // Boolean type: Stores true/false values
            // Used for logical operations and conditional statements
            bool isReady = true;
            if (isReady)
            {
                Console.WriteLine("The process is ready!");
            }

            // Double type: Stores floating-point numbers (64-bit)
            // More precise than float, commonly used for general-purpose decimal numbers
            double price = 19.99;
            Console.WriteLine($"Product price: ${price}");

            Console.WriteLine();
        }

        // Method to demonstrate custom types (user-defined classes)
        // Shows how to create and use objects from custom classes
        static void DemonstrateCustomTypes()
        {
            Console.WriteLine("2. CUSTOM TYPES");
            Console.WriteLine("---------------");

            // Creating an instance of UnitConverter class for feet to inches conversion
            // The constructor parameter (12) sets the conversion ratio
            UnitConverter feetToInchesConverter = new UnitConverter(12);
            int inches = feetToInchesConverter.Convert(30);
            Console.WriteLine($"30 feet = {inches} inches");

            // Creating another instance with a different conversion ratio
            // Each instance maintains its own state (ratio value)
            UnitConverter metersToFeetConverter = new UnitConverter(3);
            int feet = metersToFeetConverter.Convert(10);
            Console.WriteLine($"10 meters ≈ {feet} feet (simplified conversion)");

            Console.WriteLine();
        }

        // Method to demonstrate the difference between instance and static members
        // Instance members belong to specific objects, static members belong to the class
        static void DemonstrateInstanceVsStatic()
        {
            Console.WriteLine("3. INSTANCE VS STATIC MEMBERS");
            Console.WriteLine("------------------------------");

            // Creating multiple Panda instances
            // Each instance has its own Name (instance member)
            // All instances share the same Population counter (static member)
            Panda p1 = new Panda("Pan Dee");
            Panda p2 = new Panda("Pan Dah");
            Panda p3 = new Panda("Bamboo");

            // Display individual panda info
            // Each call shows the instance's name and the shared population count
            p1.DisplayInfo();
            p2.DisplayInfo();
            p3.DisplayInfo();

            // Accessing static member directly from the class (no instance needed)
            // This demonstrates that static members belong to the class, not instances
            Console.WriteLine($"Total Panda Population (accessed via class): {Panda.Population}");

            Console.WriteLine();
        }

        // Method to demonstrate type conversions in C#
        // Shows both implicit (automatic) and explicit (manual) conversions
        static void DemonstrateTypeConversions()
        {
            Console.WriteLine("4. TYPE CONVERSIONS");
            Console.WriteLine("-------------------");

            // Implicit conversion (widening conversion) - safe, no data loss
            // Smaller types can be automatically converted to larger types
            // int (32-bit) can be safely converted to long (64-bit)
            int smallNumber = 12345;
            long largeNumber = smallNumber;  // Implicit conversion to larger type
            Console.WriteLine($"Implicit conversion: int {smallNumber} to long {largeNumber}");

            // Explicit conversion (narrowing conversion) - potential data loss
            // Larger types must be explicitly cast to smaller types
            // This may result in data loss if the value doesn't fit
            int originalNumber = 65536;
            short truncatedNumber = (short)originalNumber;  // Explicit conversion with cast operator
            Console.WriteLine($"Explicit conversion: int {originalNumber} to short {truncatedNumber}");
            Console.WriteLine("Note: Data loss occurred due to short's smaller range");

            // Safe explicit conversion - no data loss when value fits
            // Even though explicit casting is required, no data is lost
            long safeLong = 100;
            int safeInt = (int)safeLong;
            Console.WriteLine($"Safe explicit conversion: long {safeLong} to int {safeInt}");

            Console.WriteLine();
        }        // Method to demonstrate the fundamental difference between value types and reference types
        // This is one of the most important concepts in C# type system
        static void DemonstrateValueVsReferenceTypes()
        {
            Console.WriteLine("5. VALUE TYPES VS REFERENCE TYPES");
            Console.WriteLine("----------------------------------");

            // Value Types (Struct) - Data is copied when assigned
            // Value types store their data directly in the variable
            // When you assign a value type to another variable, you get a complete copy
            Console.WriteLine("Value Types (Struct):");
            Point p1 = new Point(5, 10);  // Create first Point struct
            Point p2 = p1;  // Copy all data from p1 to p2 (creates independent copy)
            p2.X = 20;      // Modifying p2 doesn't affect p1 because they're separate copies

            Console.Write("p1: ");
            p1.DisplayPoint();  // p1 still has original values
            Console.Write("p2: ");
            p2.DisplayPoint();  // p2 has modified values
            Console.WriteLine("Notice: p1 and p2 have different values (data was copied)\n");

            // Reference Types (Class) - Reference (address) is copied when assigned
            // Reference types store a reference (memory address) to the actual object
            // When you assign a reference type, both variables point to the same object
            Console.WriteLine("Reference Types (Class):");
            PointClass pc1 = new PointClass(5, 10);  // Create object and store reference in pc1
            PointClass pc2 = pc1;  // Copy the reference, not the data (both point to same object)
            pc2.X = 20;            // Modifying through pc2 affects the object that pc1 also points to

            Console.Write("pc1: ");
            pc1.DisplayPoint();  // Shows modified values because pc1 and pc2 point to same object
            Console.Write("pc2: ");
            pc2.DisplayPoint();  // Shows same modified values
            Console.WriteLine("Notice: pc1 and pc2 have the same values (reference was copied)");

            Console.WriteLine();
        }

        // Method to demonstrate namespace usage and organization
        // Namespaces help organize code and prevent naming conflicts
        static void DemonstrateNamespaces()
        {
            Console.WriteLine("6. NAMESPACES");
            Console.WriteLine("-------------");

            // Using class from Animals namespace (imported at top with 'using Animals;')
            // Without the using directive, we would need to write "Animals.Dog"
            // This demonstrates how namespaces organize related classes
            Dog myDog = new Dog("Buddy");
            myDog.DisplayInfo();

            Console.WriteLine("This Dog class comes from the Animals namespace");
            Console.WriteLine("Namespaces help organize code and prevent naming conflicts");

            Console.WriteLine();
        }
    }
}

// Separate namespace to demonstrate namespace usage and organization
// This namespace contains animal-related classes, separate from the main TypesDemo namespace
// Namespaces can be defined in the same file or across multiple files
namespace Animals
{
    // Dog class: Simple example class in the Animals namespace
    // This class demonstrates the same concepts as Panda but in a different namespace
    public class Dog
    {
        // Instance field: Each Dog object has its own unique Name
        // This field stores the specific name for each individual dog
        public string Name;
        
        // Static field: Shared counter across all Dog instances
        // This tracks the total number of Dog objects that have been created
        // Static fields belong to the class, not to individual instances
        public static int TotalDogs = 0;

        // Constructor: Initializes new Dog object with a name
        // This method runs every time a new Dog object is created
        public Dog(string name)
        {
            // Set the instance-specific name for this dog
            Name = name;
            
            // Increment the class-level counter of total dogs created
            // This demonstrates static field usage across all instances
            TotalDogs++;
        }

        // Instance method: Displays information about this specific dog
        // Shows both instance data (Name) and static data (TotalDogs)
        public void DisplayInfo()
        {
            Console.WriteLine($"Dog Name: {Name}, Total Dogs Created: {TotalDogs}");
        }
    }
}


// ============================================================================
// COMPREHENSIVE C# TYPE BASICS DEMONSTRATION
// ============================================================================
// This project demonstrates all fundamental C# type concepts including:
// - Predefined types (int, string, bool) and their characteristics
// - Custom types (classes and structs) with constructors and members
// - Instance vs Static members and their different behaviors
// - Value types vs Reference types and memory implications
// - Type conversions (implicit and explicit)
// - Constructors and object instantiation
// - Null references and their behavior
// - Storage overhead and type taxonomy
// - Program entry points (Main method and top-level statements)
// - Namespace organization and type visibility
// ============================================================================

#nullable disable  // Disable nullable warnings for this educational demo

using System;
using Animals;  // Import Animals namespace to use Dog class

namespace TypeBasicsDemo
{
    /// <summary>
    /// Main program class demonstrating C# type system fundamentals.
    /// This class shows how types work as blueprints for values.
    /// </summary>
    public class Program
    {
        // ====================================================================
        // MAIN ENTRY POINT
        // ====================================================================
        
        /// <summary>
        /// Program entry point - demonstrates type system concepts
        /// </summary>
        static void Main()
        {
            Console.WriteLine("============================================================================");
            Console.WriteLine("C# TYPE BASICS COMPREHENSIVE DEMONSTRATION");
            Console.WriteLine("============================================================================");
            Console.WriteLine();
            
            // Demonstrate each type concept with clear explanations
            DemonstratePredefinedTypes();
            DemonstrateCustomTypes();
            DemonstrateInstanceVsStaticMembers();
            DemonstrateValueVsReferenceTypes();
            DemonstrateTypeConversions();
            DemonstrateNullReferences();
            DemonstrateStorageOverhead();
            DemonstrateTypeTaxonomy();
            DemonstrateNamespaceOrganization();
            DemonstrateRealWorldExample();
            
            Console.WriteLine();
            Console.WriteLine("============================================================================");
            Console.WriteLine("Type system demonstration completed successfully!");
            Console.WriteLine("============================================================================");
        }
        
        // ====================================================================
        // SECTION 1: PREDEFINED TYPES
        // ====================================================================
        
        /// <summary>
        /// Demonstrates C#'s built-in predefined types
        /// </summary>
        static void DemonstratePredefinedTypes()
        {
            Console.WriteLine("1. PREDEFINED TYPES (BUILT-IN TYPES)");
            Console.WriteLine("====================================");
            Console.WriteLine();
            
            // ----------------------------------------------------------------
            // INT TYPE - 32-bit signed integers
            // ----------------------------------------------------------------
            Console.WriteLine("--- int Type (32-bit signed integer) ---");
            
            // Variables can change their values over time
            int x = 12 * 30;  // Expression with int literals
            Console.WriteLine($"Variable calculation: 12 * 30 = {x}");
            
            // Constants represent immutable values
            const int y = 360;  // Constant cannot be changed after declaration
            Console.WriteLine($"Constant value: {y}");
            
            // int supports arithmetic operations
            int a = 15, b = 4;
            Console.WriteLine($"Arithmetic operations: {a} + {b} = {a + b}");
            Console.WriteLine($"                      {a} - {b} = {a - b}");
            Console.WriteLine($"                      {a} * {b} = {a * b}");
            Console.WriteLine($"                      {a} / {b} = {a / b}");
            Console.WriteLine($"                      {a} % {b} = {a % b}");
            
            // Range demonstration
            Console.WriteLine($"int range: {int.MinValue:N0} to {int.MaxValue:N0}");
            
            // ----------------------------------------------------------------
            // STRING TYPE - sequence of characters
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- string Type (sequence of characters) ---");
            
            // Strings are immutable - operations create new strings
            string message = "Hello world";
            string upperMessage = message.ToUpper();  // Creates new string
            Console.WriteLine($"Original message: '{message}'");
            Console.WriteLine($"Uppercase message: '{upperMessage}'");
            
            // String concatenation with ToString() method
            int year = 2022;
            message = message + year.ToString();  // ToString() available on all types
            Console.WriteLine($"Concatenated message: '{message}'");
            
            // String immutability demonstration
            string original = "Programming";
            string modified = original.Substring(0, 7);  // Creates new string
            Console.WriteLine($"Original string unchanged: '{original}'");
            Console.WriteLine($"New substring: '{modified}'");
            
            // ----------------------------------------------------------------
            // BOOL TYPE - true/false values
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- bool Type (true/false values) ---");
            
            // Basic boolean values
            bool simpleVar = false;
            if (simpleVar)
            {
                Console.WriteLine("This will not print");
            }
            else
            {
                Console.WriteLine("Boolean false prevents execution of if block");
            }
            
            // Boolean from comparison
            int distance = 5000;
            bool lessThanAMile = distance < 5280;  // 5280 feet in a mile
            if (lessThanAMile)
            {
                Console.WriteLine($"{distance} feet is less than a mile");
            }
            
            // Complex boolean expressions
            bool isValid = distance > 0 && distance < 10000;
            Console.WriteLine($"Distance {distance} is valid (0 < x < 10000): {isValid}");
            
            Console.WriteLine();
        }
        
        // ====================================================================
        // SECTION 2: CUSTOM TYPES
        // ====================================================================
        
        /// <summary>
        /// Demonstrates user-defined custom types
        /// </summary>
        static void DemonstrateCustomTypes()
        {
            Console.WriteLine("2. CUSTOM TYPES (USER-DEFINED)");
            Console.WriteLine("==============================");
            Console.WriteLine();
            
            // ----------------------------------------------------------------
            // UNITCONVERTER CLASS DEMONSTRATION
            // ----------------------------------------------------------------
            Console.WriteLine("--- UnitConverter Class ---");
            
            // Creating instances with different conversion ratios
            UnitConverter feetToInchesConverter = new UnitConverter(12);
            UnitConverter milesToFeetConverter = new UnitConverter(5280);
            
            Console.WriteLine($"Converting 30 feet to inches: {feetToInchesConverter.Convert(30)}");
            Console.WriteLine($"Converting 100 feet to inches: {feetToInchesConverter.Convert(100)}");
            
            // Chaining conversions - demonstrates object interaction
            int oneMileInInches = feetToInchesConverter.Convert(milesToFeetConverter.Convert(1));
            Console.WriteLine($"1 mile in inches: {oneMileInInches}");
            
            // ----------------------------------------------------------------
            // CUSTOM TYPE SYMMETRY WITH PREDEFINED TYPES
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Symmetry Between Predefined and Custom Types ---");
            
            // Both predefined and custom types have data and functions
            Console.WriteLine("Predefined int type:");
            int number = 42;  // Data: 32 bits
            string numberAsString = number.ToString();  // Function: ToString()
            Console.WriteLine($"  Data: {number}, Function result: '{numberAsString}'");
            
            Console.WriteLine("Custom UnitConverter type:");
            UnitConverter converter = new UnitConverter(100);  // Data: ratio field
            int converted = converter.Convert(5);  // Function: Convert method
            Console.WriteLine($"  Data: ratio=100, Function result: {converted}");
            
            Console.WriteLine();
        }
        
        // ====================================================================
        // SECTION 3: INSTANCE VS STATIC MEMBERS
        // ====================================================================
        
        /// <summary>
        /// Demonstrates the difference between instance and static members
        /// </summary>
        static void DemonstrateInstanceVsStaticMembers()
        {
            Console.WriteLine("3. INSTANCE VS STATIC MEMBERS");
            Console.WriteLine("=============================");
            Console.WriteLine();
            
            // ----------------------------------------------------------------
            // INSTANCE MEMBERS - belong to specific objects
            // ----------------------------------------------------------------
            Console.WriteLine("--- Instance Members ---");
            
            // Each Panda object has its own Name (instance member)
            Panda p1 = new Panda("Pan Dee");
            Panda p2 = new Panda("Pan Dah");
            
            // Instance members accessed through object references
            Console.WriteLine($"Panda 1 name: {p1.Name}");  // Instance field access
            Console.WriteLine($"Panda 2 name: {p2.Name}");  // Instance field access
            
            // Instance methods called on specific objects
            p1.DisplayInfo();  // Instance method call
            p2.DisplayInfo();  // Instance method call
            
            // ----------------------------------------------------------------
            // STATIC MEMBERS - belong to the type itself
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Static Members ---");
            
            // Static members accessed through type name, not object instances
            Console.WriteLine($"Total panda population: {Panda.Population}");  // Static field access
            
            // Static method example (Console.WriteLine is static)
            Console.WriteLine("Console.WriteLine is a static method - called on Console type, not instance");
            
            // Demonstrating why you can't access static members through instances
            // This would cause a compile error: p1.Population
            Console.WriteLine("Note: You cannot access static members through instance variables");
            
            Console.WriteLine();
        }
        
        // ====================================================================
        // SECTION 4: VALUE TYPES VS REFERENCE TYPES
        // ====================================================================
        
        /// <summary>
        /// Demonstrates the fundamental difference between value and reference types
        /// </summary>
        static void DemonstrateValueVsReferenceTypes()
        {
            Console.WriteLine("4. VALUE TYPES VS REFERENCE TYPES");
            Console.WriteLine("=================================");
            Console.WriteLine();
            
            // ----------------------------------------------------------------
            // VALUE TYPES - store data directly
            // ----------------------------------------------------------------
            Console.WriteLine("--- Value Types (struct) ---");
            
            // Value type assignment copies the entire instance
            Point p1 = new Point();
            p1.X = 7;
            Point p2 = p1;  // Copies all data from p1 to p2
            
            Console.WriteLine($"After assignment - p1.X: {p1.X}, p2.X: {p2.X}");
            
            // Modifying one doesn't affect the other (independent storage)
            p1.X = 9;
            Console.WriteLine($"After p1.X change - p1.X: {p1.X}, p2.X: {p2.X}");
            Console.WriteLine("Value types have independent storage locations");
            
            // ----------------------------------------------------------------
            // REFERENCE TYPES - store references to objects
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Reference Types (class) ---");
            
            // Reference type assignment copies the reference, not the object
            PointClass pc1 = new PointClass();
            pc1.X = 7;
            PointClass pc2 = pc1;  // Copies reference to same object
            
            Console.WriteLine($"After assignment - pc1.X: {pc1.X}, pc2.X: {pc2.X}");
            
            // Modifying through one reference affects the other (same object)
            pc1.X = 9;
            Console.WriteLine($"After pc1.X change - pc1.X: {pc1.X}, pc2.X: {pc2.X}");
            Console.WriteLine("Reference types share the same object in memory");
            
            // ----------------------------------------------------------------
            // MEMORY BEHAVIOR COMPARISON
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Memory Behavior Summary ---");
            Console.WriteLine("Value types: Assignment copies data (independent objects)");
            Console.WriteLine("Reference types: Assignment copies reference (shared objects)");
            Console.WriteLine("Value types stored on stack, reference types on heap");
            
            Console.WriteLine();
        }
        
        // ====================================================================
        // SECTION 5: TYPE CONVERSIONS
        // ====================================================================
        
        /// <summary>
        /// Demonstrates implicit and explicit type conversions
        /// </summary>
        static void DemonstrateTypeConversions()
        {
            Console.WriteLine("5. TYPE CONVERSIONS");
            Console.WriteLine("==================");
            Console.WriteLine();
            
            // ----------------------------------------------------------------
            // IMPLICIT CONVERSIONS - automatic and safe
            // ----------------------------------------------------------------
            Console.WriteLine("--- Implicit Conversions ---");
            
            // Widening conversions are implicit (no information loss)
            int smallInt = 12345;
            long largeLong = smallInt;  // Implicit: int to long (32-bit to 64-bit)
            Console.WriteLine($"int {smallInt} implicitly converted to long {largeLong}");
            
            // More implicit conversion examples
            byte byteValue = 100;
            int intFromByte = byteValue;  // Implicit: byte to int
            Console.WriteLine($"byte {byteValue} implicitly converted to int {intFromByte}");
            
            float floatValue = 3.14f;
            double doubleValue = floatValue;  // Implicit: float to double
            Console.WriteLine($"float {floatValue} implicitly converted to double {doubleValue}");
            
            // ----------------------------------------------------------------
            // EXPLICIT CONVERSIONS (CASTING) - manual and potentially unsafe
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Explicit Conversions (Casting) ---");
            
            // Narrowing conversions require explicit casting (potential data loss)
            int largeInt = 70000;
            short shortValue = (short)largeInt;  // Explicit: int to short
            Console.WriteLine($"int {largeInt} explicitly cast to short {shortValue}");
            Console.WriteLine("Note: Data loss occurred - short range is -32,768 to 32,767");
            
            // Safe explicit casting (no data loss)
            long safeLong = 1000;
            int safeInt = (int)safeLong;  // Explicit but safe
            Console.WriteLine($"long {safeLong} safely cast to int {safeInt}");
            
            // Floating point to integer conversion
            double pi = 3.14159;
            int truncatedPi = (int)pi;  // Explicit: double to int (truncates decimal)
            Console.WriteLine($"double {pi} cast to int {truncatedPi} (decimal part lost)");
            
            // ----------------------------------------------------------------
            // CONVERSION RULES
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Conversion Rules ---");
            Console.WriteLine("Implicit: Compiler guarantees success and no data loss");
            Console.WriteLine("Explicit: Programmer takes responsibility for potential data loss");
            Console.WriteLine("Prohibited: Conversions that would always fail");
            
            Console.WriteLine();
        }
        
        // ====================================================================
        // SECTION 6: NULL REFERENCES
        // ====================================================================
        
        /// <summary>
        /// Demonstrates null references and their behavior
        /// </summary>
        static void DemonstrateNullReferences()
        {
            Console.WriteLine("6. NULL REFERENCES");
            Console.WriteLine("=================");
            Console.WriteLine();
            
            // ----------------------------------------------------------------
            // NULL WITH REFERENCE TYPES
            // ----------------------------------------------------------------
            Console.WriteLine("--- Null with Reference Types ---");
            
            // Reference types can be assigned null
            PointClass nullPoint = null;
            Console.WriteLine($"nullPoint == null: {nullPoint == null}");
            
            // Safe null checking before use
            if (nullPoint != null)
            {
                Console.WriteLine($"Point X: {nullPoint.X}");
            }
            else
            {
                Console.WriteLine("Cannot access members of null reference");
            }
            
            // Null reference assignment
            PointClass validPoint = new PointClass();
            validPoint.X = 10;
            Console.WriteLine($"Valid point X: {validPoint.X}");
            
            validPoint = null;  // Now points to nothing
            Console.WriteLine($"After null assignment: validPoint == null is {validPoint == null}");
            
            // ----------------------------------------------------------------
            // NULL WITH VALUE TYPES
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Null with Value Types ---");
            
            // Regular value types cannot be null
            Console.WriteLine("Regular value types (int, bool, struct) cannot be null");
            
            // Nullable value types (covered in later chapters)
            int? nullableInt = null;  // Nullable value type
            Console.WriteLine($"Nullable int can be null: {nullableInt == null}");
            
            nullableInt = 42;
            Console.WriteLine($"Nullable int with value: {nullableInt}");
            
            Console.WriteLine();
        }
        
        // ====================================================================
        // SECTION 7: STORAGE OVERHEAD
        // ====================================================================
        
        /// <summary>
        /// Demonstrates storage overhead differences between value and reference types
        /// </summary>
        static void DemonstrateStorageOverhead()
        {
            Console.WriteLine("7. STORAGE OVERHEAD");
            Console.WriteLine("==================");
            Console.WriteLine();
            
            // ----------------------------------------------------------------
            // VALUE TYPE STORAGE
            // ----------------------------------------------------------------
            Console.WriteLine("--- Value Type Storage ---");
            
            // Value types consume memory equal to their fields
            Console.WriteLine("Value types consume memory precisely equal to their fields:");
            Console.WriteLine($"int: {sizeof(int)} bytes");
            Console.WriteLine($"double: {sizeof(double)} bytes");
            Console.WriteLine($"bool: {sizeof(bool)} byte");
            Console.WriteLine($"char: {sizeof(char)} bytes");
            
            // Point struct would be 8 bytes (2 × int fields)
            Console.WriteLine("Point struct (2 int fields): 8 bytes total");
            
            // ----------------------------------------------------------------
            // REFERENCE TYPE STORAGE
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Reference Type Storage ---");
            
            Console.WriteLine("Reference types have additional overhead:");
            Console.WriteLine("• Reference storage: 4 or 8 bytes (depending on platform)");
            Console.WriteLine("• Object overhead: at least 8 bytes for runtime information");
            Console.WriteLine("• Field storage: space for actual field data");
            Console.WriteLine();
            Console.WriteLine("Example: PointClass with 2 int fields");
            Console.WriteLine("• Reference: 8 bytes (64-bit platform)");
            Console.WriteLine("• Object overhead: 8+ bytes");
            Console.WriteLine("• Fields: 8 bytes (2 × int)");
            Console.WriteLine("• Total: 24+ bytes vs 8 bytes for value type");
            
            // ----------------------------------------------------------------
            // PERFORMANCE IMPLICATIONS
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Performance Implications ---");
            Console.WriteLine("Value types: Direct access, no indirection, better cache locality");
            Console.WriteLine("Reference types: Indirect access, heap allocation, garbage collection");
            Console.WriteLine("Choose based on data size, mutability, and usage patterns");
            
            Console.WriteLine();
        }
        
        // ====================================================================
        // SECTION 8: PREDEFINED TYPE TAXONOMY
        // ====================================================================
        
        /// <summary>
        /// Demonstrates the complete taxonomy of predefined types
        /// </summary>
        static void DemonstrateTypeTaxonomy()
        {
            Console.WriteLine("8. PREDEFINED TYPE TAXONOMY");
            Console.WriteLine("==========================");
            Console.WriteLine();
            
            // ----------------------------------------------------------------
            // VALUE TYPES TAXONOMY
            // ----------------------------------------------------------------
            Console.WriteLine("--- Value Types ---");
            
            Console.WriteLine("Numeric Types:");
            Console.WriteLine("  Signed Integer: sbyte, short, int, long");
            Console.WriteLine("  Unsigned Integer: byte, ushort, uint, ulong");
            Console.WriteLine("  Real Number: float, double, decimal");
            
            Console.WriteLine("Non-Numeric Value Types:");
            Console.WriteLine("  Logical: bool");
            Console.WriteLine("  Character: char");
            
            // Demonstrate various numeric types
            sbyte signedByte = -100;
            byte unsignedByte = 200;
            short shortInt = -30000;
            ushort unsignedShort = 60000;
            uint unsignedInt = 4000000000;
            ulong unsignedLong = 18000000000000000000;
            float singlePrecision = 3.14f;
            double doublePrecision = 3.14159265359;
            decimal highPrecision = 3.14159265358979323846m;
            
            Console.WriteLine($"\nNumeric type examples:");
            Console.WriteLine($"sbyte: {signedByte}, byte: {unsignedByte}");
            Console.WriteLine($"short: {shortInt}, ushort: {unsignedShort}");
            Console.WriteLine($"uint: {unsignedInt}, ulong: {unsignedLong}");
            Console.WriteLine($"float: {singlePrecision}, double: {doublePrecision}");
            Console.WriteLine($"decimal: {highPrecision}");
            
            // Character and boolean examples
            char letter = 'A';
            bool flag = true;
            Console.WriteLine($"char: {letter}, bool: {flag}");
            
            // ----------------------------------------------------------------
            // REFERENCE TYPES TAXONOMY
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Reference Types ---");
            
            Console.WriteLine("Reference Types:");
            Console.WriteLine("  String: string");
            Console.WriteLine("  Object: object");
            Console.WriteLine("  Arrays: int[], string[], etc.");
            Console.WriteLine("  Classes: user-defined classes");
            Console.WriteLine("  Interfaces: user-defined interfaces");
            Console.WriteLine("  Delegates: method references");
            
            // Demonstrate reference types
            string text = "Hello";
            object obj = "This is an object";
            int[] numbers = { 1, 2, 3, 4, 5 };
            
            Console.WriteLine($"\nReference type examples:");
            Console.WriteLine($"string: {text}");
            Console.WriteLine($"object: {obj}");
            Console.WriteLine($"array: [{string.Join(", ", numbers)}]");
            
            // ----------------------------------------------------------------
            // TYPE ALIASES
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- C# Type Aliases ---");
            Console.WriteLine("C# keywords are aliases for .NET types:");
            Console.WriteLine("int = System.Int32");
            Console.WriteLine("string = System.String");
            Console.WriteLine("bool = System.Boolean");
            Console.WriteLine("object = System.Object");
            
            // Demonstrate equivalence
            int csharpInt = 42;
            System.Int32 dotnetInt = 42;
            Console.WriteLine($"C# int and System.Int32 are identical: {csharpInt.Equals(dotnetInt)}");
            
            Console.WriteLine();
        }
        
        // ====================================================================
        // SECTION 9: NAMESPACE ORGANIZATION
        // ====================================================================
        
        /// <summary>
        /// Demonstrates namespace organization and type visibility
        /// </summary>
        static void DemonstrateNamespaceOrganization()
        {
            Console.WriteLine("9. NAMESPACE ORGANIZATION");
            Console.WriteLine("========================");
            Console.WriteLine();
            
            // ----------------------------------------------------------------
            // USING IMPORTED NAMESPACE
            // ----------------------------------------------------------------
            Console.WriteLine("--- Using Imported Namespace ---");
            
            // Dog class from Animals namespace (imported with 'using Animals;')
            Dog myDog = new Dog("Rex");
            myDog.DisplayInfo();
            
            Console.WriteLine("Dog class accessed from Animals namespace");
            Console.WriteLine("Without 'using Animals;' we would need: Animals.Dog");
            
            // ----------------------------------------------------------------
            // FULLY QUALIFIED NAMES
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Fully Qualified Names ---");
            
            // Access .NET types with full namespace qualification
            System.DateTime currentTime = System.DateTime.Now;
            Console.WriteLine($"Current time using System.DateTime: {currentTime:yyyy-MM-dd HH:mm:ss}");
            
            // Same type accessed via using directive
            DateTime shortTime = DateTime.Now;
            Console.WriteLine($"Current time using DateTime: {shortTime:yyyy-MM-dd HH:mm:ss}");
            
            // ----------------------------------------------------------------
            // NAMESPACE BENEFITS
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Namespace Benefits ---");
            Console.WriteLine("1. Organization: Group related types together");
            Console.WriteLine("2. Avoid conflicts: Multiple types can have same name in different namespaces");
            Console.WriteLine("3. Encapsulation: Control which types are visible to other code");
            Console.WriteLine("4. Scalability: Manage complexity in large applications");
            
            Console.WriteLine();
        }
        
        // ====================================================================
        // SECTION 10: REAL-WORLD EXAMPLE
        // ====================================================================
        
        /// <summary>
        /// Demonstrates all type concepts in a realistic banking scenario
        /// </summary>
        static void DemonstrateRealWorldExample()
        {
            Console.WriteLine("10. REAL-WORLD EXAMPLE: BANKING SYSTEM");
            Console.WriteLine("======================================");
            Console.WriteLine();
            
            // ----------------------------------------------------------------
            // USING VARIOUS TYPE CONCEPTS
            // ----------------------------------------------------------------
            
            Console.WriteLine("Creating a simple banking system using all type concepts:");
            Console.WriteLine();
            
            // Predefined types for account data
            string customerName = "John Smith";     // string type
            int accountNumber = 12345678;           // int type
            decimal balance = 1500.75m;             // decimal type (for money)
            bool isActive = true;                   // bool type
            
            // Custom type for currency conversion
            UnitConverter dollarsToEuros = new UnitConverter(85);  // 85 cents per euro (simplified)
            
            // Static member access
            Console.WriteLine($"Total accounts created: {BankAccount.TotalAccounts}");
            
            // Creating bank account objects (reference type)
            BankAccount account1 = new BankAccount(customerName, accountNumber, balance);
            BankAccount account2 = new BankAccount("Jane Doe", 87654321, 2500.00m);
            
            // Instance member access
            account1.DisplayAccountInfo();
            account2.DisplayAccountInfo();
            
            // Value type for transaction tracking
            TransactionRecord transaction = new TransactionRecord();
            transaction.Amount = 250.50m;
            transaction.Type = "Deposit";
            
            // Demonstrate value type copying
            TransactionRecord transactionCopy = transaction;
            transactionCopy.Amount = 500.00m;  // Doesn't affect original
            
            Console.WriteLine($"Original transaction: {transaction.Type} ${transaction.Amount}");
            Console.WriteLine($"Copied transaction: {transactionCopy.Type} ${transactionCopy.Amount}");
            
            // Type conversions in banking context
            decimal euroBalance = balance / 100 * 85;  // Simplified conversion
            int roundedBalance = (int)balance;  // Explicit conversion to int
            
            Console.WriteLine($"Balance in USD: ${balance:F2}");
            Console.WriteLine($"Balance in EUR: €{euroBalance:F2}");
            Console.WriteLine($"Rounded balance: ${roundedBalance}");
            
            // Null reference handling
            BankAccount closedAccount = null;
            if (closedAccount != null)
            {
                closedAccount.DisplayAccountInfo();
            }
            else
            {
                Console.WriteLine("Account reference is null - cannot display info");
            }
            
            // Static member showing total accounts
            Console.WriteLine($"Total bank accounts created: {BankAccount.TotalAccounts}");
            
            Console.WriteLine();
        }
    }
    
    // ====================================================================
    // SUPPORTING CUSTOM TYPES
    // ====================================================================
    
    /// <summary>
    /// Unit converter class from the original material
    /// Demonstrates custom type definition with encapsulation
    /// </summary>
    public class UnitConverter
    {
        // Private field - encapsulates internal state
        int ratio;
        
        /// <summary>
        /// Constructor - initializes object state
        /// </summary>
        /// <param name="unitRatio">Conversion ratio for this converter</param>
        public UnitConverter(int unitRatio)
        {
            ratio = unitRatio;
        }
        
        /// <summary>
        /// Instance method - operates on specific object instance
        /// </summary>
        /// <param name="unit">Value to convert</param>
        /// <returns>Converted value</returns>
        public int Convert(int unit)
        {
            return unit * ratio;
        }
    }
    
    /// <summary>
    /// Panda class demonstrating instance vs static members
    /// </summary>
    public class Panda
    {
        // Instance field - each panda has its own name
        public string Name;
        
        // Static field - shared by all panda instances
        public static int Population;
        
        /// <summary>
        /// Constructor - initializes new panda and updates population
        /// </summary>
        /// <param name="n">Name for this panda</param>
        public Panda(string n)
        {
            Name = n;               // Set instance field
            Population++;           // Increment static field
        }
        
        /// <summary>
        /// Instance method - displays info for this specific panda
        /// </summary>
        public void DisplayInfo()
        {
            Console.WriteLine($"Panda: {Name}, Total Population: {Population}");
        }
    }
    
    /// <summary>
    /// Point struct - demonstrates value type behavior
    /// </summary>
    public struct Point
    {
        public int X, Y;
        
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public void DisplayPoint()
        {
            Console.WriteLine($"Point: ({X}, {Y})");
        }
    }
    
    /// <summary>
    /// Point class - demonstrates reference type behavior
    /// Same interface as Point struct but different semantics
    /// </summary>
    public class PointClass
    {
        public int X, Y;
        
        public PointClass()
        {
            X = 0;
            Y = 0;
        }
        
        public PointClass(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public void DisplayPoint()
        {
            Console.WriteLine($"PointClass: ({X}, {Y})");
        }
    }
    
    /// <summary>
    /// Bank account class for real-world example
    /// Demonstrates practical use of type concepts
    /// </summary>
    public class BankAccount
    {
        // Instance fields
        public string CustomerName;
        public int AccountNumber;
        public decimal Balance;
        
        // Static field - tracks total accounts created
        public static int TotalAccounts = 0;
        
        /// <summary>
        /// Constructor for bank account
        /// </summary>
        public BankAccount(string name, int accountNum, decimal initialBalance)
        {
            CustomerName = name;
            AccountNumber = accountNum;
            Balance = initialBalance;
            TotalAccounts++;  // Increment static counter
        }
        
        /// <summary>
        /// Display account information
        /// </summary>
        public void DisplayAccountInfo()
        {
            Console.WriteLine($"Account: {CustomerName} #{AccountNumber}, Balance: ${Balance:F2}");
        }
    }
    
    /// <summary>
    /// Transaction record struct - demonstrates value type in business context
    /// </summary>
    public struct TransactionRecord
    {
        public decimal Amount;
        public string Type;
        
        public TransactionRecord(decimal amount, string type)
        {
            Amount = amount;
            Type = type;
        }
    }
}

// ====================================================================
// SEPARATE NAMESPACE DEMONSTRATION
// ====================================================================

/// <summary>
/// Animals namespace - demonstrates namespace organization
/// </summary>
namespace Animals
{
    /// <summary>
    /// Dog class in Animals namespace
    /// Demonstrates namespace usage and organization
    /// </summary>
    public class Dog
    {
        // Instance field
        public string Name;
        
        // Static field
        public static int TotalDogs = 0;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public Dog(string name)
        {
            Name = name;
            TotalDogs++;
        }
        
        /// <summary>
        /// Display dog information
        /// </summary>
        public void DisplayInfo()
        {
            Console.WriteLine($"Dog: {Name}, Total Dogs: {TotalDogs}");
        }
    }
}

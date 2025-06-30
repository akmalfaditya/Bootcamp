

using System.Collections; // For BitArray class demonstration

namespace BooleanDemo
{
    // Custom class to demonstrate reference type equality comparison
    // This class helps illustrate how == operator works with reference types
    public class Person
    {
        // Public field to store the person's name
        // This will be used to show that objects with same content are still different instances
        public string Name;
        
        // Constructor to initialize a Person object with a name
        // Takes a string parameter and assigns it to the Name field
        public Person(string name) 
        { 
            Name = name; 
        }

        // Override ToString method to provide meaningful string representation
        // This makes it easier to display Person objects in console output
        public override string ToString()
        {
            return $"Person: {Name}";
        }
    }

    class Program
    {
        // Main method: Entry point of the application
        // Demonstrates all Boolean-related concepts through method calls
        static void Main()
        {
            Console.WriteLine("=== C# BOOLEAN TYPES AND OPERATORS COMPREHENSIVE DEMONSTRATION ===\n");

            // Call each demonstration method to show different aspects of Boolean operations
            // Each method focuses on a specific concept from the educational material

            // 1. Basic Boolean type usage and memory considerations
            DemonstrateBooleanBasics();

            // 2. Boolean conversion restrictions and alternatives
            DemonstrateBooleanConversions();

            // 3. Equality and inequality operators with value and reference types
            DemonstrateEqualityOperators();

            // 4. Logical operators: AND (&&), OR (||), NOT (!)
            DemonstrateLogicalOperators();

            // 5. Short-circuiting behavior vs non-short-circuiting operators
            DemonstrateShortCircuiting();

            // 6. Ternary conditional operator for concise conditional expressions
            DemonstrateTernaryOperator();

            // 7. Bitwise operators for bit-level manipulation
            DemonstrateBitwiseOperators();

            // 8. Practical examples combining multiple concepts
            DemonstratePracticalExamples();

            Console.WriteLine("\n=== END OF DEMONSTRATION ===");
        }

        // Method to demonstrate basic Boolean type usage and memory considerations
        // Shows how bool variables are declared, assigned, and used in conditional statements
        static void DemonstrateBooleanBasics()
        {
            Console.WriteLine("1. BOOLEAN TYPE BASICS");
            Console.WriteLine("======================");

            // Boolean variables can only hold two values: true or false
            // The bool type is an alias for System.Boolean
            // Despite needing only 1 bit, runtime uses 1 byte for efficiency
            bool isRaining = true;   // Boolean variable set to true
            bool isSunny = false;    // Boolean variable set to false
            bool isWindy = true;     // Another boolean for demonstration

            Console.WriteLine($"isRaining: {isRaining}");
            Console.WriteLine($"isSunny: {isSunny}");
            Console.WriteLine($"isWindy: {isWindy}");

            // Using Boolean variables in conditional statements
            // The if statement evaluates the Boolean expression
            if (isRaining)
            {
                Console.WriteLine("Take an umbrella!");
            }
            else
            {
                Console.WriteLine("No umbrella needed.");
            }

            // Demonstrating memory optimization with BitArray for multiple Boolean values
            // BitArray stores each Boolean as a single bit, saving memory for large collections
            Console.WriteLine("\nMemory optimization with BitArray:");
            BitArray weatherConditions = new BitArray(8); // 8 bits for different weather conditions
            weatherConditions[0] = true;  // Rainy
            weatherConditions[1] = false; // Sunny
            weatherConditions[2] = true;  // Windy
            weatherConditions[3] = false; // Snowy

            Console.WriteLine($"Weather conditions (BitArray): Rainy={weatherConditions[0]}, " +
                            $"Sunny={weatherConditions[1]}, Windy={weatherConditions[2]}, Snowy={weatherConditions[3]}");

            Console.WriteLine();
        }

        // Method to demonstrate Boolean conversion restrictions
        // Shows that bool cannot be implicitly or explicitly converted to/from numeric types
        static void DemonstrateBooleanConversions()
        {
            Console.WriteLine("2. BOOLEAN CONVERSIONS");
            Console.WriteLine("======================");

            bool isValid = true;
            bool isComplete = false;

            Console.WriteLine($"isValid: {isValid}");
            Console.WriteLine($"isComplete: {isComplete}");

            // C# does NOT allow implicit or explicit conversions between bool and numeric types
            // The following lines would cause compilation errors:
            // int x = isValid;              // Error: Cannot convert bool to int
            // bool y = 1;                   // Error: Cannot convert int to bool
            // int z = (int)isValid;         // Error: Cannot cast bool to int

            Console.WriteLine("Note: C# does NOT allow conversions between bool and numeric types");
            Console.WriteLine("This prevents common programming errors found in other languages");

            // Alternative approaches for bool-to-number conversion (if needed)
            // Using conditional expressions to convert bool to numeric values
            int validAsNumber = isValid ? 1 : 0;     // Ternary operator approach
            int completeAsNumber = isComplete ? 1 : 0;

            Console.WriteLine($"Manual conversion - isValid as number: {validAsNumber}");
            Console.WriteLine($"Manual conversion - isComplete as number: {completeAsNumber}");

            // Alternative using Convert class methods
            int validConverted = Convert.ToInt32(isValid);
            int completeConverted = Convert.ToInt32(isComplete);

            Console.WriteLine($"Using Convert.ToInt32 - isValid: {validConverted}");
            Console.WriteLine($"Using Convert.ToInt32 - isComplete: {completeConverted}");

            Console.WriteLine();
        }

        // Method to demonstrate equality and comparison operators
        // Shows the difference between value type and reference type equality
        static void DemonstrateEqualityOperators()
        {
            Console.WriteLine("3. EQUALITY AND COMPARISON OPERATORS");
            Console.WriteLine("====================================");

            // Equality operators with value types
            // For value types, == compares the actual values stored in variables
            Console.WriteLine("Value Type Equality:");
            int x = 1;
            int y = 2;
            int z = 1;

            Console.WriteLine($"x = {x}, y = {y}, z = {z}");
            Console.WriteLine($"x == y: {x == y}");  // False - different values
            Console.WriteLine($"x == z: {x == z}");  // True - same values
            Console.WriteLine($"x != y: {x != y}");  // True - different values

            Console.WriteLine("\nReference Type Equality:");
            // For reference types, == compares references (memory addresses) by default
            // Two objects with identical content are still different instances
            Person person1 = new Person("John");
            Person person2 = new Person("John");  // Same name but different object
            Person person3 = person1;             // Same reference as person1

            Console.WriteLine($"person1: {person1}");
            Console.WriteLine($"person2: {person2}");
            Console.WriteLine($"person3: {person3}");

            // Reference comparison results
            Console.WriteLine($"person1 == person2: {person1 == person2}");  // False - different instances
            Console.WriteLine($"person1 == person3: {person1 == person3}");  // True - same reference
            Console.WriteLine($"person1.Name == person2.Name: {person1.Name == person2.Name}"); // True - string values are equal

            // Boolean equality operations
            bool flag1 = true;
            bool flag2 = false;
            bool flag3 = true;

            Console.WriteLine($"\nBoolean Equality:");
            Console.WriteLine($"flag1 == flag3: {flag1 == flag3}");  // True
            Console.WriteLine($"flag1 != flag2: {flag1 != flag2}");  // True

            Console.WriteLine();
        }

        // Method to demonstrate logical operators: AND (&&), OR (||), NOT (!)
        // Shows how these operators work in conditional expressions and method calls
        static void DemonstrateLogicalOperators()
        {
            Console.WriteLine("4. LOGICAL OPERATORS (&&, ||, !)");
            Console.WriteLine("==================================");

            bool rainy = true;
            bool sunny = false;
            bool windy = true;
            bool snowy = false;

            Console.WriteLine($"Weather conditions: rainy={rainy}, sunny={sunny}, windy={windy}, snowy={snowy}");

            // AND operator (&&): Returns true only if BOTH conditions are true
            Console.WriteLine($"\nAND (&&) operator examples:");
            Console.WriteLine($"rainy && sunny: {rainy && sunny}");   // False - both must be true
            Console.WriteLine($"rainy && windy: {rainy && windy}");   // True - both are true
            Console.WriteLine($"sunny && snowy: {sunny && snowy}");   // False - both are false

            // OR operator (||): Returns true if AT LEAST ONE condition is true
            Console.WriteLine($"\nOR (||) operator examples:");
            Console.WriteLine($"rainy || sunny: {rainy || sunny}");   // True - at least one is true
            Console.WriteLine($"sunny || snowy: {sunny || snowy}");   // False - both are false
            Console.WriteLine($"windy || snowy: {windy || snowy}");   // True - windy is true

            // NOT operator (!): Reverses the Boolean value
            Console.WriteLine($"\nNOT (!) operator examples:");
            Console.WriteLine($"!rainy: {!rainy}");     // False - reverses true to false
            Console.WriteLine($"!sunny: {!sunny}");     // True - reverses false to true
            Console.WriteLine($"!windy: {!windy}");     // False - reverses true to false

            // Complex logical expressions combining multiple operators
            Console.WriteLine($"\nComplex expressions:");
            Console.WriteLine($"!windy && (rainy || sunny): {!windy && (rainy || sunny)}");  // False
            Console.WriteLine($"(rainy || sunny) && !snowy: {(rainy || sunny) && !snowy}");  // True

            // Practical example: UseUmbrella method
            Console.WriteLine($"\nPractical example - Use umbrella decision:");
            bool shouldUseUmbrella = UseUmbrella(rainy, sunny, windy);
            Console.WriteLine($"Should use umbrella: {shouldUseUmbrella}");

            Console.WriteLine();
        }

        // Helper method demonstrating practical use of logical operators
        // Returns true if umbrella should be used based on weather conditions
        // Uses logical AND and OR operators in a real-world scenario
        static bool UseUmbrella(bool rainy, bool sunny, bool windy)
        {
            // Use umbrella if it's (rainy OR sunny) AND not windy
            // The logic: umbrella is useful for rain or sun protection, but not in windy conditions
            return !windy && (rainy || sunny);
        }

        // Method to demonstrate short-circuiting vs non-short-circuiting operators
        // Shows how && and || can prevent errors and improve performance
        static void DemonstrateShortCircuiting()
        {
            Console.WriteLine("5. SHORT-CIRCUITING vs NON-SHORT-CIRCUITING");
            Console.WriteLine("============================================");

            // Short-circuiting with && operator
            // If first condition is false, second condition is NOT evaluated
            Console.WriteLine("Short-circuiting with && operator:");
            string? nullString = null;
            string validString = "Hello";

            // Safe null check using short-circuiting
            // nullString != null is false, so nullString.Length is never evaluated
            Console.WriteLine($"nullString != null && nullString.Length > 0: " +
                            $"{nullString != null && nullString.Length > 0}");
            Console.WriteLine("No NullReferenceException because of short-circuiting!");

            // Safe check with valid string
            Console.WriteLine($"validString != null && validString.Length > 0: " +
                            $"{validString != null && validString.Length > 0}");

            // Short-circuiting with || operator
            // If first condition is true, second condition is NOT evaluated
            Console.WriteLine($"\nShort-circuiting with || operator:");
            bool condition1 = true;
            bool condition2 = false;

            Console.WriteLine($"condition1 || condition2: {condition1 || condition2}");
            Console.WriteLine("Since condition1 is true, condition2 is not evaluated");

            // Demonstrating the difference with method calls
            Console.WriteLine($"\nDemonstrating evaluation with method calls:");
            bool result1 = ReturnTrueWithMessage("First") || ReturnFalseWithMessage("Second");
            Console.WriteLine($"Result: {result1}");
            Console.WriteLine("Notice: 'Second' method was not called due to short-circuiting\n");

            // Non-short-circuiting operators & and |
            // Both sides are ALWAYS evaluated regardless of the first condition
            Console.WriteLine("Non-short-circuiting with & and | operators:");
            bool result2 = ReturnTrueWithMessage("Third") | ReturnFalseWithMessage("Fourth");
            Console.WriteLine($"Result: {result2}");
            Console.WriteLine("Notice: Both methods were called (no short-circuiting)");

            Console.WriteLine();
        }

        // Helper method to demonstrate short-circuiting behavior
        // Returns true and prints a message to show when it's called
        static bool ReturnTrueWithMessage(string message)
        {
            Console.WriteLine($"  -> {message} method called (returns true)");
            return true;
        }

        // Helper method to demonstrate short-circuiting behavior
        // Returns false and prints a message to show when it's called
        static bool ReturnFalseWithMessage(string message)
        {
            Console.WriteLine($"  -> {message} method called (returns false)");
            return false;
        }

        // Method to demonstrate the ternary conditional operator (? :)
        // Shows how to write concise conditional expressions
        static void DemonstrateTernaryOperator()
        {
            Console.WriteLine("6. TERNARY CONDITIONAL OPERATOR (? :)");
            Console.WriteLine("======================================");

            // Basic ternary operator syntax: condition ? value_if_true : value_if_false
            int a = 10;
            int b = 20;

            // Simple ternary examples
            Console.WriteLine($"a = {a}, b = {b}");
            int max = (a > b) ? a : b;  // Returns a if a > b, otherwise returns b
            Console.WriteLine($"Maximum of a and b: {max}");

            int min = (a < b) ? a : b;  // Returns a if a < b, otherwise returns b
            Console.WriteLine($"Minimum of a and b: {min}");

            // Ternary operator with different data types
            bool isWeekend = true;
            string activity = isWeekend ? "Relax" : "Work";
            Console.WriteLine($"Weekend activity: {activity}");

            // Nested ternary operators (use sparingly for readability)
            int score = 85;
            string grade = (score >= 90) ? "A" : 
                          (score >= 80) ? "B" : 
                          (score >= 70) ? "C" : 
                          (score >= 60) ? "D" : "F";
            Console.WriteLine($"Score {score} gets grade: {grade}");

            // Practical examples using methods
            Console.WriteLine($"\nPractical examples:");
            Console.WriteLine($"Max(15, 7) = {Max(15, 7)}");
            Console.WriteLine($"Max(3, 12) = {Max(3, 12)}");

            // Ternary operator for string formatting
            int itemCount = 1;
            string message = $"You have {itemCount} item{(itemCount == 1 ? "" : "s")}";
            Console.WriteLine(message);

            itemCount = 5;
            message = $"You have {itemCount} item{(itemCount == 1 ? "" : "s")}";
            Console.WriteLine(message);

            Console.WriteLine();
        }

        // Helper method demonstrating ternary operator in a method
        // Returns the maximum of two integers using ternary operator
        static int Max(int a, int b)
        {
            // Concise way to return the larger of two values
            return (a > b) ? a : b;
        }

        // Method to demonstrate bitwise operators for integral types
        // Shows how to manipulate individual bits in numbers
        static void DemonstrateBitwiseOperators()
        {
            Console.WriteLine("7. BITWISE OPERATORS");
            Console.WriteLine("====================");

            // Bitwise operators work on the binary representation of integers
            // They manipulate individual bits rather than the whole value

            Console.WriteLine("Bitwise operator examples:");

            // Complement operator (~): Flips all bits
            int original = 0xF;  // 15 in decimal, 1111 in binary
            int complement = ~original;
            Console.WriteLine($"~0x{original:X} = 0x{complement:X8} (Complement - flips all bits)");

            // AND operator (&): Returns 1 only if both bits are 1
            int value1 = 0xF0;  // 11110000 in binary
            int value2 = 0x33;  // 00110011 in binary
            int andResult = value1 & value2;  // 00110000 in binary
            Console.WriteLine($"0x{value1:X} & 0x{value2:X} = 0x{andResult:X} (AND - both bits must be 1)");

            // OR operator (|): Returns 1 if either bit is 1
            int orResult = value1 | value2;  // 11111011 in binary
            Console.WriteLine($"0x{value1:X} | 0x{value2:X} = 0x{orResult:X} (OR - either bit can be 1)");

            // XOR operator (^): Returns 1 if bits are different
            int xorValue1 = 0xFF00;  // 1111111100000000 in binary
            int xorValue2 = 0x0FF0;  // 0000111111110000 in binary
            int xorResult = xorValue1 ^ xorValue2;  // 1111000011110000 in binary
            Console.WriteLine($"0x{xorValue1:X} ^ 0x{xorValue2:X} = 0x{xorResult:X} (XOR - bits must be different)");

            // Left shift operator (<<): Shifts bits to the left
            int shiftValue = 0x20;  // 32 in decimal, 100000 in binary
            int leftShift = shiftValue << 2;  // Shifts left by 2 positions: 10000000 in binary
            Console.WriteLine($"0x{shiftValue:X} << 2 = 0x{leftShift:X} (Left shift - multiply by 2^n)");

            // Right shift operator (>>): Shifts bits to the right
            int rightShift = shiftValue >> 1;  // Shifts right by 1 position: 10000 in binary
            Console.WriteLine($"0x{shiftValue:X} >> 1 = 0x{rightShift:X} (Right shift - divide by 2^n)");

            // Unsigned right shift operator (>>>): Available in C# 11+
            // Shifts bits right without sign extension
            int negativeValue = int.MinValue;  // Most negative int value
            int unsignedRightShift = negativeValue >>> 1;  // Unsigned right shift
            Console.WriteLine($"int.MinValue >>> 1 = 0x{unsignedRightShift:X8} (Unsigned right shift)");

            // Practical bitwise operations
            Console.WriteLine($"\nPractical bitwise examples:");
            
            // Using bitwise AND to check if a number is even
            int number = 42;
            bool isEven = (number & 1) == 0;  // Check if least significant bit is 0
            Console.WriteLine($"{number} is even: {isEven} (using bitwise AND with 1)");

            // Using bitwise OR to set a specific bit
            int flags = 0b0000;  // Binary literal: 0000
            flags |= 0b0010;     // Set bit 1: 0010
            flags |= 0b1000;     // Set bit 3: 1010
            Console.WriteLine($"Flags after setting bits: 0b{Convert.ToString(flags, 2).PadLeft(4, '0')}");

            Console.WriteLine();
        }

        // Method to demonstrate practical applications combining multiple Boolean concepts
        // Shows real-world scenarios using various Boolean operations
        static void DemonstratePracticalExamples()
        {
            Console.WriteLine("8. PRACTICAL EXAMPLES");
            Console.WriteLine("=====================");

            // Example 1: User authentication system
            Console.WriteLine("Example 1: User Authentication System");
            bool isValidUser = AuthenticateUser("admin", "password123", true);
            Console.WriteLine($"Authentication result: {isValidUser}");

            // Example 2: File access permissions
            Console.WriteLine($"\nExample 2: File Access Permissions");
            bool canAccessFile = CheckFileAccess(true, false, true);
            Console.WriteLine($"Can access file: {canAccessFile}");

            // Example 3: E-commerce discount calculation
            Console.WriteLine($"\nExample 3: E-commerce Discount System");
            double finalPrice = CalculatePrice(100.0, true, false, 5);
            Console.WriteLine($"Final price: ${finalPrice:F2}");

            // Example 4: Game character status
            Console.WriteLine($"\nExample 4: Game Character Status");
            string characterStatus = GetCharacterStatus(80, 20, true, false);
            Console.WriteLine($"Character status: {characterStatus}");

            Console.WriteLine();
        }

        // Practical example: User authentication with multiple conditions
        // Combines logical operators and Boolean parameters
        static bool AuthenticateUser(string username, string password, bool accountActive)
        {
            // Simple authentication logic using Boolean conditions
            bool validCredentials = (username == "admin") && (password == "password123");
            
            // User is authenticated if credentials are valid AND account is active
            return validCredentials && accountActive;
        }

        // Practical example: File access permission checking
        // Uses logical operators to determine file access rights
        static bool CheckFileAccess(bool hasReadPermission, bool hasWritePermission, bool isOwner)
        {
            // Can access file if user has read permission OR is the owner
            // Write operations require explicit write permission
            return hasReadPermission || isOwner;
        }

        // Practical example: E-commerce price calculation with discounts
        // Uses ternary operators and Boolean conditions for business logic
        static double CalculatePrice(double basePrice, bool isPremiumMember, bool hasPromoCode, int loyaltyYears)
        {
            // Apply premium member discount (10% off)
            double price = isPremiumMember ? basePrice * 0.9 : basePrice;
            
            // Apply promo code discount (additional 5% off)
            price = hasPromoCode ? price * 0.95 : price;
            
            // Apply loyalty discount (2% off per year, max 20%)
            double loyaltyDiscount = Math.Min(loyaltyYears * 0.02, 0.20);
            price = price * (1 - loyaltyDiscount);
            
            return price;
        }

        // Practical example: Game character status evaluation
        // Combines multiple Boolean conditions to determine character state
        static string GetCharacterStatus(int health, int mana, bool hasWeapon, bool isPoisoned)
        {
            // Use complex Boolean logic to determine character status
            if (health <= 0)
                return "Dead";
            
            // Use ternary operators for concise status determination
            string healthStatus = (health > 50) ? "Healthy" : (health > 20) ? "Injured" : "Critical";
            string manaStatus = (mana > 30) ? "High Mana" : (mana > 10) ? "Low Mana" : "No Mana";
            string equipmentStatus = hasWeapon ? "Armed" : "Unarmed";
            string conditionStatus = isPoisoned ? "Poisoned" : "Normal";
            
            // Combine all status information
            return $"{healthStatus}, {manaStatus}, {equipmentStatus}, {conditionStatus}";
        }
    }
}

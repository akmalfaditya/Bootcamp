
// ============================================================================
// COMPREHENSIVE C# SYNTAX DEMONSTRATION
// ============================================================================
// This project demonstrates all fundamental C# syntax elements including:
// - Identifiers and naming conventions (camelCase, PascalCase)
// - Keywords (reserved and contextual)
// - Literals (numeric, string, boolean, etc.)
// - Punctuators (semicolons, braces, brackets)
// - Operators (arithmetic, assignment, member access, etc.)
// - Comments (single-line, multi-line, XML documentation)
// - Real-world examples showing proper syntax usage
// ============================================================================

#nullable disable  // Disable nullable warnings for this educational demo

// Using directives - demonstrate proper namespace usage
using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxDemo
{
    /// <summary>
    /// Main program class demonstrating C# syntax fundamentals.
    /// This XML documentation comment shows structured documentation syntax.
    /// </summary>
    /// <remarks>
    /// XML documentation tags provide metadata for code generation tools.
    /// They're an advanced form of commenting that creates external documentation.
    /// </remarks>
    public class Program
    {
        // ====================================================================
        // MAIN ENTRY POINT
        // ====================================================================
        
        /// <summary>
        /// Program entry point - demonstrates basic syntax structure
        /// </summary>
        /// <param name="args">Command line arguments (string array)</param>
        public static void Main(string[] args)
        {
            Console.WriteLine("============================================================================");
            Console.WriteLine("C# SYNTAX FUNDAMENTALS DEMONSTRATION");
            Console.WriteLine("============================================================================");
            Console.WriteLine();
            
            // Demonstrate each syntax category with clear explanations
            DemonstrateIdentifiers();
            DemonstrateKeywords();
            DemonstrateLiterals();
            DemonstratePunctuators();
            DemonstrateOperators();
            DemonstrateComments();
            DemonstrateRealWorldExample();
            
            Console.WriteLine();
            Console.WriteLine("============================================================================");
            Console.WriteLine("Syntax demonstration completed successfully!");
            Console.WriteLine("============================================================================");
        }
        
        // ====================================================================
        // SECTION 1: IDENTIFIERS AND NAMING CONVENTIONS
        // ====================================================================
        
        /// <summary>
        /// Demonstrates identifier naming rules and conventions
        /// </summary>
        private static void DemonstrateIdentifiers()
        {
            Console.WriteLine("1. IDENTIFIERS AND NAMING CONVENTIONS");
            Console.WriteLine("=====================================");
            Console.WriteLine();
            
            // ----------------------------------------------------------------
            // CAMEL CASE - for local variables, parameters, private fields
            // ----------------------------------------------------------------
            Console.WriteLine("--- Camel Case Identifiers (first letter lowercase) ---");
            
            // Local variables using camelCase
            int studentAge = 20;           // camelCase for local variable
            string firstName = "John";     // camelCase for local variable
            bool isStudent = true;         // camelCase for local variable
            double averageScore = 85.5;    // camelCase for local variable
            
            Console.WriteLine($"Student info: {firstName}, age {studentAge}, average {averageScore}");
            Console.WriteLine($"Is student: {isStudent}");
            
            // ----------------------------------------------------------------
            // PASCAL CASE - for classes, methods, properties, public members
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Pascal Case Identifiers (first letter uppercase) ---");
            
            // Creating instance of our custom class (PascalCase class name)
            var calculator = new SimpleCalculator();  // PascalCase class name
            int result = calculator.AddNumbers(10, 20);  // PascalCase method name
            Console.WriteLine($"Calculator result: {result}");
            
            // ----------------------------------------------------------------
            // VALID IDENTIFIER EXAMPLES
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Valid Identifier Examples ---");
            
            // Valid identifiers can start with letter or underscore
            int _privateField = 100;       // Starts with underscore (valid)
            string userName123 = "admin";  // Contains numbers (valid)
            bool isValidUser = true;       // Descriptive name (valid)
            
            // Unicode characters are allowed (though not commonly used)
            string αlpha = "Greek letter";  // Unicode character (valid but not recommended)
            
            Console.WriteLine($"Private field: {_privateField}");
            Console.WriteLine($"User name: {userName123}");
            Console.WriteLine($"Is valid: {isValidUser}");
            Console.WriteLine($"Unicode identifier: {αlpha}");
            
            // ----------------------------------------------------------------
            // CASE SENSITIVITY DEMONSTRATION
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Case Sensitivity ---");
            
            // C# is case-sensitive - these are different variables
            string myVariable = "lowercase";
            string MyVariable = "PascalCase";
            string MYVARIABLE = "UPPERCASE";
            
            Console.WriteLine($"myVariable: {myVariable}");
            Console.WriteLine($"MyVariable: {MyVariable}");
            Console.WriteLine($"MYVARIABLE: {MYVARIABLE}");
            Console.WriteLine("All three are completely different variables!");
            
            Console.WriteLine();
        }
        
        // ====================================================================
        // SECTION 2: KEYWORDS (RESERVED AND CONTEXTUAL)
        // ====================================================================
        
        /// <summary>
        /// Demonstrates C# keywords and their usage
        /// </summary>
        private static void DemonstrateKeywords()
        {
            Console.WriteLine("2. KEYWORDS (RESERVED AND CONTEXTUAL)");
            Console.WriteLine("=====================================");
            Console.WriteLine();
            
            // ----------------------------------------------------------------
            // RESERVED KEYWORDS - cannot be used as identifiers
            // ----------------------------------------------------------------
            Console.WriteLine("--- Reserved Keywords in Action ---");
            
            // Data type keywords
            int number = 42;              // 'int' keyword
            string text = "Hello";        // 'string' keyword  
            bool flag = true;             // 'bool' keyword
            double price = 19.99;         // 'double' keyword
            
            Console.WriteLine($"Data types: int={number}, string={text}, bool={flag}, double={price}");
            
            // Control flow keywords
            if (number > 0)               // 'if' keyword
            {
                Console.WriteLine("Number is positive (using 'if' keyword)");
            }
            else                          // 'else' keyword
            {
                Console.WriteLine("Number is not positive");
            }
            
            // Loop keywords
            Console.WriteLine("Counting with 'for' keyword:");
            for (int i = 1; i <= 3; i++)  // 'for' keyword
            {
                Console.WriteLine($"  Count: {i}");
            }
            
            // ----------------------------------------------------------------
            // USING @ PREFIX TO ESCAPE KEYWORDS
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Using @ Prefix to Escape Keywords ---");
            
            // You can use @ to make keywords into identifiers
            int @class = 100;             // 'class' is a keyword, but @class is valid
            string @string = "escaped";   // 'string' is a keyword, but @string is valid
            bool @if = false;             // 'if' is a keyword, but @if is valid
            
            Console.WriteLine($"Escaped keyword identifiers: @class={@class}, @string={@string}, @if={@if}");
            Console.WriteLine("Note: @ is not part of the identifier name!");
            
            // ----------------------------------------------------------------
            // CONTEXTUAL KEYWORDS
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Contextual Keywords ---");
            
            // 'var' is a contextual keyword - can be identifier in some contexts
            var automaticType = "The compiler figures out this is a string";  // 'var' as keyword
            Console.WriteLine($"Using 'var' keyword: {automaticType}");
            
            // 'value' is contextual - used in property setters
            Console.WriteLine("'value' is contextual - used in property setters");
            Console.WriteLine("'get' and 'set' are contextual - used in properties");
            Console.WriteLine("'partial' is contextual - used for partial classes");
            
            // ----------------------------------------------------------------
            // COMMON RESERVED KEYWORDS EXAMPLES
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Common Reserved Keywords Examples ---");
            Console.WriteLine("abstract, as, base, bool, break, byte, case, catch, char, checked,");
            Console.WriteLine("class, const, continue, decimal, default, delegate, do, double, else,");
            Console.WriteLine("enum, event, explicit, extern, false, finally, fixed, float, for,");
            Console.WriteLine("foreach, goto, if, implicit, in, int, interface, internal, is, lock,");
            Console.WriteLine("long, namespace, new, null, object, operator, out, override, params,");
            Console.WriteLine("private, protected, public, readonly, record, ref, return, sbyte,");
            Console.WriteLine("sealed, short, sizeof, stackalloc, static, string, struct, switch,");
            Console.WriteLine("this, throw, true, try, typeof, uint, ulong, unchecked, unsafe,");
            Console.WriteLine("ushort, using, virtual, void, volatile, while");
            
            Console.WriteLine();
        }
        
        // ====================================================================
        // SECTION 3: LITERALS
        // ====================================================================
        
        /// <summary>
        /// Demonstrates different types of literals in C#
        /// </summary>
        private static void DemonstrateLiterals()
        {
            Console.WriteLine("3. LITERALS (RAW CONSTANT VALUES)");
            Console.WriteLine("=================================");
            Console.WriteLine();
            
            // ----------------------------------------------------------------
            // NUMERIC LITERALS
            // ----------------------------------------------------------------
            Console.WriteLine("--- Numeric Literals ---");
            
            // Integer literals
            int decimal_number = 42;           // Decimal literal
            int hex_number = 0x2A;             // Hexadecimal literal (same as 42)
            int binary_number = 0b101010;      // Binary literal (same as 42)
            
            Console.WriteLine($"Same number in different formats:");
            Console.WriteLine($"  Decimal: {decimal_number}");
            Console.WriteLine($"  Hexadecimal 0x2A: {hex_number}");
            Console.WriteLine($"  Binary 0b101010: {binary_number}");
            
            // Floating-point literals
            double standardDouble = 3.14159;     // Standard decimal notation
            double scientificNotation = 1.23e4; // Scientific notation (12300)
            float floatNumber = 2.5f;            // Float literal (f suffix)
            decimal preciseDecimal = 19.99m;     // Decimal literal (m suffix)
            
            Console.WriteLine($"Floating-point literals:");
            Console.WriteLine($"  Double: {standardDouble}");
            Console.WriteLine($"  Scientific (1.23e4): {scientificNotation}");
            Console.WriteLine($"  Float: {floatNumber}");
            Console.WriteLine($"  Decimal: {preciseDecimal}");
            
            // Large number separators (C# 7.0+)
            int largeNumber = 1_000_000;         // Underscore separators for readability
            long veryLargeNumber = 123_456_789L; // Long literal with separators
            
            Console.WriteLine($"Large numbers with separators:");
            Console.WriteLine($"  Million: {largeNumber:N0}");
            Console.WriteLine($"  Very large: {veryLargeNumber:N0}");
            
            // ----------------------------------------------------------------
            // CHARACTER AND STRING LITERALS
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Character and String Literals ---");
            
            // Character literals
            char singleChar = 'A';               // Single character in quotes
            char escapeChar = '\n';              // Escape sequence
            char unicodeChar = '\u0041';         // Unicode (same as 'A')
            
            Console.WriteLine($"Character literals:");
            Console.WriteLine($"  Single char: {singleChar}");
            Console.WriteLine($"  Unicode \\u0041: {unicodeChar}");
            
            // String literals
            string regularString = "Hello, World!";              // Regular string
            string stringWithEscapes = "Line 1\nLine 2\tTabbed"; // With escape sequences
            string verbatimString = @"C:\Users\Documents\file.txt"; // Verbatim string (@ prefix)
            string interpolatedString = $"The answer is {42}";    // String interpolation
            
            Console.WriteLine($"String literals:");
            Console.WriteLine($"  Regular: {regularString}");
            Console.WriteLine($"  With escapes: {stringWithEscapes}");
            Console.WriteLine($"  Verbatim: {verbatimString}");
            Console.WriteLine($"  Interpolated: {interpolatedString}");
            
            // ----------------------------------------------------------------
            // BOOLEAN LITERALS
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Boolean Literals ---");
            
            bool trueValue = true;               // Boolean true literal
            bool falseValue = false;             // Boolean false literal
            
            Console.WriteLine($"Boolean literals: true={trueValue}, false={falseValue}");
            
            // ----------------------------------------------------------------
            // NULL LITERAL
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Null Literal ---");
            
            string nullString = null;            // Null literal
            object nullObject = null;            // Null can be assigned to reference types
            
            Console.WriteLine($"Null literal examples:");
            Console.WriteLine($"  Null string: {nullString ?? "null"}");
            Console.WriteLine($"  Null object: {nullObject ?? "null"}");
            
            Console.WriteLine();
        }
        
        // ====================================================================
        // SECTION 4: PUNCTUATORS
        // ====================================================================
        
        /// <summary>
        /// Demonstrates punctuators that structure C# code
        /// </summary>
        private static void DemonstratePunctuators()
        {
            Console.WriteLine("4. PUNCTUATORS (STRUCTURAL SYMBOLS)");
            Console.WriteLine("===================================");
            Console.WriteLine();
            
            // ----------------------------------------------------------------
            // SEMICOLON - STATEMENT TERMINATOR
            // ----------------------------------------------------------------
            Console.WriteLine("--- Semicolon (;) - Statement Terminator ---");
            
            // Every statement must end with a semicolon
            int x = 10;        // Statement terminated with semicolon
            int y = 20;        // Another statement
            int z = x + y;     // Expression statement
            
            Console.WriteLine($"Statements with semicolons: x={x}, y={y}, z={z}");
            
            // Multi-line statements still need only one semicolon at the end
            int multiLineCalculation = 1 + 
                                      2 + 
                                      3 + 
                                      4 + 
                                      5;  // Only one semicolon needed
            
            Console.WriteLine($"Multi-line statement result: {multiLineCalculation}");
            
            // ----------------------------------------------------------------
            // BRACES - CODE BLOCKS
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Braces {{ }} - Code Blocks ---");
            
            // Braces define code blocks
            if (x > 5)
            {  // Opening brace starts block
                Console.WriteLine("Inside if block - braces define scope");
                int localVariable = 100;  // Variable scoped to this block
                Console.WriteLine($"Local variable: {localVariable}");
            }  // Closing brace ends block
            
            // Braces for loops
            for (int i = 1; i <= 2; i++)
            {
                Console.WriteLine($"Loop iteration {i} - inside braces");
            }
            
            // ----------------------------------------------------------------
            // PARENTHESES - MULTIPLE USES
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Parentheses ( ) - Multiple Uses ---");
            
            // Method calls
            Console.WriteLine("Method call parentheses contain arguments");
            
            // Expression grouping
            int result1 = 2 + 3 * 4;        // Without parentheses: 2 + (3 * 4) = 14
            int result2 = (2 + 3) * 4;      // With parentheses: (2 + 3) * 4 = 20
            
            Console.WriteLine($"Without parentheses (2 + 3 * 4): {result1}");
            Console.WriteLine($"With parentheses ((2 + 3) * 4): {result2}");
            
            // Method parameter lists
            DemonstrateParenthesesInMethods(42, "test");  // Arguments in parentheses
            
            // ----------------------------------------------------------------
            // BRACKETS - ARRAYS AND INDEXING
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Brackets [ ] - Arrays and Indexing ---");
            
            // Array declaration and initialization
            int[] numbers = { 10, 20, 30, 40, 50 };     // Braces for array initialization
            
            // Array indexing
            Console.WriteLine($"Array elements:");
            Console.WriteLine($"  First element [0]: {numbers[0]}");   // Brackets for indexing
            Console.WriteLine($"  Third element [2]: {numbers[2]}");
            Console.WriteLine($"  Last element [4]: {numbers[4]}");
            
            // ----------------------------------------------------------------
            // COMMA - SEPARATORS
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Comma (,) - Separators ---");
            
            // Variable declarations
            int a = 1, b = 2, c = 3;  // Comma separates variable declarations
            Console.WriteLine($"Multiple declarations: a={a}, b={b}, c={c}");
            
            // Method parameters
            Console.WriteLine("Commas separate method parameters and arguments");
            
            // ----------------------------------------------------------------
            // DOT - MEMBER ACCESS
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Dot (.) - Member Access ---");
            
            // Accessing members of objects
            string text = "Hello World";
            int length = text.Length;           // Dot accesses Length property
            string upper = text.ToUpper();      // Dot accesses ToUpper method
            
            Console.WriteLine($"Member access examples:");
            Console.WriteLine($"  text.Length: {length}");
            Console.WriteLine($"  text.ToUpper(): {upper}");
            
            // Static member access
            DateTime now = DateTime.Now;        // Dot accesses static property
            Console.WriteLine($"  DateTime.Now: {now:yyyy-MM-dd HH:mm:ss}");
            
            Console.WriteLine();
        }
        
        // Helper method for parentheses demonstration
        private static void DemonstrateParenthesesInMethods(int number, string text)
        {
            Console.WriteLine($"Method called with parameters: {number}, '{text}'");
        }
        
        // ====================================================================
        // SECTION 5: OPERATORS
        // ====================================================================
        
        /// <summary>
        /// Demonstrates various operators in C#
        /// </summary>
        private static void DemonstrateOperators()
        {
            Console.WriteLine("5. OPERATORS (SYMBOLS FOR OPERATIONS)");
            Console.WriteLine("=====================================");
            Console.WriteLine();
            
            // ----------------------------------------------------------------
            // ARITHMETIC OPERATORS
            // ----------------------------------------------------------------
            Console.WriteLine("--- Arithmetic Operators ---");
            
            int a = 15, b = 4;
            
            Console.WriteLine($"Given: a = {a}, b = {b}");
            Console.WriteLine($"Addition (+): a + b = {a + b}");          // Addition
            Console.WriteLine($"Subtraction (-): a - b = {a - b}");       // Subtraction
            Console.WriteLine($"Multiplication (*): a * b = {a * b}");    // Multiplication
            Console.WriteLine($"Division (/): a / b = {a / b}");          // Division
            Console.WriteLine($"Modulus (%): a % b = {a % b}");           // Remainder
            
            // ----------------------------------------------------------------
            // ASSIGNMENT OPERATORS
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Assignment Operators ---");
            
            int value = 10;                    // Basic assignment (=)
            Console.WriteLine($"Initial value: {value}");
            
            value += 5;                        // Add and assign (equivalent to: value = value + 5)
            Console.WriteLine($"After += 5: {value}");
            
            value -= 3;                        // Subtract and assign
            Console.WriteLine($"After -= 3: {value}");
            
            value *= 2;                        // Multiply and assign
            Console.WriteLine($"After *= 2: {value}");
            
            value /= 4;                        // Divide and assign
            Console.WriteLine($"After /= 4: {value}");
            
            // ----------------------------------------------------------------
            // COMPARISON OPERATORS
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Comparison Operators ---");
            
            int x = 10, y = 20;
            Console.WriteLine($"Given: x = {x}, y = {y}");
            Console.WriteLine($"Equal (==): x == y is {x == y}");              // Equality
            Console.WriteLine($"Not equal (!=): x != y is {x != y}");          // Inequality
            Console.WriteLine($"Less than (<): x < y is {x < y}");             // Less than
            Console.WriteLine($"Greater than (>): x > y is {x > y}");          // Greater than
            Console.WriteLine($"Less or equal (<=): x <= y is {x <= y}");      // Less than or equal
            Console.WriteLine($"Greater or equal (>=): x >= y is {x >= y}");   // Greater than or equal
            
            // ----------------------------------------------------------------
            // LOGICAL OPERATORS
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Logical Operators ---");
            
            bool condition1 = true, condition2 = false;
            Console.WriteLine($"Given: condition1 = {condition1}, condition2 = {condition2}");
            Console.WriteLine($"AND (&&): condition1 && condition2 = {condition1 && condition2}");
            Console.WriteLine($"OR (||): condition1 || condition2 = {condition1 || condition2}");
            Console.WriteLine($"NOT (!): !condition1 = {!condition1}");
            
            // ----------------------------------------------------------------
            // INCREMENT/DECREMENT OPERATORS
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Increment/Decrement Operators ---");
            
            int counter = 5;
            Console.WriteLine($"Initial counter: {counter}");
            Console.WriteLine($"Pre-increment (++counter): {++counter}");     // Increment then use
            Console.WriteLine($"Post-increment (counter++): {counter++}");    // Use then increment
            Console.WriteLine($"Counter after post-increment: {counter}");
            Console.WriteLine($"Pre-decrement (--counter): {--counter}");     // Decrement then use
            Console.WriteLine($"Post-decrement (counter--): {counter--}");    // Use then decrement
            Console.WriteLine($"Final counter value: {counter}");
            
            // ----------------------------------------------------------------
            // MEMBER ACCESS OPERATOR
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Member Access Operator (.) ---");
            
            string text = "Programming";
            Console.WriteLine($"String: '{text}'");
            Console.WriteLine($"text.Length: {text.Length}");                 // Property access
            Console.WriteLine($"text.ToUpper(): {text.ToUpper()}");           // Method access
            Console.WriteLine($"text.Substring(0, 4): {text.Substring(0, 4)}"); // Method with parameters
            
            // ----------------------------------------------------------------
            // TERNARY CONDITIONAL OPERATOR
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Ternary Conditional Operator (? :) ---");
            
            int age = 18;
            string status = age >= 18 ? "Adult" : "Minor";  // condition ? true_value : false_value
            Console.WriteLine($"Age {age} is classified as: {status}");
            
            // ----------------------------------------------------------------
            // NULL-COALESCING OPERATOR
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Null-Coalescing Operator (??) ---");
            
            string nullableString = null;
            string result = nullableString ?? "Default Value";  // Use right side if left is null
            Console.WriteLine($"null ?? \"Default Value\" = '{result}'");
            
            Console.WriteLine();
        }
        
        // ====================================================================
        // SECTION 6: COMMENTS
        // ====================================================================
        
        /// <summary>
        /// Demonstrates different types of comments in C#
        /// </summary>
        private static void DemonstrateComments()
        {
            Console.WriteLine("6. COMMENTS (CODE DOCUMENTATION)");
            Console.WriteLine("=================================");
            Console.WriteLine();
            
            // ----------------------------------------------------------------
            // SINGLE-LINE COMMENTS
            // ----------------------------------------------------------------
            Console.WriteLine("--- Single-Line Comments ---");
            
            // This is a single-line comment
            int number = 42;  // Comment at end of line
            
            Console.WriteLine("Single-line comments start with // and go to end of line");
            Console.WriteLine($"Number value: {number}");  // Another end-of-line comment
            
            // You can have multiple single-line comments
            // This is line 1 of comments
            // This is line 2 of comments
            // This is line 3 of comments
              // ----------------------------------------------------------------
            // MULTI-LINE COMMENTS
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Multi-Line Comments ---");
            
            /* This is a multi-line comment
               that spans multiple lines.
               Everything between the start and end markers is commented out.
               Very useful for longer explanations. */
            
            int value = 100;  /* Inline multi-line comment */ 
            
            Console.WriteLine("Multi-line comments use /* to start and */ to end");
            Console.WriteLine($"Value: {value}");
            
            /*
             * Sometimes people format multi-line comments like this
             * with asterisks on each line for better readability.
             * This is a common convention but not required.
             */
            
            // ----------------------------------------------------------------
            // XML DOCUMENTATION COMMENTS
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- XML Documentation Comments ---");
            
            Console.WriteLine("XML documentation comments start with /// (triple slash)");
            Console.WriteLine("They provide structured documentation for code elements");
            Console.WriteLine("You can see examples at the top of methods in this class");
            Console.WriteLine("They support tags like <summary>, <param>, <returns>, etc.");
            
            // Example of using a documented method
            int calculationResult = CalculateSum(10, 20);  // This method has XML documentation
            Console.WriteLine($"Documented method result: {calculationResult}");
            
            // ----------------------------------------------------------------
            // COMMENT BEST PRACTICES
            // ----------------------------------------------------------------
            Console.WriteLine("\n--- Comment Best Practices ---");
            
            Console.WriteLine("Good comments explain WHY, not WHAT:");
            Console.WriteLine("  BAD:  x = x + 1;  // Add 1 to x");
            Console.WriteLine("  GOOD: x = x + 1;  // Move to next array index");
            Console.WriteLine();
            Console.WriteLine("Use comments to:");
            Console.WriteLine("• Explain complex business logic");
            Console.WriteLine("• Document assumptions and limitations");
            Console.WriteLine("• Provide examples of usage");
            Console.WriteLine("• Warn about potential issues");
            
            Console.WriteLine();
        }
        
        /// <summary>
        /// Calculates the sum of two integers.
        /// This demonstrates XML documentation comments.
        /// </summary>
        /// <param name="first">The first number to add</param>
        /// <param name="second">The second number to add</param>
        /// <returns>The sum of the two input numbers</returns>
        /// <example>
        /// <code>
        /// int result = CalculateSum(5, 3);  // Returns 8
        /// </code>
        /// </example>
        private static int CalculateSum(int first, int second)
        {
            // Simple addition - return the sum
            return first + second;
        }
        
        // ====================================================================
        // SECTION 7: REAL-WORLD EXAMPLE
        // ====================================================================
        
        /// <summary>
        /// Demonstrates all syntax elements in a realistic scenario
        /// </summary>
        private static void DemonstrateRealWorldExample()
        {
            Console.WriteLine("7. REAL-WORLD EXAMPLE: STUDENT GRADE CALCULATOR");
            Console.WriteLine("===============================================");
            Console.WriteLine();
            
            // ----------------------------------------------------------------
            // REALISTIC SCENARIO - STUDENT GRADE SYSTEM
            // ----------------------------------------------------------------
            
            // Identifiers using proper naming conventions
            string studentName = "Sarah Johnson";        // camelCase local variable
            int studentId = 12345;                       // camelCase local variable
            
            // Array literal with punctuators
            double[] examScores = { 85.5, 92.0, 78.5, 95.0, 88.0 };  // Array initialization
            
            // Operators for calculations
            double totalScore = 0.0;                     // Assignment operator
            
            // Loop with various punctuators and operators
            for (int i = 0; i < examScores.Length; i++)  // Semicolons, comparison, increment
            {
                totalScore += examScores[i];             // Compound assignment operator
                Console.WriteLine($"Exam {i + 1}: {examScores[i]:F1} points");  // String interpolation
            }
            
            // More calculations with operators
            double averageScore = totalScore / examScores.Length;  // Division operator
            
            // Conditional logic with comparison operators
            string letterGrade;                          // Declaration
            if (averageScore >= 90.0)                    // Comparison operator
            {
                letterGrade = "A";                       // Assignment in block
            }
            else if (averageScore >= 80.0)               // else if keywords
            {
                letterGrade = "B";
            }
            else if (averageScore >= 70.0)
            {
                letterGrade = "C";
            }
            else if (averageScore >= 60.0)
            {
                letterGrade = "D";
            }
            else
            {
                letterGrade = "F";
            }
            
            // Ternary operator example
            string status = averageScore >= 60.0 ? "PASS" : "FAIL";  // Conditional operator
            
            // Member access operator with method calls
            Console.WriteLine($"\n--- Student Report ---");
            Console.WriteLine($"Student: {studentName.ToUpper()}");     // Member access + method call
            Console.WriteLine($"ID: {studentId}");
            Console.WriteLine($"Total Points: {totalScore:F1}");
            Console.WriteLine($"Average Score: {averageScore:F1}%");
            Console.WriteLine($"Letter Grade: {letterGrade}");
            Console.WriteLine($"Status: {status}");
            
            // Logical operators in complex condition
            bool isHonorRoll = averageScore >= 85.0 && letterGrade != "F";  // AND operator
            bool needsHelp = averageScore < 70.0 || letterGrade == "F";     // OR operator
            
            Console.WriteLine($"Honor Roll: {isHonorRoll}");
            Console.WriteLine($"Needs Academic Support: {needsHelp}");
            
            /* Multi-line comment explaining the grading system:
               This example demonstrates how all C# syntax elements work together
               in a real application. We use proper identifier naming, various
               operators, literals, punctuators, and keywords to create a
               functional student grading system. */
            
            // Creating and using custom class instance
            var gradeCalculator = new SimpleCalculator();
            int bonusPoints = gradeCalculator.AddNumbers(5, 3);  // Method call with parentheses
            Console.WriteLine($"Bonus points available: {bonusPoints}");
            
            Console.WriteLine();
        }
    }
    
    // ====================================================================
    // SUPPORTING CLASS - DEMONSTRATES CLASS-LEVEL SYNTAX
    // ====================================================================
    
    /// <summary>
    /// Simple calculator class demonstrating class syntax elements.
    /// Shows proper use of PascalCase for class and method names.
    /// </summary>
    public class SimpleCalculator
    {
        // Private field using camelCase with underscore prefix
        private int _operationCount;
        
        /// <summary>
        /// Public property using PascalCase naming convention
        /// </summary>
        public int OperationCount 
        { 
            get { return _operationCount; }      // Getter with return keyword
            private set { _operationCount = value; }  // Private setter with value keyword
        }
        
        /// <summary>
        /// Adds two numbers and increments operation counter.
        /// Demonstrates method syntax with parameters and return value.
        /// </summary>
        /// <param name="first">First number to add</param>
        /// <param name="second">Second number to add</param>
        /// <returns>Sum of the two numbers</returns>
        public int AddNumbers(int first, int second)  // Method with parameters
        {
            _operationCount++;                        // Increment operator
            return first + second;                    // Return statement with expression
        }  // Method ends with closing brace
        
        /// <summary>
        /// Multiplies two numbers with detailed syntax demonstration
        /// </summary>
        /// <param name="x">First operand</param>
        /// <param name="y">Second operand</param>
        /// <returns>Product of x and y</returns>
        public int MultiplyNumbers(int x, int y)
        {
            // Local variables with camelCase
            int result = x * y;                       // Multiplication operator
            _operationCount++;                        // Increment operation count
            
            return result;                            // Return the calculated result
        }
    }
}


using System;
using System.IO;
using System.Net;

namespace ExceptionHandlingDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Exception Handling in C# - Complete Training Demonstration ===\n");
            Console.WriteLine("This program demonstrates all major concepts of exception handling:");
            Console.WriteLine("• Basic try-catch blocks");
            Console.WriteLine("• Multiple catch clauses with specific exception types");
            Console.WriteLine("• Exception filters with 'when' keyword");
            Console.WriteLine("• Finally blocks for cleanup");
            Console.WriteLine("• Using statements for automatic resource disposal");
            Console.WriteLine("• Throwing and rethrowing exceptions");
            Console.WriteLine("• TryXXX pattern as alternative to exceptions");
            Console.WriteLine("• Real-world exception handling scenarios\n");

            // Run all demonstrations in order
            BasicTryCatchDemo();
            MultipleCatchBlocksDemo();
            ExceptionFiltersDemo();
            FinallyBlockDemo();
            UsingStatementDemo();
            UsingDeclarationDemo(); // New C# 8+ feature
            ThrowingExceptionsDemo();
            ThrowExpressionsDemo(); // C# 7+ feature
            RethrowingExceptionsDemo();
            CommonExceptionTypesDemo(); // New section
            TryParsePatternDemo();
            ArgumentNullThrowIfNullDemo(); // .NET 6+ feature
            ReturnCodesAlternativeDemo();
            RealWorldScenarioDemo();

            Console.WriteLine("=== Training Summary ===");
            Console.WriteLine("You've now seen comprehensive examples of exception handling in C#.");
            Console.WriteLine("Remember: Use exceptions for truly exceptional cases, not for normal program flow!");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        #region Basic Try-Catch Demonstrations

        static void BasicTryCatchDemo()
        {
            Console.WriteLine("1. BASIC TRY-CATCH DEMONSTRATION");
            Console.WriteLine("=================================");
            Console.WriteLine("The most fundamental concept in exception handling is the try-catch block.");
            Console.WriteLine("A try block contains code that might throw an exception.");
            Console.WriteLine("A catch block handles the exception if it occurs.\n");

            // This demonstrates the basic structure: try { risky code } catch { handle error }
            Console.WriteLine("Testing division by zero - without try-catch this would crash:");

            try
            {
                // This line will throw a DivideByZeroException
                int result = Calc(0);
                Console.WriteLine($"Result: {result}"); // This line won't execute
            }
            catch (DivideByZeroException ex)
            {
                // Execution jumps here when the exception is thrown
                Console.WriteLine("✓ Caught DivideByZeroException - program continues running");
                Console.WriteLine($"  Exception message: {ex.Message}");
                Console.WriteLine($"  Exception type: {ex.GetType().Name}");
            }

            Console.WriteLine("✓ Program execution continues after exception handling\n");

            // Important principle: Prevention is better than exception handling
            Console.WriteLine("Better approach - validate input before risky operations:");
            int safeResult = SafeCalc(0);
            Console.WriteLine($"Safe result: {safeResult}");
            Console.WriteLine("Remember: Exceptions are expensive - use them for truly exceptional situations!\n");
        }

        // Method that demonstrates what happens without input validation
        static int Calc(int x)
        {
            // This will throw DivideByZeroException if x is 0
            // In a real application, this represents any risky operation:
            // - File operations, network calls, parsing, etc.
            return 10 / x;
        }

        // Better approach - defensive programming with validation
        static int SafeCalc(int x)
        {
            // Always validate inputs when possible rather than relying on exception handling
            if (x == 0)
            {
                Console.WriteLine("  Warning: Division by zero attempted, returning safe value");
                return 0; // Or throw a more descriptive exception
            }
            return 10 / x;
        }

        #endregion

        #region Multiple Catch Blocks

        static void MultipleCatchBlocksDemo()
        {
            Console.WriteLine("2. MULTIPLE CATCH BLOCKS DEMONSTRATION");
            Console.WriteLine("======================================");

            // Simulate command line arguments for testing
            string[] testArgs = { "300" }; // Try: "300", "abc", "", or null

            Console.WriteLine($"Testing with argument: '{testArgs[0]}'");

            try
            {
                byte b = byte.Parse(testArgs[0]);
                Console.WriteLine($"Successfully parsed: {b}");
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Error: Please provide at least one argument");
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: That's not a valid number!");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Error: The number is too large to fit in a byte (max: 255)!");
            }
            catch (Exception ex) // General catch-all (should be last)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            // Test different scenarios
            Console.WriteLine("\nTesting different error scenarios:");
            TestParsingScenarios();
            Console.WriteLine();
        }

        static void TestParsingScenarios()
        {
            string[] testCases = { "100", "abc", "500", "" };

            foreach (string testCase in testCases)
            {
                Console.WriteLine($"  Testing '{testCase}':");
                try
                {
                    byte result = byte.Parse(testCase);
                    Console.WriteLine($"    Success: {result}");
                }
                catch (FormatException)
                {
                    Console.WriteLine("    Error: Invalid format");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("    Error: Number too large for byte");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("    Error: Empty string");
                }
            }
        }

        #endregion

        #region Exception Filters

        static void ExceptionFiltersDemo()
        {
            Console.WriteLine("3. EXCEPTION FILTERS DEMONSTRATION");
            Console.WriteLine("==================================");

            // Simulate different web exception scenarios
            Console.WriteLine("Testing exception filters with 'when' keyword:");

            SimulateWebException(WebExceptionStatus.Timeout);
            SimulateWebException(WebExceptionStatus.SendFailure);
            SimulateWebException(WebExceptionStatus.ConnectFailure);

            Console.WriteLine();
        }

        static void SimulateWebException(WebExceptionStatus status)
        {
            try
            {
                // Create and throw a WebException with specific status
                var ex = new WebException("Simulated web error", status);
                throw ex;
            }
            catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
            {
                Console.WriteLine("  Handled: Request timeout - retrying with longer timeout");
            }
            catch (WebException ex) when (ex.Status == WebExceptionStatus.SendFailure)
            {
                Console.WriteLine("  Handled: Send failure - checking network connection");
            }
            catch (WebException ex) when (ex.Status == WebExceptionStatus.ConnectFailure)
            {
                Console.WriteLine("  Handled: Connection failure - server might be down");
            }
            catch (WebException ex)
            {
                Console.WriteLine($"  Handled: Other web exception - {ex.Status}");
            }
        }

        #endregion

        #region Finally Block

        static void FinallyBlockDemo()
        {
            Console.WriteLine("4. FINALLY BLOCK DEMONSTRATION");
            Console.WriteLine("==============================");

            Console.WriteLine("Testing finally block execution:");

            // Test scenario 1: No exception
            Console.WriteLine("Scenario 1: Normal execution");
            TestFinallyBlock(false);

            // Test scenario 2: With exception
            Console.WriteLine("\nScenario 2: With exception");
            TestFinallyBlock(true);

            Console.WriteLine();
        }

        static void TestFinallyBlock(bool throwException)
        {
            string resource = null;

            try
            {
                Console.WriteLine("  Acquiring resource...");
                resource = "Important Resource";

                if (throwException)
                {
                    throw new InvalidOperationException("Simulated error");
                }

                Console.WriteLine("  Using resource successfully");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"  Caught exception: {ex.Message}");
            }
            finally
            {
                // This ALWAYS runs, regardless of exceptions
                if (resource != null)
                {
                    Console.WriteLine("  Finally block: Cleaning up resource");
                    resource = null; // Simulate cleanup
                }
                else
                {
                    Console.WriteLine("  Finally block: No resource to clean up");
                }
            }

            Console.WriteLine("  Method completed");
        }

        #endregion

        #region Using Statement

        static void UsingStatementDemo()
        {
            Console.WriteLine("5. USING STATEMENT DEMONSTRATION");
            Console.WriteLine("=================================");

            Console.WriteLine("Comparing manual resource management vs using statement:");

            // Manual way (what we did before using statements)
            Console.WriteLine("\nManual resource management:");
            ReadFileManually();

            // Using statement way (cleaner and safer)
            Console.WriteLine("\nUsing statement approach:");
            ReadFileWithUsing();

            Console.WriteLine();
        }

        static void ReadFileManually()
        {
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter("manual_test.txt");
                writer.WriteLine("This file was created manually");
                Console.WriteLine("  File written successfully (manual way)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Error writing file: {ex.Message}");
            }
            finally
            {
                // We must remember to dispose manually
                if (writer != null)
                {
                    writer.Dispose();
                    Console.WriteLine("  Resource disposed manually in finally block");
                }
            }
        }

        static void ReadFileWithUsing()
        {
            try
            {
                // Using statement automatically calls Dispose() when exiting the block
                using (StreamWriter writer = new StreamWriter("using_test.txt"))
                {
                    writer.WriteLine("This file was created with using statement");
                    Console.WriteLine("  File written successfully (using statement)");
                    // No need for manual cleanup - Dispose() is called automatically
                }
                Console.WriteLine("  Resource automatically disposed by using statement");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Error writing file: {ex.Message}");
            }
        }

        #endregion

        #region Using Declarations (C# 8+)

        static void UsingDeclarationDemo()
        {
            Console.WriteLine("5B. USING DECLARATIONS DEMONSTRATION (C# 8+)");
            Console.WriteLine("==============================================");
            Console.WriteLine("Using declarations provide a cleaner syntax for resource management.");
            Console.WriteLine("The resource is automatically disposed when execution leaves the scope.\n");

            Console.WriteLine("Demonstrating using declaration syntax:");
            DemonstrateUsingDeclaration();
            Console.WriteLine();
        }

        static void DemonstrateUsingDeclaration()
        {
            // Create a temporary file for demonstration
            string tempFile = "using_declaration_demo.txt";
            
            try
            {
                if (File.Exists(tempFile))
                {
                    // Using declaration - resource disposed when leaving the 'if' block
                    using var reader = File.OpenText(tempFile);
                    Console.WriteLine("  ✓ File opened with using declaration");
                    string? firstLine = reader.ReadLine();
                    Console.WriteLine($"  ✓ Read line: {firstLine ?? "empty"}");
                    // reader.Dispose() is automatically called here when leaving scope
                }
                else
                {
                    // Create the file first for demonstration
                    using var writer = new StreamWriter(tempFile);
                    writer.WriteLine("Demo content for using declaration");
                    Console.WriteLine("  ✓ File created with using declaration");
                    // writer.Dispose() is automatically called here
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ✗ Error: {ex.Message}");
            }
            finally
            {
                // Clean up the demo file
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                    Console.WriteLine("  ✓ Demo file cleaned up");
                }
            }
        }

        #endregion

        #region Throw Expressions (C# 7+)

        static void ThrowExpressionsDemo()
        {
            Console.WriteLine("7B. THROW EXPRESSIONS DEMONSTRATION (C# 7+)");
            Console.WriteLine("============================================");
            Console.WriteLine("C# 7+ allows 'throw' to be used as an expression, not just a statement.");
            Console.WriteLine("This enables throwing exceptions in expression contexts like ternary operators.\n");

            Console.WriteLine("Testing throw expressions in different contexts:");

            // Test expression-bodied method that throws
            try
            {
                string result = GetNotImplementedFeature();
                Console.WriteLine(result);
            }
            catch (NotImplementedException ex)
            {
                Console.WriteLine($"  ✓ Caught from expression-bodied method: {ex.Message}");
            }

            // Test throw in ternary conditional
            try
            {
                string result = ProperCase(null);
                Console.WriteLine(result);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  ✓ Caught from ternary expression: {ex.Message}");
            }

            // Test with valid input
            try
            {
                string result = ProperCase("hello world");
                Console.WriteLine($"  ✓ ProperCase result: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ✗ Unexpected error: {ex.Message}");
            }

            Console.WriteLine();
        }

        // Expression-bodied member that throws (C# 7+ throw expression)
        static string GetNotImplementedFeature() => 
            throw new NotImplementedException("This feature is planned for version 2.0");

        // Method using throw expression in ternary conditional
        static string ProperCase(string? value) =>
            value == null ? throw new ArgumentException("Value cannot be null") :
            value == "" ? "" :
            char.ToUpper(value[0]) + value.Substring(1).ToLower();

        #endregion

        #region Common Exception Types

        static void CommonExceptionTypesDemo()
        {
            Console.WriteLine("8B. COMMON EXCEPTION TYPES DEMONSTRATION");
            Console.WriteLine("========================================");
            Console.WriteLine("C# has many built-in exception types for different error scenarios.");
            Console.WriteLine("Understanding when to use each type is crucial for good error handling.\n");

            // ArgumentException family
            DemonstrateArgumentExceptions();
            
            // Operation state exceptions
            DemonstrateOperationExceptions();
            
            // System exceptions
            DemonstrateSystemExceptions();

            Console.WriteLine();
        }

        static void DemonstrateArgumentExceptions()
        {
            Console.WriteLine("ArgumentException Family:");
            Console.WriteLine("-------------------------");

            // ArgumentNullException
            try
            {
                ValidateUserInput(null, 25);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"  ✓ ArgumentNullException: {ex.ParamName} - {ex.Message}");
            }

            // ArgumentException (general)
            try
            {
                ValidateUserInput("", 25);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  ✓ ArgumentException: {ex.ParamName} - {ex.Message}");
            }

            // ArgumentOutOfRangeException
            try
            {
                ValidateUserInput("John", -5);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"  ✓ ArgumentOutOfRangeException: {ex.ParamName} - {ex.Message}");
            }
        }

        static void ValidateUserInput(string? name, int age)
        {
            // These exceptions indicate programming errors in the calling code
            if (name == null)
                throw new ArgumentNullException(nameof(name), "Name parameter cannot be null");
            
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty or whitespace", nameof(name));
            
            if (age < 0 || age > 150)
                throw new ArgumentOutOfRangeException(nameof(age), age, "Age must be between 0 and 150");
            
            Console.WriteLine($"  ✓ Valid input: {name}, age {age}");
        }

        static void DemonstrateOperationExceptions()
        {
            Console.WriteLine("\nOperation State Exceptions:");
            Console.WriteLine("---------------------------");

            var demoObject = new DisposableDemo();

            // InvalidOperationException
            try
            {
                demoObject.PerformOperation(false);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"  ✓ InvalidOperationException: {ex.Message}");
            }

            // ObjectDisposedException
            demoObject.Dispose();
            try
            {
                demoObject.PerformOperation(true);
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine($"  ✓ ObjectDisposedException: {ex.Message}");
            }

            // NotSupportedException
            try
            {
                var list = new List<string> { "item1", "item2" };
                var readOnlyCollection = list.AsReadOnly();
                
                // Attempting to modify a read-only collection
                ((ICollection<string>)readOnlyCollection).Add("item3"); // This throws NotSupportedException
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine($"  ✓ NotSupportedException: {ex.Message}");
            }
        }

        static void DemonstrateSystemExceptions()
        {
            Console.WriteLine("\nSystem Exceptions:");
            Console.WriteLine("------------------");

            // NullReferenceException (usually indicates a programming bug)
            try
            {
                string? nullString = null;
                int length = nullString!.Length; // null-forgiving operator for demo purposes
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"  ✓ NullReferenceException: {ex.Message}");
                Console.WriteLine("    Note: This usually indicates a programming bug - always validate for null!");
            }

            // FormatException
            try
            {
                int number = int.Parse("not_a_number");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"  ✓ FormatException: {ex.Message}");
            }
        }

        // Helper class for demonstrating object state exceptions
        class DisposableDemo : IDisposable
        {
            private bool _isReady = false;
            private bool _disposed = false;

            public void PerformOperation(bool setReady)
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(DisposableDemo));
                
                if (setReady)
                    _isReady = true;

                if (!_isReady)
                    throw new InvalidOperationException("Object is not in a ready state for this operation");
                
                Console.WriteLine("  ✓ Operation completed successfully");
            }

            public void Dispose()
            {
                _disposed = true;
                Console.WriteLine("  ✓ Object disposed");
            }
        }

        #endregion

        #region ArgumentNullException.ThrowIfNull (.NET 6+)

        static void ArgumentNullThrowIfNullDemo()
        {
            Console.WriteLine("9B. ARGUMENTNULLEXCEPTION.THROWIFNULL (.NET 6+)");
            Console.WriteLine("================================================");
            Console.WriteLine("The ThrowIfNull method provides a concise way to validate null arguments.");
            Console.WriteLine("It's cleaner than manually writing if-checks and throw statements.\n");

            Console.WriteLine("Comparing old vs new null validation approaches:");

            // Test with valid input
            try
            {
                ProcessUserDataOldWay("Valid User");
                ProcessUserDataNewWay("Valid User");
                Console.WriteLine("  ✓ Both methods succeeded with valid input");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ✗ Unexpected error: {ex.Message}");
            }

            Console.WriteLine();

            // Test with null input
            try
            {
                ProcessUserDataOldWay(null);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"  ✓ Old way caught: {ex.ParamName} - {ex.Message}");
            }

            try
            {
                ProcessUserDataNewWay(null);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"  ✓ New way caught: {ex.ParamName} - {ex.Message}");
            }

            Console.WriteLine();
        }

        // Traditional approach (pre-.NET 6)
        static void ProcessUserDataOldWay(string? userData)
        {
            // Manual null check with explicit throw
            if (userData == null)
                throw new ArgumentNullException(nameof(userData), "User data cannot be null");
            
            Console.WriteLine($"  Processing (old way): {userData}");
        }

        // Modern approach (.NET 6+)
        static void ProcessUserDataNewWay(string? userData)
        {
            // Concise null check - throws ArgumentNullException if null
            ArgumentNullException.ThrowIfNull(userData);
            
            Console.WriteLine($"  Processing (new way): {userData}");
        }

        #endregion

        #region Throwing Exceptions

        static void ThrowingExceptionsDemo()
        {
            Console.WriteLine("7. THROWING EXCEPTIONS DEMONSTRATION");
            Console.WriteLine("====================================");
            Console.WriteLine("Your code can throw exceptions explicitly using the 'throw' keyword.");
            Console.WriteLine("This is useful for validating inputs and enforcing business rules.\n");

            Console.WriteLine("Testing custom exception throwing:");

            // Test with valid input
            try
            {
                DisplayName("John Doe");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"  ✗ Caught: {ex.Message}");
            }

            // Test with null input
            try
            {
                DisplayName(null!); // null-forgiving operator for demo
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"  ✓ Caught ArgumentNullException: {ex.ParamName} - {ex.Message}");
            }

            // Test with empty input
            try
            {
                DisplayName("");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  ✓ Caught ArgumentException: {ex.ParamName} - {ex.Message}");
            }

            Console.WriteLine();
        }

        static void DisplayName(string? name)
        {
            // Input validation with specific exceptions
            // ArgumentNullException for null values
            if (name == null)
                throw new ArgumentNullException(nameof(name), "Name cannot be null");

            // ArgumentException for invalid but non-null values
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty or whitespace", nameof(name));

            Console.WriteLine($"  ✓ Hello, {name}!");
        }

        #endregion

        #region Rethrowing Exceptions

        static void RethrowingExceptionsDemo()
        {
            Console.WriteLine("8. RETHROWING EXCEPTIONS DEMONSTRATION");
            Console.WriteLine("======================================");
            Console.WriteLine("You can catch an exception, do some processing (like logging), and rethrow it.");
            Console.WriteLine("Use 'throw;' to preserve the original stack trace, or 'throw new Exception()' to wrap.\n");

            Console.WriteLine("Testing exception rethrowing and wrapping:");

            try
            {
                ProcessDataWithLogging();
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine($"  ✓ Final catch - InvalidDataException: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"  ✓ Inner exception preserved: {ex.InnerException.GetType().Name} - {ex.InnerException.Message}");
                }
            }

            Console.WriteLine("\nDemonstrating simple rethrow (preserving stack trace):");
            try
            {
                MethodThatRethrows();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"  ✓ Caught rethrown exception: {ex.Message}");
                Console.WriteLine($"    Exception was logged and rethrown from intermediate method");
            }

            Console.WriteLine();
        }

        static void ProcessDataWithLogging()
        {
            try
            {
                ParseCriticalData("invalid_data");
            }
            catch (FormatException ex)
            {
                // Log the error for debugging (in real app, this would go to a log file)
                Console.WriteLine("  → Logging original error for debugging purposes...");
                Console.WriteLine($"    Original error: {ex.GetType().Name} - {ex.Message}");
                
                // Wrap the original exception in a domain-specific exception
                // The original exception becomes the InnerException
                throw new InvalidDataException("Failed to process critical business data", ex);
            }
        }

        static void ParseCriticalData(string data)
        {
            // Simulate parsing that can fail
            if (data == "invalid_data")
            {
                throw new FormatException("Data format is not recognized by the parser");
            }
            
            Console.WriteLine($"  ✓ Successfully parsed: {data}");
        }

        static void MethodThatRethrows()
        {
            try
            {
                // Simulate an operation that fails
                throw new InvalidOperationException("Original operation failed");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("  → Logging in intermediate method...");
                // Use 'throw;' to rethrow the same exception with preserved stack trace
                // Never use 'throw ex;' as it resets the stack trace
                throw;
                throw;
            }
        }

        // Custom exception for business logic
        public class InvalidDataException : Exception
        {
            public InvalidDataException(string message) : base(message) { }
            public InvalidDataException(string message, Exception innerException) : base(message, innerException) { }
        }

        #endregion

        #region TryXXX Pattern

        static void TryParsePatternDemo()
        {
            Console.WriteLine("9. TRY-PARSE PATTERN DEMONSTRATION");
            Console.WriteLine("===================================");
            Console.WriteLine("The TryXXX pattern provides an alternative to exceptions for expected failures.");
            Console.WriteLine("It returns a boolean for success/failure and uses 'out' parameters for results.");
            Console.WriteLine("This is more efficient than try-catch for scenarios where failure is common.\n");

            Console.WriteLine("Comparing exception-based vs TryParse approaches:");

            string[] testInputs = { "123", "abc", "999999999999", "45.67", "" };

            foreach (string input in testInputs)
            {
                Console.WriteLine($"\nTesting input: '{input}'");

                // Exception-based approach - expensive when failures are common
                Console.WriteLine("  Exception-based approach:");
                try
                {
                    int result = int.Parse(input);
                    Console.WriteLine($"    ✓ Success: {result}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    ✗ Failed: {ex.GetType().Name}");
                }

                // TryParse approach - efficient, no exceptions thrown
                Console.WriteLine("  TryParse approach:");
                if (int.TryParse(input, out int tryResult))
                {
                    Console.WriteLine($"    ✓ Success: {tryResult}");
                }
                else
                {
                    Console.WriteLine("    ✗ Failed: Invalid format or overflow");
                }
            }

            // Demonstrate custom TryXXX method
            Console.WriteLine("\nCustom TryDivide method demonstration:");
            TestCustomTryMethod();

            Console.WriteLine();
        }

        static void TestCustomTryMethod()
        {
            int[][] testCases = { 
                new int[] { 10, 2 }, 
                new int[] { 15, 3 }, 
                new int[] { 7, 0 },   // Division by zero case
                new int[] { -20, 4 } 
            };

            foreach (var testCase in testCases)
            {
                int numerator = testCase[0];
                int denominator = testCase[1];

                Console.WriteLine($"  Testing {numerator} ÷ {denominator}:");

                // Using our custom TryDivide method
                if (TryDivide(numerator, denominator, out int result))
                {
                    Console.WriteLine($"    ✓ Success: {result}");
                }
                else
                {
                    Console.WriteLine("    ✗ Failed: Division by zero not allowed");
                }
            }
        }

        // Custom TryXXX method implementation
        // Returns true for success, false for failure
        // Result is provided via 'out' parameter
        static bool TryDivide(int numerator, int denominator, out int result)
        {
            if (denominator == 0)
            {
                result = 0; // Set a default value
                return false; // Indicate failure
            }

            result = numerator / denominator;
            return true; // Indicate success
        }

        #endregion

        #region Return Codes Alternative

        static void ReturnCodesAlternativeDemo()
        {
            Console.WriteLine("10. RETURN CODES VS EXCEPTIONS COMPARISON");
            Console.WriteLine("=========================================");
            Console.WriteLine("Return codes are an alternative to exceptions for error handling.");
            Console.WriteLine("They're faster but require more manual checking and are easier to ignore.\n");

            string[] filePaths = { "valid_file.txt", "", "nonexistent.txt" };

            foreach (string path in filePaths)
            {
                Console.WriteLine($"Testing file path: '{path}'");

                // Return code approach - faster, but easy to ignore errors
                int resultCode = OpenFileWithReturnCode(path);
                Console.WriteLine($"  Return code approach: {GetResultMessage(resultCode)}");

                // Exception approach - cannot be ignored, but has performance overhead
                try
                {
                    OpenFileWithException(path);
                    Console.WriteLine("  Exception approach: ✓ Success");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  Exception approach: ✗ {ex.GetType().Name}");
                }

                Console.WriteLine();
            }

            Console.WriteLine("When to use each approach:");
            Console.WriteLine("• Return codes: Performance-critical code, expected failures");
            Console.WriteLine("• Exceptions: Truly exceptional cases, when errors cannot be ignored");
            Console.WriteLine();
        }

        // Return code approach - uses integer codes to indicate success/failure
        static int OpenFileWithReturnCode(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return -1; // Error code: Invalid file path

            if (!File.Exists(filePath))
                return -2; // Error code: File not found

            // In a real application, this would actually open the file
            return 0; // Success code
        }

        static string GetResultMessage(int code)
        {
            return code switch
            {
                0 => "✓ Success",
                -1 => "✗ Error: Invalid file path",
                -2 => "✗ Error: File not found",
                _ => "✗ Unknown error"
            };
        }

        // Exception approach - throws specific exceptions for different error conditions
        static void OpenFileWithException(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path cannot be null or empty");

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            // In a real application, this would actually open the file
        }

        #endregion

        #region Real World Scenario

        static void RealWorldScenarioDemo()
        {
            Console.WriteLine("11. REAL WORLD SCENARIO - FILE PROCESSING SYSTEM");
            Console.WriteLine("=================================================");
            Console.WriteLine("This demonstrates comprehensive exception handling in a realistic scenario.");
            Console.WriteLine("It shows proper resource management, logging, and graceful error handling.\n");

            var processor = new FileProcessor();

            // Test different scenarios to show various exception handling patterns
            Console.WriteLine("Processing various file scenarios:\n");

            processor.ProcessFile("valid_data.txt");
            processor.ProcessFile(""); // Empty path
            processor.ProcessFile("nonexistent.txt"); // File not found
            processor.ProcessFile("corrupted.txt"); // Simulate corruption

            // Demonstrate the TryProcess alternative approach
            Console.WriteLine("\nDemonstrating TryProcess alternative:");
            if (processor.TryProcessFile("test.txt", out string? errorMessage))
            {
                Console.WriteLine("  ✓ File processed successfully using TryProcess");
            }
            else
            {
                Console.WriteLine($"  ✗ TryProcess failed: {errorMessage}");
            }

            Console.WriteLine();
        }

        #endregion
    }

    #region Real World File Processor

    public class FileProcessor
    {
        public void ProcessFile(string filePath)
        {
            Console.WriteLine($"Processing file: '{filePath}'");
            FileStream? fileStream = null;
            StreamReader? reader = null;

            try
            {
                // Validate input parameters
                if (string.IsNullOrWhiteSpace(filePath))
                    throw new ArgumentException("File path cannot be empty", nameof(filePath));

                // Simulate different file conditions for demonstration
                if (filePath == "nonexistent.txt")
                    throw new FileNotFoundException($"Could not find file: {filePath}");

                if (filePath == "corrupted.txt")
                    throw new InvalidDataException("File appears to be corrupted or unreadable");

                // Simulate successful file processing
                Console.WriteLine("  ✓ File validation passed");
                Console.WriteLine("  ✓ File opened successfully");
                Console.WriteLine("  ✓ Data processed and validated");
                Console.WriteLine("  ✓ Processing completed successfully");

            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  ✗ Input Validation Error: {ex.Message}");
                LogError("ARGUMENT_ERROR", ex);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"  ✗ File System Error: {ex.Message}");
                LogError("FILE_NOT_FOUND", ex);
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine($"  ✗ Data Processing Error: {ex.Message}");
                LogError("DATA_CORRUPTION", ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"  ✗ Access Denied: {ex.Message}");
                LogError("ACCESS_DENIED", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ✗ Unexpected Error: {ex.Message}");
                LogError("UNEXPECTED_ERROR", ex);
                // In production, you might want to rethrow unexpected exceptions
                // throw;
            }
            finally
            {
                // Resource cleanup - this ALWAYS executes
                Console.WriteLine("  → Performing cleanup operations...");
                
                // Dispose resources in reverse order of acquisition
                reader?.Dispose();
                fileStream?.Dispose();
                
                Console.WriteLine("  → Resource cleanup completed");
            }

            Console.WriteLine(); // Add spacing between file processing attempts
        }

        private void LogError(string errorType, Exception ex)
        {
            // In a real application, this would write to a logging framework
            // like Serilog, NLog, or Microsoft.Extensions.Logging
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($"  → [LOG] {timestamp} - {errorType}: {ex.Message}");
            
            // Could also log stack trace for debugging
            // Console.WriteLine($"  → [LOG] Stack Trace: {ex.StackTrace}");
        }

        // Alternative TryProcess method - no exceptions thrown
        // Returns success/failure and provides error details via out parameter
        public bool TryProcessFile(string filePath, out string? errorMessage)
        {
            errorMessage = null;

            try
            {
                ProcessFile(filePath);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }

    #endregion
}

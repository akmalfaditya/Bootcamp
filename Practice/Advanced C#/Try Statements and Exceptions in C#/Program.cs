using System;
using System.IO;
using System.Net;

namespace ExceptionHandlingDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Exception Handling in C# - Complete Demonstration ===\n");

            // Run all demonstrations
            BasicTryCatchDemo();
            MultipleCatchBlocksDemo();
            ExceptionFiltersDemo();
            FinallyBlockDemo();
            UsingStatementDemo();
            ThrowingExceptionsDemo();
            RethrowingExceptionsDemo();
            TryParsePatternDemo();
            ReturnCodesAlternativeDemo();
            RealWorldScenarioDemo();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        #region Basic Try-Catch Demonstrations

        static void BasicTryCatchDemo()
        {
            Console.WriteLine("1. BASIC TRY-CATCH DEMONSTRATION");
            Console.WriteLine("=================================");

            // Example of code that would crash without exception handling
            Console.WriteLine("Testing division by zero handling:");

            try
            {
                int result = Calc(0);
                Console.WriteLine($"Result: {result}");
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine("Error: x cannot be zero");
                Console.WriteLine($"Exception message: {ex.Message}");
            }

            Console.WriteLine("Program continues running after exception was handled\n");

            // Best practice: Check before operation when possible
            Console.WriteLine("Better approach - checking before division:");
            int safeResult = SafeCalc(0);
            Console.WriteLine($"Safe result: {safeResult}\n");
        }

        // Method that can throw an exception
        static int Calc(int x)
        {
            // This will throw DivideByZeroException if x is 0
            return 10 / x;
        }

        // Better approach - defensive programming
        static int SafeCalc(int x)
        {
            if (x == 0)
            {
                Console.WriteLine("Warning: Division by zero attempted, returning 0");
                return 0;
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

        #region Throwing Exceptions

        static void ThrowingExceptionsDemo()
        {
            Console.WriteLine("6. THROWING EXCEPTIONS DEMONSTRATION");
            Console.WriteLine("====================================");

            Console.WriteLine("Testing custom exception throwing:");

            // Test with valid input
            try
            {
                DisplayName("John Doe");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Caught: {ex.Message}");
            }

            // Test with null input
            try
            {
                DisplayName(null);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Caught: {ex.Message}");
            }

            // Test expression-bodied member that throws
            try
            {
                string result = GetNotImplementedFeature();
                Console.WriteLine(result);
            }
            catch (NotImplementedException ex)
            {
                Console.WriteLine($"Caught: {ex.Message}");
            }

            Console.WriteLine();
        }

        static void DisplayName(string name)
        {
            // Validate input and throw appropriate exception
            if (name == null)
                throw new ArgumentNullException(nameof(name), "Name cannot be null");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty or whitespace", nameof(name));

            Console.WriteLine($"  Hello, {name}!");
        }

        // Expression-bodied member that throws an exception
        static string GetNotImplementedFeature() => throw new NotImplementedException("This feature is coming in version 2.0");

        #endregion

        #region Rethrowing Exceptions

        static void RethrowingExceptionsDemo()
        {
            Console.WriteLine("7. RETHROWING EXCEPTIONS DEMONSTRATION");
            Console.WriteLine("======================================");

            Console.WriteLine("Testing exception rethrowing:");

            try
            {
                ProcessDataWithLogging();
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine($"Final catch: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
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
                // Log the error but wrap it in a more specific exception
                Console.WriteLine("  Logging original error for debugging...");
                
                // Throw a new exception while preserving the original
                throw new InvalidDataException("Failed to process critical data", ex);
            }
        }

        static void ParseCriticalData(string data)
        {
            if (data == "invalid_data")
            {
                throw new FormatException("Data format is not recognized");
            }
            
            Console.WriteLine($"  Successfully parsed: {data}");
        }

        // Custom exception for our domain
        public class InvalidDataException : Exception
        {
            public InvalidDataException(string message) : base(message) { }
            public InvalidDataException(string message, Exception innerException) : base(message, innerException) { }
        }

        #endregion

        #region TryXXX Pattern

        static void TryParsePatternDemo()
        {
            Console.WriteLine("8. TRY-PARSE PATTERN DEMONSTRATION");
            Console.WriteLine("===================================");

            Console.WriteLine("Comparing exception-based vs TryParse approaches:");

            string[] testInputs = { "123", "abc", "999999999999", "45.67" };

            foreach (string input in testInputs)
            {
                Console.WriteLine($"\nTesting input: '{input}'");

                // Traditional exception-based approach
                Console.WriteLine("  Exception-based approach:");
                try
                {
                    int result = int.Parse(input);
                    Console.WriteLine($"    Success: {result}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"    Failed: {ex.GetType().Name}");
                }

                // TryParse approach (no exceptions)
                Console.WriteLine("  TryParse approach:");
                if (int.TryParse(input, out int tryResult))
                {
                    Console.WriteLine($"    Success: {tryResult}");
                }
                else
                {
                    Console.WriteLine("    Failed: Invalid format or overflow");
                }
            }

            // Demonstrate custom TryXXX method
            Console.WriteLine("\nCustom TryDivide method:");
            TestCustomTryMethod();

            Console.WriteLine();
        }

        static void TestCustomTryMethod()
        {
            int[][] testCases = { 
                new int[] { 10, 2 }, 
                new int[] { 15, 3 }, 
                new int[] { 7, 0 },   // Division by zero
                new int[] { -20, 4 } 
            };

            foreach (var testCase in testCases)
            {
                int numerator = testCase[0];
                int denominator = testCase[1];

                Console.WriteLine($"  Testing {numerator} ÷ {denominator}:");

                if (TryDivide(numerator, denominator, out int result))
                {
                    Console.WriteLine($"    Success: {result}");
                }
                else
                {
                    Console.WriteLine("    Failed: Division by zero");
                }
            }
        }

        // Custom TryXXX method implementation
        static bool TryDivide(int numerator, int denominator, out int result)
        {
            if (denominator == 0)
            {
                result = 0;
                return false; // Indicate failure
            }

            result = numerator / denominator;
            return true; // Indicate success
        }

        #endregion

        #region Return Codes Alternative

        static void ReturnCodesAlternativeDemo()
        {
            Console.WriteLine("9. RETURN CODES ALTERNATIVE DEMONSTRATION");
            Console.WriteLine("=========================================");

            Console.WriteLine("Comparing exceptions vs return codes:");

            string[] filePaths = { "existing_file.txt", "", "nonexistent.txt" };

            foreach (string path in filePaths)
            {
                Console.WriteLine($"\nTesting file path: '{path}'");

                // Return code approach
                int resultCode = OpenFileWithReturnCode(path);
                Console.WriteLine($"  Return code approach: {GetResultMessage(resultCode)}");

                // Exception approach
                try
                {
                    OpenFileWithException(path);
                    Console.WriteLine("  Exception approach: Success");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  Exception approach: {ex.GetType().Name}");
                }
            }

            Console.WriteLine();
        }

        // Return code approach
        static int OpenFileWithReturnCode(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return -1; // Invalid file path

            if (!File.Exists(filePath))
                return -2; // File not found

            // Simulate opening the file
            return 0; // Success
        }

        static string GetResultMessage(int code)
        {
            return code switch
            {
                0 => "Success",
                -1 => "Error: Invalid file path",
                -2 => "Error: File not found",
                _ => "Unknown error"
            };
        }

        // Exception approach
        static void OpenFileWithException(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path cannot be null or empty");

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            // Simulate opening the file
        }

        #endregion

        #region Real World Scenario

        static void RealWorldScenarioDemo()
        {
            Console.WriteLine("10. REAL WORLD SCENARIO - FILE PROCESSING SYSTEM");
            Console.WriteLine("=================================================");

            var processor = new FileProcessor();

            // Test different scenarios
            Console.WriteLine("Processing various file scenarios:\n");

            processor.ProcessFile("data.txt");
            processor.ProcessFile(""); // Empty path
            processor.ProcessFile("nonexistent.txt"); // File not found
            processor.ProcessFile("corrupted.txt"); // Simulate corruption

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
            FileStream fileStream = null;
            StreamReader reader = null;

            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(filePath))
                    throw new ArgumentException("File path cannot be empty", nameof(filePath));

                // Simulate different file conditions
                if (filePath == "nonexistent.txt")
                    throw new FileNotFoundException($"Could not find file: {filePath}");

                if (filePath == "corrupted.txt")
                    throw new InvalidDataException("File appears to be corrupted");

                // Simulate successful processing
                Console.WriteLine("  ✓ File validation passed");
                Console.WriteLine("  ✓ File opened successfully");
                Console.WriteLine("  ✓ Data processed");
                Console.WriteLine("  ✓ Processing completed successfully");

            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"  ✗ Input Error: {ex.Message}");
                LogError("ARGUMENT_ERROR", ex);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"  ✗ File Error: {ex.Message}");
                LogError("FILE_NOT_FOUND", ex);
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine($"  ✗ Data Error: {ex.Message}");
                LogError("DATA_CORRUPTION", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ✗ Unexpected Error: {ex.Message}");
                LogError("UNEXPECTED_ERROR", ex);
            }
            finally
            {
                // Cleanup resources (this always runs)
                Console.WriteLine("  → Cleaning up resources...");
                
                reader?.Dispose();
                fileStream?.Dispose();
                
                Console.WriteLine("  → Cleanup completed");
            }

            Console.WriteLine(); // Add spacing between files
        }

        private void LogError(string errorType, Exception ex)
        {
            // In a real application, this would write to a log file or logging system
            Console.WriteLine($"  → Logged error: {errorType} at {DateTime.Now:HH:mm:ss}");
        }

        // TryProcess method as an alternative approach
        public bool TryProcessFile(string filePath, out string errorMessage)
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

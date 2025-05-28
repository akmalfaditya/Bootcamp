using System.Text;

namespace VariablesAndParametersDemo
{
    // Example structure for demonstrating 'in' modifier with large structs
    // In real projects, you'd want to avoid copying large structs unnecessarily
    public struct BigCalculationData
    {
        public double[] Values;
        public string Description;
        public DateTime Timestamp;
        
        public BigCalculationData(int size, string desc)
        {
            Values = new double[size];
            Description = desc;
            Timestamp = DateTime.Now;
            
            // Fill with some sample data
            for (int i = 0; i < size; i++)
            {
                Values[i] = i * 1.5;
            }
        }
        
        public double Sum()
        {
            double total = 0;
            foreach (double val in Values)
                total += val;
            return total;
        }
    }
    
    class Program
    {
        // Static field to demonstrate ref returns
        // In production code, be careful with global state like this
        private static string globalMessage = "Initial Global Value";
        
        static void Main(string[] args)
        {
            Console.WriteLine("=== VARIABLES AND PARAMETERS IN C# ===");
            Console.WriteLine("This demo covers everything from basic variables to advanced parameter techniques\n");
            
            // Section 1: Stack vs Heap Demonstration
            DemonstrateStackAndHeap();
            
            // Section 2: Definite Assignment Rules
            DemonstrateDefiniteAssignment();
            
            // Section 3: Default Values
            DemonstrateDefaultValues();
            
            // Section 4: Parameter Passing Modes
            DemonstrateParameterPassing();
            
            // Section 5: Advanced Parameter Features
            DemonstrateAdvancedParameters();
            
            // Section 6: Ref Locals and Returns
            DemonstrateRefLocalsAndReturns();
            
            // Section 7: Real-world practical examples
            DemonstrateRealWorldScenarios();
            
            Console.WriteLine("\n=== SUMMARY ===");
            Console.WriteLine("You've now seen all the essential concepts for variables and parameters in C#");
            Console.WriteLine("Practice these patterns - they're the foundation of effective C# programming!");
        }
        
        #region Stack and Heap Demonstrations
        
        static void DemonstrateStackAndHeap()
        {
            Console.WriteLine("=== STACK VS HEAP MEMORY ===");
            Console.WriteLine("Understanding where your data lives is crucial for performance and memory management\n");
            
            // Stack example - local variables and method calls
            Console.WriteLine("--- Stack Memory (Local Variables) ---");
            Console.WriteLine("Each method call creates a stack frame. Watch how recursion builds up the stack:");
            
            int number = 5;
            int result = CalculateFactorial(number);
            Console.WriteLine($"Factorial of {number} = {result}");
            Console.WriteLine("Notice: Each recursive call added a new 'x' variable to the stack\n");
            
            // Heap example - reference types
            Console.WriteLine("--- Heap Memory (Reference Types) ---");
            Console.WriteLine("Objects created with 'new' go on the heap. Multiple variables can reference the same object:");
            
            // Creating objects on the heap
            StringBuilder builder1 = new StringBuilder("First object");
            Console.WriteLine($"builder1 content: {builder1}");
            
            StringBuilder builder2 = new StringBuilder("Second object");
            StringBuilder builder3 = builder2; // Both reference the same heap object
            
            Console.WriteLine($"builder2 content: {builder2}");
            Console.WriteLine($"builder3 content: {builder3}");
            
            // Modifying through one reference affects the other
            builder2.Append(" - modified!");
            Console.WriteLine($"After modifying builder2:");
            Console.WriteLine($"builder2 content: {builder2}");
            Console.WriteLine($"builder3 content: {builder3}"); // Same object!
            Console.WriteLine("Key point: builder2 and builder3 point to the same heap object\n");
        }
        
        // Recursive method to demonstrate stack usage
        // Each call creates a new stack frame with its own 'x' parameter
        static int CalculateFactorial(int x)
        {
            Console.WriteLine($"  Computing factorial for: {x} (new stack frame created)");
            
            if (x == 0 || x == 1) 
            {
                Console.WriteLine($"  Base case reached, returning 1 (stack frame will be destroyed)");
                return 1;
            }
            
            return x * CalculateFactorial(x - 1);
        }
        
        #endregion
        
        #region Definite Assignment
        
        static void DemonstrateDefiniteAssignment()
        {
            Console.WriteLine("=== DEFINITE ASSIGNMENT RULES ===");
            Console.WriteLine("C# ensures variables are initialized before use - this prevents nasty bugs!\n");
            
            // This would cause a compile error:
            // int uninitialized;
            // Console.WriteLine(uninitialized); // Error: Use of unassigned local variable
            
            Console.WriteLine("--- Local Variables Must Be Initialized ---");
            int localVar = 42; // Must initialize before use
            Console.WriteLine($"Local variable properly initialized: {localVar}");
            
            Console.WriteLine("\n--- Arrays and Fields Auto-Initialize ---");
            int[] numbers = new int[3];
            Console.WriteLine($"Array elements auto-initialize to default values:");
            for (int i = 0; i < numbers.Length; i++)
            {
                Console.WriteLine($"  numbers[{i}] = {numbers[i]} (auto-initialized to 0)");
            }
            
            // Demonstrating conditional initialization
            Console.WriteLine("\n--- Conditional Initialization Example ---");
            int conditionalValue;
            bool shouldInitialize = DateTime.Now.Second % 2 == 0;
            
            if (shouldInitialize)
            {
                conditionalValue = 100;
            }
            else
            {
                conditionalValue = 200;
            }
            
            // Now it's safe to use conditionalValue because both code paths initialize it
            Console.WriteLine($"Conditionally initialized value: {conditionalValue}");
            Console.WriteLine("Compiler ensures all code paths initialize the variable before use\n");
        }
        
        #endregion
        
        #region Default Values
        
        static void DemonstrateDefaultValues()
        {
            Console.WriteLine("=== DEFAULT VALUES FOR TYPES ===");
            Console.WriteLine("Every type in C# has a predictable default value. Know these by heart!\n");
            
            // Numeric types default to zero
            Console.WriteLine("--- Numeric Type Defaults ---");
            Console.WriteLine($"int default: {default(int)}");
            Console.WriteLine($"double default: {default(double)}");
            Console.WriteLine($"decimal default: {default(decimal)}");
            Console.WriteLine($"float default: {default(float)}");
            
            // Boolean defaults to false
            Console.WriteLine($"\nbool default: {default(bool)}");
            
            // Character defaults to null character
            Console.WriteLine($"char default: '{default(char)}' (null character)");
            
            // Reference types default to null
            Console.WriteLine($"\nstring default: {default(string) ?? "null"}");
            Console.WriteLine($"object default: {default(object) ?? "null"}");
              // Custom structs get all fields defaulted
            Console.WriteLine("\n--- Custom Struct Defaults ---");
            BigCalculationData defaultStruct = default(BigCalculationData);
            Console.WriteLine($"Custom struct Values array: {(defaultStruct.Values == null ? "null" : $"array with {defaultStruct.Values.Length} elements")}");
            Console.WriteLine($"Custom struct Description: {defaultStruct.Description ?? "null"}");
            Console.WriteLine($"Custom struct Timestamp: {defaultStruct.Timestamp}");
            
            Console.WriteLine("\nRemember: Default values are predictable and safe - use them to your advantage!\n");
        }
        
        #endregion
        
        #region Parameter Passing Demonstrations
        
        static void DemonstrateParameterPassing()
        {
            Console.WriteLine("=== PARAMETER PASSING MODES ===");
            Console.WriteLine("Master these concepts and you'll write more efficient, predictable code\n");
            
            // Pass by value (default behavior)
            Console.WriteLine("--- Pass by Value (Default) ---");
            int originalValue = 10;
            Console.WriteLine($"Before calling ModifyByValue: originalValue = {originalValue}");
            ModifyByValue(originalValue);
            Console.WriteLine($"After calling ModifyByValue: originalValue = {originalValue}");
            Console.WriteLine("Value unchanged - method worked with a copy\n");
            
            // Pass by reference with 'ref'
            Console.WriteLine("--- Pass by Reference (ref keyword) ---");
            int refValue = 10;
            Console.WriteLine($"Before calling ModifyByRef: refValue = {refValue}");
            ModifyByRef(ref refValue);
            Console.WriteLine($"After calling ModifyByRef: refValue = {refValue}");
            Console.WriteLine("Value changed - method worked with the original variable\n");
            
            // Output parameters with 'out'
            Console.WriteLine("--- Output Parameters (out keyword) ---");
            string fullName = "John Michael Smith";
            Console.WriteLine($"Original name: {fullName}");
            
            SplitName(fullName, out string firstName, out string lastName);
            Console.WriteLine($"First name: {firstName}");
            Console.WriteLine($"Last name: {lastName}");
            Console.WriteLine("'out' parameters don't need to be initialized before the call\n");
            
            // Input parameters with 'in' - for performance with large structs
            Console.WriteLine("--- Input Parameters (in keyword) ---");
            BigCalculationData bigData = new BigCalculationData(1000, "Performance Test Data");
            Console.WriteLine($"Original data description: {bigData.Description}");
            
            double sum = ProcessBigData(in bigData);
            Console.WriteLine($"Sum calculated: {sum:F2}");
            Console.WriteLine("'in' keyword prevents copying large structs - better performance!\n");
        }
        
        // Pass by value - method gets a copy
        static void ModifyByValue(int parameter)
        {
            Console.WriteLine($"  Inside ModifyByValue: received {parameter}");
            parameter = parameter * 2;
            Console.WriteLine($"  Inside ModifyByValue: modified to {parameter}");
            // Original variable remains unchanged
        }
        
        // Pass by reference - method works with original variable
        static void ModifyByRef(ref int parameter)
        {
            Console.WriteLine($"  Inside ModifyByRef: received {parameter}");
            parameter = parameter * 2;
            Console.WriteLine($"  Inside ModifyByRef: modified to {parameter}");
            // Original variable is changed
        }
        
        // Output parameters - method must assign values
        static void SplitName(string fullName, out string firstName, out string lastName)
        {
            int lastSpaceIndex = fullName.LastIndexOf(' ');
            if (lastSpaceIndex > 0)
            {
                firstName = fullName.Substring(0, lastSpaceIndex);
                lastName = fullName.Substring(lastSpaceIndex + 1);
            }
            else
            {
                firstName = fullName;
                lastName = "";
            }
        }
        
        // Input parameter - readonly reference for performance
        static double ProcessBigData(in BigCalculationData data)
        {
            Console.WriteLine($"  Processing: {data.Description}");
            Console.WriteLine($"  Array size: {data.Values?.Length ?? 0} elements");
            
            // We can read from the parameter but cannot modify it
            // data.Description = "Modified"; // This would cause a compiler error
            
            return data.Sum();
        }
        
        #endregion
        
        #region Advanced Parameter Features
        
        static void DemonstrateAdvancedParameters()
        {
            Console.WriteLine("=== ADVANCED PARAMETER FEATURES ===");
            Console.WriteLine("These features make your methods more flexible and user-friendly\n");
            
            // Variable arguments with 'params'
            Console.WriteLine("--- Variable Arguments (params) ---");
            int sum1 = CalculateSum(1, 2, 3);
            Console.WriteLine($"Sum of 1, 2, 3 = {sum1}");
            
            int sum2 = CalculateSum(5, 10, 15, 20, 25);
            Console.WriteLine($"Sum of 5, 10, 15, 20, 25 = {sum2}");
            
            // Can also pass an array
            int[] numbers = { 2, 4, 6, 8 };
            int sum3 = CalculateSum(numbers);
            Console.WriteLine($"Sum of array [2, 4, 6, 8] = {sum3}");
            
            Console.WriteLine("\n--- Optional Parameters ---");
            DisplayMessage(); // Uses default message
            DisplayMessage("Custom message");
            DisplayMessage("Important", true); // Custom message with emphasis
            
            Console.WriteLine("\n--- Named Arguments ---");
            // You can specify parameters by name, in any order
            FormatText(text: "Hello World", uppercase: true, prefix: ">>> ");
            FormatText(prefix: "### ", text: "Named Arguments Rock!");
            FormatText(text: "Mixed approach", uppercase: false); // Mix named and positional
            
            Console.WriteLine();
        }
        
        // Variable number of parameters
        static int CalculateSum(params int[] numbers)
        {
            int total = 0;
            Console.WriteLine($"  Calculating sum of {numbers.Length} numbers:");
            
            foreach (int num in numbers)
            {
                Console.Write($"{num} ");
                total += num;
            }
            Console.WriteLine($"= {total}");
            
            return total;
        }
        
        // Optional parameters with default values
        static void DisplayMessage(string message = "Default message", bool emphasize = false)
        {
            if (emphasize)
            {
                Console.WriteLine($"*** {message.ToUpper()} ***");
            }
            else
            {
                Console.WriteLine($"Message: {message}");
            }
        }
        
        // Method demonstrating named arguments
        static void FormatText(string text, bool uppercase = false, string prefix = "")
        {
            string result = prefix + (uppercase ? text.ToUpper() : text);
            Console.WriteLine($"Formatted: {result}");
        }
        
        #endregion
        
        #region Ref Locals and Returns
        
        static void DemonstrateRefLocalsAndReturns()
        {
            Console.WriteLine("=== REF LOCALS AND REF RETURNS ===");
            Console.WriteLine("Advanced feature for high-performance scenarios - use with care!\n");
            
            Console.WriteLine("--- Ref Locals with Arrays ---");
            int[] scores = { 85, 92, 78, 95, 88 };
            Console.WriteLine($"Original scores: [{string.Join(", ", scores)}]");
            
            // Create a reference to an array element
            ref int topScore = ref scores[3]; // Reference to the 4th element (95)
            Console.WriteLine($"Top score (referenced): {topScore}");
            
            // Modify through the reference
            topScore = 100;
            Console.WriteLine($"After boosting top score: [{string.Join(", ", scores)}]");
            Console.WriteLine("Notice: The original array was modified through the ref local\n");
            
            Console.WriteLine("--- Ref Returns for Global State ---");
            Console.WriteLine($"Original global message: {globalMessage}");
            
            // Get a reference to the global field
            ref string messageRef = ref GetGlobalMessage();
            Console.WriteLine($"Reference retrieved: {messageRef}");
            
            // Modify through the reference
            messageRef = "Modified through ref return!";
            Console.WriteLine($"After modification through ref: {globalMessage}");
            
            Console.WriteLine("\n--- Practical Example: Finding Maximum ---");
            int[] values = { 23, 67, 45, 89, 12, 56 };
            Console.WriteLine($"Values: [{string.Join(", ", values)}]");
            
            ref int maxRef = ref FindMaximum(values);
            Console.WriteLine($"Maximum value: {maxRef}");
            
            // We can modify the maximum value in the original array
            maxRef *= 2;
            Console.WriteLine($"After doubling max: [{string.Join(", ", values)}]");
            Console.WriteLine("The maximum value in the original array was doubled!\n");
        }
        
        // Returns a reference to the global field
        static ref string GetGlobalMessage()
        {
            return ref globalMessage;
        }
        
        // Returns a reference to the maximum element in an array
        static ref int FindMaximum(int[] array)
        {
            if (array.Length == 0)
                throw new ArgumentException("Array cannot be empty");
            
            int maxIndex = 0;
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] > array[maxIndex])
                    maxIndex = i;
            }
            
            return ref array[maxIndex];
        }
        
        #endregion
        
        #region Real-World Scenarios
        
        static void DemonstrateRealWorldScenarios()
        {
            Console.WriteLine("=== REAL-WORLD PRACTICAL EXAMPLES ===");
            Console.WriteLine("Here's how you'll actually use these concepts in production code\n");
            
            Console.WriteLine("--- Scenario 1: User Registration System ---");
            bool registrationSuccess = RegisterUser(
                email: "john.doe@example.com",
                password: "SecurePass123!",
                confirmPassword: "SecurePass123!",
                out string userId,
                out string validationMessage
            );
            
            Console.WriteLine($"Registration successful: {registrationSuccess}");
            Console.WriteLine($"User ID: {userId}");
            Console.WriteLine($"Message: {validationMessage}\n");
            
            Console.WriteLine("--- Scenario 2: Configuration Parser ---");
            var configData = "server=localhost;port=5432;database=myapp;timeout=30";
            var config = ParseConfiguration(configData);
            
            Console.WriteLine($"Parsed configuration:");
            foreach (var setting in config)
            {
                Console.WriteLine($"  {setting.Key} = {setting.Value}");
            }
            
            Console.WriteLine("\n--- Scenario 3: Batch Data Processing ---");
            var salesData = new double[] { 1250.50, 2100.75, 980.25, 1750.00, 3200.80 };
            
            ProcessSalesData(salesData, out double total, out double average, out double highest);
            
            Console.WriteLine($"Sales Analysis Results:");
            Console.WriteLine($"  Total Sales: ${total:F2}");
            Console.WriteLine($"  Average Sale: ${average:F2}");
            Console.WriteLine($"  Highest Sale: ${highest:F2}");
            
            Console.WriteLine("\n--- Scenario 4: Smart Calculator ---");
            double result1 = Calculate(Operation.Add, 15, 25);
            double result2 = Calculate(Operation.Multiply, 7, 8, 3); // Using params
            double result3 = Calculate(Operation.Divide, 100, 4);
            
            Console.WriteLine($"15 + 25 = {result1}");
            Console.WriteLine($"7 × 8 × 3 = {result2}");
            Console.WriteLine($"100 ÷ 4 = {result3}");
        }
        
        // Real-world method: User registration with validation
        static bool RegisterUser(string email, string password, string confirmPassword, 
            out string userId, out string validationMessage)
        {
            userId = "";
            validationMessage = "";
            
            // Validate email
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                validationMessage = "Invalid email address";
                return false;
            }
            
            // Validate password
            if (password.Length < 8)
            {
                validationMessage = "Password must be at least 8 characters";
                return false;
            }
            
            // Confirm password match
            if (password != confirmPassword)
            {
                validationMessage = "Passwords do not match";
                return false;
            }
            
            // Generate user ID (simplified)
            userId = $"USR_{DateTime.Now.Ticks % 100000:D5}";
            validationMessage = "User registered successfully";
            return true;
        }
        
        // Configuration parser returning dictionary
        static Dictionary<string, string> ParseConfiguration(string configString)
        {
            var result = new Dictionary<string, string>();
            var pairs = configString.Split(';');
            
            foreach (var pair in pairs)
            {
                var keyValue = pair.Split('=');
                if (keyValue.Length == 2)
                {
                    result[keyValue[0].Trim()] = keyValue[1].Trim();
                }
            }
            
            return result;
        }
        
        // Batch processing with multiple output parameters
        static void ProcessSalesData(double[] sales, out double total, out double average, out double highest)
        {
            total = 0;
            highest = 0;
            
            foreach (double sale in sales)
            {
                total += sale;
                if (sale > highest)
                    highest = sale;
            }
            
            average = sales.Length > 0 ? total / sales.Length : 0;
        }
        
        // Calculator with params and optional operations
        enum Operation { Add, Subtract, Multiply, Divide }
        
        static double Calculate(Operation op, params double[] numbers)
        {
            if (numbers.Length == 0) return 0;
            
            double result = numbers[0];
            
            for (int i = 1; i < numbers.Length; i++)
            {
                switch (op)
                {
                    case Operation.Add:
                        result += numbers[i];
                        break;
                    case Operation.Subtract:
                        result -= numbers[i];
                        break;
                    case Operation.Multiply:
                        result *= numbers[i];
                        break;
                    case Operation.Divide:
                        if (numbers[i] != 0)
                            result /= numbers[i];
                        break;
                }
            }
            
            return result;
        }
        
        #endregion
    }
}

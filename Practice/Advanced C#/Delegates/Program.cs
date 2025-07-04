using System;
using System.IO;
using System.Threading;

namespace DelegatesDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Delegates in C# - Complete Demonstration ===\n");

            // Run all demonstrations to show delegate concepts in action
            BasicDelegateDemo();
            PluginMethodsDemo();
            InstanceAndStaticMethodTargetsDemo();
            MulticastDelegatesDemo();
            GenericDelegatesDemo();
            FuncAndActionDelegatesDemo();
            DelegateVsInterfaceDemo();
            DelegateCompatibilityDemo();
            ParameterCompatibilityDemo();
            ReturnTypeCompatibilityDemo();
            RealWorldScenarioDemo();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        #region Basic Delegate Usage

        // First, let's define a delegate type - this is like creating a blueprint
        // Any method that takes an int and returns an int can be assigned to this delegate
        delegate int Transformer(int x);

        static void BasicDelegateDemo()
        {
            Console.WriteLine("1. BASIC DELEGATE USAGE - THE FOUNDATION");
            Console.WriteLine("========================================");

            // Step 1: Create a delegate instance pointing to a method
            Transformer t = Square;  // This is shorthand for: new Transformer(Square)
            
            // Step 2: Invoke the delegate just like calling a method
            int result = t(3);  // This calls Square(3) through the delegate
            
            Console.WriteLine($"Square of 3 through delegate: {result}");
            
            // The beauty is indirection - we can change what method gets called
            t = Cube;  // Now t points to a different method
            result = t(3);  // Same syntax, different behavior
            
            Console.WriteLine($"Cube of 3 through same delegate: {result}");
            
            // You can also use the explicit Invoke method
            result = t.Invoke(4);
            Console.WriteLine($"Cube of 4 using Invoke: {result}");
            
            Console.WriteLine();
        }

        // Static methods that match our delegate signature
        static int Square(int x) => x * x;
        static int Cube(int x) => x * x * x;
        #endregion

        #region Plugin Methods with Delegates

        static void PluginMethodsDemo()
        {
            Console.WriteLine("2. WRITING PLUGIN METHODS WITH DELEGATES");
            Console.WriteLine("========================================");

            // This demonstrates the power of delegates for creating pluggable behavior
            int[] values = { 1, 2, 3, 4, 5 };
            
            Console.WriteLine($"Original values: [{string.Join(", ", values)}]");
            
            // Transform array using Square as the plugin
            Transform(values, Square);
            Console.WriteLine($"After Square transform: [{string.Join(", ", values)}]");
            
            // Reset values
            values = new int[] { 1, 2, 3, 4, 5 };
            
            // Same Transform method, different behavior by passing different delegate
            Transform(values, Cube);
            Console.WriteLine($"After Cube transform: [{string.Join(", ", values)}]");
            
            // You can even pass lambda expressions as plugins
            values = new int[] { 1, 2, 3, 4, 5 };
            Transform(values, x => x + 10);  // Add 10 to each element
            Console.WriteLine($"After +10 transform: [{string.Join(", ", values)}]");
            
            Console.WriteLine();
        }

        // This is a higher-order function - it takes a function as a parameter
        static void Transform(int[] values, Transformer t)
        {
            for (int i = 0; i < values.Length; i++)
                values[i] = t(values[i]);  // Apply the plugged-in transformation
        }

        #endregion

        #region Instance and Static Method Targets

        static void InstanceAndStaticMethodTargetsDemo()
        {
            Console.WriteLine("3. INSTANCE AND STATIC METHOD TARGETS");
            Console.WriteLine("=====================================");

            // Static method target - no object instance needed
            Console.WriteLine("Static method delegation:");
            Transformer staticDelegate = Square;
            Console.WriteLine($"Static Square of 6: {staticDelegate(6)}");
            
            // Instance method target - delegate holds both method AND object reference
            Console.WriteLine("\nInstance method delegation:");
            Calculator calc = new Calculator(5);  // Object with multiplier = 5
            Transformer instanceDelegate = calc.MultiplyBy;  // Points to instance method
            
            Console.WriteLine($"Multiply 8 by {calc.Multiplier}: {instanceDelegate(8)}");
            
            // The delegate keeps the object alive - demonstrate this with Target property
            Console.WriteLine($"Delegate Target is null (static): {staticDelegate.Target == null}");
            Console.WriteLine($"Delegate Target is Calculator instance: {instanceDelegate.Target is Calculator}");
            
            // Multiple instances, multiple delegates
            Calculator calc2 = new Calculator(3);
            Transformer instanceDelegate2 = calc2.MultiplyBy;
            
            Console.WriteLine($"Different instance - multiply 8 by {calc2.Multiplier}: {instanceDelegate2(8)}");
            
            Console.WriteLine();
        }

        #endregion

        #region Multicast Delegates

        // For multicast demos, we need void return type
        // With non-void return types, only the last method's return value is kept
        delegate void ProgressReporter(int percentComplete);

        static void MulticastDelegatesDemo()
        {
            Console.WriteLine("4. MULTICAST DELEGATES - COMBINING MULTIPLE METHODS");
            Console.WriteLine("===================================================");

            // Start with a single method
            ProgressReporter reporter = WriteProgressToConsole;
            
            // Add more methods using += operator
            // Remember: delegates are immutable, so += creates a new delegate
            reporter += WriteProgressToFile;
            reporter += SendProgressAlert;
            
            Console.WriteLine("Progress reporting with multicast delegate (3 methods):");
            reporter(50);  // This calls ALL three methods in the order they were added
            
            Console.WriteLine("\nRemoving console reporter using -= operator:");
            reporter -= WriteProgressToConsole;
            
            Console.WriteLine("Progress reporting after removal (2 methods):");
            if (reporter != null)
                reporter(75);
            
            // Demonstrate that return values are lost in multicast (except the last one)
            Console.WriteLine("\nMulticast with return values (only last one is kept):");
            Transformer multiTransformer = Square;
            multiTransformer += Cube;  // Now has two methods
            
            int lastResult = multiTransformer(3);  // Calls Square(3) then Cube(3)
            Console.WriteLine($"Only the last result is returned: {lastResult}");  // Will be 27 (cube), not 9 (square)
            
            Console.WriteLine();
        }

        static void WriteProgressToConsole(int percentComplete)
        {
            Console.WriteLine($"  Console Log: {percentComplete}% complete");
        }

        static void WriteProgressToFile(int percentComplete)
        {
            Console.WriteLine($"  File Log: Writing {percentComplete}% to progress.log");
        }

        static void SendProgressAlert(int percentComplete)
        {
            if (percentComplete >= 75)
                Console.WriteLine($"  Alert: High progress reached - {percentComplete}%!");
        }

        #endregion

        #region Generic Delegate Types

        // Generic delegate - works with any type T
        public delegate TResult Transformer<TArg, TResult>(TArg arg);

        static void GenericDelegatesDemo()
        {
            Console.WriteLine("5. GENERIC DELEGATE TYPES - ULTIMATE REUSABILITY");
            Console.WriteLine("================================================");

            // Same delegate type, different type arguments
            Transformer<int, int> intSquarer = x => x * x;
            Transformer<string, int> stringLength = s => s.Length;
            Transformer<double, string> doubleFormatter = d => $"Value: {d:F2}";
            
            Console.WriteLine($"Int squarer (5): {intSquarer(5)}");
            Console.WriteLine($"String length ('Hello'): {stringLength("Hello")}");
            Console.WriteLine($"Double formatter (3.14159): {doubleFormatter(3.14159)}");
            
            // Using generic Transform method
            Console.WriteLine("\nGeneric Transform method demo:");
            int[] numbers = { 1, 2, 3, 4 };
            Console.WriteLine($"Original numbers: [{string.Join(", ", numbers)}]");
            
            TransformGeneric(numbers, x => x * x);  // Square each number
            Console.WriteLine($"Squared numbers: [{string.Join(", ", numbers)}]");
            
            string[] words = { "cat", "dog", "elephant" };
            Console.WriteLine($"Original words: [{string.Join(", ", words)}]");
            
            TransformGeneric(words, s => s.ToUpper());  // Uppercase each word
            Console.WriteLine($"Uppercase words: [{string.Join(", ", words)}]");
            
            Console.WriteLine();
        }

        // Truly generic transform method
        public static void TransformGeneric<T>(T[] values, Transformer<T, T> transformer)
        {
            for (int i = 0; i < values.Length; i++)
                values[i] = transformer(values[i]);
        }

        #endregion

        #region Func and Action Delegates

        static void FuncAndActionDelegatesDemo()
        {
            Console.WriteLine("6. FUNC AND ACTION DELEGATES - BUILT-IN CONVENIENCE");
            Console.WriteLine("===================================================");

            // Func delegates return values
            // Func<TResult> - no parameters, returns TResult
            // Func<T, TResult> - one parameter of type T, returns TResult
            // ... up to Func<T1, T2, ..., T16, TResult>
            
            Func<int, int> squareFunc = x => x * x;
            Func<int, int, int> addFunc = (a, b) => a + b;
            Func<string> getTimeFunc = () => DateTime.Now.ToString("HH:mm:ss");
            
            Console.WriteLine($"Func square of 7: {squareFunc(7)}");
            Console.WriteLine($"Func add 5 + 8: {addFunc(5, 8)}");
            Console.WriteLine($"Func current time: {getTimeFunc()}");
            
            // Action delegates return void
            // Action - no parameters
            // Action<T> - one parameter of type T
            // ... up to Action<T1, T2, ..., T16>
            
            Action simpleAction = () => Console.WriteLine("  Simple action executed");
            Action<string> messageAction = msg => Console.WriteLine($"  Message: {msg}");
            Action<int, string> complexAction = (num, text) => 
                Console.WriteLine($"  Number: {num}, Text: {text}");
            
            Console.WriteLine("Action demonstrations:");
            simpleAction();
            messageAction("Hello from Action!");
            complexAction(42, "The Answer");
            
            // The beauty: our Transform method can now use Func instead of custom delegate
            Console.WriteLine("\nUsing Func with Transform method:");
            int[] values = { 1, 2, 3, 4, 5 };
            TransformWithFunc(values, x => x * 2);  // Double each value
            Console.WriteLine($"Doubled values: [{string.Join(", ", values)}]");
            
            Console.WriteLine();
        }

        // Transform method using built-in Func delegate
        public static void TransformWithFunc<T>(T[] values, Func<T, T> transformer)
        {
            for (int i = 0; i < values.Length; i++)
                values[i] = transformer(values[i]);
        }

        #endregion

        #region Delegates vs Interfaces

        static void DelegateVsInterfaceDemo()
        {
            Console.WriteLine("7. DELEGATES VS INTERFACES - WHEN TO USE WHAT");
            Console.WriteLine("=============================================");

            // Interface approach
            Console.WriteLine("Interface approach:");
            ITransformer squareTransformer = new SquareTransformer();
            ITransformer cubeTransformer = new CubeTransformer();
            
            TransformWithInterface(new int[] { 2, 3, 4 }, squareTransformer);
            TransformWithInterface(new int[] { 2, 3, 4 }, cubeTransformer);
            
            // Delegate approach (more concise for single-method scenarios)
            Console.WriteLine("\nDelegate approach:");
            Func<int, int> squareDelegate = x => x * x;
            Func<int, int> cubeDelegate = x => x * x * x;
            
            TransformWithDelegate(new int[] { 2, 3, 4 }, squareDelegate);
            TransformWithDelegate(new int[] { 2, 3, 4 }, cubeDelegate);
            
            // Multiple implementations from single class (advantage of delegates)
            Console.WriteLine("\nMultiple implementations from single class:");
            MathOperations math = new MathOperations();
            
            // One class, multiple compatible methods
            TransformWithDelegate(new int[] { 2, 3, 4 }, math.Square);
            TransformWithDelegate(new int[] { 2, 3, 4 }, math.Cube);
            TransformWithDelegate(new int[] { 2, 3, 4 }, math.Double);
            
            Console.WriteLine();
        }

        // Interface approach
        interface ITransformer
        {
            int Transform(int x);
        }

        class SquareTransformer : ITransformer
        {
            public int Transform(int x) => x * x;
        }

        class CubeTransformer : ITransformer
        {
            public int Transform(int x) => x * x * x;
        }

        static void TransformWithInterface(int[] values, ITransformer transformer)
        {
            for (int i = 0; i < values.Length; i++)
                values[i] = transformer.Transform(values[i]);
            Console.WriteLine($"  Result: [{string.Join(", ", values)}]");
        }

        static void TransformWithDelegate(int[] values, Func<int, int> transformer)
        {
            for (int i = 0; i < values.Length; i++)
                values[i] = transformer(values[i]);
            Console.WriteLine($"  Result: [{string.Join(", ", values)}]");
        }

        // Single class with multiple methods (delegate advantage)
        class MathOperations
        {
            public int Square(int x) => x * x;
            public int Cube(int x) => x * x * x;
            public int Double(int x) => x * 2;
        }

        #endregion

        #region Delegate Compatibility

        delegate void D1();
        delegate void D2();

        static void DelegateCompatibilityDemo()
        {
            Console.WriteLine("8. DELEGATE COMPATIBILITY - TYPE SAFETY RULES");
            Console.WriteLine("=============================================");

            void TestMethod() => Console.WriteLine("  Test method executed");

            // Same signature, different delegate types - not compatible
            D1 d1 = TestMethod;
            
            // This would cause compile error:
            // D2 d2 = d1;  // Cannot convert D1 to D2
            
            // But explicit construction works
            D2 d2 = new D2(d1);
            
            Console.WriteLine("Both delegates call the same method:");
            d1();
            d2();
            
            // Delegate equality based on method targets and order
            D1 d1Copy = TestMethod;
            Console.WriteLine($"d1 == d1Copy (same method): {d1 == d1Copy}");  // True
            
            // Multicast delegates - equality depends on all methods in same order
            D1 d1Multi = TestMethod;
            d1Multi += TestMethod;  // Now has two references to TestMethod
            
            Console.WriteLine($"d1 == d1Multi (different invocation lists): {d1 == d1Multi}");  // False
            
            Console.WriteLine();
        }

        #endregion

        #region Parameter Compatibility (Contravariance)

        static void ParameterCompatibilityDemo()
        {
            Console.WriteLine("9. PARAMETER COMPATIBILITY - CONTRAVARIANCE");
            Console.WriteLine("===========================================");

            // Method that takes a more general parameter type
            void ActOnObject(object obj) => Console.WriteLine($"  Processing object: {obj}");
            
            // Delegate that expects a more specific parameter type
            Action<string> stringAction;
            
            // This works! String is more specific than object (contravariance)
            // When we call stringAction("hello"), the string gets passed to ActOnObject
            // and is implicitly upcast to object
            stringAction = ActOnObject;
            
            Console.WriteLine("Calling Action<string> delegate that points to method expecting object:");
            stringAction("Hello contravariance!");
            
            // Real-world example: event handling
            Console.WriteLine("\nReal-world example - event handling:");
            
            // Generic event handler that can handle any EventArgs
            void GenericEventHandler(object sender, EventArgs e)
            {
                Console.WriteLine($"  Event from {sender?.GetType().Name ?? "unknown"} at {DateTime.Now:HH:mm:ss}");
            }
            
            // Specific event handler delegate type
            Action<object, EventArgs> eventHandler = GenericEventHandler;
            
            // Can be used for specific event types due to contravariance
            eventHandler(new Program(), new EventArgs());
            
            Console.WriteLine();
        }

        #endregion

        #region Return Type Compatibility (Covariance)

        static void ReturnTypeCompatibilityDemo()
        {
            Console.WriteLine("10. RETURN TYPE COMPATIBILITY - COVARIANCE");
            Console.WriteLine("==========================================");

            // Method that returns a more specific type
            string GetSpecificString() => "Hello from specific string method";
            
            // Delegate that expects a more general return type
            Func<object> objectGetter;
            
            // This works! String is more specific than object (covariance)
            // The string return value gets implicitly upcast to object
            objectGetter = GetSpecificString;
            
            Console.WriteLine("Calling Func<object> delegate that points to method returning string:");
            object result = objectGetter();
            Console.WriteLine($"  Returned: {result} (actual type: {result.GetType().Name})");
            
            // Real-world example: factory patterns
            Console.WriteLine("\nReal-world example - factory pattern:");
            
            // Factory that returns specific types
            string CreateString() => "Factory created string";
            object CreateInt() => 42;  // Return object instead of int
            
            // General factory delegate
            Func<object> factory;
            
            // Can point to specific factories due to covariance
            factory = CreateString;
            Console.WriteLine($"  String factory result: {factory()}");
            
            factory = CreateInt;
            Console.WriteLine($"  Int factory result: {factory()}");
            
            Console.WriteLine();
        }

        #endregion

        #region Real World Scenario

        static void RealWorldScenarioDemo()
        {
            Console.WriteLine("11. REAL WORLD SCENARIO - FILE PROCESSING SYSTEM");
            Console.WriteLine("================================================");

            // Simulate a file processing system with pluggable processing logic
            FileProcessor processor = new FileProcessor();
            
            // Subscribe to progress events (multicast delegate in action)
            processor.Progress += (percent) => Console.WriteLine($"  Console: {percent}% processed");
            processor.Progress += (percent) => 
            {
                if (percent % 25 == 0)  // Every 25%
                    Console.WriteLine($"  Milestone: Reached {percent}% completion!");
            };
            
            // Different processing strategies using delegates
            Console.WriteLine("Processing with different strategies:");
            
            // Strategy 1: Simple text processing
            Console.WriteLine("\n1. Text processing strategy:");
            processor.ProcessFiles(new[] { "doc1.txt", "doc2.txt" }, ProcessTextFile);
            
            // Strategy 2: Image processing
            Console.WriteLine("\n2. Image processing strategy:");
            processor.ProcessFiles(new[] { "img1.jpg", "img2.png" }, ProcessImageFile);
            
            // Strategy 3: Lambda expression for custom processing
            Console.WriteLine("\n3. Custom processing with lambda:");
            processor.ProcessFiles(new[] { "data1.xml", "data2.json" }, 
                fileName => 
                {
                    Console.WriteLine($"    Custom processing: {fileName}");
                    Thread.Sleep(200);  // Simulate work
                    return $"Processed_{fileName}";
                });
            
            Console.WriteLine();
        }

        // Processing strategy methods
        static string ProcessTextFile(string fileName)
        {
            Console.WriteLine($"    Text processing: {fileName}");
            Thread.Sleep(300);  // Simulate processing time
            return $"TEXT_{fileName}";
        }

        static string ProcessImageFile(string fileName)
        {
            Console.WriteLine($"    Image processing: {fileName}");
            Thread.Sleep(500);  // Simulate longer processing
            return $"IMG_{fileName}";
        }

        #endregion

        #region Supporting Classes

        // Calculator class for instance method demonstration
        public class Calculator
        {
            private int multiplier;
            
            public Calculator(int multiplier)
            {
                this.multiplier = multiplier;
            }
            
            public int Multiplier => multiplier;
            
            // Instance method that matches our Transformer delegate
            public int MultiplyBy(int input)
            {
                return input * multiplier;
            }
        }

        // File processor for real-world scenario
        public class FileProcessor
        {
            // Event using multicast delegate
            public event Action<int>? Progress;
            
            // Method that uses strategy pattern with delegates
            public void ProcessFiles(string[] fileNames, Func<string, string> processingStrategy)
            {
                for (int i = 0; i < fileNames.Length; i++)
                {
                    // Calculate progress
                    int percent = (i * 100) / fileNames.Length;
                    Progress?.Invoke(percent);  // Notify all subscribers
                    
                    // Apply the plugged-in processing strategy
                    string result = processingStrategy(fileNames[i]);
                    Console.WriteLine($"    Result: {result}");
                }
                
                // Final progress report
                Progress?.Invoke(100);
            }
        }

        #endregion
    }
}

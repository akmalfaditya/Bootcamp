namespace DelegatesDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Delegates in C# - Complete Demonstration ===\n");

            // Run all demonstrations
            BasicDelegateDemo();
            InstanceMethodDemo();
            MulticastDelegateDemo();
            GenericDelegateDemo();
            FuncAndActionDemo();
            DelegateCompatibilityDemo();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        #region Basic Delegate Demonstrations
        
        // First, let's define a custom delegate type
        // This delegate can point to any method that takes an int and returns an int
        delegate int Transformer(int x);

        static void BasicDelegateDemo()
        {
            Console.WriteLine("1. BASIC DELEGATE DEMONSTRATION");
            Console.WriteLine("================================");

            // Assign a static method to our delegate
            Transformer squareDelegate = Square;
            
            // Call the delegate - it's like calling the method directly
            int input = 5;
            int result = squareDelegate(input);
            
            Console.WriteLine($"Input: {input}");
            Console.WriteLine($"Square result using delegate: {result}");
            
            // You can also assign different methods to the same delegate type
            Transformer doubleDelegate = Double;
            result = doubleDelegate(input);
            Console.WriteLine($"Double result using delegate: {result}");
            
            Console.WriteLine();
        }

        // Static methods that match our Transformer delegate signature
        static int Square(int x) => x * x;
        static int Double(int x) => x * 2;
        static int Triple(int x) => x * 3;

        #endregion

        #region Instance Method Demonstrations

        static void InstanceMethodDemo()
        {
            Console.WriteLine("2. INSTANCE METHOD DELEGATE DEMONSTRATION");
            Console.WriteLine("=========================================");

            // Create an instance of our Calculator class
            Calculator calc = new Calculator();
            calc.Multiplier = 10;

            // Assign an instance method to our delegate
            // Notice how the delegate holds both the method AND the instance
            Transformer multiplyDelegate = calc.MultiplyByFactor;
            
            int input = 7;
            int result = multiplyDelegate(input);
            
            Console.WriteLine($"Input: {input}");
            Console.WriteLine($"Multiplier: {calc.Multiplier}");
            Console.WriteLine($"Result using instance method delegate: {result}");
            
            Console.WriteLine();
        }

        #endregion

        #region Multicast Delegate Demonstrations

        // For multicast, we need a delegate that returns void
        // because multicast delegates with return values only return the last result
        delegate void ProgressReporter(int percentComplete);

        static void MulticastDelegateDemo()
        {
            Console.WriteLine("3. MULTICAST DELEGATE DEMONSTRATION");
            Console.WriteLine("===================================");

            // Start with one method
            ProgressReporter reporter = WriteProgressToConsole;
            
            // Add more methods using +=
            reporter += WriteProgressToFile;
            reporter += SendProgressNotification;

            Console.WriteLine("Calling multicast delegate with 75% progress:");
            
            // This single call will invoke ALL three methods in order
            reporter(75);
            
            Console.WriteLine("\nRemoving console reporter using -=:");
            reporter -= WriteProgressToConsole;
            
            Console.WriteLine("Calling multicast delegate again with 100% progress:");
            reporter(100);
            
            Console.WriteLine();
        }

        // Methods for multicast delegate demonstration
        static void WriteProgressToConsole(int percentComplete)
        {
            Console.WriteLine($"  Console: Progress is {percentComplete}%");
        }

        static void WriteProgressToFile(int percentComplete)
        {
            // In a real app, you'd write to an actual file
            Console.WriteLine($"  File: Writing {percentComplete}% to log file");
        }

        static void SendProgressNotification(int percentComplete)
        {
            Console.WriteLine($"  Notification: Sending {percentComplete}% update to users");
        }

        #endregion

        #region Generic Delegate Demonstrations

        // Generic delegate that works with any type T
        delegate T GenericTransformer<T>(T input);

        static void GenericDelegateDemo()
        {
            Console.WriteLine("4. GENERIC DELEGATE DEMONSTRATION");
            Console.WriteLine("=================================");

            // Using the generic delegate with integers
            GenericTransformer<int> intSquarer = x => x * x;
            Console.WriteLine($"Square of 8: {intSquarer(8)}");

            // Using the generic delegate with strings
            GenericTransformer<string> stringRepeater = s => s + s;
            Console.WriteLine($"Double 'Hello': {stringRepeater("Hello")}");

            // Using the generic delegate with doubles
            GenericTransformer<double> doubleSquareRoot = Math.Sqrt;
            Console.WriteLine($"Square root of 16.0: {doubleSquareRoot(16.0)}");

            // Demonstrating with arrays using a utility method
            int[] numbers = { 1, 2, 3, 4, 5 };
            Console.WriteLine($"Original array: [{string.Join(", ", numbers)}]");
            
            TransformArray(numbers, x => x * x);
            Console.WriteLine($"After squaring: [{string.Join(", ", numbers)}]");

            Console.WriteLine();
        }

        // Utility method that uses generic delegates
        static void TransformArray<T>(T[] array, GenericTransformer<T> transformer)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = transformer(array[i]);
            }
        }

        #endregion

        #region Func and Action Demonstrations

        static void FuncAndActionDemo()
        {
            Console.WriteLine("5. FUNC AND ACTION DELEGATE DEMONSTRATION");
            Console.WriteLine("=========================================");

            // Func delegates return a value
            // Func<int, int> means: takes int parameter, returns int
            Func<int, int> quadratic = x => x * x * x;
            Console.WriteLine($"Cube of 4 using Func: {quadratic(4)}");

            // Func with multiple parameters
            // Func<int, int, int> means: takes two int parameters, returns int
            Func<int, int, int> adder = (x, y) => x + y;
            Console.WriteLine($"5 + 3 using Func: {adder(5, 3)}");

            // Action delegates don't return anything (void)
            Action<string> printer = message => Console.WriteLine($"Message: {message}");
            printer("Hello from Action delegate!");

            // Action with multiple parameters
            Action<string, int> repeater = (text, times) =>
            {
                for (int i = 0; i < times; i++)
                {
                    Console.WriteLine($"  {text}");
                }
            };
            
            Console.WriteLine("Repeating 'Training' 3 times:");
            repeater("Training", 3);

            // Func and Action can also be multicast (for Action)
            Action<string> multiLogger = msg => Console.WriteLine($"Logger 1: {msg}");
            multiLogger += msg => Console.WriteLine($"Logger 2: {msg}");
            
            Console.WriteLine("Multicast Action demo:");
            multiLogger("This message goes to multiple loggers");

            Console.WriteLine();
        }

        #endregion

        #region Delegate Compatibility Demonstrations

        // Two delegate types with identical signatures but different names
        delegate void TypeA();
        delegate void TypeB();

        static void DelegateCompatibilityDemo()
        {
            Console.WriteLine("6. DELEGATE COMPATIBILITY DEMONSTRATION");
            Console.WriteLine("======================================");

            // Method that matches both delegate signatures
            void SampleMethod() => Console.WriteLine("Sample method executed");

            // Create delegates of different types
            TypeA delegateA = SampleMethod;
            
            // This would cause a compilation error:
            // TypeB delegateB = delegateA; // Cannot convert TypeA to TypeB
            
            // But we can explicitly create a new delegate instance
            TypeB delegateB = new TypeB(delegateA);
            
            Console.WriteLine("Calling delegateA:");
            delegateA();
            
            Console.WriteLine("Calling delegateB (created from delegateA):");
            delegateB();

            // Demonstrate delegate equality
            TypeA delegateA2 = SampleMethod;
            Console.WriteLine($"delegateA == delegateA2: {delegateA == delegateA2}"); // True
            
            // Multicast delegate equality
            TypeA multiDelegate = SampleMethod;
            multiDelegate += SampleMethod;  // Now has two references
            Console.WriteLine($"delegateA == multiDelegate: {delegateA == multiDelegate}"); // False

            // Parameter compatibility (contravariance)
            DemonstrateParameterCompatibility();

            Console.WriteLine();
        }

        static void DemonstrateParameterCompatibility()
        {
            Console.WriteLine("\nParameter Compatibility (Contravariance):");
            
            // Delegate that expects a string parameter
            Action<string> stringAction;
            
            // Method that accepts object (more general than string)
            void ProcessObject(object obj) => Console.WriteLine($"Processing: {obj}");
            
            // This works because string IS-A object (contravariance)
            stringAction = ProcessObject;
            stringAction("Hello contravariance!");
        }

        #endregion
    }

    #region Supporting Classes

    // Helper class for instance method demonstrations
    public class Calculator
    {
        public int Multiplier { get; set; }

        // Instance method that matches our Transformer delegate
        public int MultiplyByFactor(int input)
        {
            return input * Multiplier;
        }
    }

    #endregion
}

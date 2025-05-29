namespace Classes
{
class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Classes and Type Members Demo ===\n");

            // Demonstrate basic class usage
            DemonstrateBasicClasses();
            
            // Show field concepts
            DemonstrateFields();
            
            // Constants demonstration
            DemonstrateConstants();
            
            // Methods and expression-bodied methods
            DemonstrateMethods();
            
            // Constructor overloading
            DemonstrateConstructors();
            
            // Properties (both manual and automatic)
            DemonstrateProperties();
            
            // Indexers in action
            DemonstrateIndexers();
            
            // Static classes
            DemonstrateStaticClasses();
            
            // Finalizers (just showing the concept)
            DemonstrateFinalizers();
            
            // Partial classes
            DemonstratePartialClasses();

            Console.WriteLine("\n=== Demo Complete ===");
            Console.ReadKey();
        }

        static void DemonstrateBasicClasses()
        {
            Console.WriteLine("1. Basic Classes:");
            
            // Creating an employee - this shows basic class usage
            var employee = new Employee("John Doe", 30);
            Console.WriteLine($"Employee: {employee.Name}, Age: {employee.Age}");
            Console.WriteLine();
        }

        static void DemonstrateFields()
        {
            Console.WriteLine("2. Fields (Static, Instance, Readonly):");
            
            // Regular instance fields
            var octopus1 = new Octopus("Ollie");
            var octopus2 = new Octopus("Oscar");
            
            Console.WriteLine($"Octopus 1: {octopus1.Name}, Age: {octopus1.Age}");
            Console.WriteLine($"Octopus 2: {octopus2.Name}, Age: {octopus2.Age}");
            
            // Static field - shared across all instances
            Console.WriteLine($"All octopuses have {Octopus.Legs} legs");
            Console.WriteLine();
        }

        static void DemonstrateConstants()
        {
            Console.WriteLine("3. Constants:");
            
            // Constants are evaluated at compile time
            Console.WriteLine($"PI value: {MathConstants.PI}");
            Console.WriteLine($"Speed of light: {MathConstants.SPEED_OF_LIGHT} m/s");
            Console.WriteLine();
        }

        static void DemonstrateMethods()
        {
            Console.WriteLine("4. Methods (Regular and Expression-bodied):");
            
            var mathOps = new MathOperations();
            
            // Regular method
            int sum = mathOps.Add(5, 3);
            Console.WriteLine($"5 + 3 = {sum}");
            
            // Expression-bodied method
            int product = mathOps.Multiply(4, 7);
            Console.WriteLine($"4 * 7 = {product}");
            
            // Method with more complex logic
            bool isEven = mathOps.IsEven(10);
            Console.WriteLine($"Is 10 even? {isEven}");
            Console.WriteLine();
        }

        static void DemonstrateConstructors()
        {
            Console.WriteLine("5. Constructor Overloading:");
            
            // Using different constructors
            var car1 = new Car("Toyota");  // Only make specified
            var car2 = new Car("Honda", "Civic");  // Both make and model
            
            Console.WriteLine($"Car 1: {car1.Make} {car1.Model}");
            Console.WriteLine($"Car 2: {car2.Make} {car2.Model}");
            Console.WriteLine();
        }

        static void DemonstrateProperties()
        {
            Console.WriteLine("6. Properties (Manual and Automatic):");
            
            var stock = new Stock();
            
            // Using automatic property
            stock.Symbol = "AAPL";
            Console.WriteLine($"Stock symbol: {stock.Symbol}");
            
            // Using manual property with validation
            stock.CurrentPrice = 150.75m;
            Console.WriteLine($"Current price: ${stock.CurrentPrice}");
            
            // Try to set invalid price (this will be prevented)
            try
            {
                stock.CurrentPrice = -10;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.WriteLine();
        }

        static void DemonstrateIndexers()
        {
            Console.WriteLine("7. Indexers:");
            
            var sentence = new Sentence();
            
            // Using indexer to access words
            Console.WriteLine($"Word at index 0: {sentence[0]}");
            Console.WriteLine($"Word at index 2: {sentence[2]}");
            
            // Modifying a word using indexer
            sentence[3] = "dog";
            Console.WriteLine($"After changing word 3: {sentence[0]} {sentence[1]} {sentence[2]} {sentence[3]}");
            Console.WriteLine();
        }

        static void DemonstrateStaticClasses()
        {
            Console.WriteLine("8. Static Classes:");
            
            // Static classes cannot be instantiated
            // All members must be accessed through the class name
            double result = MathUtilities.Add(10.5, 20.3);
            Console.WriteLine($"10.5 + 20.3 = {result}");
            
            double squareRoot = MathUtilities.SquareRoot(16);
            Console.WriteLine($"Square root of 16 = {squareRoot}");
            Console.WriteLine();
        }

        static void DemonstrateFinalizers()
        {
            Console.WriteLine("9. Finalizers:");
            
            // Create objects that will be finalized
            // Note: Finalizers are called by GC, timing is unpredictable
            var resource1 = new ResourceManager("Database Connection");
            var resource2 = new ResourceManager("File Handle");
            
            Console.WriteLine("ResourceManager objects created");
            Console.WriteLine("(Finalizers will be called when GC runs)");
            
            // Force garbage collection for demonstration
            // In real code, you rarely call this explicitly
            resource1 = null;
            resource2 = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Console.WriteLine();
        }

        static void DemonstratePartialClasses()
        {
            Console.WriteLine("10. Partial Classes:");
            
            var payment = new PaymentForm();
            payment.ProcessPayment(100.50m);
            payment.ValidateForm();
            
            Console.WriteLine("Partial class methods executed successfully");
            Console.WriteLine();
        }
    }
}

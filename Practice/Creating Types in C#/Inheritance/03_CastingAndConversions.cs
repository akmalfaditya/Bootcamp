using System;

namespace Inheritance
{
    /// <summary>
    /// This file demonstrates casting and reference conversions in C# inheritance.
    /// These concepts are crucial for understanding how to work with objects in inheritance hierarchies.
    /// 
    /// Key concepts covered:
    /// 1. Upcasting (implicit, always safe)
    /// 2. Downcasting (explicit, can fail)
    /// 3. The 'as' operator (safe downcasting)
    /// 4. The 'is' operator (type checking)
    /// 5. Pattern variables (C# 7+)
    /// </summary>

    // Base class for our casting demonstrations
    public class Vehicle
    {
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        
        public virtual void StartEngine()
        {
            Console.WriteLine($"Starting {Brand} {Model} engine...");
        }
        
        public void ShowBasicInfo()
        {
            Console.WriteLine($"{Year} {Brand} {Model}");
        }
    }

    // First derived class
    public class Car : Vehicle
    {
        public int NumberOfDoors { get; set; }
        public bool HasAirConditioning { get; set; }
        
        public override void StartEngine()
        {
            Console.WriteLine($"Car engine started with key. {Brand} {Model} is ready to drive!");
        }
        
        public void OpenTrunk()
        {
            Console.WriteLine($"Opening {Brand} {Model} trunk...");
        }
        
        public void ShowCarSpecifics()
        {
            Console.WriteLine($"Doors: {NumberOfDoors}, AC: {(HasAirConditioning ? "Yes" : "No")}");
        }
    }

    // Second derived class
    public class Motorcycle : Vehicle
    {
        public int EngineCC { get; set; }
        public bool HasSidecar { get; set; }
        
        public override void StartEngine()
        {
            Console.WriteLine($"Motorcycle engine roaring to life! {Brand} {Model} - {EngineCC}cc");
        }
        
        public void PopWheelies()
        {
            Console.WriteLine($"Popping wheelies on the {Brand} {Model}!");
        }
        
        public void ShowMotorcycleSpecifics()
        {
            Console.WriteLine($"Engine: {EngineCC}cc, Sidecar: {(HasSidecar ? "Yes" : "No")}");
        }
    }

    // Third derived class
    public class Truck : Vehicle
    {
        public decimal PayloadCapacity { get; set; }
        public bool IsCommercial { get; set; }
        
        public override void StartEngine()
        {
            Console.WriteLine($"Heavy truck engine starting... {Brand} {Model} ready for heavy work!");
        }
        
        public void LoadCargo()
        {
            Console.WriteLine($"Loading cargo into {Brand} {Model} (capacity: {PayloadCapacity} tons)");
        }
        
        public void ShowTruckSpecifics()
        {
            Console.WriteLine($"Payload: {PayloadCapacity} tons, Commercial: {(IsCommercial ? "Yes" : "No")}");
        }
    }

    /// <summary>
    /// Demonstration class for casting and reference conversions
    /// </summary>
    public static class CastingAndConversionsDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== CASTING AND REFERENCE CONVERSIONS DEMONSTRATION ===\n");
            
            // Create instances of derived classes
            var myCar = new Car
            {
                Brand = "Toyota",
                Model = "Camry",
                Year = 2023,
                NumberOfDoors = 4,
                HasAirConditioning = true
            };
            
            var myMotorcycle = new Motorcycle
            {
                Brand = "Harley-Davidson",
                Model = "Street 750",
                Year = 2022,
                EngineCC = 750,
                HasSidecar = false
            };
            
            var myTruck = new Truck
            {
                Brand = "Ford",
                Model = "F-150",
                Year = 2023,
                PayloadCapacity = 1.5m,
                IsCommercial = false
            };
            
            // 1. UPCASTING DEMONSTRATION
            Console.WriteLine("1. UPCASTING (Implicit - Always Safe):");
            Console.WriteLine("Converting from derived class to base class reference\n");
            
            // Upcasting - implicit conversion from derived to base
            // This is always safe because derived classes contain everything the base class has
            Vehicle vehicle1 = myCar;        // Car -> Vehicle (implicit upcast)
            Vehicle vehicle2 = myMotorcycle; // Motorcycle -> Vehicle (implicit upcast)
            Vehicle vehicle3 = myTruck;      // Truck -> Vehicle (implicit upcast)
            
            Console.WriteLine("After upcasting, all references are Vehicle type:");
            Console.WriteLine($"vehicle1 type: {vehicle1.GetType().Name} (was Car)");
            Console.WriteLine($"vehicle2 type: {vehicle2.GetType().Name} (was Motorcycle)");
            Console.WriteLine($"vehicle3 type: {vehicle3.GetType().Name} (was Truck)");
            
            Console.WriteLine("\nBut they still refer to the same objects:");
            Console.WriteLine($"myCar == vehicle1: {ReferenceEquals(myCar, vehicle1)}");
            Console.WriteLine($"myMotorcycle == vehicle2: {ReferenceEquals(myMotorcycle, vehicle2)}");
            
            Console.WriteLine("\nAfter upcasting, you can only access base class members:");
            vehicle1.StartEngine();     // OK - StartEngine is in Vehicle
            vehicle1.ShowBasicInfo();   // OK - ShowBasicInfo is in Vehicle
            // vehicle1.OpenTrunk();    // ERROR - OpenTrunk is Car-specific, not available on Vehicle reference
            
            // 2. DOWNCASTING DEMONSTRATION
            Console.WriteLine("\n2. DOWNCASTING (Explicit - Can Fail):");
            Console.WriteLine("Converting from base class reference back to derived class\n");
            
            // Downcasting - explicit conversion from base to derived
            // This can fail if the object isn't actually of the target type
            Car carAgain = (Car)vehicle1;           // Explicit downcast: Vehicle -> Car
            Motorcycle motorcycleAgain = (Motorcycle)vehicle2; // Vehicle -> Motorcycle
            
            Console.WriteLine("Successful downcasts:");
            Console.WriteLine($"carAgain type: {carAgain.GetType().Name}");
            carAgain.OpenTrunk();        // Now we can access Car-specific methods
            carAgain.ShowCarSpecifics();
            
            Console.WriteLine($"\nmotorcycleAgain type: {motorcycleAgain.GetType().Name}");
            motorcycleAgain.PopWheelies(); // Now we can access Motorcycle-specific methods
            
            // Demonstrate failed downcasting
            Console.WriteLine("\n2a. Failed Downcast Example:");
            try
            {
                // This will throw InvalidCastException because vehicle1 is actually a Car, not a Motorcycle
                Motorcycle wrongCast = (Motorcycle)vehicle1;
                wrongCast.PopWheelies();
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine($"InvalidCastException caught: {ex.Message}");
                Console.WriteLine("You can't cast a Car to a Motorcycle!");
            }
            
            // 3. THE 'AS' OPERATOR DEMONSTRATION
            Console.WriteLine("\n3. THE 'AS' OPERATOR (Safe Downcasting):");
            Console.WriteLine("Returns null instead of throwing exception if cast fails\n");
            
            // The 'as' operator returns null if the cast fails, instead of throwing an exception
            Car? possibleCar = vehicle1 as Car;           // Safe downcast
            Motorcycle? possibleMotorcycle = vehicle1 as Motorcycle; // This will be null
            Truck? possibleTruck = vehicle1 as Truck;     // This will also be null
            
            Console.WriteLine("Using 'as' operator for safe casting:");
            if (possibleCar != null)
            {
                Console.WriteLine($"vehicle1 is indeed a Car: {possibleCar.Brand} {possibleCar.Model}");
                possibleCar.OpenTrunk();
            }
            
            if (possibleMotorcycle != null)
            {
                Console.WriteLine("vehicle1 is a Motorcycle");
                possibleMotorcycle.PopWheelies();
            }
            else
            {
                Console.WriteLine("vehicle1 is NOT a Motorcycle (possibleMotorcycle is null)");
            }
            
            if (possibleTruck != null)
            {
                Console.WriteLine("vehicle1 is a Truck");
            }
            else
            {
                Console.WriteLine("vehicle1 is NOT a Truck (possibleTruck is null)");
            }
            
            // 4. THE 'IS' OPERATOR DEMONSTRATION
            Console.WriteLine("\n4. THE 'IS' OPERATOR (Type Checking):");
            Console.WriteLine("Tests whether an object is of a specific type\n");
            
            // The 'is' operator checks if an object can be cast to a specific type
            Console.WriteLine("Type checking with 'is' operator:");
            Console.WriteLine($"vehicle1 is Vehicle: {vehicle1 is Vehicle}");   // True
            Console.WriteLine($"vehicle1 is Car: {vehicle1 is Car}");           // True
            Console.WriteLine($"vehicle1 is Motorcycle: {vehicle1 is Motorcycle}"); // False
            Console.WriteLine($"vehicle1 is Truck: {vehicle1 is Truck}");       // False
            
            Console.WriteLine($"\nvehicle2 is Vehicle: {vehicle2 is Vehicle}");   // True
            Console.WriteLine($"vehicle2 is Car: {vehicle2 is Car}");           // False
            Console.WriteLine($"vehicle2 is Motorcycle: {vehicle2 is Motorcycle}"); // True
            
            // Using 'is' for safe method calls
            Console.WriteLine("\nUsing 'is' for conditional method calls:");
            if (vehicle1 is Car)
            {
                ((Car)vehicle1).OpenTrunk();
                Console.WriteLine("Called Car-specific method after 'is' check");
            }
            
            if (vehicle2 is Motorcycle)
            {
                ((Motorcycle)vehicle2).PopWheelies();
                Console.WriteLine("Called Motorcycle-specific method after 'is' check");
            }
            
            // 5. PATTERN VARIABLES (C# 7+)
            Console.WriteLine("\n5. PATTERN VARIABLES (C# 7+):");
            Console.WriteLine("Combine type checking and casting in one expression\n");
            
            // Pattern variables - combine 'is' check with variable declaration
            // If the check succeeds, the variable is automatically cast and assigned
            Console.WriteLine("Using pattern variables for cleaner code:");
            
            if (vehicle1 is Car carVar)
            {
                Console.WriteLine($"vehicle1 is a Car - Brand: {carVar.Brand}, Doors: {carVar.NumberOfDoors}");
                carVar.ShowCarSpecifics();
            }
            
            if (vehicle2 is Motorcycle motoVar)
            {
                Console.WriteLine($"vehicle2 is a Motorcycle - Brand: {motoVar.Brand}, Engine: {motoVar.EngineCC}cc");
                motoVar.ShowMotorcycleSpecifics();
            }
            
            if (vehicle3 is Truck truckVar)
            {
                Console.WriteLine($"vehicle3 is a Truck - Brand: {truckVar.Brand}, Payload: {truckVar.PayloadCapacity} tons");
                truckVar.ShowTruckSpecifics();
            }
            
            // 6. PRACTICAL EXAMPLE - PROCESSING MIXED TYPES
            Console.WriteLine("\n6. PRACTICAL EXAMPLE - Processing Mixed Vehicle Types:");
            ProcessVehicles(myCar, myMotorcycle, myTruck);
        }
        
        /// <summary>
        /// Practical example showing how to handle different types in a single method
        /// This demonstrates real-world usage of casting and type checking
        /// </summary>
        private static void ProcessVehicles(params Vehicle[] vehicles)
        {
            Console.WriteLine("\nProcessing mixed vehicle types in a single method:");
            
            foreach (Vehicle vehicle in vehicles)
            {
                // All vehicles can do these base operations
                Console.WriteLine($"\n--- Processing {vehicle.GetType().Name} ---");
                vehicle.ShowBasicInfo();
                vehicle.StartEngine();
                
                // Use pattern variables to handle type-specific operations
                switch (vehicle)
                {
                    case Car car:
                        Console.WriteLine("This is a car - performing car-specific operations:");
                        car.ShowCarSpecifics();
                        car.OpenTrunk();
                        break;
                        
                    case Motorcycle motorcycle:
                        Console.WriteLine("This is a motorcycle - performing motorcycle-specific operations:");
                        motorcycle.ShowMotorcycleSpecifics();
                        motorcycle.PopWheelies();
                        break;
                        
                    case Truck truck:
                        Console.WriteLine("This is a truck - performing truck-specific operations:");
                        truck.ShowTruckSpecifics();
                        truck.LoadCargo();
                        break;
                        
                    default:
                        Console.WriteLine("Unknown vehicle type");
                        break;
                }
            }
            
            // Alternative approach using 'is' with pattern variables
            Console.WriteLine("\nAlternative processing using 'is' pattern variables:");
            foreach (Vehicle vehicle in vehicles)
            {
                Console.WriteLine($"\nProcessing {vehicle.Brand} {vehicle.Model}:");
                
                if (vehicle is Car c)
                {
                    Console.WriteLine($"  Car with {c.NumberOfDoors} doors");
                }
                else if (vehicle is Motorcycle m)
                {
                    Console.WriteLine($"  Motorcycle with {m.EngineCC}cc engine");
                }
                else if (vehicle is Truck t)
                {
                    Console.WriteLine($"  Truck with {t.PayloadCapacity} ton capacity");
                }
            }
        }
    }
}

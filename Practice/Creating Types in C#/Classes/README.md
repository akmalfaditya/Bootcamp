# C# Classes: A Comprehensive Guide to Object-Oriented Programming

## Overview

Welcome to this in-depth exploration of C# classes, the fundamental building block of object-oriented programming (OOP). This project is designed as a practical, hands-on guide for developers who are learning C# or wish to solidify their understanding of core OOP principles. It provides clear, well-commented examples for every major class feature, from the basics of fields and methods to advanced topics like partial classes and primary constructors.

## Learning Objectives

By studying and running this project, you will gain a solid understanding of:

- **Class Fundamentals**: How to define and instantiate classes, and work with fields, methods, and properties.
- **Object Initialization**: The roles of constructors, constructor chaining, and modern object initializers.
- **Advanced Class Mechanics**: Implementing indexers, deconstructors, static members, and finalizers.
- **Modern C# Features**: Leveraging primary constructors (C# 12), expression-bodied members, and partial types.
- **Core OOP Principles**: Applying encapsulation, data validation, and effective resource management.

## Project Structure

The project is organized into individual files, each demonstrating a specific concept. The `Program.cs` file acts as a central runner that executes demonstrations for each feature in a logical sequence.

```
Classes/
├── Program.cs              # The main entry point that runs all demonstrations.
├── Employee.cs             # Demonstrates a basic class structure.
├── Octopus.cs              # Explains instance, static, and readonly fields.
├── MathConstants.cs        # Compares compile-time `const` vs. runtime `static readonly` fields.
├── MathOperations.cs       # Covers various method types, including overloading and local methods.
├── Car.cs                  # Shows constructor overloading and chaining for flexible object creation.
├── Stock.cs                # Implements properties with custom validation logic.
├── Sentence.cs             # Uses an indexer to provide array-like access to an object.
├── Bunny.cs                # Illustrates the use of object initializers for clean syntax.
├── Rectangle.cs            # Features a deconstructor to easily extract object data.
├── MathUtilities.cs        # A static class that serves as a collection of utility methods.
├── Person.cs               # Uses a primary constructor (C# 12) for concise syntax.
├── Point.cs                # Demonstrates a primary constructor and its use in properties.
├── PaymentForm.cs          # A partial class, with its implementation split across files.
├── Panda.cs                # Shows how the `this` keyword enables method chaining and self-reference.
├── ResourceManager.cs      # Implements a finalizer for resource cleanup during garbage collection.
└── README.md               # This documentation file.
```

## Key Features Demonstrated

### 1. **Fields**: The data held by a class.
- **Instance Fields**: Data unique to each object instance.
- **Static Fields**: Data shared across all instances of a class.
- **Readonly Fields**: Fields that can only be assigned upon declaration or in a constructor.

```csharp
// From Octopus.cs
public readonly Guid Id;             // Readonly, unique to each instance
public static int TotalCreated;      // Static, shared by all instances
```

### 2. **Properties**: A modern and safe way to expose class data.
- **Auto-Implemented Properties**: For simple get/set behavior without extra logic.
- **Properties with Backing Fields**: For implementing custom validation or computation.
- **Calculated Properties**: Properties that compute their value on the fly.

```csharp
// From Stock.cs - A property with validation
public decimal CurrentPrice
{
    get { return _currentPrice; }
    set
    {
        if (value < 0)
            throw new ArgumentException("Stock price cannot be negative!");
        _currentPrice = value;
    }
}
```

### 3. **Methods**: The behaviors or actions an object can perform.
- **Instance Methods**: Operate on the data of a specific object instance.
- **Method Overloading**: Defining multiple methods with the same name but different parameters.
- **Expression-Bodied Methods**: A concise syntax for methods that contain a single expression.

```csharp
// From MathOperations.cs
public int Power(int baseNum, int exponent) { /* ... */ }
public double Power(double baseNum, double exponent) { /* ... */ } // Overload
```

### 4. **Constructors**: Special methods for creating and initializing objects.
- **Constructor Overloading**: Providing multiple constructors for flexible object creation.
- **Constructor Chaining**: When one constructor calls another, reducing code duplication.

```csharp
// From Car.cs
public Car(string make, string model) : this(make, model, DateTime.Now.Year)
{
    // This constructor chains to a more detailed one, providing a default year.
}
```

### 5. **Indexers**: Allow objects to be indexed like an array.
- Provides a natural syntax for accessing elements in a collection-like object.

```csharp
// From Sentence.cs
public string this[int index]
{
    get { return _words[index]; }
    set { _words[index] = value; }
}
```

### 6. **Object Initializers & Deconstructors**
- **Object Initializers**: Set properties at the time of creation without calling a constructor.
- **Deconstructors**: Allow an object to be "unpacked" into a set of variables.

```csharp
// Initializer
var bunny = new Bunny { Name = "Fluffy", LikesCarrots = true };

// Deconstructor
var (width, height) = new Rectangle(10, 5);
```

### 7. **Static Classes & Members**
- **Static Classes**: Cannot be instantiated and can only contain static members. Ideal for utility functions.
- **Static Members**: Methods or properties that belong to the class itself, not to any single instance.

```csharp
// From MathUtilities.cs
public static class MathUtilities
{
    public static double SquareRoot(double number) => Math.Sqrt(number);
}
```

### 8. **Primary Constructors (C# 12)**
- A concise way to declare a constructor whose parameters are available throughout the class body.

```csharp
// From Person.cs
public class Person(string firstName, string lastName)
{
    public string FullName => $"{firstName} {lastName}";
}
```

### 9. **Partial Classes & Methods**
- **Partial Classes**: Allow a single class definition to be split across multiple files.
- **Partial Methods**: Can be defined in one part of a partial class and optionally implemented in another.

### 10. **The `this` Keyword**
- Refers to the current instance of the class. It is used to access members, as a parameter to other methods, and to enable fluent method chaining.

```csharp
// From Panda.cs
public Panda SetMate(Panda newMate)
{
    this.Mate = newMate;
    return this; // Return the instance to allow method chaining
}
```

### 11. **Finalizers**
- A special method used to clean up unmanaged resources before an object is garbage collected.

```csharp
// From ResourceManager.cs
~ResourceManager()
{
    // This code runs when the object is garbage collected.
    Console.WriteLine($"🗑️ Finalizer called for resource '{_name}'. Cleaning up.");
}
```

## How to Run This Project

1.  **Prerequisites**: Ensure you have the .NET 8.0 SDK or a later version installed.
2.  **Open a Terminal**: Navigate to the `Classes` project directory.
3.  **Build the Project**: Run the command `dotnet build`. This will compile the code and report any errors.
4.  **Run the Demonstrations**: Execute the command `dotnet run`. You will see the output of all demonstrations in your console.

## Educational Flow

The program is structured to guide you from simple to complex topics, creating a natural learning path:
1.  **Fundamentals**: Begins with basic classes, fields, and methods.
2.  **Object Creation**: Moves to properties, constructors, and indexers.
3.  **Advanced Concepts**: Introduces static features, partial classes, and resource management.
4.  **Modern C#**: Concludes with primary constructors and other contemporary patterns.

## Key Takeaways

- **Classes are the blueprints** for creating objects in C#.
- **Encapsulation** is a core principle where data (fields) is protected and accessed through methods and properties.
- **Static members belong to the type**, not to individual instances, and are ideal for shared data or utility functions.
- **Modern C# features** like primary constructors and object initializers help write cleaner, more concise code.
- **Proper resource management** using finalizers or `IDisposable` is essential for building robust applications.



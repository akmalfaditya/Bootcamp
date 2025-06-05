# Inheritance in C#

## Learning Objectives

Master the "is-a" relationship in object-oriented programming! Inheritance allows you to build class hierarchies, share common functionality, and create specialized versions of existing classes while maintaining polymorphic behavior.

## What You'll Learn

### Core Concepts Covered:

1. **Inheritance Fundamentals**
   - Base classes and derived classes
   - The "is-a" relationship principle
   - Single inheritance limitation in C#
   - Object as the universal base class

2. **Polymorphism and Virtual Members**
   - **Virtual methods**: Overrideable behavior in base classes
   - **Override**: Replacing base class behavior
   - **Abstract classes**: Classes that cannot be instantiated
   - **Abstract methods**: Methods that must be implemented

3. **Member Access and Hiding**
   - **`protected` access**: Visible to derived classes
   - **`new` keyword**: Hiding base class members
   - **`base` keyword**: Accessing base class functionality
   - **Method resolution**: How C# chooses which method to call

4. **Casting and Type Conversion**
   - **Upcasting**: Derived to base (implicit)
   - **Downcasting**: Base to derived (explicit)
   - **`is` operator**: Type checking
   - **`as` operator**: Safe casting with null return

5. **Constructor Inheritance**
   - Constructor chaining with `base()`
   - Initialization order in inheritance hierarchies
   - Default constructor behavior
   - Best practices for constructor design

## Key Features Demonstrated

### Basic Inheritance:
```csharp
// Base class defines common functionality
public class Animal
{
    public string Name { get; set; }
    public virtual void MakeSound() => Console.WriteLine("Some generic animal sound");
    public virtual void Move() => Console.WriteLine("Moving...");
}

// Derived class specializes the base class
public class Dog : Animal
{
    public override void MakeSound() => Console.WriteLine("Woof!");
    public override void Move() => Console.WriteLine("Running on four legs");
    
    // Dog-specific behavior
    public void Fetch() => Console.WriteLine("Fetching the ball!");
}
```

### Abstract Classes:
```csharp
// Abstract class - cannot be instantiated directly
public abstract class Shape
{
    public abstract double Area { get; }  // Must be implemented
    public abstract double Perimeter { get; } // Must be implemented
    
    // Concrete method - shared by all shapes
    public virtual void Display() => Console.WriteLine($"Area: {Area}, Perimeter: {Perimeter}");
}

public class Circle : Shape
{
    public double Radius { get; set; }
    
    public override double Area => Math.PI * Radius * Radius;
    public override double Perimeter => 2 * Math.PI * Radius;
}
```

### Polymorphism in Action:
```csharp
// Polymorphic behavior - same interface, different implementations
Animal[] animals = 
{
    new Dog { Name = "Buddy" },
    new Cat { Name = "Whiskers" },
    new Bird { Name = "Tweety" }
};

foreach (Animal animal in animals)
{
    animal.MakeSound(); // Calls the appropriate override
}
```

### Safe Casting:
```csharp
Animal animal = GetSomeAnimal();

// Type checking
if (animal is Dog dog)
{
    dog.Fetch(); // Safe to call Dog-specific methods
}

// Safe casting
Dog? possibleDog = animal as Dog;
if (possibleDog != null)
{
    possibleDog.Fetch();
}
```

## Tips

> **"Is-a" vs "Has-a"**: Use inheritance for "is-a" relationships (Dog is-a Animal) and composition for "has-a" relationships (Car has-a Engine). Don't force inheritance where it doesn't naturally fit!

> **Virtual by Design**: If you might want to override a method in the future, make it virtual from the start. Adding virtuality later is a breaking change for existing overrides.

> **Abstract vs Interface**: Use abstract classes when you want to share implementation and have an "is-a" relationship. Use interfaces for "can-do" relationships and when you need multiple inheritance.

## What to Focus On

1. **Proper inheritance hierarchies**: Design logical "is-a" relationships
2. **Virtual/override pairs**: Enabling polymorphic behavior
3. **Constructor chaining**: Proper initialization in hierarchies
4. **Safe casting**: Avoiding runtime type errors

## Run the Project

```bash
dotnet run
```

The demo showcases:
- Basic inheritance and specialization
- Polymorphism with virtual/override
- Abstract classes and methods
- Type casting and safety
- Constructor inheritance patterns
- Member hiding vs overriding

## Best Practices

1. **Favor composition over inheritance** when the relationship isn't clearly "is-a"
2. **Make methods virtual** if you expect them to be overridden
3. **Use abstract classes** for shared implementation with required overrides
4. **Call base constructors** explicitly when needed
5. **Design inheritance hierarchies carefully** - they're hard to change later
6. **Use sealed classes** when inheritance isn't intended

## Real-World Applications

### Common Inheritance Patterns:
```csharp
// Exception hierarchy
public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}

// Entity framework
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class User : BaseEntity
{
    public string Username { get; set; }
    public string Email { get; set; }
}

// UI Controls
public abstract class Control
{
    public virtual void Render() { /* base rendering */ }
    public abstract void HandleClick();
}

public class Button : Control
{
    public override void HandleClick() => OnClick?.Invoke();
    public event Action OnClick;
}
```

## When to Use Inheritance

**Good for:**
- Clear "is-a" relationships
- Sharing common implementation
- Building framework hierarchies
- Specializing existing behavior

**Avoid when:**
- Relationship is "has-a" or "can-do"
- You need multiple inheritance
- Base class is likely to change frequently
- Composition would be simpler

## Advanced Inheritance Concepts

### Sealed Classes and Methods:
```csharp
public sealed class StringHelper // Cannot be inherited
{
    public static string Capitalize(string input) => /* implementation */;
}

public class BaseClass
{
    public virtual void Method() { }
}

public class MiddleClass : BaseClass
{
    public sealed override void Method() { } // Cannot be overridden further
}
```

### Constructor Chaining:
```csharp
public class Vehicle
{
    public Vehicle(string make, string model)
    {
        Make = make;
        Model = model;
    }
}

public class Car : Vehicle
{
    public Car(string make, string model, int doors) : base(make, model)
    {
        Doors = doors;
    }
}
```

## Design Patterns Using Inheritance

- **Template Method**: Abstract class defines algorithm structure
- **Factory Method**: Abstract creator with concrete implementations
- **Strategy (with inheritance)**: Family of algorithms with common base
- **Command**: Abstract command with concrete implementations
- **Composite**: Tree structures with common component interface

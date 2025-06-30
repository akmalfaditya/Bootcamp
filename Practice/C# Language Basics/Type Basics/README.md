# Type Basics - Understanding C#'s Type System

Welcome to the Type Basics project! This comprehensive demonstration covers the fundamental concepts of C#'s type system. As a developer, understanding types is crucial because every piece of data in C# has a specific type that determines how it behaves and what operations you can perform on it.

## What This Project Demonstrates

This project provides hands-on examples of:

### 1. Predefined Types
- **int type**: 32-bit signed integers with arithmetic operations
- **string type**: Immutable character sequences with string manipulation
- **bool type**: Boolean logic with true/false values
- Real-world examples showing how these types work in practice

### 2. Custom Types
- **Classes**: Reference types that serve as blueprints for objects
- **Structs**: Value types for lightweight data structures
- Constructor usage and object instantiation
- The UnitConverter example showing practical custom type usage

### 3. Value Types vs Reference Types
- **Value types**: Store data directly (structs, primitives)
- **Reference types**: Store references to data (classes, strings)
- Memory behavior differences with clear examples
- Assignment behavior comparison

### 4. Type Conversions
- **Implicit conversions**: Automatic safe conversions (int to long)
- **Explicit conversions**: Manual casting when data might be lost
- Practical examples of when each type is needed

### 5. Instance vs Static Members
- **Instance members**: Operate on specific object instances
- **Static members**: Belong to the type itself, not instances
- The Panda class example showing population tracking

### 6. Null References
- Understanding null values with reference types
- NullReferenceException prevention
- Safe null checking practices

### 7. Storage and Memory
- Memory overhead differences between value and reference types
- Understanding the cost of different type choices

### 8. Real-World Banking Example
- Complete BankAccount system demonstrating all concepts
- Transaction records using structs
- Practical application of type system knowledge

## Running the Project

Navigate to the project directory and run:

```powershell
dotnet run
```

The program will execute various demonstrations, each clearly labeled and explained through console output.

## Code Structure

The project is organized into logical sections:

1. **Predefined Types Demo** - Shows int, string, and bool usage
2. **Custom Types Demo** - UnitConverter class example
3. **Value vs Reference Demo** - Point struct vs PointClass comparison
4. **Conversion Examples** - Safe and unsafe type conversions
5. **Static vs Instance Demo** - Panda class with population tracking
6. **Null Handling Demo** - Safe null reference practices
7. **Banking System Demo** - Complete real-world application

## Key Learning Points

### Type Safety
C# is a strongly-typed language, meaning the compiler catches type-related errors at compile time rather than runtime. This prevents many common programming errors.

### Performance Considerations
- Value types are generally faster for small data structures
- Reference types provide flexibility but have memory overhead
- Choose the right type for your specific use case

### Best Practices Demonstrated
- Using meaningful type names and constructor parameters
- Proper null checking to prevent runtime errors
- Organizing related functionality within appropriate types
- Leveraging both static and instance members effectively

### Memory Management
Understanding how different types use memory helps you write more efficient code:
- Value types: Stack allocation, direct storage
- Reference types: Heap allocation, reference storage

## Questions to Consider

As you study this code, ask yourself:
1. When would you choose a struct over a class?
2. How does the assignment behavior differ between value and reference types?
3. Why are strings immutable, and how does this affect performance?
4. When would you use static members vs instance members?
5. How can proper type design prevent runtime errors?

## Next Steps

After understanding these basics, you'll be ready to explore:
- Inheritance and polymorphism
- Interfaces and abstract classes
- Generic types and constraints
- Advanced null handling with nullable reference types

This foundation in type basics is essential for all advanced C# programming concepts!


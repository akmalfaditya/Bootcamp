## What You'll Find Here

This project isn't just theory - it's a working example that shows you exactly how these principles work in real code.

## Project Structure

```
├── BadExamples/           # The "don't do this" examples
├── GoodExamples/          # The "do this instead" examples
├── Models/                # Our domain objects
├── Interfaces/            # Contracts that define behavior
└── Program.cs            # Where we run everything
```

## SOLID Principles Demonstrated

### 1. Single Responsibility Principle (SRP)
- **Bad**: `UserService` that handles registration, email validation, AND sending emails
- **Good**: Separate `UserService`, `EmailService`, and `ValidationService`

### 2. Open/Closed Principle (OCP)
- **Bad**: `AreaCalculator` that needs modification for each new shape
- **Good**: Abstract `Shape` class that allows extension without modification

### 3. Liskov Substitution Principle (LSP)
- **Bad**: `Penguin` inheriting from `Bird` but throwing exceptions for `Fly()`
- **Good**: Proper abstractions with `IFlyable` interface

### 4. Interface Segregation Principle (ISP)
- **Bad**: Fat `ILead` interface forcing unnecessary methods
- **Good**: Focused `ITaskManager` and `IWorker` interfaces

### 5. Dependency Inversion Principle (DIP)
- **Bad**: Direct dependencies on concrete classes
- **Good**: Dependency injection with interfaces

## KISS Principle Examples

- Simple calculation methods vs over-engineered solutions
- Clean dependency injection vs tight coupling
- Meaningful naming and focused responsibilities

## How to Run

```bash
dotnet run
```

The program will show you side-by-side comparisons of bad vs good implementations. Each example includes detailed comments explaining what's happening and why.

## Additional Resources

- **[LEARNING_PATH.md](LEARNING_PATH.md)** - Step-by-step guide to mastering these principles
- **[QUICK_REFERENCE.md](QUICK_REFERENCE.md)** - Cheat sheet for all principles with quick tips
- **[EXERCISES.md](EXERCISES.md)** - Practice exercises to test your understanding

## Key Takeaways

1. **SOLID principles make code maintainable** - easier to change, test, and extend
2. **KISS keeps things understandable** - simple solutions are often the best solutions
3. **These aren't just rules** - they're tools that make your life as a developer easier



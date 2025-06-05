## Quick Reference Guide

### SOLID Principles Cheat Sheet

#### Single Responsibility Principle (SRP)
**"A class should have only one reason to change"**
- Bad: `UserService` handles registration, email, validation, and database
- Good: Separate `UserService`, `EmailService`, `ValidationService`, `UserRepository`

#### Open/Closed Principle (OCP)
**"Open for extension, closed for modification"**
- Bad: Modify `AreaCalculator` every time you add a new shape
- Good: Use abstract `Shape` class, add new shapes without changing calculator

#### Liskov Substitution Principle (LSP)
**"Subclasses should be substitutable for their base classes"**
- Bad: `Penguin` inherits from `Bird` but throws exception on `Fly()`
- Good: Use `IFlyable` interface only for birds that can actually fly

#### Interface Segregation Principle (ISP)
**"Don't force classes to implement methods they don't use"**
- Bad: Fat `ILead` interface with `WorkOnTask()` that managers can't use
- Good: Separate `ITaskManager` and `IWorker` interfaces

#### 🔗 Dependency Inversion Principle (DIP)
**"Depend on abstractions, not concrete classes"**
- Bad: `OrderService` creates `FileLogger` and `EmailSender` directly
- Good: Inject `ILogger` and `IEmailService` interfaces

### KISS Principle Guidelines

#### Keep It Simple Rules
1. **Solve today's problem** - Don't over-engineer for imaginary future needs
2. **One method, one purpose** - Each method should do one thing well
3. **Meaningful names** - Code should read like well-written prose
4. **Avoid deep nesting** - Flat is better than nested
5. **Prefer composition** - Combine simple pieces rather than complex inheritance

#### Warning Signs of Complexity
- Methods with more than 5 parameters
- Classes with more than 10 methods
- Deeply nested if-else chains
- Comments explaining "why" instead of "what"
- Hard to write unit tests

### When to Apply These Principles

#### Start with KISS
- Begin every solution with the simplest approach
- Only add complexity when absolutely necessary
- Refactor when you feel pain, not before

#### Apply SOLID When
- Your codebase is growing beyond 1000 lines
- Multiple developers are working on the same code
- You're building a system that will evolve over time
- Testing becomes difficult due to dependencies

### Common Mistakes to Avoid

1. **Over-abstraction** - Creating interfaces for everything "just in case"
2. **Premature optimization** - Solving performance problems that don't exist
3. **Gold plating** - Adding features nobody asked for
4. **Copy-paste programming** - Duplicating code instead of extracting common functionality
5. **God objects** - Classes that know too much or do too much

### Remember
> "Simplicity is the ultimate sophistication" - Leonardo da Vinci

> "Make it work, make it right, make it fast" - Kent Beck

> "Any fool can write code that a computer can understand. Good programmers write code that humans can understand" - Martin Fowler

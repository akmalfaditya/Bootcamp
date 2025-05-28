
// Demonstrating PascalCase for class names
public class Program
{
    // Demonstrating PascalCase for a public field (though properties are more common)
    public static string ClassPurpose = "To demonstrate C# syntax elements.";

    // Demonstrating a private field with camelCase
    private int _internalCounter;

    // XML Documentation Comment
    /// <summary>
    /// This is the main entry point for the application.
    /// It demonstrates various C# syntax elements.
    /// </summary>
    /// <param name="args">Command-line arguments (not used in this demo).</param>
    public static void Main()
    {
        // 1. IDENTIFIERS AND KEYWORDS
        //---------------------------------------------------------------------

        // **Identifiers**
        // 'args' is an identifier (parameter name)
        // 'SyntaxDemonstrator' is an identifier (class name)
        // 'Main' is an identifier (method name)
        // 'localVariable' is an identifier (variable name)
        // 'Console' is an identifier (class name from System namespace)
        // 'WriteLine' is an identifier (method name from Console class)

        // **Naming Conventions**
        int localVariable = 10; // camelCase for local variable
        Console.WriteLine("--- Identifiers and Keywords ---");
        Console.WriteLine($"Local variable (camelCase): {localVariable}");
        Console.WriteLine($"Class purpose (PascalCase): {ClassPurpose}");

        // **Keywords**
        // 'public', 'static', 'void', 'string', 'int', 'using', 'class' are keywords.
        // 'int' is a keyword representing the integer type.
        int myAge = 30; // 'int' is a keyword
        bool isLearning = true; // 'bool' is a keyword
        const double PI = 3.14159; // 'const' is a keyword

        Console.WriteLine($"Keyword 'int' used for myAge: {myAge}");
        Console.WriteLine($"Keyword 'bool' used for isLearning: {isLearning}");
        Console.WriteLine($"Keyword 'const' used for PI: {PI}");

        // Using a keyword as an identifier with @
        string @namespace = "MyProject.Utilities"; // 'namespace' is a keyword, @namespace is an identifier
        int @int = 123; // 'int' is a keyword, @int is an identifier
        Console.WriteLine($"Using '@' to use keyword as identifier: {@namespace}, {@int}");


        // **Contextual Keywords**
        // 'var' is a contextual keyword for type inference.
        var message = "Hello from var!"; // 'var' infers the type as string
        var count = 5 * 2;               // 'var' infers the type as int

        // 'yield' could be demonstrated in an iterator method (more advanced).
        // 'async' and 'await' would be used for asynchronous programming (more advanced).
        Console.WriteLine($"Contextual keyword 'var' for message: {message}");
        Console.WriteLine($"Contextual keyword 'var' for count: {count}");

        Console.WriteLine(); // For spacing

        // 2. LITERALS, PUNCTUATORS, AND OPERATORS
        //---------------------------------------------------------------------
        Console.WriteLine("--- Literals, Punctuators, and Operators ---");

        // **Literals**
        int integerLiteral = 42;
        double floatingPointLiteral = 3.14159;
        string stringLiteral = "This is a string literal.";
        bool booleanLiteral = false;
        char charLiteral = 'C'; // char literal

        Console.WriteLine($"Integer literal: {integerLiteral}");
        Console.WriteLine($"Floating-point literal: {floatingPointLiteral}");
        Console.WriteLine($"String literal: {stringLiteral}");
        Console.WriteLine($"Boolean literal: {booleanLiteral}");
        Console.WriteLine($"Character literal: {charLiteral}");

        // **Punctuators**
        // Semicolon (;): Terminates statements (used throughout this code).
        // Parentheses (()): Used for method declarations (e.g., Main(string[] args)) and calls (e.g., Console.WriteLine()).
        // Period (.): Accesses members (e.g., Console.WriteLine, anObject.aMethod).
        // Curly Braces {}: Define code blocks (e.g., for classes, methods, if statements).

        MyClass demoObject = new MyClass(); // Instantiating an object
        demoObject.DisplayValue(integerLiteral); // Using '.' and '()'

        // **Operators**
        // Arithmetic Operators:
        int sum = 10 + 5;         // + (addition)
        int difference = 10 - 5;  // - (subtraction)
        int product = 10 * 5;     // * (multiplication)
        int quotient = 10 / 5;    // / (division)
        int remainder = 10 % 3;   // % (modulus)

        Console.WriteLine($"Sum (10 + 5): {sum}");
        Console.WriteLine($"Product (10 * 5): {product}");

        // Assignment Operator: =
        int assignedValue = 100;
        Console.WriteLine($"Assigned value: {assignedValue}");
        assignedValue += 50; // Compound assignment (equivalent to assignedValue = assignedValue + 50)
        Console.WriteLine($"After compound assignment (+= 50): {assignedValue}");


        // Comparison Operators (result in a boolean):
        bool isEqual = (sum == 15);    // == (equal to)
        bool isNotEqual = (sum != 10); // != (not equal to)
        bool isGreater = (sum > 10);   // > (greater than)
        Console.WriteLine($"Is sum == 15? {isEqual}");

        // Logical Operators:
        bool logicalAnd = (isLearning && isEqual); // && (logical AND)
        bool logicalOr = (isLearning || booleanLiteral); // || (logical OR)
        bool logicalNot = !booleanLiteral;           // ! (logical NOT)
        Console.WriteLine($"Logical AND (isLearning && isEqual): {logicalAnd}");


        // Invocation Operator: ()
        // Used to call methods, e.g., Console.WriteLine(), PerformCalculation()
        int calculationResult = PerformCalculation(5, 3);
        Console.WriteLine($"Result of PerformCalculation(5, 3): {calculationResult}");

        Console.WriteLine(); // For spacing

        // 3. COMMENTS
        //---------------------------------------------------------------------
        Console.WriteLine("--- Comments ---");

        // This is a single-line comment.
        // It explains that the next line will print a message.
        Console.WriteLine("This line is preceded by a single-line comment.");

        /*
           This is a
           multi-line comment.
           It can span across several lines and is often used
           for more detailed explanations or for temporarily disabling code blocks.
           int temporarilyDisabledCode = 0;
        */
        Console.WriteLine("The above section contains a multi-line comment example in the source code.");

        // XML Documentation Comments are shown above the Main method and the AddNumbers method.
        // They are used to generate documentation.
        int sumFromDocumentedMethod = AddNumbers(10, 20);
        Console.WriteLine($"Sum from XML documented method AddNumbers(10, 20): {sumFromDocumentedMethod}");

        Console.WriteLine("\n--- Demo Complete ---");
    }

    // Method demonstrating PascalCase for method name
    // and camelCase for parameters
    public static int PerformCalculation(int operandOne, int operandTwo)
    {
        // Using arithmetic operator '*'
        return operandOne * operandTwo;
    }

    /// <summary>
    /// This method calculates the sum of two integers.
    /// It serves as an example for XML documentation comments.
    /// </summary>
    /// <param name="numberOne">The first integer.</param>
    /// <param name="numberTwo">The second integer.</param>
    /// <returns>The sum of numberOne and numberTwo.</returns>
    public static int AddNumbers(int numberOne, int numberTwo)
    {
        return numberOne + numberTwo; // Using arithmetic operator '+'
    }
}

// Another class to demonstrate member access punctuator '.'
// PascalCase for class name
public class MyClass
{
    // PascalCase for public property
    public int Value { get; set; }

    // PascalCase for public method
    public void DisplayValue(int valueToDisplay)
    {
        this.Value = valueToDisplay; // 'this' is a keyword
        // Using '.' to access 'WriteLine' method of 'Console' class
        // Using '()' for method invocation
        Console.WriteLine($"Value displayed from MyClass: {this.Value}");
    }
}


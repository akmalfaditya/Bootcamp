namespace ExpressionsAndOperatorsDemo
{
    // Sample class to demonstrate member access operators
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        
        public Product(string name, decimal price, int stock)
        {
            Name = name;
            Price = price;
            Stock = stock;
        }
        
        public decimal CalculateTotal(int quantity)
        {
            return Price * quantity;
        }
        
        public bool IsInStock()
        {
            return Stock > 0;
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== EXPRESSIONS AND OPERATORS IN C# ===");
            Console.WriteLine("This is where you learn to build complex logic from simple building blocks\n");
            
            // Section 1: Constants and Variables - The Foundation
            DemonstrateConstantsAndVariables();
            
            // Section 2: Binary Operators - Working with Two Values
            DemonstrateBinaryOperators();
            
            // Section 3: Nested Expressions - Building Complex Logic
            DemonstrateNestedExpressions();
            
            // Section 4: Unary Operators - Single Value Operations
            DemonstrateUnaryOperators();
            
            // Section 5: Void Expressions - Operations Without Return Values
            DemonstrateVoidExpressions();
            
            // Section 6: Assignment Expressions - Storing and Chaining Values
            DemonstrateAssignmentExpressions();
            
            // Section 7: Operator Precedence and Associativity
            DemonstrateOperatorPrecedence();
            
            // Section 8: Complete Operator Categories
            DemonstrateOperatorCategories();
            
            // Section 9: Real-World Practical Examples
            DemonstrateRealWorldScenarios();
            
            Console.WriteLine("\n=== MASTERY ACHIEVED ===");
            Console.WriteLine("You now understand how expressions and operators work together");
            Console.WriteLine("Use this knowledge to write clear, efficient, and maintainable code!");
        }
        
        #region Constants and Variables
        
        static void DemonstrateConstantsAndVariables()
        {
            Console.WriteLine("=== CONSTANTS AND VARIABLES ===");
            Console.WriteLine("These are the simplest expressions - the atoms of your code\n");
            
            // Constants - values that never change
            Console.WriteLine("--- Constants (Fixed Values) ---");
            const int MaxUsers = 1000;        // Integer constant
            const double Pi = 3.14159;        // Double constant
            const string AppName = "MyApp";   // String constant
            const bool IsProduction = false; // Boolean constant
            
            Console.WriteLine($"Integer constant: {MaxUsers}");
            Console.WriteLine($"Double constant: {Pi}");
            Console.WriteLine($"String constant: {AppName}");
            Console.WriteLine($"Boolean constant: {IsProduction}");
            
            // Variables - values that can change during execution
            Console.WriteLine("\n--- Variables (Changeable Values) ---");
            int currentUsers = 250;           // Start with 250 users
            double radius = 5.0;              // Circle radius
            string userName = "Alice";        // Current user
            bool isLoggedIn = true;           // Login status
            
            Console.WriteLine($"Current users: {currentUsers}");
            Console.WriteLine($"Circle radius: {radius}");
            Console.WriteLine($"Username: {userName}");
            Console.WriteLine($"Logged in: {isLoggedIn}");
            
            // Variables can be changed - that's the point!
            currentUsers = 275;
            userName = "Bob";
            isLoggedIn = false;
            
            Console.WriteLine("\nAfter changes:");
            Console.WriteLine($"Current users: {currentUsers}");
            Console.WriteLine($"Username: {userName}");
            Console.WriteLine($"Logged in: {isLoggedIn}");
            
            Console.WriteLine("\nKey point: Constants provide stability, variables provide flexibility\n");
        }
        
        #endregion
        
        #region Binary Operators
        
        static void DemonstrateBinaryOperators()
        {
            Console.WriteLine("=== BINARY OPERATORS ===");
            Console.WriteLine("Two operands, one operator - the workhorses of programming\n");
            
            // Arithmetic binary operators
            Console.WriteLine("--- Arithmetic Operators ---");
            int a = 12, b = 5;
            
            Console.WriteLine($"Starting values: a = {a}, b = {b}");
            Console.WriteLine($"Addition: {a} + {b} = {a + b}");
            Console.WriteLine($"Subtraction: {a} - {b} = {a - b}");
            Console.WriteLine($"Multiplication: {a} * {b} = {a * b}");
            Console.WriteLine($"Division: {a} / {b} = {a / b}");
            Console.WriteLine($"Remainder: {a} % {b} = {a % b}");
            
            // String concatenation - binary operator for strings
            Console.WriteLine("\n--- String Concatenation ---");
            string firstName = "John";
            string lastName = "Doe";
            string fullName = firstName + " " + lastName; // Binary + operator for strings
            
            Console.WriteLine($"First name: '{firstName}'");
            Console.WriteLine($"Last name: '{lastName}'");
            Console.WriteLine($"Full name: '{fullName}'");
            
            // Comparison operators
            Console.WriteLine("\n--- Comparison Operators ---");
            int x = 10, y = 20;
            
            Console.WriteLine($"Values: x = {x}, y = {y}");
            Console.WriteLine($"x == y: {x == y}");  // Equal to
            Console.WriteLine($"x != y: {x != y}");  // Not equal to
            Console.WriteLine($"x < y: {x < y}");    // Less than
            Console.WriteLine($"x > y: {x > y}");    // Greater than
            Console.WriteLine($"x <= y: {x <= y}");  // Less than or equal
            Console.WriteLine($"x >= y: {x >= y}");  // Greater than or equal
            
            // Logical operators
            Console.WriteLine("\n--- Logical Operators ---");
            bool isWeekend = true;
            bool hasFreetime = false;
            
            Console.WriteLine($"Is weekend: {isWeekend}");
            Console.WriteLine($"Has free time: {hasFreetime}");
            Console.WriteLine($"Can relax (AND): {isWeekend && hasFreetime}");
            Console.WriteLine($"Can enjoy (OR): {isWeekend || hasFreetime}");
            
            Console.WriteLine("\nRemember: Binary operators need exactly two operands to work\n");
        }
        
        #endregion
        
        #region Nested Expressions
        
        static void DemonstrateNestedExpressions()
        {
            Console.WriteLine("=== NESTED EXPRESSIONS ===");
            Console.WriteLine("Building complex logic by combining simpler expressions\n");
            
            Console.WriteLine("--- Basic Nesting with Parentheses ---");
            // Simple nested expression from the material
            int result1 = 1 + (12 * 30);
            Console.WriteLine($"1 + (12 * 30) = {result1}");
            Console.WriteLine("The parentheses force multiplication to happen first");
            
            // More complex nesting
            Console.WriteLine("\n--- Complex Nested Expressions ---");
            int a = 5, b = 10, c = 2;
            
            int result2 = (a + b) * c;
            Console.WriteLine($"({a} + {b}) * {c} = {result2}");
            
            int result3 = a + (b * c);
            Console.WriteLine($"{a} + ({b} * {c}) = {result3}");
            
            // Deeply nested expression
            int result4 = ((a + b) * c) + (a * (b - c));
            Console.WriteLine($"(({a} + {b}) * {c}) + ({a} * ({b} - {c})) = {result4}");
            
            // Real-world example: calculating compound interest
            Console.WriteLine("\n--- Real-World Example: Compound Interest ---");
            double principal = 1000.0;  // Initial amount
            double rate = 0.05;         // 5% annual rate
            int years = 3;              // 3 years
            int compoundsPerYear = 12;  // Monthly compounding
            
            // A = P(1 + r/n)^(nt)
            double amount = principal * Math.Pow((1 + (rate / compoundsPerYear)), (compoundsPerYear * years));
            
            Console.WriteLine($"Principal: ${principal:F2}");
            Console.WriteLine($"Rate: {rate * 100}% annually");
            Console.WriteLine($"Time: {years} years, compounded {compoundsPerYear} times per year");
            Console.WriteLine($"Final amount: ${amount:F2}");
            Console.WriteLine($"Interest earned: ${amount - principal:F2}");
            
            // Boolean logic nesting
            Console.WriteLine("\n--- Nested Boolean Logic ---");
            int age = 25;
            int income = 45000;
            bool hasGoodCredit = true;
            bool isEmployed = true;
            
            // Complex eligibility check
            bool isEligibleForLoan = (age >= 18 && age <= 65) && 
                                   (income >= 30000) && 
                                   (hasGoodCredit || (isEmployed && income >= 40000));
            
            Console.WriteLine($"Age: {age}");
            Console.WriteLine($"Income: ${income}");
            Console.WriteLine($"Good credit: {hasGoodCredit}");
            Console.WriteLine($"Employed: {isEmployed}");
            Console.WriteLine($"Loan eligible: {isEligibleForLoan}");
            
            Console.WriteLine("\nKey lesson: Use parentheses to make your intentions crystal clear\n");
        }
        
        #endregion
        
        #region Unary Operators
        
        static void DemonstrateUnaryOperators()
        {
            Console.WriteLine("=== UNARY OPERATORS ===");
            Console.WriteLine("One operand, powerful effects - the precision tools of programming\n");
            
            // Increment and decrement operators
            Console.WriteLine("--- Increment and Decrement Operators ---");
            int counter = 5;
            
            Console.WriteLine($"Starting counter: {counter}");
            Console.WriteLine($"counter++: {counter++} (post-increment, shows {counter - 1}, then increments)");
            Console.WriteLine($"Current counter: {counter}");
            Console.WriteLine($"++counter: {++counter} (pre-increment, increments first, then shows {counter})");
            Console.WriteLine($"Current counter: {counter}");
            
            Console.WriteLine($"counter--: {counter--} (post-decrement, shows {counter + 1}, then decrements)");
            Console.WriteLine($"Current counter: {counter}");
            Console.WriteLine($"--counter: {--counter} (pre-decrement, decrements first, then shows {counter})");
            Console.WriteLine($"Final counter: {counter}");
            
            // Unary plus and minus
            Console.WriteLine("\n--- Unary Plus and Minus ---");
            int positive = 42;
            int negative = -positive;  // Unary minus
            int stillPositive = +positive;  // Unary plus (rarely used)
            
            Console.WriteLine($"Original: {positive}");
            Console.WriteLine($"Negated: {negative}");
            Console.WriteLine($"Explicitly positive: {stillPositive}");
            
            // Logical NOT operator
            Console.WriteLine("\n--- Logical NOT Operator ---");
            bool isActive = true;
            bool isInactive = !isActive;  // Logical NOT
            
            Console.WriteLine($"Is active: {isActive}");
            Console.WriteLine($"Is inactive: {isInactive}");
            
            // Practical example with user status
            bool isLoggedIn = false;
            bool needsLogin = !isLoggedIn;
            
            Console.WriteLine($"User logged in: {isLoggedIn}");
            Console.WriteLine($"Needs to login: {needsLogin}");
            
            // Bitwise NOT operator (complement)
            Console.WriteLine("\n--- Bitwise NOT Operator ---");
            byte value = 5;  // Binary: 00000101
            byte complement = (byte)~value;  // Binary: 11111010
            
            Console.WriteLine($"Original value: {value} (binary: {Convert.ToString(value, 2).PadLeft(8, '0')})");
            Console.WriteLine($"Bitwise NOT: {complement} (binary: {Convert.ToString(complement, 2).PadLeft(8, '0')})");
            
            // Type casting operator
            Console.WriteLine("\n--- Type Casting (Conversion) Operators ---");
            double preciseValue = 123.789;
            int roundedDown = (int)preciseValue;  // Explicit cast
            
            Console.WriteLine($"Original double: {preciseValue}");
            Console.WriteLine($"Cast to int: {roundedDown} (truncated, not rounded)");
            
            // typeof operator
            Console.WriteLine("\n--- typeof Operator ---");
            Type stringType = typeof(string);
            Type intType = typeof(int);
            
            Console.WriteLine($"Type of string: {stringType}");
            Console.WriteLine($"Type of int: {intType}");
            
            Console.WriteLine("\nUnary operators are compact but powerful - master them for cleaner code\n");
        }
        
        #endregion
        
        #region Void Expressions
        
        static void DemonstrateVoidExpressions()
        {
            Console.WriteLine("=== VOID EXPRESSIONS ===");
            Console.WriteLine("Operations that do work but don't return values\n");
            
            Console.WriteLine("--- Understanding Void Methods ---");
            Console.WriteLine("Methods like Console.WriteLine() perform actions but return nothing");
            
            // This is a void expression - it does something but returns no value
            Console.WriteLine("This line itself is a void expression!");
            
            // Demonstrating that void expressions can't be used as operands
            Console.WriteLine("\n--- Why Void Expressions Can't Be Operands ---");
            Console.WriteLine("The following would cause compile errors:");
            Console.WriteLine("// int x = 1 + Console.WriteLine(\"Hello\"); // ERROR!");
            Console.WriteLine("// string result = \"Result: \" + PrintMessage(); // ERROR!");
            
            // Valid uses of void expressions
            Console.WriteLine("\n--- Valid Uses of Void Expressions ---");
            PrintWelcomeMessage();  // Called as a statement
            
            int count = 5;
            ProcessData(count);     // Called as a statement
            
            // Void expressions in control structures
            if (DateTime.Now.Hour < 12)
            {
                PrintMorningGreeting();  // Void expression in if block
            }
            else
            {
                PrintAfternoonGreeting(); // Void expression in else block
            }
            
            // Void expressions in loops
            Console.WriteLine("\n--- Void Expressions in Loops ---");
            for (int i = 1; i <= 3; i++)
            {
                PrintCountdown(i);  // Void expression in loop
            }
            
            Console.WriteLine("\nKey point: Void expressions do important work, they just don't give you a value back\n");
        }
        
        // Helper methods for void expression demonstration
        static void PrintWelcomeMessage()
        {
            Console.WriteLine("Welcome to our application!");
        }
        
        static void ProcessData(int count)
        {
            Console.WriteLine($"Processing {count} items...");
        }
        
        static void PrintMorningGreeting()
        {
            Console.WriteLine("Good morning!");
        }
        
        static void PrintAfternoonGreeting()
        {
            Console.WriteLine("Good afternoon!");
        }
        
        static void PrintCountdown(int number)
        {
            Console.WriteLine($"  Countdown: {number}");
        }
        
        #endregion
        
        #region Assignment Expressions
        
        static void DemonstrateAssignmentExpressions()
        {
            Console.WriteLine("=== ASSIGNMENT EXPRESSIONS ===");
            Console.WriteLine("Storing values and building chains - the backbone of state management\n");
            
            // Basic assignment
            Console.WriteLine("--- Basic Assignment ---");
            int x = 10;  // Assignment expression that evaluates to 10
            Console.WriteLine($"x = 10 assigns and evaluates to: {x}");
            
            // Assignment as part of larger expressions
            Console.WriteLine("\n--- Assignment Within Expressions ---");
            int y = 5 * (x = 7);  // First x gets 7, then 5 * 7 = 35 goes to y
            Console.WriteLine($"After y = 5 * (x = 7):");
            Console.WriteLine($"x = {x}");
            Console.WriteLine($"y = {y}");
            
            // Chained assignments
            Console.WriteLine("\n--- Chained Assignments ---");
            int a, b, c, d;
            a = b = c = d = 100;  // All variables get 100
            
            Console.WriteLine("After a = b = c = d = 100:");
            Console.WriteLine($"a = {a}, b = {b}, c = {c}, d = {d}");
            
            // Compound assignment operators
            Console.WriteLine("\n--- Compound Assignment Operators ---");
            int value = 20;
            Console.WriteLine($"Starting value: {value}");
            
            value += 10;  // Equivalent to: value = value + 10
            Console.WriteLine($"After value += 10: {value}");
            
            value -= 5;   // Equivalent to: value = value - 5
            Console.WriteLine($"After value -= 5: {value}");
            
            value *= 2;   // Equivalent to: value = value * 2
            Console.WriteLine($"After value *= 2: {value}");
            
            value /= 3;   // Equivalent to: value = value / 3
            Console.WriteLine($"After value /= 3: {value}");
            
            value %= 7;   // Equivalent to: value = value % 7
            Console.WriteLine($"After value %= 7: {value}");
            
            // String compound assignment
            Console.WriteLine("\n--- String Compound Assignment ---");
            string message = "Hello";
            Console.WriteLine($"Starting message: '{message}'");
            
            message += " World";
            Console.WriteLine($"After message += \" World\": '{message}'");
            
            message += "!";
            Console.WriteLine($"After message += \"!\": '{message}'");
            
            // Real-world example: score tracking
            Console.WriteLine("\n--- Real-World Example: Game Score Tracking ---");
            int playerScore = 0;
            int round = 1;
            
            Console.WriteLine($"Game starts - Round {round}, Score: {playerScore}");
            
            playerScore += 150;  // Player scores points
            Console.WriteLine($"Round {round++} complete - Score: {playerScore}");
            
            playerScore += 200;  // Another round
            Console.WriteLine($"Round {round++} complete - Score: {playerScore}");
            
            playerScore -= 50;   // Penalty
            Console.WriteLine($"Penalty applied - Score: {playerScore}");
            
            playerScore *= 2;    // Bonus multiplier
            Console.WriteLine($"Bonus multiplier applied - Final Score: {playerScore}");
            
            Console.WriteLine("\nAssignment expressions are your primary tool for managing program state\n");
        }
        
        #endregion
        
        #region Operator Precedence and Associativity
        
        static void DemonstrateOperatorPrecedence()
        {
            Console.WriteLine("=== OPERATOR PRECEDENCE AND ASSOCIATIVITY ===");
            Console.WriteLine("Understanding the order of operations - crucial for predictable code\n");
            
            // Precedence demonstration
            Console.WriteLine("--- Operator Precedence ---");
            int result1 = 1 + 2 * 3;  // Multiplication has higher precedence
            Console.WriteLine($"1 + 2 * 3 = {result1} (multiplication first: 1 + 6)");
            
            int result2 = (1 + 2) * 3;  // Parentheses override precedence
            Console.WriteLine($"(1 + 2) * 3 = {result2} (parentheses first: 3 * 3)");
            
            // Complex precedence example
            Console.WriteLine("\n--- Complex Precedence Example ---");
            int a = 2, b = 3, c = 4, d = 5;
            int result3 = a + b * c - d / 2;
            Console.WriteLine($"Given: a={a}, b={b}, c={c}, d={d}");
            Console.WriteLine($"a + b * c - d / 2 = {result3}");
            Console.WriteLine("Order: (b*c) first, then (d/2), then left-to-right: a + 12 - 2");
            
            // Associativity demonstration
            Console.WriteLine("\n--- Left Associativity ---");
            int result4 = 20 / 4 / 2;  // Left-to-right: (20 / 4) / 2
            Console.WriteLine($"20 / 4 / 2 = {result4} (left-to-right: 5 / 2)");
            
            int result5 = 15 - 5 - 3;  // Left-to-right: (15 - 5) - 3
            Console.WriteLine($"15 - 5 - 3 = {result5} (left-to-right: 10 - 3)");
            
            // Right associativity with assignment
            Console.WriteLine("\n--- Right Associativity (Assignment) ---");
            int x, y, z;
            x = y = z = 42;  // Right-to-left: x = (y = (z = 42))
            Console.WriteLine($"After x = y = z = 42:");
            Console.WriteLine($"x = {x}, y = {y}, z = {z}");
            
            // Logical operator precedence
            Console.WriteLine("\n--- Logical Operator Precedence ---");
            bool result6 = true || false && false;  // && has higher precedence than ||
            Console.WriteLine($"true || false && false = {result6}");
            Console.WriteLine("Order: (false && false) first = false, then true || false = true");
            
            bool result7 = (true || false) && false;  // Parentheses change the order
            Console.WriteLine($"(true || false) && false = {result7}");
            Console.WriteLine("Order: (true || false) first = true, then true && false = false");
            
            // Real-world precedence trap
            Console.WriteLine("\n--- Common Precedence Trap ---");
            int hours = 8;
            int rate = 25;
            int overtime = 2;
            int overtimeRate = 37;
            
            // Wrong way (without parentheses)
            int wrongPay = hours * rate + overtime * overtimeRate;
            Console.WriteLine($"hours * rate + overtime * overtimeRate = {wrongPay}");
            
            // Correct way (with parentheses for clarity)
            int correctPay = (hours * rate) + (overtime * overtimeRate);
            Console.WriteLine($"(hours * rate) + (overtime * overtimeRate) = {correctPay}");
            Console.WriteLine("Same result, but parentheses make the intention clear");
            
            Console.WriteLine("\nWhen in doubt, use parentheses - clarity beats cleverness!\n");
        }
        
        #endregion
        
        #region Operator Categories
        
        static void DemonstrateOperatorCategories()
        {
            Console.WriteLine("=== COMPLETE OPERATOR CATEGORIES ===");
            Console.WriteLine("From highest to lowest precedence - your reference guide\n");
            
            // Primary operators (highest precedence)
            Console.WriteLine("--- Primary Operators (Highest Precedence) ---");
            Product laptop = new Product("Gaming Laptop", 1299.99m, 5);
            
            // Member access operator (.)
            Console.WriteLine($"Product name: {laptop.Name}");
            Console.WriteLine($"Product price: ${laptop.Price}");
            
            // Method call operator (())
            bool inStock = laptop.IsInStock();
            Console.WriteLine($"In stock: {inStock}");
            
            decimal total = laptop.CalculateTotal(2);
            Console.WriteLine($"Total for 2 units: ${total}");
            
            // Array/indexer access operator ([])
            string[] colors = { "Red", "Green", "Blue" };
            Console.WriteLine($"First color: {colors[0]}");
            
            // typeof operator
            Type productType = typeof(Product);
            Console.WriteLine($"Product type: {productType.Name}");
            
            // nameof operator (C# 6.0+)
            string propertyName = nameof(laptop.Price);
            Console.WriteLine($"Property name: {propertyName}");
            
            // Unary operators
            Console.WriteLine("\n--- Unary Operators ---");
            int number = 10;
            Console.WriteLine($"Original: {number}");
            Console.WriteLine($"Negated: {-number}");
            Console.WriteLine($"Logical NOT of true: {!true}");
            Console.WriteLine($"Pre-increment: {++number}");
            Console.WriteLine($"Current value: {number}");
            
            // Multiplicative operators
            Console.WriteLine("\n--- Multiplicative Operators ---");
            int a = 15, b = 4;
            Console.WriteLine($"{a} * {b} = {a * b}");
            Console.WriteLine($"{a} / {b} = {a / b}");
            Console.WriteLine($"{a} % {b} = {a % b}");
            
            // Additive operators
            Console.WriteLine("\n--- Additive Operators ---");
            Console.WriteLine($"{a} + {b} = {a + b}");
            Console.WriteLine($"{a} - {b} = {a - b}");
            
            // Shift operators
            Console.WriteLine("\n--- Shift Operators ---");
            int value = 8;  // Binary: 1000
            Console.WriteLine($"{value} << 2 = {value << 2} (shift left)");
            Console.WriteLine($"{value} >> 1 = {value >> 1} (shift right)");
            
            // Relational operators
            Console.WriteLine("\n--- Relational Operators ---");
            int x = 10, y = 20;
            Console.WriteLine($"{x} < {y}: {x < y}");
            Console.WriteLine($"{x} <= {y}: {x <= y}");
            Console.WriteLine($"{x} > {y}: {x > y}");
            Console.WriteLine($"{x} >= {y}: {x >= y}");
            
            // Equality operators
            Console.WriteLine("\n--- Equality Operators ---");
            Console.WriteLine($"{x} == {y}: {x == y}");
            Console.WriteLine($"{x} != {y}: {x != y}");
            
            // Logical operators
            Console.WriteLine("\n--- Logical Operators ---");
            bool condition1 = true, condition2 = false;
            Console.WriteLine($"{condition1} && {condition2}: {condition1 && condition2}");
            Console.WriteLine($"{condition1} || {condition2}: {condition1 || condition2}");
            
            // Ternary conditional operator
            Console.WriteLine("\n--- Ternary Conditional Operator ---");
            int age = 17;
            string status = age >= 18 ? "Adult" : "Minor";
            Console.WriteLine($"Age {age} is classified as: {status}");
            
            // Assignment operators (lowest precedence)
            Console.WriteLine("\n--- Assignment Operators (Lowest Precedence) ---");
            int result = 0;
            result += 10;
            result *= 2;
            result -= 5;
            Console.WriteLine($"Final result after compound assignments: {result}");
            
            Console.WriteLine("\nMaster these categories and you'll write more predictable code\n");
        }
        
        #endregion
        
        #region Real-World Scenarios
        
        static void DemonstrateRealWorldScenarios()
        {
            Console.WriteLine("=== REAL-WORLD PRACTICAL SCENARIOS ===");
            Console.WriteLine("How you'll actually use these concepts in production code\n");
            
            // Scenario 1: E-commerce price calculation
            Console.WriteLine("--- Scenario 1: E-commerce Price Calculator ---");
            decimal itemPrice = 89.99m;
            int quantity = 3;
            decimal discountRate = 0.15m;  // 15% discount
            decimal taxRate = 0.08m;       // 8% tax
            
            // Complex expression with proper precedence
            decimal subtotal = itemPrice * quantity;
            decimal discount = subtotal * discountRate;
            decimal afterDiscount = subtotal - discount;
            decimal tax = afterDiscount * taxRate;
            decimal finalTotal = afterDiscount + tax;
            
            Console.WriteLine($"Item price: ${itemPrice}");
            Console.WriteLine($"Quantity: {quantity}");
            Console.WriteLine($"Subtotal: ${subtotal}");
            Console.WriteLine($"Discount ({discountRate * 100}%): -${discount:F2}");
            Console.WriteLine($"After discount: ${afterDiscount:F2}");
            Console.WriteLine($"Tax ({taxRate * 100}%): +${tax:F2}");
            Console.WriteLine($"Final total: ${finalTotal:F2}");
            
            // Scenario 2: User authentication system
            Console.WriteLine("\n--- Scenario 2: User Authentication Logic ---");
            string inputUsername = "admin";
            string inputPassword = "secure123";
            bool accountLocked = false;
            int failedAttempts = 0;
            int maxAttempts = 3;
            
            // Complex boolean expression
            bool isValidCredentials = (inputUsername == "admin" && inputPassword == "secure123");
            bool canAttemptLogin = !accountLocked && failedAttempts < maxAttempts;
            bool loginSuccessful = isValidCredentials && canAttemptLogin;
            
            Console.WriteLine($"Username: {inputUsername}");
            Console.WriteLine($"Valid credentials: {isValidCredentials}");
            Console.WriteLine($"Account locked: {accountLocked}");
            Console.WriteLine($"Failed attempts: {failedAttempts}/{maxAttempts}");
            Console.WriteLine($"Can attempt login: {canAttemptLogin}");
            Console.WriteLine($"Login successful: {loginSuccessful}");
            
            // Update state based on login result
            if (!loginSuccessful && isValidCredentials == false)
            {
                failedAttempts++;
                Console.WriteLine($"Failed attempts updated to: {failedAttempts}");
            }
            
            // Scenario 3: Data validation pipeline
            Console.WriteLine("\n--- Scenario 3: Data Validation Pipeline ---");
            string userInput = "  John@Example.COM  ";
            
            // Chain of operations with method calls and operators
            string processedInput = userInput.Trim().ToLower();
            bool hasValidFormat = processedInput.Contains("@") && processedInput.Contains(".");
            bool hasValidLength = processedInput.Length >= 5 && processedInput.Length <= 50;
            bool startsWithLetter = char.IsLetter(processedInput[0]);
            
            // Complex validation expression
            bool isValidEmail = hasValidFormat && hasValidLength && startsWithLetter && 
                               !processedInput.StartsWith("@") && !processedInput.EndsWith("@");
            
            Console.WriteLine($"Original input: '{userInput}'");
            Console.WriteLine($"Processed input: '{processedInput}'");
            Console.WriteLine($"Has valid format: {hasValidFormat}");
            Console.WriteLine($"Has valid length: {hasValidLength}");
            Console.WriteLine($"Starts with letter: {startsWithLetter}");
            Console.WriteLine($"Is valid email: {isValidEmail}");
            
            // Scenario 4: Game scoring system
            Console.WriteLine("\n--- Scenario 4: Game Scoring System ---");
            int baseScore = 1000;
            int timeBonus = 250;
            int accuracyPercentage = 85;
            int difficultyMultiplier = 2;
            bool perfectRound = accuracyPercentage == 100;
            
            // Complex scoring calculation with ternary operators
            int accuracyBonus = (accuracyPercentage >= 90) ? 500 : 
                               (accuracyPercentage >= 80) ? 300 : 
                               (accuracyPercentage >= 70) ? 100 : 0;
            
            int perfectBonus = perfectRound ? 1000 : 0;
            
            int totalScore = (baseScore + timeBonus + accuracyBonus + perfectBonus) * difficultyMultiplier;
            
            Console.WriteLine($"Base score: {baseScore}");
            Console.WriteLine($"Time bonus: {timeBonus}");
            Console.WriteLine($"Accuracy: {accuracyPercentage}%");
            Console.WriteLine($"Accuracy bonus: {accuracyBonus}");
            Console.WriteLine($"Perfect round: {perfectRound}");
            Console.WriteLine($"Perfect bonus: {perfectBonus}");
            Console.WriteLine($"Difficulty multiplier: x{difficultyMultiplier}");
            Console.WriteLine($"Total score: {totalScore}");
            
            // Scenario 5: Resource management
            Console.WriteLine("\n--- Scenario 5: System Resource Management ---");
            double cpuUsage = 75.5;
            double memoryUsage = 82.3;
            double diskUsage = 45.7;
            int activeConnections = 150;
            int maxConnections = 200;
            
            // Complex resource evaluation
            bool cpuCritical = cpuUsage > 90;
            bool memoryCritical = memoryUsage > 85;
            bool diskCritical = diskUsage > 95;
            bool connectionsCritical = activeConnections > (maxConnections * 0.9);
            
            bool systemHealthy = !cpuCritical && !memoryCritical && !diskCritical && !connectionsCritical;
            string alertLevel = (cpuCritical || memoryCritical) ? "CRITICAL" :
                               (cpuUsage > 80 || memoryUsage > 75) ? "WARNING" : "NORMAL";
            
            Console.WriteLine($"CPU Usage: {cpuUsage}%");
            Console.WriteLine($"Memory Usage: {memoryUsage}%");
            Console.WriteLine($"Disk Usage: {diskUsage}%");
            Console.WriteLine($"Connections: {activeConnections}/{maxConnections}");
            Console.WriteLine($"System Healthy: {systemHealthy}");
            Console.WriteLine($"Alert Level: {alertLevel}");
            
            Console.WriteLine("\nThese scenarios show how operators and expressions solve real business problems");
        }
        
        #endregion
    }
}

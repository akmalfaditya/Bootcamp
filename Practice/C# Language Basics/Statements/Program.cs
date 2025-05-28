#nullable disable // Disable nullable warnings for this educational demo

using System.Text;

namespace StatementsDemo
{
    // Supporting classes for demonstration purposes
    public class Player
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public bool IsActive { get; set; }
        
        public Player(string name)
        {
            Name = name;
            Score = 0;
            IsActive = true;
        }
        
        public void AddScore(int points)
        {
            Score += points;
        }
        
        public void DisplayStatus()
        {
            Console.WriteLine($"Player: {Name}, Score: {Score}, Active: {IsActive}");
        }
    }
    
    public class GameSession
    {
        public List<Player> Players { get; set; }
        public int Round { get; set; }
        
        public GameSession()
        {
            Players = new List<Player>();
            Round = 1;
        }
        
        public void AddPlayer(string name)
        {
            Players.Add(new Player(name));
        }
        
        public void DisplayLeaderboard()
        {
            Console.WriteLine($"\n--- Round {Round} Leaderboard ---");
            foreach (var player in Players.OrderByDescending(p => p.Score))
            {
                player.DisplayStatus();
            }
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# STATEMENTS MASTERCLASS ===");
            Console.WriteLine("Building blocks of program control flow\n");
            
            // Section 1: Declaration Statements
            DemonstrateDeclarationStatements();
            
            // Section 2: Expression Statements  
            DemonstrateExpressionStatements();
            
            // Section 3: Selection Statements
            DemonstrateSelectionStatements();
            
            // Section 4: Iteration Statements
            DemonstrateIterationStatements();
            
            // Section 5: Jump Statements
            DemonstrateJumpStatements();
            
            // Section 6: Miscellaneous Statements
            DemonstrateMiscellaneousStatements();
            
            // Section 7: Real-World Application
            DemonstrateRealWorldApplication();
            
            Console.WriteLine("\n=== STATEMENTS MASTERY COMPLETE ===");
            Console.WriteLine("You now understand the building blocks of C# program flow!");
        }
        
        #region Declaration Statements
        
        static void DemonstrateDeclarationStatements()
        {
            Console.WriteLine("=== DECLARATION STATEMENTS ===");
            Console.WriteLine("Creating variables and constants - the foundation of data storage\n");
            
            // Basic variable declarations with initialization
            Console.WriteLine("--- Single Variable Declarations ---");
            string someWord = "rosebud";  // String variable with initial value
            int someNumber = 42;          // Integer variable with initial value
            double pi = 3.14159;          // Double precision floating point
            bool isLearning = true;       // Boolean variable
            
            Console.WriteLine($"someWord: '{someWord}'");
            Console.WriteLine($"someNumber: {someNumber}");
            Console.WriteLine($"pi: {pi}");
            Console.WriteLine($"isLearning: {isLearning}");
            
            // Multiple variable declarations of the same type
            Console.WriteLine("\n--- Multiple Variable Declarations ---");
            bool rich = true, famous = false;  // Two booleans in one statement
            int x = 10, y = 20, z = 30;       // Three integers in one statement
            
            Console.WriteLine($"rich: {rich}, famous: {famous}");
            Console.WriteLine($"x: {x}, y: {y}, z: {z}");
            
            // Constant declarations - values that never change
            Console.WriteLine("\n--- Constant Declarations ---");
            const double c = 2.99792458E08;  // Speed of light in m/s
            const string appName = "Statement Demo";
            const int maxRetries = 3;
            
            Console.WriteLine($"Speed of light: {c:E2} m/s");
            Console.WriteLine($"Application name: {appName}");
            Console.WriteLine($"Maximum retries allowed: {maxRetries}");
            
            // Demonstrating that constants cannot be modified
            Console.WriteLine("\n--- Constant Immutability ---");
            Console.WriteLine("Constants cannot be changed after declaration");
            Console.WriteLine("Uncommenting the next line would cause a compile error:");
            Console.WriteLine("// c += 10;  // ERROR: Cannot modify a constant value");
            
            // Variable scope demonstration
            Console.WriteLine("\n--- Variable Scope ---");
            int outerVariable = 100;
            Console.WriteLine($"Outer variable: {outerVariable}");
            
            {
                // This is a nested block - creates new scope
                int innerVariable = 200;
                Console.WriteLine($"Inner variable: {innerVariable}");
                Console.WriteLine($"Outer variable is still accessible: {outerVariable}");
                
                // This would cause an error if uncommented:
                // int outerVariable = 300;  // ERROR: Already defined in outer scope
            }
            
            // innerVariable is no longer accessible here
            Console.WriteLine("Inner variable is no longer accessible outside its block");
            Console.WriteLine("This demonstrates block-level scoping in C#\n");
        }
        
        #endregion
        
        #region Expression Statements
        
        static void DemonstrateExpressionStatements()
        {
            Console.WriteLine("=== EXPRESSION STATEMENTS ===");
            Console.WriteLine("Statements that perform actions and change program state\n");
            
            // Assignment expressions
            Console.WriteLine("--- Assignment Expressions ---");
            int x = 1 + 2;  // Expression evaluates to 3, assigned to x
            Console.WriteLine($"x = 1 + 2 → x = {x}");
            
            x++;  // Increment expression - x becomes 4
            Console.WriteLine($"After x++: x = {x}");
            
            x += 5;  // Compound assignment - x becomes 9
            Console.WriteLine($"After x += 5: x = {x}");
            
            // Method call expressions
            Console.WriteLine("\n--- Method Call Expressions ---");
            Console.WriteLine("Calling Console.WriteLine is an expression statement");
            
            // String manipulation method calls
            string greeting = "Hello World";
            string upperGreeting = greeting.ToUpper();  // Method call that returns a value
            Console.WriteLine($"Original: {greeting}");
            Console.WriteLine($"Uppercase: {upperGreeting}");
            
            // Object instantiation expressions
            Console.WriteLine("\n--- Object Instantiation Expressions ---");
            StringBuilder sb = new StringBuilder();  // Create object and assign to variable
            Console.WriteLine("StringBuilder created and assigned to variable 'sb'");
            
            sb.Append("Building a string");  // Method call on the object
            sb.Append(" step by step");
            Console.WriteLine($"StringBuilder content: {sb}");
            
            // Object instantiation without assignment (legal but often useless)
            Console.WriteLine("\n--- Unused Object Creation ---");
            new StringBuilder();  // Legal but the object is immediately discarded
            Console.WriteLine("Created a StringBuilder but didn't assign it - perfectly legal but wasteful");
            
            // Chained method calls
            Console.WriteLine("\n--- Chained Method Calls ---");
            string result = new StringBuilder()
                .Append("Method")
                .Append(" chaining")
                .Append(" is powerful")
                .ToString();
            Console.WriteLine($"Chained result: {result}");
            
            // Complex expressions
            Console.WriteLine("\n--- Complex Expressions ---");
            int a = 5, b = 10, c = 15;
            int complexResult = (a + b) * c / 2;  // Complex arithmetic expression
            Console.WriteLine($"(a + b) * c / 2 = ({a} + {b}) * {c} / 2 = {complexResult}");
            
            Console.WriteLine("\nRemember: Expression statements perform actions that change your program's state\n");
        }
        
        #endregion
        
        #region Selection Statements
        
        static void DemonstrateSelectionStatements()
        {
            Console.WriteLine("=== SELECTION STATEMENTS ===");
            Console.WriteLine("Making decisions in your code - the brain of program logic\n");
            
            // If statement demonstrations
            Console.WriteLine("--- Basic If Statements ---");
            if (5 < 2 * 3)
            {
                Console.WriteLine("True: 5 is less than 6");  // This executes
            }
              if (2 + 2 == 5)
                Console.WriteLine("Does not compute");
            else
                Console.WriteLine("Math still works: 2 + 2 ≠ 5");  // This executes
            
            // If-else if chains
            Console.WriteLine("\n--- If-Else If Chains ---");
            int score = 85;
            if (score >= 90)
                Console.WriteLine("Grade: A");
            else if (score >= 80)
                Console.WriteLine($"Grade: B (Score: {score})");  // This executes
            else if (score >= 70)
                Console.WriteLine("Grade: C");
            else if (score >= 60)
                Console.WriteLine("Grade: D");
            else
                Console.WriteLine("Grade: F");
            
            // Nested if statements
            Console.WriteLine("\n--- Nested If Statements ---");
            bool hasLicense = true;
            int age = 25;
            
            if (hasLicense)
            {
                if (age >= 18)
                {
                    Console.WriteLine("Can drive a car");
                    if (age >= 25)
                    {
                        Console.WriteLine("Eligible for lower insurance rates");
                    }
                }
                else
                {
                    Console.WriteLine("Too young to drive");
                }
            }
            else
            {
                Console.WriteLine("Cannot drive without a license");
            }
            
            // Switch statement demonstrations
            Console.WriteLine("\n--- Switch Statements ---");
            DemonstrateCardSwitch(13);  // King
            DemonstrateCardSwitch(12);  // Queen
            DemonstrateCardSwitch(11);  // Jack
            DemonstrateCardSwitch(7);   // Regular number
            DemonstrateCardSwitch(-1);  // Joker (demonstrates goto)
            
            // Switch with multiple cases
            Console.WriteLine("\n--- Switch with Stacked Cases ---");
            for (int cardNumber = 10; cardNumber <= 13; cardNumber++)
            {
                DemonstrateCardType(cardNumber);
            }
            
            // Modern switch expressions (if using C# 8.0+)
            Console.WriteLine("\n--- Modern Switch Expressions ---");
            string[] days = { "Monday", "Tuesday", "Wednesday", "Saturday", "Sunday" };
            foreach (string day in days)
            {
                string dayType = GetDayType(day);
                Console.WriteLine($"{day} is a {dayType}");
            }
            
            Console.WriteLine("\nSelection statements let you create intelligent, responsive programs\n");
        }
        
        static void DemonstrateCardSwitch(int cardNumber)
        {
            Console.Write($"Card {cardNumber}: ");
            switch (cardNumber)
            {
                case 13:
                    Console.WriteLine("King");
                    break;
                case 12:
                    Console.WriteLine("Queen");
                    break;
                case 11:
                    Console.WriteLine("Jack");
                    break;
                case -1:
                    Console.WriteLine("Joker (treated as Queen)");
                    goto case 12;  // Demonstrates goto in switch
                default:
                    Console.WriteLine($"Number card: {cardNumber}");
                    break;
            }
        }
        
        static void DemonstrateCardType(int cardNumber)
        {
            Console.Write($"Card {cardNumber}: ");
            switch (cardNumber)
            {
                case 13:
                case 12:
                case 11:
                    Console.WriteLine("Face card");
                    break;
                default:
                    Console.WriteLine("Plain card");
                    break;
            }
        }
        
        static string GetDayType(string day)
        {
            return day switch
            {
                "Monday" or "Tuesday" or "Wednesday" or "Thursday" or "Friday" => "weekday",
                "Saturday" or "Sunday" => "weekend",
                _ => "unknown day"
            };
        }
        
        #endregion
        
        #region Iteration Statements
        
        static void DemonstrateIterationStatements()
        {
            Console.WriteLine("=== ITERATION STATEMENTS ===");
            Console.WriteLine("Repeating code efficiently - the workhorse of programming\n");
            
            // While loop demonstration
            Console.WriteLine("--- While Loops ---");
            Console.WriteLine("Counting from 0 to 2 with while loop:");
            int i = 0;
            while (i < 3)
            {
                Console.Write(i + " ");  // Prints: 0 1 2
                i++;
            }
            Console.WriteLine();
            
            // Do-while loop demonstration
            Console.WriteLine("\n--- Do-While Loops ---");
            Console.WriteLine("Do-while guarantees at least one execution:");
            i = 0;
            do
            {
                Console.WriteLine($"Iteration {i}");  // Prints 0, 1, 2
                i++;
            } while (i < 3);
            
            // Demonstrate do-while executes at least once even with false condition
            Console.WriteLine("Do-while with false condition still executes once:");
            bool condition = false;
            do
            {
                Console.WriteLine("This executes even though condition is false");
            } while (condition);
            
            // For loop demonstration
            Console.WriteLine("\n--- For Loops ---");
            Console.WriteLine("Classic for loop - most common iteration pattern:");
            for (int j = 0; j < 3; j++)
            {
                Console.WriteLine($"For loop iteration: {j}");
            }
            
            // For loop with different increment
            Console.WriteLine("For loop counting by 2:");
            for (int k = 0; k < 10; k += 2)
            {
                Console.Write(k + " ");  // Prints: 0 2 4 6 8
            }
            Console.WriteLine();
            
            // For loop counting backwards
            Console.WriteLine("Countdown with for loop:");
            for (int countdown = 5; countdown > 0; countdown--)
            {
                Console.Write(countdown + " ");  // Prints: 5 4 3 2 1
            }
            Console.WriteLine("Blast off!");
            
            // Infinite loop example (commented out for safety)
            Console.WriteLine("\n--- Infinite Loops ---");
            Console.WriteLine("Infinite loop structure (not executed):");
            Console.WriteLine("for (;;) { Console.WriteLine(\"Interrupt me\"); }");
            
            // Foreach loop demonstration
            Console.WriteLine("\n--- Foreach Loops ---");
            Console.WriteLine("Iterating through a string:");
            foreach (char c in "beer")
            {
                Console.WriteLine($"Character: {c}");
            }
            
            // Foreach with arrays
            Console.WriteLine("Iterating through an array:");
            string[] fruits = { "apple", "banana", "cherry", "date" };
            foreach (string fruit in fruits)
            {
                Console.WriteLine($"Fruit: {fruit}");
            }
            
            // Foreach with lists
            Console.WriteLine("Iterating through a list with index:");
            List<string> colors = new List<string> { "red", "green", "blue", "yellow" };
            int index = 0;
            foreach (string color in colors)
            {
                Console.WriteLine($"Color #{index}: {color}");
                index++;
            }
            
            // Nested loops
            Console.WriteLine("\n--- Nested Loops ---");
            Console.WriteLine("Creating a multiplication table:");
            for (int row = 1; row <= 3; row++)
            {
                for (int col = 1; col <= 3; col++)
                {
                    Console.Write($"{row * col,3} ");
                }
                Console.WriteLine();
            }
            
            Console.WriteLine("\nIteration statements are essential for processing collections and repetitive tasks\n");
        }
        
        #endregion
        
        #region Jump Statements
        
        static void DemonstrateJumpStatements()
        {
            Console.WriteLine("=== JUMP STATEMENTS ===");
            Console.WriteLine("Controlling program flow with precision jumps\n");
            
            // Break statement demonstration
            Console.WriteLine("--- Break Statements ---");
            Console.WriteLine("Break exits loops early:");
            int x = 0;
            while (true)  // Infinite loop
            {
                Console.WriteLine($"x = {x}");
                if (x++ > 3)
                    break;  // Exits the loop when x exceeds 3
            }
            Console.WriteLine("Loop exited with break");
            
            // Break in for loop
            Console.WriteLine("\nBreak in for loop - finding first even number:");
            int[] numbers = { 1, 3, 7, 8, 9, 12, 15 };
            for (int i = 0; i < numbers.Length; i++)
            {
                Console.WriteLine($"Checking: {numbers[i]}");
                if (numbers[i] % 2 == 0)
                {
                    Console.WriteLine($"Found first even number: {numbers[i]}");
                    break;  // Stop searching after finding first even number
                }
            }
            
            // Continue statement demonstration
            Console.WriteLine("\n--- Continue Statements ---");
            Console.WriteLine("Continue skips to next iteration:");
            for (int i = 0; i < 10; i++)
            {
                if ((i % 2) == 0)
                    continue;  // Skip even numbers
                Console.Write(i + " ");  // Only prints odd numbers
            }
            Console.WriteLine("(only odd numbers printed)");
            
            // Continue with while loop
            Console.WriteLine("\nContinue in while loop - processing valid inputs only:");
            string[] inputs = { "123", "abc", "456", "def", "789" };
            int inputIndex = 0;
            while (inputIndex < inputs.Length)
            {
                string current = inputs[inputIndex++];
                if (!int.TryParse(current, out int value))
                {
                    Console.WriteLine($"Skipping invalid input: {current}");
                    continue;  // Skip non-numeric inputs
                }
                Console.WriteLine($"Processing valid number: {value}");
            }
            
            // Return statement demonstration
            Console.WriteLine("\n--- Return Statements ---");
            int result1 = AddNumbers(5, 3);
            int result2 = AddNumbers(-1, 10);  // This will return early
            Console.WriteLine($"AddNumbers(5, 3) = {result1}");
            Console.WriteLine($"AddNumbers(-1, 10) = {result2}");
            
            // Goto statement demonstration (use sparingly!)
            Console.WriteLine("\n--- Goto Statements ---");
            Console.WriteLine("Goto statements (use with caution):");
            
            int attempts = 0;
            
            tryAgain:
            attempts++;
            Console.WriteLine($"Attempt #{attempts}");
            
            if (attempts < 3)
            {
                Console.WriteLine("Going to try again...");
                goto tryAgain;  // Jump back to label
            }
            
            Console.WriteLine("Finished with goto demonstration");
            
            // Demonstrating goto with switch (already shown in switch section)
            Console.WriteLine("\nGoto is also useful in switch statements for case fall-through");
            
            Console.WriteLine("\nJump statements give you precise control over program flow\n");
        }
        
        static int AddNumbers(int a, int b)
        {
            if (a < 0 || b < 0)
            {
                Console.WriteLine("  Negative numbers not allowed");
                return -1;  // Early return with error code
            }
            
            return a + b;  // Normal return with result
        }
        
        #endregion
        
        #region Miscellaneous Statements
        
        static void DemonstrateMiscellaneousStatements()
        {
            Console.WriteLine("=== MISCELLANEOUS STATEMENTS ===");
            Console.WriteLine("Special statements for resource management and thread safety\n");
            
            // Using statement demonstration
            Console.WriteLine("--- Using Statements ---");
            Console.WriteLine("Using statements ensure proper resource cleanup:");
            
            // Traditional using statement
            using (var writer = new StringWriter())
            {
                writer.WriteLine("This text is written to StringWriter");
                writer.WriteLine("The StringWriter will be automatically disposed");
                Console.WriteLine("Content written: " + writer.ToString());
            }  // StringWriter.Dispose() is called automatically here
            
            Console.WriteLine("StringWriter has been automatically disposed");
            
            // Using declaration (C# 8.0+)
            Console.WriteLine("\nUsing declaration (simpler syntax):");            {
                using var reader = new StringReader("Sample text for reading");
                string line = reader.ReadLine();
                Console.WriteLine($"Read from StringReader: {line ?? "null"}");
            }  // StringReader.Dispose() called automatically at end of scope
            
            // Multiple using statements
            Console.WriteLine("\nMultiple resources with using:");
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine("Writing to memory stream");
                writer.Flush();
                Console.WriteLine($"Bytes written to stream: {stream.Length}");
            }  // Both StreamWriter and MemoryStream disposed automatically
            
            // Lock statement demonstration
            Console.WriteLine("\n--- Lock Statements ---");
            Console.WriteLine("Lock statements ensure thread-safe access:");
            
            var sharedResource = new SharedCounter();
            
            // Simulate thread-safe operations
            Console.WriteLine("Demonstrating thread-safe counter operations:");
            
            // In a real application, these would be called from different threads
            for (int i = 0; i < 5; i++)
            {
                sharedResource.Increment();
                Console.WriteLine($"Counter value: {sharedResource.Value}");
            }
            
            // Try-catch-finally (bonus - exception handling)
            Console.WriteLine("\n--- Exception Handling Statements ---");
            Console.WriteLine("Try-catch-finally for robust error handling:");
            
            try
            {
                Console.WriteLine("Attempting risky operation...");
                int riskyCalculation = 10 / 2;  // Safe operation
                Console.WriteLine($"Result: {riskyCalculation}");
                
                // Uncomment to see exception handling:
                // int error = 10 / 0;  // Would throw DivideByZeroException
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"Caught exception: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Caught general exception: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Finally block always executes");
            }
            
            Console.WriteLine("\nThese statements help you write more robust and safe applications\n");
        }
        
        // Helper class for lock demonstration
        public class SharedCounter
        {
            private int _value = 0;
            private readonly object _lock = new object();
            
            public int Value
            {
                get
                {
                    lock (_lock)
                    {
                        return _value;
                    }
                }
            }
            
            public void Increment()
            {
                lock (_lock)  // Ensures thread-safe access
                {
                    _value++;
                    Thread.Sleep(1);  // Simulate some work
                }
            }
        }
        
        #endregion
        
        #region Real-World Application
        
        static void DemonstrateRealWorldApplication()
        {
            Console.WriteLine("=== REAL-WORLD APPLICATION ===");
            Console.WriteLine("Putting it all together: A simple game management system\n");
            
            // Declaration statements - setting up game data
            const int maxPlayers = 4;
            const int winningScore = 100;
            var gameSession = new GameSession();
            
            // Expression statements - initializing the game
            gameSession.AddPlayer("Alice");
            gameSession.AddPlayer("Bob");
            gameSession.AddPlayer("Charlie");
            
            Console.WriteLine($"Game initialized with {gameSession.Players.Count} players");
            Console.WriteLine($"Playing to {winningScore} points, maximum {maxPlayers} players");
            
            // Iteration statements - game loop
            bool gameWon = false;
            while (!gameWon && gameSession.Round <= 10)  // Selection + iteration
            {
                Console.WriteLine($"\n--- Round {gameSession.Round} ---");
                
                // Foreach loop to process each player
                foreach (var player in gameSession.Players)
                {
                    // Selection statements for different player actions
                    if (!player.IsActive)
                    {
                        continue;  // Jump statement - skip inactive players
                    }
                    
                    // Expression statement - generate random score
                    Random rnd = new Random();
                    int roundScore = rnd.Next(1, 21);  // 1-20 points
                    
                    // Selection statement for bonus points
                    if (roundScore >= 18)
                    {
                        Console.WriteLine($"{player.Name} rolled {roundScore} - BONUS ROUND!");
                        roundScore *= 2;  // Double the score
                    }
                    else if (roundScore <= 3)
                    {
                        Console.WriteLine($"{player.Name} rolled {roundScore} - bad luck!");
                        player.IsActive = false;  // Player eliminated
                        continue;  // Jump to next player
                    }
                    
                    // Expression statement - update score
                    player.AddScore(roundScore);
                    Console.WriteLine($"{player.Name} scored {roundScore} points (Total: {player.Score})");
                    
                    // Selection statement - check for winner
                    if (player.Score >= winningScore)
                    {
                        Console.WriteLine($"\n🎉 {player.Name} WINS with {player.Score} points! 🎉");
                        gameWon = true;
                        break;  // Jump statement - exit player loop
                    }
                }
                
                // Display current standings
                gameSession.DisplayLeaderboard();
                
                // Check if all players eliminated
                if (gameSession.Players.All(p => !p.IsActive))
                {
                    Console.WriteLine("\nAll players eliminated! Game over.");
                    break;  // Jump statement - exit game loop
                }
                
                gameSession.Round++;
            }
            
            // Final results using selection statements
            if (!gameWon)
            {
                var leader = gameSession.Players.Where(p => p.IsActive).OrderByDescending(p => p.Score).FirstOrDefault();
                if (leader != null)
                {
                    Console.WriteLine($"\nGame ended after 10 rounds. Leader: {leader.Name} with {leader.Score} points");
                }
                else
                {
                    Console.WriteLine("\nNo players remained active!");
                }
            }
            
            // Using statement for cleanup
            using (var logWriter = new StringWriter())
            {
                logWriter.WriteLine("=== GAME SUMMARY ===");
                foreach (var player in gameSession.Players)
                {
                    logWriter.WriteLine($"{player.Name}: {player.Score} points, Active: {player.IsActive}");
                }
                Console.WriteLine("\nGame log:");
                Console.WriteLine(logWriter.ToString());
            }
            
            Console.WriteLine("This example demonstrates how all statement types work together in real applications!");
        }
        
        #endregion
    }
}

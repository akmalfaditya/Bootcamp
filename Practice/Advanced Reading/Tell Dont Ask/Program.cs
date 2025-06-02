/*
 * TELL, DON'T ASK - Interactive Demonstration
 * 
 * This program demonstrates the "Tell, Don't Ask" principle through practical examples.
 * The principle states that objects should encapsulate their behavior and external code
 * should tell them what to do, rather than asking for their state and making decisions.
 * 
 * Created by: [Your Name]
 * Purpose: Training demonstration of object-oriented design principles
 */

using Tell_Dont_Ask.Models;
using Tell_Dont_Ask.BadExamples;
using Tell_Dont_Ask.GoodExamples;
using Tell_Dont_Ask.Scenarios;

namespace Tell_Dont_Ask
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("🎯 TELL, DON'T ASK PRINCIPLE DEMONSTRATION");
            Console.WriteLine("===========================================");
            Console.WriteLine();
            
            // Show the main menu
            ShowMainMenu();
        }

        static void ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("📋 MAIN MENU - Choose a demonstration:");
                Console.WriteLine("1. 📊 Quick Side-by-Side Comparison");
                Console.WriteLine("2. 🔍 Detailed Code Examples");
                Console.WriteLine("3. 🌎 Real-World Scenarios");
                Console.WriteLine("4. 🧪 Interactive Playground");
                Console.WriteLine("5. 📚 Principle Explanation");
                Console.WriteLine("0. ❌ Exit");
                Console.WriteLine();
                Console.Write("Enter your choice (0-5): ");

                string choice = Console.ReadLine() ?? "";
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        ShowQuickComparison();
                        break;
                    case "2":
                        ShowDetailedExamples();
                        break;
                    case "3":
                        ScenarioRunner.RunAllScenarios();
                        break;
                    case "4":
                        ShowInteractivePlayground();
                        break;
                    case "5":
                        ShowPrincipleExplanation();
                        break;
                    case "0":
                        Console.WriteLine("👋 Thanks for exploring Tell, Don't Ask!");
                        return;
                    default:
                        Console.WriteLine("❌ Invalid choice. Please select 0-5.");
                        break;
                }

                Console.WriteLine("\nPress any key to return to main menu...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void ShowQuickComparison()
        {
            Console.WriteLine("📊 QUICK SIDE-BY-SIDE COMPARISON");
            Console.WriteLine("=================================");
            Console.WriteLine();

            var alarm = new Alarm();            Console.WriteLine("🔴 BAD EXAMPLE (Ask Approach):");
            Console.WriteLine("-------------------------------");
            var askMonitor = new AskMonitor("Temperature Sensor", 25, alarm);
            askMonitor.SetValue(30);

            // Demonstrate the ask approach problems
            Console.WriteLine($"Monitor name: {askMonitor.GetName()}");
            Console.WriteLine($"Current value: {askMonitor.GetValue()}");
            Console.WriteLine($"Limit: {askMonitor.GetLimit()}");
            Console.WriteLine("External code must check: if (value > limit) alarm.Warn(...)");
            if (askMonitor.GetValue() > askMonitor.GetLimit())
            {
                alarm.Warn($"{askMonitor.GetName()} exceeded limit!");
            }

            Console.WriteLine();
            Console.WriteLine("✅ GOOD EXAMPLE (Tell Approach):");
            Console.WriteLine("----------------------------------");
            var tellMonitor = new TellMonitor("Temperature Sensor", 25, alarm);
            Console.WriteLine("Simply tell the monitor: SetValue(30)");
            tellMonitor.SetValue(30); // Monitor handles everything internally
            Console.WriteLine($"Status: {tellMonitor.GetStatusReport()}");

            Console.WriteLine();
            Console.WriteLine("🎯 KEY DIFFERENCES:");
            Console.WriteLine("• Ask: External code knows internal structure and makes decisions");
            Console.WriteLine("• Tell: Object encapsulates behavior and handles its own logic");
            Console.WriteLine("• Tell approach is more maintainable and less error-prone");
        }

        static void ShowDetailedExamples()
        {
            Console.WriteLine("🔍 DETAILED CODE EXAMPLES");
            Console.WriteLine("==========================");
            Console.WriteLine();

            Console.WriteLine("Select an example to see in detail:");
            Console.WriteLine("1. 📊 Monitor System (Temperature/Pressure monitoring)");
            Console.WriteLine("2. 💰 Bank Account (Financial operations)");
            Console.WriteLine("3. 🛒 Shopping Cart (E-commerce operations)");
            Console.WriteLine("4. 🔐 Security System (Access control)");
            Console.WriteLine("0. ← Back to main menu");

            Console.Write("Choice: ");
            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    DemonstrateMonitorExample();
                    break;
                case "2":
                    DemonstrateBankExample();
                    break;
                case "3":
                    DemonstrateShoppingExample();
                    break;
                case "4":
                    DemonstrateSecurityExample();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        static void DemonstrateMonitorExample()
        {
            Console.WriteLine("📊 MONITOR SYSTEM EXAMPLE");
            Console.WriteLine("=========================");
            
            var alarm = new Alarm();
              // Create monitors
            var askMonitor = new AskMonitor("Pressure Sensor", 100, alarm);
            var tellMonitor = new TellMonitor("Pressure Sensor", 100, alarm);
            
            Console.WriteLine("Simulating pressure readings: 85, 105, 120, 95");
            Console.WriteLine();
            
            int[] readings = { 85, 105, 120, 95 };
            
            Console.WriteLine("🔴 ASK APPROACH:");
            foreach (int reading in readings)
            {
                askMonitor.SetValue(reading);
                Console.WriteLine($"Reading: {reading} | Value: {askMonitor.GetValue()} | Limit: {askMonitor.GetLimit()}");
                
                // External code has to make the decision
                if (askMonitor.GetValue() > askMonitor.GetLimit())
                {
                    Console.WriteLine($"⚠️  EXTERNAL CODE SAYS: Pressure too high!");
                }
            }
            
            Console.WriteLine("\n✅ TELL APPROACH:");
            foreach (int reading in readings)
            {
                tellMonitor.SetValue(reading); // Monitor handles everything
                Console.WriteLine(tellMonitor.GetStatusReport());
            }
        }

        static void DemonstrateBankExample()
        {
            Console.WriteLine("💰 BANK ACCOUNT EXAMPLE");
            Console.WriteLine("=======================");
              var askAccount = new AskBankAccount("ASK-001", 1000m, 500m);
            var tellAccount = new TellBankAccount("TELL-001", 1000m);
            
            Console.WriteLine("Attempting to withdraw $1200 (more than balance):");
            Console.WriteLine();
            
            Console.WriteLine("🔴 ASK APPROACH:");
            decimal withdrawAmount = 1200m;
            Console.WriteLine($"Current balance: {askAccount.GetBalance():C}");
            
            // External code has to check and decide
            if (askAccount.GetBalance() >= withdrawAmount)
            {
                askAccount.SetBalance(askAccount.GetBalance() - withdrawAmount);
                Console.WriteLine("Withdrawal successful");
            }
            else
            {
                Console.WriteLine("❌ External code says: Insufficient funds");
            }
            
            Console.WriteLine("\n✅ TELL APPROACH:");
            Console.WriteLine("Simply tell account to withdraw $1200:");
            bool success = tellAccount.Withdraw(1200m); // Account handles validation
            Console.WriteLine($"Result: {(success ? "Success" : "Failed")}");
        }

        static void DemonstrateShoppingExample()
        {
            Console.WriteLine("🛒 SHOPPING CART EXAMPLE");
            Console.WriteLine("========================");
              var laptop = new Product("LAPTOP", "Gaming Laptop", 1299.99m);
            var mouse = new Product("MOUSE", "Wireless Mouse", 29.99m);
            
            var askCart = new AskShoppingCart();
            var tellCart = new TellShoppingCart();
            
            Console.WriteLine("Adding products to cart:");
            Console.WriteLine();
            
            Console.WriteLine("🔴 ASK APPROACH:");
            // External code has to manage cart internals
            var items = askCart.GetItems();
            items.Add(new CartItem(laptop, 1));
            items.Add(new CartItem(mouse, 2));
            
            // External code calculates total
            decimal askTotal = 0;
            foreach (var item in askCart.GetItems())
            {
                askTotal += item.Product.Price * item.Quantity;
            }
            Console.WriteLine($"External calculation - Total: {askTotal:C}");
            
            Console.WriteLine("\n✅ TELL APPROACH:");
            tellCart.AddItem(laptop, 1);   // Cart handles the logic
            tellCart.AddItem(mouse, 2);    // Cart handles the logic
            Console.WriteLine($"Cart calculation - Total: {tellCart.CalculateTotal():C}");
            Console.WriteLine(tellCart.GetCartSummary());
        }

        static void DemonstrateSecurityExample()
        {
            Console.WriteLine("🔐 SECURITY SYSTEM EXAMPLE");
            Console.WriteLine("==========================");
            
            var askSecurity = new AskSecuritySystem();
            var tellSecurity = new TellSecuritySystem();
            
            // Setup
            askSecurity.AddUser("john", AccessLevel.Basic);
            askSecurity.SetResourcePermission("admin-panel", AccessLevel.Admin);
            
            var john = new SecurityUser("john", AccessLevel.Basic);
            tellSecurity.RegisterUser(john);
            tellSecurity.SetResourcePermission("admin-panel", AccessLevel.Admin);
            
            Console.WriteLine("John (Basic access) trying to access Admin Panel:");
            Console.WriteLine();
            
            Console.WriteLine("🔴 ASK APPROACH:");
            var userLevel = askSecurity.GetUserAccessLevel("john");
            var requiredLevel = askSecurity.GetResourceRequiredLevel("admin-panel");
            Console.WriteLine($"User level: {userLevel}");
            Console.WriteLine($"Required level: {requiredLevel}");
            
            // External code makes security decision
            if (userLevel >= requiredLevel)
            {
                Console.WriteLine("✅ External code says: Access granted");
            }
            else
            {
                Console.WriteLine("❌ External code says: Access denied");
            }
            
            Console.WriteLine("\n✅ TELL APPROACH:");
            Console.WriteLine("Simply tell security system: RequestAccess('john', 'admin-panel')");
            bool hasAccess = tellSecurity.RequestAccess("john", "admin-panel");
            // Security system has already handled and logged the decision
        }

        static void ShowInteractivePlayground()
        {
            Console.WriteLine("🧪 INTERACTIVE PLAYGROUND");
            Console.WriteLine("=========================");
            Console.WriteLine("Create your own examples to see the difference!");
            Console.WriteLine();

            var alarm = new Alarm();
            var tellMonitor = new TellMonitor("Custom Monitor", 50, alarm);
            
            Console.WriteLine("You have a TellMonitor with limit 50.");
            Console.WriteLine("Enter temperature values to see how it behaves:");
            Console.WriteLine("(Enter 'quit' to exit)");
            Console.WriteLine();

            while (true)
            {
                Console.Write("Enter temperature value: ");
                string input = Console.ReadLine() ?? "";
                
                if (input.ToLower() == "quit")
                    break;
                
                if (int.TryParse(input, out int temp))
                {
                    tellMonitor.SetValue(temp);
                    Console.WriteLine($"Result: {tellMonitor.GetStatusReport()}");
                }
                else
                {
                    Console.WriteLine("Please enter a valid number or 'quit'");
                }
                Console.WriteLine();
            }
        }

        static void ShowPrincipleExplanation()
        {
            Console.WriteLine("📚 TELL, DON'T ASK PRINCIPLE EXPLANATION");
            Console.WriteLine("=========================================");
            Console.WriteLine();
            
            Console.WriteLine("🎯 WHAT IS TELL, DON'T ASK?");
            Console.WriteLine("The Tell, Don't Ask principle is a design guideline that encourages");
            Console.WriteLine("objects to encapsulate behavior rather than just data. Instead of");
            Console.WriteLine("asking objects for their state and making decisions based on that");
            Console.WriteLine("state, you should tell objects what you want them to do.");
            Console.WriteLine();
            
            Console.WriteLine("❌ DON'T ASK (Bad):");
            Console.WriteLine("if (account.getBalance() > amount) {");
            Console.WriteLine("    account.setBalance(account.getBalance() - amount);");
            Console.WriteLine("}");
            Console.WriteLine();
            
            Console.WriteLine("✅ DO TELL (Good):");
            Console.WriteLine("bool success = account.withdraw(amount);");
            Console.WriteLine();
            
            Console.WriteLine("🔍 WHY IS THIS IMPORTANT?");
            Console.WriteLine("1. ENCAPSULATION: Business logic stays with the data it operates on");
            Console.WriteLine("2. MAINTAINABILITY: Changes to business rules only affect one place");
            Console.WriteLine("3. TESTABILITY: Objects can be tested independently");
            Console.WriteLine("4. REUSABILITY: Objects can be used in different contexts safely");
            Console.WriteLine("5. ROBUSTNESS: Less chance for external code to break object invariants");
            Console.WriteLine();
            
            Console.WriteLine("🚀 BENEFITS IN PRACTICE:");
            Console.WriteLine("• Reduced coupling between objects");
            Console.WriteLine("• Easier to understand and modify code");
            Console.WriteLine("• Better separation of concerns");
            Console.WriteLine("• More object-oriented design");
            Console.WriteLine("• Fewer bugs due to misused object state");
            Console.WriteLine();
            
            Console.WriteLine("⚠️  WHEN TO BE CAREFUL:");
            Console.WriteLine("• Sometimes you do need to query object state (for display, reporting)");
            Console.WriteLine("• Don't create unnecessary methods just to avoid getters");
            Console.WriteLine("• Balance between encapsulation and practical usability");
            Console.WriteLine("• Consider the context - not every situation requires this principle");
        }
    }
}

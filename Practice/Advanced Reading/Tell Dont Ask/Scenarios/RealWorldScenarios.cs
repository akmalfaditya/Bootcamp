using Tell_Dont_Ask.Models;
using Tell_Dont_Ask.BadExamples;
using Tell_Dont_Ask.GoodExamples;

namespace Tell_Dont_Ask.Scenarios
{
    // SCENARIO 1: E-commerce Order Processing
    // This scenario shows how Tell Don't Ask applies to a real business process
    public static class ECommerceScenario
    {
        public static void DemonstrateOrderProcessing()
        {
            Console.WriteLine("🛒 SCENARIO: E-commerce Order Processing");
            Console.WriteLine("==========================================");
              // Create some test products
            var products = new List<Product>
            {
                new Product("LAPTOP001", "Gaming Laptop", 1299.99m),
                new Product("MOUSE001", "Wireless Mouse", 29.99m),
                new Product("KEYBOARD001", "Mechanical Keyboard", 89.99m)
            };

            Console.WriteLine("\n--- BAD APPROACH (Ask-based) ---");
            DemonstrateAskBasedOrdering(products);

            Console.WriteLine("\n--- GOOD APPROACH (Tell-based) ---");
            DemonstrateTellBasedOrdering(products);
        }

        private static void DemonstrateAskBasedOrdering(List<Product> products)
        {
            var askCart = new AskShoppingCart();
            
            // With Ask approach, external code has to know too much about cart internals
            foreach (var product in products.Take(2))
            {
                // Bad: We have to get the items list and check if product exists
                var existingItems = askCart.GetItems();
                var existingItem = existingItems.FirstOrDefault(item => item.Product.Id == product.Id);
                
                if (existingItem != null)
                {
                    // Bad: We're manipulating the cart's internal state from outside
                    existingItem.Quantity += 1;
                }
                else
                {
                    // Bad: We're directly adding to the cart's internal collection
                    existingItems.Add(new CartItem(product, 1));
                }
            }

            // Bad: We have to calculate total ourselves by asking for internal data
            var items = askCart.GetItems();
            decimal total = 0;
            foreach (var item in items)
            {
                total += item.Product.Price * item.Quantity;
            }
            
            Console.WriteLine($"Ask-based cart total: {total:C}");
            Console.WriteLine("Problems: External code knows too much about cart structure, no encapsulation of business rules");
        }

        private static void DemonstrateTellBasedOrdering(List<Product> products)
        {
            var tellCart = new TellShoppingCart();
            
            // With Tell approach, we just tell the cart what to do
            foreach (var product in products.Take(2))
            {
                // Good: We tell the cart to add an item, it handles the logic
                tellCart.AddItem(product, 1);
            }

            // Good: We tell the cart to calculate its own total
            decimal total = tellCart.CalculateTotal();
            
            // Good: We tell the cart to apply a discount, it knows how to do this
            tellCart.ApplyDiscount(15);
            
            Console.WriteLine(tellCart.GetCartSummary());
            Console.WriteLine("Benefits: Clean interface, encapsulated business logic, easier to maintain");
        }
    }

    // SCENARIO 2: IoT Temperature Monitoring System
    // Shows how Tell Don't Ask applies to real-time systems
    public static class IoTMonitoringScenario
    {
        public static void DemonstrateTemperatureMonitoring()
        {
            Console.WriteLine("\n🌡️ SCENARIO: IoT Temperature Monitoring");
            Console.WriteLine("=========================================");
            
            var alarm = new Alarm();
            
            Console.WriteLine("\n--- BAD APPROACH (Ask-based) ---");
            DemonstrateAskBasedMonitoring(alarm);
            
            Console.WriteLine("\n--- GOOD APPROACH (Tell-based) ---");
            DemonstrateTellBasedMonitoring(alarm);
        }

        private static void DemonstrateAskBasedMonitoring(Alarm alarm)
        {
            var askMonitor = new AskMonitor("Server Room", 25, alarm);
            
            // Simulate sensor readings
            int[] sensorReadings = { 22, 24, 27, 30, 28, 23 };
            
            foreach (int reading in sensorReadings)
            {
                // Bad: External code has to ask monitor for its state and make decisions
                askMonitor.SetValue(reading);
                
                // Bad: We have to ask for current value and limit to make alarm decisions
                if (askMonitor.GetValue() > askMonitor.GetLimit())
                {
                    // Bad: External code is responsible for the business logic
                    alarm.Warn($"{askMonitor.GetName()} too hot: {askMonitor.GetValue()}°C > {askMonitor.GetLimit()}°C");
                }
                
                Console.WriteLine($"Reading: {reading}°C (Limit: {askMonitor.GetLimit()}°C)");
            }
            
            Console.WriteLine("Problems: Scattered business logic, tight coupling between monitor and controller");
        }

        private static void DemonstrateTellBasedMonitoring(Alarm alarm)
        {
            var tellMonitor = new TellMonitor("Server Room", 25, alarm);
            
            // Simulate the same sensor readings
            int[] sensorReadings = { 22, 24, 27, 30, 28, 23 };
            
            foreach (int reading in sensorReadings)
            {
                // Good: We just tell the monitor what the new reading is
                // It handles all the business logic internally
                tellMonitor.SetValue(reading);
                Console.WriteLine(tellMonitor.GetStatusReport());
            }
            
            Console.WriteLine("Benefits: Business logic encapsulated in monitor, loose coupling, easier testing");
        }
    }

    // SCENARIO 3: Security Access Control System
    // Demonstrates how Tell Don't Ask improves security system design
    public static class SecurityScenario
    {
        public static void DemonstrateAccessControl()
        {
            Console.WriteLine("\n🔐 SCENARIO: Security Access Control");
            Console.WriteLine("====================================");
            
            Console.WriteLine("\n--- BAD APPROACH (Ask-based) ---");
            DemonstrateAskBasedSecurity();
            
            Console.WriteLine("\n--- GOOD APPROACH (Tell-based) ---");
            DemonstrateTellBasedSecurity();
        }

        private static void DemonstrateAskBasedSecurity()
        {
            var askSecurity = new AskSecuritySystem();
            
            // Set up users and resources
            askSecurity.AddUser("john", AccessLevel.Basic);
            askSecurity.AddUser("admin", AccessLevel.Admin);
            askSecurity.SetResourcePermission("database", AccessLevel.Admin);
            askSecurity.SetResourcePermission("reports", AccessLevel.Basic);
            
            // Bad: External code has to ask system for user details and make security decisions
            string[] accessRequests = { "john:reports", "john:database", "admin:database" };
            
            foreach (string request in accessRequests)
            {
                string[] parts = request.Split(':');
                string username = parts[0];
                string resource = parts[1];
                
                // Bad: We have to ask the system for user level and resource requirements
                var userLevel = askSecurity.GetUserAccessLevel(username);
                var requiredLevel = askSecurity.GetResourceRequiredLevel(resource);
                
                // Bad: External code makes the security decision
                if (userLevel >= requiredLevel)
                {
                    Console.WriteLine($"✅ Access granted: {username} can access {resource}");
                }
                else
                {
                    Console.WriteLine($"❌ Access denied: {username} cannot access {resource}");
                }
            }
            
            Console.WriteLine("Problems: Security logic scattered, potential for security bugs, hard to audit");
        }

        private static void DemonstrateTellBasedSecurity()
        {
            var tellSecurity = new TellSecuritySystem();
            
            // Set up users and resources
            var john = new SecurityUser("john", AccessLevel.Basic);
            var admin = new SecurityUser("admin", AccessLevel.Admin);
            
            tellSecurity.RegisterUser(john);
            tellSecurity.RegisterUser(admin);
            tellSecurity.SetResourcePermission("database", AccessLevel.Admin);
            tellSecurity.SetResourcePermission("reports", AccessLevel.Basic);
            
            // Good: We just tell the security system about access requests
            string[] accessRequests = { "john:reports", "john:database", "admin:database" };
            
            foreach (string request in accessRequests)
            {
                string[] parts = request.Split(':');
                string username = parts[0];
                string resource = parts[1];
                
                // Good: Security system handles all the logic internally
                bool accessGranted = tellSecurity.RequestAccess(username, resource);
                // The system has already logged the result, we just get the boolean outcome
            }
            
            Console.WriteLine("Benefits: Centralized security logic, easier to audit, consistent enforcement");
        }
    }

    // SCENARIO 4: Banking Transaction Processing
    // Shows how Tell Don't Ask prevents common financial processing errors
    public static class BankingScenario
    {
        public static void DemonstrateBankingOperations()
        {
            Console.WriteLine("\n💰 SCENARIO: Banking Transaction Processing");
            Console.WriteLine("===========================================");
            
            Console.WriteLine("\n--- BAD APPROACH (Ask-based) ---");
            DemonstrateAskBasedBanking();
            
            Console.WriteLine("\n--- GOOD APPROACH (Tell-based) ---");
            DemonstrateTellBasedBanking();
        }

        private static void DemonstrateAskBasedBanking()
        {            var account1 = new AskBankAccount("12345", 1000m, 500m);
            var account2 = new AskBankAccount("67890", 500m, 200m);
            
            decimal transferAmount = 300m;
            
            Console.WriteLine($"Initial balances - Account1: {account1.GetBalance():C}, Account2: {account2.GetBalance():C}");
            
            // Bad: External code has to ask accounts for balances and make decisions
            if (account1.GetBalance() >= transferAmount)
            {
                // Bad: We're directly manipulating account balances from outside
                account1.SetBalance(account1.GetBalance() - transferAmount);
                account2.SetBalance(account2.GetBalance() + transferAmount);
                Console.WriteLine($"Transfer successful: {transferAmount:C}");
            }
            else
            {
                Console.WriteLine("Transfer failed: Insufficient funds");
            }
            
            Console.WriteLine($"Final balances - Account1: {account1.GetBalance():C}, Account2: {account2.GetBalance():C}");
            Console.WriteLine("Problems: No validation, no transaction integrity, balance manipulation from outside");
        }

        private static void DemonstrateTellBasedBanking()
        {
            var account1 = new TellBankAccount("12345", 1000m);
            var account2 = new TellBankAccount("67890", 500m);
            
            decimal transferAmount = 300m;
            
            Console.WriteLine("Initial accounts created with starting balances");
            
            // Good: We tell accounts what to do, they handle all the business logic
            bool withdrawalSuccessful = account1.Withdraw(transferAmount);
            
            if (withdrawalSuccessful)
            {
                account2.Deposit(transferAmount);
                Console.WriteLine("Transfer completed successfully");
            }
            else
            {
                Console.WriteLine("Transfer failed - withdrawal was not authorized");
            }
            
            Console.WriteLine($"Account summaries:");
            Console.WriteLine($"- {account1.GetAccountSummary()}");
            Console.WriteLine($"- {account2.GetAccountSummary()}");
            Console.WriteLine("Benefits: Built-in validation, transaction safety, encapsulated business rules");
        }
    }

    // Main scenario runner
    public static class ScenarioRunner
    {
        public static void RunAllScenarios()
        {
            Console.WriteLine("🎯 TELL, DON'T ASK - REAL WORLD SCENARIOS");
            Console.WriteLine("==========================================");
            Console.WriteLine("These scenarios demonstrate the practical benefits of the Tell, Don't Ask principle");
            Console.WriteLine("in real-world applications. Notice how the 'Tell' approach results in:");
            Console.WriteLine("• Better encapsulation of business logic");
            Console.WriteLine("• Reduced coupling between objects");
            Console.WriteLine("• Easier testing and maintenance");
            Console.WriteLine("• More robust and error-resistant code\n");

            ECommerceScenario.DemonstrateOrderProcessing();
            IoTMonitoringScenario.DemonstrateTemperatureMonitoring();
            SecurityScenario.DemonstrateAccessControl();
            BankingScenario.DemonstrateBankingOperations();

            Console.WriteLine("\n🎉 SCENARIOS COMPLETED!");
            Console.WriteLine("=============================");
            Console.WriteLine("Key Takeaways:");
            Console.WriteLine("1. Objects should encapsulate their own behavior, not just data");
            Console.WriteLine("2. External code should tell objects what to do, not ask for data to make decisions");
            Console.WriteLine("3. Business logic belongs inside the objects that own the data");
            Console.WriteLine("4. This leads to more maintainable, testable, and robust code");
        }
    }
}

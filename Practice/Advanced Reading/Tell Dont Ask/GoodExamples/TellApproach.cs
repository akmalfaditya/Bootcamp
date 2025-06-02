using Tell_Dont_Ask.Models;

namespace Tell_Dont_Ask.GoodExamples
{
    // GOOD EXAMPLE: Monitor that encapsulates its own behavior
    // Instead of asking for values and making decisions outside,
    // we tell the monitor what value to set and it handles everything internally
    public class TellMonitor
    {
        private int _value;
        private readonly int _limit;
        private readonly string _name;
        private readonly Alarm _alarm;

        public TellMonitor(string name, int limit, Alarm alarm)
        {
            _name = name;
            _limit = limit;
            _alarm = alarm;
        }

        // Notice how this method doesn't just set the value - it also handles the business logic
        // The object is responsible for its own behavior, not some external controller
        public void SetValue(int newValue)
        {
            _value = newValue;
            
            // The monitor decides what to do when the value changes
            // No need for external code to ask "is it too high?" and then decide what to do
            if (_value > _limit)
            {
                _alarm.Warn($"{_name} too high: {_value} exceeds limit {_limit}");
            }
        }

        // Sometimes you still need to expose some information, but keep it minimal
        // Only expose what's absolutely necessary for the outside world
        public string GetStatusReport()
        {
            return $"{_name}: {_value}/{_limit} {(_value > _limit ? "(ALARM!)" : "(OK)")}";
        }
    }

    // GOOD EXAMPLE: Bank account that protects its own rules
    // Instead of exposing balance and letting external code decide if withdrawal is valid,
    // the account handles all the business logic internally
    public class TellBankAccount
    {
        private decimal _balance;
        private readonly string _accountNumber;

        public TellBankAccount(string accountNumber, decimal initialBalance)
        {
            _accountNumber = accountNumber;
            _balance = initialBalance;
        }

        // The account knows how to handle withdrawals properly
        // It encapsulates all the business rules internally
        public bool Withdraw(decimal amount)
        {
            // All the validation logic stays inside the object
            if (amount <= 0)
            {
                Console.WriteLine("Invalid withdrawal amount");
                return false;
            }

            if (_balance < amount)
            {
                Console.WriteLine($"Insufficient funds. Available: {_balance:C}");
                return false;
            }

            // The account is responsible for maintaining its own integrity
            _balance -= amount;
            Console.WriteLine($"Withdrew {amount:C}. New balance: {_balance:C}");
            return true;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Invalid deposit amount");
                return;
            }

            _balance += amount;
            Console.WriteLine($"Deposited {amount:C}. New balance: {_balance:C}");
        }

        // Only expose what's necessary - a summary, not the raw data
        public string GetAccountSummary()
        {
            return $"Account {_accountNumber}: {_balance:C}";
        }
    }

    // GOOD EXAMPLE: Shopping cart that manages its own operations
    // Instead of exposing the items list and letting external code manipulate it,
    // the cart provides specific operations and handles the complexity internally
    public class TellShoppingCart
    {
        private readonly List<CartItem> _items;

        public TellShoppingCart()
        {
            _items = new List<CartItem>();
        }

        // Tell the cart to add an item - it handles the logic internally
        public void AddItem(Product product, int quantity)
        {
            if (product == null || quantity <= 0)
            {
                Console.WriteLine("Invalid product or quantity");
                return;
            }

            // The cart decides how to handle existing items
            var existingItem = _items.FirstOrDefault(item => item.Product.Id == product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                Console.WriteLine($"Updated quantity for {product.Name}. New quantity: {existingItem.Quantity}");
            }
            else
            {
                _items.Add(new CartItem(product, quantity));
                Console.WriteLine($"Added {product.Name} to cart (quantity: {quantity})");
            }
        }

        // Tell the cart to remove items - it knows how to handle this properly
        public void RemoveItem(string productId, int quantity)
        {
            var item = _items.FirstOrDefault(i => i.Product.Id == productId);
            if (item == null)
            {
                Console.WriteLine("Product not found in cart");
                return;
            }

            if (quantity >= item.Quantity)
            {
                _items.Remove(item);
                Console.WriteLine($"Removed {item.Product.Name} from cart completely");
            }
            else
            {
                item.Quantity -= quantity;
                Console.WriteLine($"Reduced {item.Product.Name} quantity by {quantity}");
            }
        }

        // The cart knows how to calculate its own total
        // No need to expose internal collection and let external code iterate
        public decimal CalculateTotal()
        {
            return _items.Sum(item => item.Product.Price * item.Quantity);
        }

        // Provide meaningful operations, not raw data access
        public void ApplyDiscount(decimal discountPercentage)
        {
            if (discountPercentage < 0 || discountPercentage > 100)
            {
                Console.WriteLine("Invalid discount percentage");
                return;
            }

            var total = CalculateTotal();
            var discountAmount = total * (discountPercentage / 100);
            Console.WriteLine($"Applied {discountPercentage}% discount. Saved: {discountAmount:C}");
        }

        public string GetCartSummary()
        {
            if (!_items.Any())
                return "Cart is empty";

            var summary = "Cart Contents:\n";
            foreach (var item in _items)
            {
                var itemTotal = item.Product.Price * item.Quantity;
                summary += $"- {item.Product.Name} (x{item.Quantity}): {itemTotal:C}\n";
            }
            summary += $"Total: {CalculateTotal():C}";
            return summary;
        }
    }

    // GOOD EXAMPLE: Security system that manages access control internally
    // Instead of exposing user levels and letting external code make security decisions,
    // the system encapsulates all security logic
    public class TellSecuritySystem
    {
        private readonly Dictionary<string, SecurityUser> _users;
        private readonly Dictionary<string, AccessLevel> _resourcePermissions;

        public TellSecuritySystem()
        {
            _users = new Dictionary<string, SecurityUser>();
            _resourcePermissions = new Dictionary<string, AccessLevel>();
        }

        public void RegisterUser(SecurityUser user)
        {
            if (user == null || string.IsNullOrEmpty(user.Username))
            {
                Console.WriteLine("Invalid user data");
                return;
            }

            _users[user.Username] = user;
            Console.WriteLine($"User {user.Username} registered with {user.AccessLevel} access");
        }

        public void SetResourcePermission(string resourceName, AccessLevel requiredLevel)
        {
            _resourcePermissions[resourceName] = requiredLevel;
            Console.WriteLine($"Resource '{resourceName}' now requires {requiredLevel} access");
        }

        // The security system decides if access should be granted
        // External code doesn't need to know about access levels or make comparisons
        public bool RequestAccess(string username, string resourceName)
        {
            if (!_users.ContainsKey(username))
            {
                Console.WriteLine($"Access denied: User '{username}' not found");
                return false;
            }

            if (!_resourcePermissions.ContainsKey(resourceName))
            {
                Console.WriteLine($"Access granted: Resource '{resourceName}' has no restrictions");
                return true;
            }

            var user = _users[username];
            var requiredLevel = _resourcePermissions[resourceName];

            // The security system encapsulates the access control logic
            bool hasAccess = user.AccessLevel >= requiredLevel;
            
            if (hasAccess)
            {
                Console.WriteLine($"Access granted: {username} can access {resourceName}");
                return true;
            }
            else
            {
                Console.WriteLine($"Access denied: {username} needs {requiredLevel} access for {resourceName}");
                return false;
            }
        }

        public string GetSecurityReport()
        {
            var report = $"Security System Status:\n";
            report += $"Registered Users: {_users.Count}\n";
            report += $"Protected Resources: {_resourcePermissions.Count}\n";
            return report;
        }
    }

    // GOOD EXAMPLE: Temperature controller that manages its own behavior
    // Instead of just storing temperature and letting external code decide what to do,
    // the controller actively manages the temperature based on its settings
    public class TellTemperatureController
    {
        private int _currentTemperature;
        private readonly int _targetTemperature;
        private readonly int _tolerance;
        private bool _heaterOn;
        private bool _coolerOn;

        public TellTemperatureController(int targetTemperature, int tolerance = 2)
        {
            _targetTemperature = targetTemperature;
            _tolerance = tolerance;
            _currentTemperature = targetTemperature; // Start at target
        }

        // Tell the controller about a temperature reading - it decides what to do
        public void UpdateTemperature(int newTemperature)
        {
            _currentTemperature = newTemperature;
            
            // The controller encapsulates its own control logic
            // No external code needs to ask "should I turn on the heater?"
            AdjustSystemsBasedOnTemperature();
        }

        private void AdjustSystemsBasedOnTemperature()
        {
            int lowerBound = _targetTemperature - _tolerance;
            int upperBound = _targetTemperature + _tolerance;

            // Turn off both systems first
            _heaterOn = false;
            _coolerOn = false;

            if (_currentTemperature < lowerBound)
            {
                _heaterOn = true;
                Console.WriteLine($"Temperature {_currentTemperature}°C is too low. Heater ON");
            }
            else if (_currentTemperature > upperBound)
            {
                _coolerOn = true;
                Console.WriteLine($"Temperature {_currentTemperature}°C is too high. Cooler ON");
            }
            else
            {
                Console.WriteLine($"Temperature {_currentTemperature}°C is within range. Systems OFF");
            }
        }

        // Tell the controller to adjust its target - it handles the implications
        public void SetTargetTemperature(int newTarget)
        {
            Console.WriteLine($"Target temperature changed from {_targetTemperature}°C to {newTarget}°C");
            
            // We can't change the readonly field, but in a real implementation you'd handle this
            // For now, we'll just re-evaluate with the current temperature
            AdjustSystemsBasedOnTemperature();
        }

        public string GetControllerStatus()
        {
            string heaterStatus = _heaterOn ? "ON" : "OFF";
            string coolerStatus = _coolerOn ? "ON" : "OFF";
            
            return $"Temperature Controller Status:\n" +
                   $"Current: {_currentTemperature}°C | Target: {_targetTemperature}°C\n" +
                   $"Heater: {heaterStatus} | Cooler: {coolerStatus}";
        }
    }

    // GOOD EXAMPLE: Smart controllers that work with Tell-based objects
    // These controllers focus on coordination rather than making decisions based on internal state
    public static class TellBasedControllers
    {
        // This controller doesn't ask monitors for their values and make decisions
        // Instead, it tells monitors what values to set and they handle the logic
        public static void UpdateMonitoringSystem(List<TellMonitor> monitors, Dictionary<string, int> sensorReadings)
        {
            Console.WriteLine("=== Updating Monitoring System ===");
            
            foreach (var monitor in monitors)
            {
                // We don't ask "what's your current value?" or "what's your limit?"
                // We simply tell each monitor to update with new sensor data
                // Each monitor is responsible for deciding what to do with that information
                
                // In a real system, you'd match monitors to sensors by name or ID
                // For demo purposes, we'll update all monitors with random values
                var randomValue = new Random().Next(0, 10);
                monitor.SetValue(randomValue);
            }
        }

        // This controller coordinates bank operations without peeking into account details
        public static void ProcessBankTransactions(TellBankAccount fromAccount, TellBankAccount toAccount, decimal amount)
        {
            Console.WriteLine($"=== Processing Transfer of {amount:C} ===");
            
            // We don't ask accounts for their balances and make decisions
            // We tell each account what to do and let them handle the business rules
            bool withdrawalSuccessful = fromAccount.Withdraw(amount);
            
            if (withdrawalSuccessful)
            {
                toAccount.Deposit(amount);
                Console.WriteLine("Transfer completed successfully");
            }
            else
            {
                Console.WriteLine("Transfer failed - withdrawal was declined");
            }
        }

        // This controller manages shopping without accessing cart internals
        public static void ProcessShopping(TellShoppingCart cart, List<Product> availableProducts)
        {
            Console.WriteLine("=== Processing Shopping Session ===");
            
            // We don't ask the cart "what items do you have?" and manipulate them externally
            // We tell the cart what operations to perform
            
            // Add some products
            foreach (var product in availableProducts.Take(3))
            {
                cart.AddItem(product, new Random().Next(1, 4));
            }
            
            // Apply a discount
            cart.ApplyDiscount(10);
            
            // Show summary (this is acceptable - we're asking for a summary, not raw data to manipulate)
            Console.WriteLine(cart.GetCartSummary());
        }
    }
}

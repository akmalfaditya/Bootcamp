using Tell_Dont_Ask.Models;

namespace Tell_Dont_Ask.BadExamples;

/// <summary>
/// BAD EXAMPLE - Classic "Ask" approach to monitoring
/// This class is just a data container - it has no real behavior
/// All the logic happens outside the object, which violates encapsulation
/// </summary>
public class AskMonitor
{
    private int _value;
    private readonly int _limit;
    private readonly string _name;
    private readonly Alarm _alarm;

    public AskMonitor(string name, int limit, Alarm alarm)
    {
        _name = name;
        _limit = limit;
        _alarm = alarm;
    }

    // Look at all these getters! This is a red flag.
    // We're exposing internal state for others to make decisions
    public int GetValue() => _value;
    public void SetValue(int value) => _value = value; // Just sets data, no logic
    public int GetLimit() => _limit;
    public string GetName() => _name;
    public Alarm GetAlarm() => _alarm;
}

/// <summary>
/// BAD EXAMPLE - Bank account that's just a data holder
/// Notice how it doesn't enforce any business rules - that's done externally
/// </summary>
public class AskBankAccount
{
    private decimal _balance;
    private readonly string _accountNumber;
    private readonly decimal _overdraftLimit;

    public AskBankAccount(string accountNumber, decimal initialBalance, decimal overdraftLimit)
    {
        _accountNumber = accountNumber;
        _balance = initialBalance;
        _overdraftLimit = overdraftLimit;
    }

    // More getters and setters - no business logic here
    public decimal GetBalance() => _balance;
    public void SetBalance(decimal balance) => _balance = balance;
    public string GetAccountNumber() => _accountNumber;
    public decimal GetOverdraftLimit() => _overdraftLimit;
}

/// <summary>
/// BAD EXAMPLE - Shopping cart that's just a list container
/// All pricing, discount, and validation logic happens outside
/// </summary>
public class AskShoppingCart
{
    private readonly List<CartItem> _items = new();
    private decimal _discountPercentage;

    public List<CartItem> GetItems() => _items; // Exposing internal collection - very bad!
    public decimal GetDiscountPercentage() => _discountPercentage;
    public void SetDiscountPercentage(decimal discount) => _discountPercentage = discount;
    
    public void AddItem(CartItem item) => _items.Add(item); // Just adds, no validation
}

/// <summary>
/// BAD EXAMPLE - Security system that exposes all internal state
/// Access control logic is handled by external code
/// </summary>
public class AskSecuritySystem
{
    private readonly List<SecurityUser> _users = new();
    private readonly List<string> _accessAttempts = new();
    private readonly Dictionary<string, AccessLevel> _resourcePermissions = new();
    private bool _isLocked;

    public List<SecurityUser> GetUsers() => _users;
    public List<string> GetAccessAttempts() => _accessAttempts;
    public bool IsLocked() => _isLocked;
    public void SetLocked(bool locked) => _isLocked = locked;
    
    public void AddUser(SecurityUser user) => _users.Add(user);
    public void AddUser(string username, AccessLevel accessLevel) => _users.Add(new SecurityUser(username, accessLevel));
    public void LogAttempt(string attempt) => _accessAttempts.Add(attempt);
    
    // More getters for external code to make decisions - this is the problem!
    public AccessLevel GetUserAccessLevel(string username)
    {
        var user = _users.FirstOrDefault(u => u.Username == username);
        return user?.AccessLevel ?? AccessLevel.Guest;
    }
    
    public AccessLevel GetResourceRequiredLevel(string resourceName)
    {
        return _resourcePermissions.ContainsKey(resourceName) ? _resourcePermissions[resourceName] : AccessLevel.Guest;
    }
    
    public void SetResourcePermission(string resourceName, AccessLevel requiredLevel)
    {
        _resourcePermissions[resourceName] = requiredLevel;
    }
}

/// <summary>
/// BAD EXAMPLE - Temperature controller that's just a data store
/// All the heating/cooling logic lives outside this class
/// </summary>
public class AskTemperatureController
{
    private double _currentTemperature;
    private double _targetTemperature;
    private bool _heatingOn;
    private bool _coolingOn;

    public double GetCurrentTemperature() => _currentTemperature;
    public void SetCurrentTemperature(double temp) => _currentTemperature = temp;
    public double GetTargetTemperature() => _targetTemperature;
    public void SetTargetTemperature(double temp) => _targetTemperature = temp;
    public bool IsHeatingOn() => _heatingOn;
    public void SetHeatingOn(bool on) => _heatingOn = on;
    public bool IsCoolingOn() => _coolingOn;
    public void SetCoolingOn(bool on) => _coolingOn = on;
}

/// <summary>
/// This is what using the "Ask" approach looks like in practice
/// Notice how scattered and complex the logic becomes when it's outside the objects
/// </summary>
public static class AskBasedControllers
{
    /// <summary>
    /// External logic for monitoring - this is problematic!
    /// We're asking the monitor for all its data, then making decisions
    /// </summary>
    public static void HandleMonitorValue(AskMonitor monitor, int newValue)
    {
        monitor.SetValue(newValue);
        
        // This logic should be INSIDE the monitor, not out here!
        if (monitor.GetValue() > monitor.GetLimit())
        {
            monitor.GetAlarm().Warn($"{monitor.GetName()} too high: {monitor.GetValue()}");
        }
    }

    /// <summary>
    /// External banking logic - way too complex and error-prone
    /// What if we forget to check overdraft? What if the business rules change?
    /// </summary>
    public static bool ProcessWithdrawal(AskBankAccount account, decimal amount)
    {
        // Look at all this external logic that should be in the account!
        var currentBalance = account.GetBalance();
        var overdraftLimit = account.GetOverdraftLimit();
        var availableBalance = currentBalance + overdraftLimit;
        
        if (amount <= 0)
        {
            Console.WriteLine("❌ Invalid withdrawal amount");
            return false;
        }
        
        if (amount > availableBalance)
        {
            Console.WriteLine($"❌ Insufficient funds. Available: ${availableBalance:F2}");
            return false;
        }
        
        // We're manually updating the balance - the account doesn't control its own state!
        account.SetBalance(currentBalance - amount);
        Console.WriteLine($"💰 Withdrew ${amount:F2}. New balance: ${account.GetBalance():F2}");
        return true;
    }

    /// <summary>
    /// External shopping cart logic - complex and fragile
    /// What happens when discount rules change? We have to find all these external controllers!
    /// </summary>
    public static decimal CalculateCartTotal(AskShoppingCart cart)
    {
        var items = cart.GetItems(); // Getting internal data - bad!
        var subtotal = 0m;
        
        // External calculation logic
        foreach (var item in items)
        {
            subtotal += item.GetSubtotal();
        }
        
        // External discount logic
        var discountAmount = subtotal * (cart.GetDiscountPercentage() / 100);
        var total = subtotal - discountAmount;
        
        return total;
    }

    /// <summary>
    /// External security logic - becomes a nightmare to maintain
    /// Access control rules are scattered across multiple controllers
    /// </summary>
    public static bool CheckAccess(AskSecuritySystem system, string username, string password, AccessLevel requiredLevel)
    {
        if (system.IsLocked())
        {
            Console.WriteLine("🔒 System is locked");
            return false;
        }
        
        var users = system.GetUsers(); // Getting internal data again!
        var user = users.FirstOrDefault(u => u.Name == username);
        
        if (user == null)
        {
            system.LogAttempt($"Failed login attempt: Unknown user {username}");
            return false;
        }
        
        // This access logic should be in the security system, not here!
        if (!user.CanAccess(requiredLevel))
        {
            system.LogAttempt($"Access denied: {username} lacks {requiredLevel} clearance");
            return false;
        }
        
        user.RecordLogin();
        system.LogAttempt($"Successful login: {username}");
        return true;
    }

    /// <summary>
    /// External temperature control logic - very fragile
    /// What if the heating/cooling rules change? We have to update this external code!
    /// </summary>
    public static void AdjustTemperature(AskTemperatureController controller)
    {
        var current = controller.GetCurrentTemperature();
        var target = controller.GetTargetTemperature();
        var tolerance = 2.0; // This constant should probably be configurable
        
        // Complex external logic that should be inside the controller
        if (current < target - tolerance)
        {
            if (!controller.IsHeatingOn())
            {
                controller.SetHeatingOn(true);
                controller.SetCoolingOn(false);
                Console.WriteLine($"🔥 Heating turned ON (Current: {current}°F, Target: {target}°F)");
            }
        }
        else if (current > target + tolerance)
        {
            if (!controller.IsCoolingOn())
            {
                controller.SetCoolingOn(true);
                controller.SetHeatingOn(false);
                Console.WriteLine($"❄️ Cooling turned ON (Current: {current}°F, Target: {target}°F)");
            }
        }
        else
        {
            // In the sweet spot - turn off both
            if (controller.IsHeatingOn() || controller.IsCoolingOn())
            {
                controller.SetHeatingOn(false);
                controller.SetCoolingOn(false);
                Console.WriteLine($"✅ Temperature stable (Current: {current}°F, Target: {target}°F)");
            }
        }
    }
}

/*
 * 💀 PROBLEMS WITH THE "ASK" APPROACH:
 * 
 * 1. SCATTERED LOGIC: Business rules are spread across multiple controllers
 * 2. FRAGILE CODE: Changes require updating multiple external classes  
 * 3. POOR ENCAPSULATION: Objects are just data containers with no behavior
 * 4. HARD TO TEST: Logic is mixed with control flow in external methods
 * 5. KNOWLEDGE DUPLICATION: Multiple places need to know the same business rules
 * 6. TIGHT COUPLING: Controllers are tightly coupled to the internal structure of objects
 * 
 * The "Ask" approach treats objects like databases - you query them for data
 * and make decisions somewhere else. This leads to procedural programming
 * disguised as object-oriented programming.
 */

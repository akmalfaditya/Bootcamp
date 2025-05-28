
#nullable disable // Disable nullable warnings for this educational demo


using System.Text;

namespace NullOperatorsDemo
{
    // Sample classes to demonstrate null operators in action
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public UserProfile Profile { get; set; }
        
        public User(string name, string email = null)
        {
            Name = name;
            Email = email;
        }
        
        public string GetDisplayName()
        {
            return Name ?? "Anonymous User";
        }
    }
    
    public class UserProfile
    {
        public string Bio { get; set; }
        public Address Address { get; set; }
        public List<string> Interests { get; set; }
        
        public UserProfile()
        {
            Interests = new List<string>();
        }
    }
    
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        
        public string GetFullAddress()
        {
            return $"{Street}, {City}, {Country}";
        }
    }
    
    // Configuration class for lazy initialization demo
    public class AppConfig
    {
        private static AppConfig _instance;
        public string DatabaseConnection { get; set; }
        public string ApiKey { get; set; }
        
        public static AppConfig Instance
        {
            get
            {
                // This will be demonstrated with ??= operator
                return _instance ??= new AppConfig
                {
                    DatabaseConnection = "DefaultConnection",
                    ApiKey = "DefaultApiKey"
                };
            }
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== NULL OPERATORS IN C# ===");
            Console.WriteLine("Your complete toolkit for handling null values like a pro\n");
            
            // Section 1: Null-Coalescing Operator (??)
            DemonstrateNullCoalescing();
            
            // Section 2: Null-Coalescing Assignment Operator (??=)
            DemonstrateNullCoalescingAssignment();
            
            // Section 3: Null-Conditional Operator (?.)
            DemonstrateNullConditional();
            
            // Section 4: Combining Operators
            DemonstrateCombinedOperators();
            
            // Section 5: Chaining Null-Conditional Operators
            DemonstrateOperatorChaining();
            
            // Section 6: Working with Nullable Value Types
            DemonstrateNullableValueTypes();
            
            Console.WriteLine("\n=== NULL SAFETY MASTERED ===");
            Console.WriteLine("You're now equipped to write null-safe code that won't crash in production");
            Console.WriteLine("Remember: These operators are your first line of defense against NullReferenceException!");
        }
        
        #region Null-Coalescing Operator (??)
        
        static void DemonstrateNullCoalescing()
        {
            Console.WriteLine("=== NULL-COALESCING OPERATOR (??) ===");
            Console.WriteLine("Your fallback plan when things might be null\n");
            
            // Basic null-coalescing examples from the material
            Console.WriteLine("--- Basic Usage ---");
            string s1 = null;
            string s2 = s1 ?? "nothing";
            Console.WriteLine($"s1 = null, s2 = s1 ?? \"nothing\" → s2 = \"{s2}\"");
            
            string s3 = "something";
            string s4 = s3 ?? "nothing";
            Console.WriteLine($"s3 = \"something\", s4 = s3 ?? \"nothing\" → s4 = \"{s4}\"");
            
            // Performance benefit - right side not evaluated if left is not null
            Console.WriteLine("\n--- Performance Optimization ---");
            Console.WriteLine("The ?? operator doesn't evaluate the right side if left side is not null");
            
            string userInput = "ValidData";
            string processedData = userInput ?? GetExpensiveDefault();
            Console.WriteLine($"userInput has value, so GetExpensiveDefault() was NOT called");
            Console.WriteLine($"Result: {processedData}");
            
            string nullInput = null;
            string processedNull = nullInput ?? GetExpensiveDefault();
            Console.WriteLine($"nullInput is null, so GetExpensiveDefault() WAS called");
            Console.WriteLine($"Result: {processedNull}");
            
            // Multiple null-coalescing operators
            Console.WriteLine("\n--- Multiple Fallbacks ---");
            string primary = null;
            string secondary = null;
            string tertiary = "Final fallback";
            
            string result = primary ?? secondary ?? tertiary ?? "Ultimate fallback";
            Console.WriteLine($"primary ?? secondary ?? tertiary ?? \"Ultimate fallback\"");
            Console.WriteLine($"Result: \"{result}\"");
            
            // Working with method return values
            Console.WriteLine("\n--- With Method Returns ---");
            string configValue = GetConfigValue("DatabaseUrl");
            string connectionString = configValue ?? "DefaultConnectionString";
            Console.WriteLine($"Config value: {configValue ?? "null"}");
            Console.WriteLine($"Connection string: {connectionString}");
            
            Console.WriteLine("\nKey point: ?? gives you graceful degradation instead of crashes\n");
        }
        
        // Helper method for demonstration
        static string GetExpensiveDefault()
        {
            Console.WriteLine("  [Expensive operation executed]");
            return "Expensive default value";
        }
        
        // Simulates reading from config file
        static string GetConfigValue(string key)
        {
            // Simulate config not found
            return null;
        }
        
        #endregion
        
        #region Null-Coalescing Assignment Operator (??=)
        
        static void DemonstrateNullCoalescingAssignment()
        {
            Console.WriteLine("=== NULL-COALESCING ASSIGNMENT OPERATOR (??=) ===");
            Console.WriteLine("Assign only when null - perfect for lazy initialization\n");
            
            // Basic null-coalescing assignment
            Console.WriteLine("--- Basic Assignment ---");
            string myVariable = null;
            Console.WriteLine($"myVariable before: {myVariable ?? "null"}");
            
            myVariable ??= "Default Value";
            Console.WriteLine($"After myVariable ??= \"Default Value\": {myVariable}");
            
            // Try to assign again - it won't change because it's not null anymore
            myVariable ??= "Another Value";
            Console.WriteLine($"After myVariable ??= \"Another Value\": {myVariable}");
            Console.WriteLine("Notice: The value didn't change because it was already non-null");
            
            // Lazy initialization pattern
            Console.WriteLine("\n--- Lazy Initialization Pattern ---");
            List<string> expensiveList = null;
            
            Console.WriteLine($"expensiveList before: {(expensiveList == null ? "null" : $"{expensiveList.Count} items")}");
            
            // Initialize only if null
            expensiveList ??= CreateExpensiveList();
            Console.WriteLine($"After first ??= assignment: {expensiveList.Count} items");
            
            // Try again - the expensive method won't be called
            expensiveList ??= CreateExpensiveList();
            Console.WriteLine($"After second ??= assignment: still {expensiveList.Count} items");
            Console.WriteLine("CreateExpensiveList() was only called once!");
            
            // Singleton pattern demonstration
            Console.WriteLine("\n--- Singleton Pattern ---");
            var config1 = AppConfig.Instance;
            var config2 = AppConfig.Instance;
            
            Console.WriteLine($"config1 database: {config1.DatabaseConnection}");
            Console.WriteLine($"config2 database: {config2.DatabaseConnection}");
            Console.WriteLine($"Same instance: {object.ReferenceEquals(config1, config2)}");
            
            // Working with properties
            Console.WriteLine("\n--- Property Initialization ---");
            var user = new User("John");
            user.Profile ??= new UserProfile();
            Console.WriteLine($"User profile created: {user.Profile != null}");
            
            user.Profile ??= new UserProfile(); // Won't create a new one
            Console.WriteLine("Profile creation attempt 2: No new instance created");
            
            Console.WriteLine("\nKey point: ??= prevents unnecessary work and multiple initializations\n");
        }
        
        static List<string> CreateExpensiveList()
        {
            Console.WriteLine("  [Creating expensive list - this should only happen once]");
            return new List<string> { "Item1", "Item2", "Item3" };
        }
        
        #endregion
        
        #region Null-Conditional Operator (?.)
        
        static void DemonstrateNullConditional()
        {
            Console.WriteLine("=== NULL-CONDITIONAL OPERATOR (?.) ===");
            Console.WriteLine("Safe navigation through object hierarchies - no more crashes!\n");
            
            // Basic null-conditional usage from the material
            Console.WriteLine("--- Basic Usage with Methods ---");
            StringBuilder sb = null;
            string s = sb?.ToString();
            Console.WriteLine($"sb is null, sb?.ToString() = {s ?? "null"}");
            Console.WriteLine("No NullReferenceException thrown!");
            
            sb = new StringBuilder("Hello World");
            s = sb?.ToString();
            Console.WriteLine($"sb has value, sb?.ToString() = \"{s}\"");
            
            // Null-conditional with indexers
            Console.WriteLine("\n--- With Array Indexers ---");
            string[] words = null;
            string word = words?[1];
            Console.WriteLine($"words is null, words?[1] = {word ?? "null"}");
            
            words = new string[] { "apple", "banana", "cherry" };
            word = words?[1];
            Console.WriteLine($"words has values, words?[1] = \"{word}\"");
            
            // Null-conditional with properties
            Console.WriteLine("\n--- With Object Properties ---");
            User nullUser = null;
            string userName = nullUser?.Name;
            Console.WriteLine($"nullUser?.Name = {userName ?? "null"}");
            
            User validUser = new User("Alice", "alice@example.com");
            userName = validUser?.Name;
            Console.WriteLine($"validUser?.Name = \"{userName}\"");
            
            // Short-circuiting behavior
            Console.WriteLine("\n--- Short-Circuiting Behavior ---");
            StringBuilder nullBuilder = null;
            string upperResult = nullBuilder?.ToString().ToUpper();
            Console.WriteLine($"nullBuilder?.ToString().ToUpper() = {upperResult ?? "null"}");
            Console.WriteLine("Even though ToUpper() comes after ToString(), no exception occurred!");
            
            StringBuilder validBuilder = new StringBuilder("hello");
            upperResult = validBuilder?.ToString().ToUpper();
            Console.WriteLine($"validBuilder?.ToString().ToUpper() = \"{upperResult}\"");
            
            // Working with methods that return objects
            Console.WriteLine("\n--- With Method Calls ---");
            User userWithoutProfile = new User("Bob");
            string bio = userWithoutProfile.Profile?.Bio;
            Console.WriteLine($"User without profile, bio = {bio ?? "null"}");
            
            User userWithProfile = new User("Charlie");
            userWithProfile.Profile = new UserProfile { Bio = "Software Developer" };
            bio = userWithProfile.Profile?.Bio;
            Console.WriteLine($"User with profile, bio = \"{bio}\"");
            
            Console.WriteLine("\nKey point: ?. lets you navigate object hierarchies safely\n");
        }
        
        #endregion
        
        #region Combining Operators
        
        static void DemonstrateCombinedOperators()
        {
            Console.WriteLine("=== COMBINING NULL OPERATORS ===");
            Console.WriteLine("The power combo - ?. and ?? working together\n");
            
            // Example from the material
            Console.WriteLine("--- Basic Combination ---");
            StringBuilder sb = null;
            string s = sb?.ToString() ?? "nothing";
            Console.WriteLine($"sb?.ToString() ?? \"nothing\" = \"{s}\"");
            
            sb = new StringBuilder("Something");
            s = sb?.ToString() ?? "nothing";
            Console.WriteLine($"Non-null sb?.ToString() ?? \"nothing\" = \"{s}\"");
            
            // Real-world example: user display names
            Console.WriteLine("\n--- User Display Names ---");
            User[] users = {
                null,
                new User(null),
                new User("Alice"),
                new User("Bob", "bob@example.com")
            };
            
            for (int i = 0; i < users.Length; i++)
            {
                string displayName = users[i]?.Name ?? "Anonymous";
                string email = users[i]?.Email ?? "No email provided";
                
                Console.WriteLine($"User {i}: Name = \"{displayName}\", Email = \"{email}\"");
            }
            
            // Complex object navigation with fallbacks
            Console.WriteLine("\n--- Complex Navigation with Fallbacks ---");
            User userWithAddress = new User("David");
            userWithAddress.Profile = new UserProfile();
            userWithAddress.Profile.Address = new Address 
            { 
                City = "New York",
                Country = "USA" 
            };
            
            User userWithoutAddress = new User("Eve");
            userWithoutAddress.Profile = new UserProfile();
            
            User nullUser = null;
            
            User[] testUsers = { userWithAddress, userWithoutAddress, nullUser };
            
            foreach (var user in testUsers)
            {
                string city = user?.Profile?.Address?.City ?? "Unknown City";
                string country = user?.Profile?.Address?.Country ?? "Unknown Country";
                
                Console.WriteLine($"User: {user?.Name ?? "null"}, Location: {city}, {country}");
            }
            
            Console.WriteLine("\nKey point: Combining operators gives you bulletproof navigation\n");
        }
        
        #endregion
        
        #region Operator Chaining
        
        static void DemonstrateOperatorChaining()
        {
            Console.WriteLine("=== CHAINING NULL-CONDITIONAL OPERATORS ===");
            Console.WriteLine("Navigate deep object hierarchies safely\n");
            
            // Deep navigation example
            Console.WriteLine("--- Deep Object Navigation ---");
            
            // Create a user with full profile
            User completeUser = new User("Alice");
            completeUser.Profile = new UserProfile 
            { 
                Bio = "Software Engineer",
                Address = new Address 
                { 
                    Street = "123 Main St",
                    City = "Seattle",
                    Country = "USA"
                }
            };
            completeUser.Profile.Interests.AddRange(new[] { "Coding", "Reading", "Gaming" });
            
            // Create a user with partial profile
            User partialUser = new User("Bob");
            partialUser.Profile = new UserProfile { Bio = "Designer" };
            // No address or interests
            
            // Create a user with no profile
            User minimalUser = new User("Charlie");
            
            User[] testUsers = { completeUser, partialUser, minimalUser, null };
            
            Console.WriteLine("Testing deep navigation on different user scenarios:");
            
            for (int i = 0; i < testUsers.Length; i++)
            {
                var user = testUsers[i];
                
                // Chain multiple null-conditional operators
                string fullAddress = user?.Profile?.Address?.GetFullAddress();
                string city = user?.Profile?.Address?.City;
                int? interestCount = user?.Profile?.Interests?.Count;
                string firstInterest = user?.Profile?.Interests?[0];
                
                Console.WriteLine($"\nUser {i}: {user?.Name ?? "null"}");
                Console.WriteLine($"  Full address: {fullAddress ?? "Not available"}");
                Console.WriteLine($"  City: {city ?? "Not available"}");
                Console.WriteLine($"  Interest count: {interestCount?.ToString() ?? "Not available"}");
                Console.WriteLine($"  First interest: {firstInterest ?? "Not available"}");
            }
            
            // Demonstrating the nullable type requirement
            Console.WriteLine("\n--- Working with Value Types ---");
            User userForLength = new User("TestUser");
            
            // This works because int? can be null
            int? nameLength = userForLength?.Name?.Length;
            Console.WriteLine($"Name length (int?): {nameLength}");
            
            // This would NOT compile:
            // int nameLength = userForLength?.Name?.Length; // Error!
            Console.WriteLine("Note: Result must be nullable when using ?. with value types");
            
            // Method chaining with null-conditional
            Console.WriteLine("\n--- Method Chaining ---");
            StringBuilder builder = new StringBuilder("hello world");
            string result = builder?.ToString()?.ToUpper()?.Replace("WORLD", "UNIVERSE");
            Console.WriteLine($"Method chain result: {result}");
            
            builder = null;
            result = builder?.ToString()?.ToUpper()?.Replace("WORLD", "UNIVERSE");
            Console.WriteLine($"Method chain with null builder: {result ?? "null"}");
            
            Console.WriteLine("\nKey point: Chain as deep as you need - one null anywhere makes the whole expression null\n");
        }
        
        #endregion
        
        #region Nullable Value Types
        
        static void DemonstrateNullableValueTypes()
        {
            Console.WriteLine("=== WORKING WITH NULLABLE VALUE TYPES ===");
            Console.WriteLine("Bringing null safety to numbers, dates, and other value types\n");
            
            // Basic nullable value types with null-coalescing
            Console.WriteLine("--- Nullable Primitives ---");
            int? nullableInt = null;
            double? nullableDouble = null;
            bool? nullableBool = null;
            DateTime? nullableDate = null;
            
            int defaultInt = nullableInt ?? 42;
            double defaultDouble = nullableDouble ?? 3.14;
            bool defaultBool = nullableBool ?? true;
            DateTime defaultDate = nullableDate ?? DateTime.Now;
            
            Console.WriteLine($"int? null → default {defaultInt}");
            Console.WriteLine($"double? null → default {defaultDouble}");
            Console.WriteLine($"bool? null → default {defaultBool}");
            Console.WriteLine($"DateTime? null → default {defaultDate:yyyy-MM-dd}");
            
            // Null-coalescing assignment with nullable value types
            Console.WriteLine("\n--- Nullable Assignment ---");
            int? score = null;
            Console.WriteLine($"Score before: {score?.ToString() ?? "null"}");
            
            score ??= 100;
            Console.WriteLine($"After score ??= 100: {score}");
            
            score ??= 200; // Won't change
            Console.WriteLine($"After score ??= 200: {score}");
            
            // Working with nullable properties
            Console.WriteLine("\n--- Nullable Properties ---");
            var userAges = new Dictionary<string, int?>
            {
                ["Alice"] = 25,
                ["Bob"] = null,
                ["Charlie"] = 30
            };
            
            foreach (var kvp in userAges)
            {
                int displayAge = kvp.Value ?? 0;
                string ageStatus = kvp.Value?.ToString() ?? "Age not provided";
                
                Console.WriteLine($"{kvp.Key}: {ageStatus} (display as: {displayAge})");
            }
            
            // Null-conditional with nullable return types
            Console.WriteLine("\n--- Methods Returning Nullable Types ---");
            string numberString = "123";
            int? parsedNumber = TryParseInt(numberString);
            int finalNumber = parsedNumber ?? -1;
            
            Console.WriteLine($"Parsing '{numberString}': {parsedNumber?.ToString() ?? "failed"} → {finalNumber}");
            
            numberString = "abc";
            parsedNumber = TryParseInt(numberString);
            finalNumber = parsedNumber ?? -1;
            
            Console.WriteLine($"Parsing '{numberString}': {parsedNumber?.ToString() ?? "failed"} → {finalNumber}");
            
            // Real-world scenario: configuration values
            Console.WriteLine("\n--- Configuration Values ---");
            var config = new Dictionary<string, string>
            {
                ["MaxRetries"] = "5",
                ["Timeout"] = "30",
                ["Debug"] = "true"
                // Note: no "Port" setting
            };
              int maxRetries = TryParseInt(config.GetValueOrDefault("MaxRetries")) ?? 3;
            int timeout = TryParseInt(config.GetValueOrDefault("Timeout")) ?? 60;
            int port = TryParseInt(config.GetValueOrDefault("Port")) ?? 8080;
            bool debug = TryParseBool(config.GetValueOrDefault("Debug")) ?? false;
            
            Console.WriteLine($"Max retries: {maxRetries}");
            Console.WriteLine($"Timeout: {timeout}s");
            Console.WriteLine($"Port: {port}");
            Console.WriteLine($"Debug mode: {debug}");
            
            Console.WriteLine("\nKey point: Nullable value types + null operators = robust configuration handling\n");
        }
        
        // Helper method for parsing integers
        static int? TryParseInt(string value)
        {
            if (int.TryParse(value, out int result))
                return result;
            return null;
        }
        
        // Helper method for parsing booleans
        static bool? TryParseBool(string value)
        {
            if (bool.TryParse(value, out bool result))
                return result;
            return null;
        }
        
        #endregion
    }
}

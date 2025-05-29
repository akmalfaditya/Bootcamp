using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PluggingEqualityAndOrder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Plugging in Equality and Order in C# ===");
            Console.WriteLine("Learn how to customize comparison behavior for collections\n");

            // Demonstrate all the key concepts
            BasicEqualityProblemsDemo();
            CustomEqualityComparerDemo();
            EqualityComparerDefaultDemo();
            CustomOrderComparerDemo();
            StringComparerDemo();
            StructuralEqualityDemo();
            RealWorldScenariosDemo();

            Console.WriteLine("\n=== All Equality and Order Demonstrations Complete ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Shows why we need custom equality - default reference equality often isn't what we want
        /// This is the "aha moment" for most developers
        /// </summary>
        static void BasicEqualityProblemsDemo()
        {
            Console.WriteLine("=== 1. The Problem with Default Equality ===");
            Console.WriteLine("Why reference equality doesn't work for business objects\n");

            // Create two customers with identical data
            var customer1 = new Customer("Smith", "John");
            var customer2 = new Customer("Smith", "John");

            Console.WriteLine("Two customers with identical data:");
            Console.WriteLine($"Customer 1: {customer1.LastName}, {customer1.FirstName}");
            Console.WriteLine($"Customer 2: {customer2.LastName}, {customer2.FirstName}");

            // These will be false because they're different objects in memory
            Console.WriteLine($"\nUsing == operator: {customer1 == customer2}");
            Console.WriteLine($"Using .Equals(): {customer1.Equals(customer2)}");
            Console.WriteLine("Both are FALSE because they compare memory addresses, not data!");

            // This causes problems in collections
            var customerDict = new Dictionary<Customer, string>();
            customerDict[customer1] = "VIP Customer";

            Console.WriteLine($"\nDictionary contains customer1: {customerDict.ContainsKey(customer1)}");
            Console.WriteLine($"Dictionary contains customer2: {customerDict.ContainsKey(customer2)}");
            Console.WriteLine("customer2 not found even though data is identical - this is a problem!\n");
        }

        /// <summary>
        /// Shows how to solve the problem with custom IEqualityComparer
        /// This is where the magic happens - we tell collections how to compare our objects
        /// </summary>
        static void CustomEqualityComparerDemo()
        {
            Console.WriteLine("=== 2. Custom IEqualityComparer - The Solution ===");
            Console.WriteLine("Teaching collections how to compare objects by data, not reference\n");

            var customer1 = new Customer("Smith", "John");
            var customer2 = new Customer("Smith", "John");
            var customer3 = new Customer("Johnson", "Mary");

            // Create dictionary with our custom comparer
            var customerComparer = new LastFirstEqualityComparer();
            var customerDict = new Dictionary<Customer, string>(customerComparer);

            customerDict[customer1] = "VIP Customer";
            customerDict[customer3] = "Regular Customer";

            Console.WriteLine("Dictionary using custom equality comparer:");
            Console.WriteLine($"Added customer1: {customer1.LastName}, {customer1.FirstName}");
            Console.WriteLine($"Dictionary size: {customerDict.Count}");

            // Now customer2 should be found because our comparer looks at data, not reference
            Console.WriteLine($"\nLooking for customer2 (same data as customer1):");
            Console.WriteLine($"Dictionary contains customer2: {customerDict.ContainsKey(customer2)}");
            Console.WriteLine($"Value for customer2: {customerDict[customer2]}");
            Console.WriteLine("SUCCESS! Found customer2 because our comparer checks name data");

            // Test with HashSet to see uniqueness behavior
            var customerSet = new HashSet<Customer>(customerComparer);
            customerSet.Add(customer1);
            customerSet.Add(customer2); // Should not add because it's "equal" to customer1
            customerSet.Add(customer3);

            Console.WriteLine($"\nHashSet with custom comparer:");
            Console.WriteLine($"Added 3 customers, but set size is: {customerSet.Count}");
            Console.WriteLine("customer1 and customer2 are treated as duplicates - exactly what we wanted!\n");
        }

        /// <summary>
        /// Demonstrates EqualityComparer<T>.Default and when to use it
        /// This is super useful for generic methods where you don't know the type
        /// </summary>
        static void EqualityComparerDefaultDemo()
        {
            Console.WriteLine("=== 3. EqualityComparer<T>.Default - Generic Equality ===");
            Console.WriteLine("The smart way to compare objects when you don't know the type\n");

            // This method works with any type and uses the best available comparer
            Console.WriteLine("Testing generic equality method:");
            Console.WriteLine($"Comparing ints: {AreEqual(5, 5)}");
            Console.WriteLine($"Comparing strings: {AreEqual("hello", "hello")}");
            Console.WriteLine($"Comparing strings (different case): {AreEqual("Hello", "hello")}");

            // With value types, it avoids boxing
            Console.WriteLine($"Comparing doubles: {AreEqual(3.14, 3.14)}");
            Console.WriteLine($"Comparing booleans: {AreEqual(true, false)}");

            // Works with custom objects that implement IEquatable<T>
            var person1 = new Person("Alice", 25);
            var person2 = new Person("Alice", 25);
            Console.WriteLine($"Comparing Person objects: {AreEqual(person1, person2)}");
            Console.WriteLine("Person class implements IEquatable<T>, so it uses value equality\n");
        }

        // Generic method that uses the best available equality comparer
        static bool AreEqual<T>(T x, T y)
        {
            // EqualityComparer<T>.Default automatically picks the best comparer:
            // 1. If T implements IEquatable<T>, uses that
            // 2. Otherwise, falls back to Object.Equals
            // 3. For value types, avoids boxing
            return EqualityComparer<T>.Default.Equals(x, y);
        }

        /// <summary>
        /// Shows how to implement IComparer for custom sorting
        /// Sorting is all about defining "what comes before what"
        /// </summary>
        static void CustomOrderComparerDemo()
        {
            Console.WriteLine("=== 4. Custom IComparer - Defining Sort Order ===");
            Console.WriteLine("Teaching collections how to sort your objects\n");

            // Create a wish list with different priorities
            var wishList = new List<Wish>
            {
                new Wish("World Peace", 2),
                new Wish("Win Lottery", 3),
                new Wish("Learn C#", 1),
                new Wish("Get Promoted", 2),
                new Wish("Travel the World", 4)
            };

            Console.WriteLine("Original wish list:");
            foreach (var wish in wishList)
            {
                Console.WriteLine($"  {wish.Name} (Priority: {wish.Priority})");
            }

            // Sort by priority using our custom comparer
            wishList.Sort(new PriorityComparer());

            Console.WriteLine("\nSorted by priority (1 = highest priority):");
            foreach (var wish in wishList)
            {
                Console.WriteLine($"  {wish.Name} (Priority: {wish.Priority})");
            }

            // Let's also try sorting by name length (just for fun)
            wishList.Sort(new WishNameLengthComparer());

            Console.WriteLine("\nSorted by name length:");
            foreach (var wish in wishList)
            {
                Console.WriteLine($"  {wish.Name} (Length: {wish.Name.Length})");
            }

            // Demonstrate reverse sorting using Comparer.Create
            wishList.Sort(Comparer<Wish>.Create((x, y) => y.Priority.CompareTo(x.Priority)));

            Console.WriteLine("\nSorted by priority (descending - 4 first):");
            foreach (var wish in wishList)
            {
                Console.WriteLine($"  {wish.Name} (Priority: {wish.Priority})");
            }

            Console.WriteLine("Custom comparers give you complete control over sort order!\n");
        }

        /// <summary>
        /// StringComparer is incredibly useful for real-world string scenarios
        /// Case sensitivity and culture can make or break your app
        /// </summary>
        static void StringComparerDemo()
        {
            Console.WriteLine("=== 5. StringComparer - Mastering String Comparisons ===");
            Console.WriteLine("Different ways to compare strings for different needs\n");

            // Case-sensitive vs case-insensitive dictionaries
            var caseSensitiveDict = new Dictionary<string, int>();
            var caseInsensitiveDict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            caseSensitiveDict["Apple"] = 1;
            caseSensitiveDict["apple"] = 2;

            caseInsensitiveDict["Apple"] = 1;
            caseInsensitiveDict["apple"] = 2; // This overwrites the first entry

            Console.WriteLine("Case-sensitive dictionary:");
            foreach (var kvp in caseSensitiveDict)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine("Case-insensitive dictionary:");
            foreach (var kvp in caseInsensitiveDict)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            // Demonstrate different StringComparer types
            var testStrings = new List<string> { "apple", "Apple", "APPLE", "Äpfel" };

            Console.WriteLine("\nSorting with different StringComparers:");

            // Ordinal (byte-by-byte comparison)
            var ordinalSorted = testStrings.OrderBy(s => s, StringComparer.Ordinal).ToList();
            Console.WriteLine($"Ordinal: [{string.Join(", ", ordinalSorted)}]");

            // OrdinalIgnoreCase
            var ordinalIgnoreCaseSorted = testStrings.OrderBy(s => s, StringComparer.OrdinalIgnoreCase).ToList();
            Console.WriteLine($"OrdinalIgnoreCase: [{string.Join(", ", ordinalIgnoreCaseSorted)}]");

            // CurrentCulture (respects local culture)
            var cultureSorted = testStrings.OrderBy(s => s, StringComparer.CurrentCulture).ToList();
            Console.WriteLine($"CurrentCulture: [{string.Join(", ", cultureSorted)}]");

            // Practical example: User lookup that should be case-insensitive
            var userLookup = new Dictionary<string, UserAccount>(StringComparer.OrdinalIgnoreCase);
            userLookup["john.doe"] = new UserAccount("John Doe", "john.doe@company.com");
            userLookup["jane.smith"] = new UserAccount("Jane Smith", "jane.smith@company.com");

            // User types different cases - should still work
            Console.WriteLine("\nUser lookup (case-insensitive):");
            var foundUser1 = userLookup.TryGetValue("JOHN.DOE", out var user1);
            var foundUser2 = userLookup.TryGetValue("Jane.Smith", out var user2);

            Console.WriteLine($"Found 'JOHN.DOE': {foundUser1} - {user1?.FullName}");
            Console.WriteLine($"Found 'Jane.Smith': {foundUser2} - {user2?.FullName}");
            Console.WriteLine("Case-insensitive lookup is essential for user-friendly applications!\n");
        }

        /// <summary>
        /// IStructuralEquatable and IStructuralComparable for deep comparisons
        /// Perfect when you need to compare arrays or complex structures element by element
        /// </summary>
        static void StructuralEqualityDemo()
        {
            Console.WriteLine("=== 6. Structural Equality - Deep Comparisons ===");
            Console.WriteLine("Comparing arrays and structures element by element\n");

            // Arrays with same content but different references
            int[] array1 = { 1, 2, 3, 4, 5 };
            int[] array2 = { 1, 2, 3, 4, 5 };
            int[] array3 = { 1, 2, 3, 4, 6 };

            // Regular equality compares references
            Console.WriteLine("Regular array equality (reference comparison):");
            Console.WriteLine($"array1 == array2: {array1 == array2}");
            Console.WriteLine($"array1.Equals(array2): {array1.Equals(array2)}");

            // Structural equality compares contents
            Console.WriteLine("\nStructural equality (content comparison):");
            IStructuralEquatable se1 = array1;
            Console.WriteLine($"array1 structurally equals array2: {se1.Equals(array2, EqualityComparer<int>.Default)}");
            Console.WriteLine($"array1 structurally equals array3: {se1.Equals(array3, EqualityComparer<int>.Default)}");

            // Structural comparison for ordering
            IStructuralComparable sc1 = array1;
            IStructuralComparable sc3 = array3;
            Console.WriteLine($"array1 compared to array3: {sc1.CompareTo(array3, Comparer<int>.Default)}");

            // Practical example: Comparing coordinate arrays
            var point1 = new int[] { 10, 20 };
            var point2 = new int[] { 10, 20 };
            var point3 = new int[] { 15, 25 };

            Console.WriteLine("\nCoordinate comparison:");
            Console.WriteLine($"Point1: [{string.Join(", ", point1)}]");
            Console.WriteLine($"Point2: [{string.Join(", ", point2)}]");
            Console.WriteLine($"Point3: [{string.Join(", ", point3)}]");

            IStructuralEquatable p1 = point1;
            Console.WriteLine($"Point1 equals Point2: {p1.Equals(point2, EqualityComparer<int>.Default)}");
            Console.WriteLine($"Point1 equals Point3: {p1.Equals(point3, EqualityComparer<int>.Default)}");

            // Custom structural comparison with tolerance for floating point
            var vector1 = new double[] { 1.0, 2.0, 3.0 };
            var vector2 = new double[] { 1.001, 2.001, 3.001 };

            var toleranceComparer = new ToleranceEqualityComparer(0.01);
            IStructuralEquatable v1 = vector1;
            Console.WriteLine($"\nVector comparison with tolerance:");
            Console.WriteLine($"Vector1: [{string.Join(", ", vector1)}]");
            Console.WriteLine($"Vector2: [{string.Join(", ", vector2)}]");
            Console.WriteLine($"Equal within tolerance: {v1.Equals(vector2, toleranceComparer)}");

            Console.WriteLine("Structural equality is perfect for comparing collections element by element!\n");
        }

        /// <summary>
        /// Real-world scenarios where custom equality and ordering shine
        /// These examples show practical applications you'll actually use
        /// </summary>
        static void RealWorldScenariosDemo()
        {
            Console.WriteLine("=== 7. Real-World Scenarios - Practical Applications ===");
            Console.WriteLine("Where custom equality and ordering solve real problems\n");

            // Scenario 1: Product catalog with case-insensitive SKU lookup
            Console.WriteLine("Scenario 1: Product Catalog");
            var productCatalog = new Dictionary<string, Product>(StringComparer.OrdinalIgnoreCase);
            productCatalog["LAPTOP-001"] = new Product("Gaming Laptop", 1299.99m);
            productCatalog["MOUSE-002"] = new Product("Wireless Mouse", 29.99m);
            productCatalog["KB-003"] = new Product("Mechanical Keyboard", 159.99m);

            // Customer searches with different cases
            Console.WriteLine("Customer searches (case-insensitive):");
            var searches = new[] { "laptop-001", "MOUSE-002", "kb-003" };
            foreach (var search in searches)
            {
                if (productCatalog.TryGetValue(search, out var product))
                {
                    Console.WriteLine($"  Found '{search}': {product.Name} - ${product.Price}");
                }
            }

            // Scenario 2: Employee database with custom sorting
            Console.WriteLine("\nScenario 2: Employee Database");
            var employees = new List<Employee>
            {
                new Employee("Smith", "John", "Engineering", 75000),
                new Employee("Johnson", "Alice", "Engineering", 85000),
                new Employee("Williams", "Bob", "Marketing", 65000),
                new Employee("Brown", "Carol", "Engineering", 90000),
                new Employee("Davis", "Eve", "Sales", 70000)
            };

            // Sort by department, then by salary (descending)
            employees.Sort(new EmployeeComparer());
            Console.WriteLine("Employees sorted by department, then salary (highest first):");
            foreach (var emp in employees)
            {
                Console.WriteLine($"  {emp.Department}: {emp.LastName}, {emp.FirstName} - ${emp.Salary:N0}");
            }

            // Scenario 3: Caching with complex key comparison
            Console.WriteLine("\nScenario 3: Intelligent Caching");
            var cacheComparer = new CacheKeyEqualityComparer();
            var cache = new Dictionary<CacheKey, string>(cacheComparer);

            var key1 = new CacheKey("users", new[] { "id=123", "include=profile" });
            var key2 = new CacheKey("users", new[] { "include=profile", "id=123" }); // Same params, different order
            var key3 = new CacheKey("products", new[] { "category=electronics" });

            cache[key1] = "User data for ID 123";
            cache[key3] = "Electronics products";

            Console.WriteLine("Cache with intelligent key comparison:");
            Console.WriteLine($"Cache contains key1: {cache.ContainsKey(key1)}");
            Console.WriteLine($"Cache contains key2 (same params, different order): {cache.ContainsKey(key2)}");
            Console.WriteLine($"Retrieved with key2: {cache[key2]}");
            Console.WriteLine("Parameter order doesn't matter - cache hit successful!");

            // Scenario 4: Configuration with case-insensitive keys
            Console.WriteLine("\nScenario 4: Configuration Management");
            var config = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            config["DatabaseUrl"] = "server=localhost;database=myapp";
            config["apikey"] = "abc123def456";
            config["TIMEOUT"] = "30";
            config["debug"] = "true";

            Console.WriteLine("Configuration (sorted, case-insensitive):");
            foreach (var setting in config)
            {
                Console.WriteLine($"  {setting.Key}: {setting.Value}");
            }

            Console.WriteLine("\nKey takeaways:");
            Console.WriteLine("✓ Use StringComparer.OrdinalIgnoreCase for user-friendly lookups");
            Console.WriteLine("✓ Custom IEqualityComparer for business object equality");
            Console.WriteLine("✓ Custom IComparer for complex sorting requirements");
            Console.WriteLine("✓ Structural equality for deep array/collection comparisons");
            Console.WriteLine("✓ Consider performance - hash codes must be consistent!");
        }
    }

    #region Customer Example - Basic Equality Problem

    // Simple customer class that demonstrates the reference equality problem
    public class Customer
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public Customer(string lastName, string firstName)
        {
            LastName = lastName;
            FirstName = firstName;
        }
    }    // Custom equality comparer that compares customers by name, not reference
    public class LastFirstEqualityComparer : EqualityComparer<Customer>
    {
        public override bool Equals(Customer? x, Customer? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;
            
            // Compare by actual data, not memory address
            return x.LastName == y.LastName && x.FirstName == y.FirstName;
        }        public override int GetHashCode(Customer? obj)
        {
            if (obj is null) return 0;
            
            // CRITICAL: Hash code must be consistent with Equals!
            // If two objects are equal, they MUST have the same hash code
            return HashCode.Combine(obj.LastName, obj.FirstName);
        }
    }

    #endregion

    #region Person Example - IEquatable Implementation

    // Person class that implements IEquatable<T> for value equality
    public class Person : IEquatable<Person>
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }        // Implementing IEquatable<T> makes EqualityComparer<T>.Default use this
        public bool Equals(Person? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Age == other.Age;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Person);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Age);
        }
    }

    #endregion

    #region Wish Example - Custom Ordering

    // Wish class for demonstrating custom sorting
    public class Wish
    {
        public string Name { get; set; }
        public int Priority { get; set; }

        public Wish(string name, int priority)
        {
            Name = name;
            Priority = priority;
        }
    }    // Comparer that sorts wishes by priority (1 = highest priority)
    public class PriorityComparer : Comparer<Wish>
    {
        public override int Compare(Wish? x, Wish? y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x is null) return -1;
            if (y is null) return 1;

            // Sort by priority first
            int priorityComparison = x.Priority.CompareTo(y.Priority);
            if (priorityComparison != 0) return priorityComparison;

            // If priorities are equal, sort by name
            return string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
        }
    }    // Alternative comparer that sorts by name length
    public class WishNameLengthComparer : Comparer<Wish>
    {
        public override int Compare(Wish? x, Wish? y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x is null) return -1;
            if (y is null) return 1;

            return x.Name.Length.CompareTo(y.Name.Length);
        }
    }

    #endregion

    #region Real-World Examples

    // Product class for catalog example
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }

    // Employee class for sorting example
    public class Employee
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }

        public Employee(string lastName, string firstName, string department, decimal salary)
        {
            LastName = lastName;
            FirstName = firstName;
            Department = department;
            Salary = salary;
        }
    }    // Employee comparer: sort by department, then by salary (descending)
    public class EmployeeComparer : Comparer<Employee>
    {
        public override int Compare(Employee? x, Employee? y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x is null) return -1;
            if (y is null) return 1;

            // First, compare by department
            int deptComparison = string.Compare(x.Department, y.Department, StringComparison.OrdinalIgnoreCase);
            if (deptComparison != 0) return deptComparison;

            // If departments are the same, compare by salary (descending - higher salary first)
            return y.Salary.CompareTo(x.Salary);
        }
    }

    // User account for lookup example
    public class UserAccount
    {
        public string FullName { get; set; }
        public string Email { get; set; }

        public UserAccount(string fullName, string email)
        {
            FullName = fullName;
            Email = email;
        }
    }

    // Cache key with parameter order independence
    public class CacheKey
    {
        public string Resource { get; set; }
        public string[] Parameters { get; set; }

        public CacheKey(string resource, string[] parameters)
        {
            Resource = resource;
            Parameters = parameters;
        }
    }    // Cache key comparer that ignores parameter order
    public class CacheKeyEqualityComparer : EqualityComparer<CacheKey>
    {
        public override bool Equals(CacheKey? x, CacheKey? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;

            if (x.Resource != y.Resource) return false;
            if (x.Parameters.Length != y.Parameters.Length) return false;

            // Sort parameters to ignore order
            var xSorted = x.Parameters.OrderBy(p => p).ToArray();
            var ySorted = y.Parameters.OrderBy(p => p).ToArray();

            return xSorted.SequenceEqual(ySorted);
        }

        public override int GetHashCode(CacheKey? obj)
        {
            if (obj is null) return 0;

            // Hash code must be independent of parameter order
            var sortedParams = obj.Parameters.OrderBy(p => p);
            var combinedParams = string.Join("|", sortedParams);
            return HashCode.Combine(obj.Resource, combinedParams);
        }
    }

    // Tolerance comparer for floating point numbers
    public class ToleranceEqualityComparer : EqualityComparer<double>
    {
        private readonly double tolerance;

        public ToleranceEqualityComparer(double tolerance)
        {
            this.tolerance = tolerance;
        }

        public override bool Equals(double x, double y)
        {
            return Math.Abs(x - y) <= tolerance;
        }

        public override int GetHashCode(double obj)
        {
            // For tolerance-based equality, hash code is tricky
            // We'll round to the tolerance precision
            return Math.Round(obj / tolerance).GetHashCode();
        }
    }

    #endregion
}

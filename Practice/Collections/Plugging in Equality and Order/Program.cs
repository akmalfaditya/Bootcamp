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
            Console.WriteLine("Master custom comparison behavior for collections\n");

            // Demonstrate all the key concepts from the material
            BasicEqualityProblemsDemo();
            CustomEqualityComparerDemo();
            EqualityComparerDefaultDemo();
            ReferenceEqualityComparerDemo();
            CustomOrderComparerDemo();
            StringComparerDemo();
            StructuralEqualityDemo();
            SurnameComparerDemo();
            MutableKeyWarningDemo();
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

            // Basic int array example from the material
            int[] a1 = { 1, 2, 3 };
            int[] a2 = { 1, 2, 3 };

            Console.WriteLine("Regular array equality (reference comparison):");
            Console.WriteLine($"a1.Equals(a2): {a1.Equals(a2)}");  // False - different references

            // Structural equality compares contents
            IStructuralEquatable se1 = a1;
            Console.WriteLine("\nStructural equality (content comparison):");
            Console.WriteLine($"a1 structurally equals a2: {se1.Equals(a2, EqualityComparer<int>.Default)}");  // True

            // String array example with case-insensitive comparison
            string[] sArr1 = "the quick brown fox".Split();
            string[] sArr2 = "THE QUICK BROWN FOX".Split();

            Console.WriteLine("\nCase-insensitive string array comparison:");
            Console.WriteLine($"sArr1: [{string.Join(", ", sArr1)}]");
            Console.WriteLine($"sArr2: [{string.Join(", ", sArr2)}]");

            IStructuralEquatable seArr1 = sArr1;
            bool isCaseInsensitiveEqual = seArr1.Equals(sArr2, StringComparer.InvariantCultureIgnoreCase);
            Console.WriteLine($"Arrays equal (ignore case): {isCaseInsensitiveEqual}");

            // Structural comparison for ordering
            int[] smaller = { 1, 2, 3 };
            int[] larger = { 1, 2, 4 };

            IStructuralComparable sc1 = smaller;
            int comparisonResult = sc1.CompareTo(larger, Comparer<int>.Default);
            Console.WriteLine($"\nStructural comparison result: {comparisonResult}");
            Console.WriteLine($"smaller < larger: {comparisonResult < 0}");

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

            Console.WriteLine("\nKey takeaways:");
            Console.WriteLine("✓ Use IStructuralEquatable for deep element-by-element comparison");
            Console.WriteLine("✓ Perfect for arrays, tuples, and composite data structures");
            Console.WriteLine("✓ You can plug in custom element comparers for specialized behavior\n");
        }

        /// <summary>
        /// Demonstrates ReferenceEqualityComparer from .NET 5+
        /// Sometimes you need reference equality even when objects override Equals
        /// </summary>
        static void ReferenceEqualityComparerDemo()
        {
            Console.WriteLine("=== 3.5. ReferenceEqualityComparer - Force Reference Comparison ===");
            Console.WriteLine("When you need reference equality regardless of Equals overrides\n");

            var person1 = new Person("Alice", 25);
            var person2 = new Person("Alice", 25);
            var person3 = person1; // Same reference

            Console.WriteLine("Person objects with identical data:");
            Console.WriteLine($"person1: {person1.Name}, {person1.Age}");
            Console.WriteLine($"person2: {person2.Name}, {person2.Age}");
            Console.WriteLine($"person3: Same reference as person1");

            // Regular equality (uses Person.Equals implementation)
            Console.WriteLine("\nUsing default equality (Person implements IEquatable<T>):");
            Console.WriteLine($"person1.Equals(person2): {person1.Equals(person2)}");
            Console.WriteLine($"person1.Equals(person3): {person1.Equals(person3)}");

            // Force reference equality using ReferenceEqualityComparer
            var refComparer = ReferenceEqualityComparer.Instance;
            Console.WriteLine("\nUsing ReferenceEqualityComparer (reference only):");
            Console.WriteLine($"person1 == person2 (by reference): {refComparer.Equals(person1, person2)}");
            Console.WriteLine($"person1 == person3 (by reference): {refComparer.Equals(person1, person3)}");

            // Practical use case: tracking object instances in a cache
            var instanceTracker = new HashSet<Person>(ReferenceEqualityComparer.Instance);
            instanceTracker.Add(person1);
            instanceTracker.Add(person2); // Different instance, so it gets added
            instanceTracker.Add(person3); // Same instance as person1, won't be added

            Console.WriteLine($"\nInstance tracker size: {instanceTracker.Count}");
            Console.WriteLine("Only unique object instances are tracked, not unique data values");
            Console.WriteLine("Perfect for tracking object lifetimes or preventing memory leaks\n");
        }

        /// <summary>
        /// Critical warning about mutable keys in hash-based collections
        /// This is one of the most common mistakes that can break your app
        /// </summary>
        static void MutableKeyWarningDemo()
        {
            Console.WriteLine("=== 6.5. Mutable Key Warning - The Danger Zone ===");
            Console.WriteLine("Why changing key properties after adding to collections is dangerous\n");

            var mutableCustomer = new MutableCustomer("Smith", "John");
            var customerDict = new Dictionary<MutableCustomer, string>(new MutableCustomerComparer());

            // Add customer to dictionary
            customerDict[mutableCustomer] = "VIP Customer";
            Console.WriteLine($"Added customer: {mutableCustomer.LastName}, {mutableCustomer.FirstName}");
            Console.WriteLine($"Dictionary contains key: {customerDict.ContainsKey(mutableCustomer)}");

            // DANGER: Modify the key after it's in the collection
            Console.WriteLine("\nWARNING: Modifying key properties after insertion...");
            mutableCustomer.LastName = "Johnson"; // This breaks the hash contract!

            Console.WriteLine($"Modified customer: {mutableCustomer.LastName}, {mutableCustomer.FirstName}");
            
            // The object is now unfindable because its hash changed
            Console.WriteLine($"Dictionary contains key: {customerDict.ContainsKey(mutableCustomer)}");
            Console.WriteLine("Object is LOST in the dictionary - can't find it anymore!");

            // Try to retrieve the value - this will fail
            var found = customerDict.TryGetValue(mutableCustomer, out string? value);
            Console.WriteLine($"Can retrieve value: {found}");

            // The dictionary is now in an inconsistent state
            Console.WriteLine($"Dictionary count: {customerDict.Count}");
            Console.WriteLine("Dictionary still contains the object, but it's unfindable!");

            Console.WriteLine("\nKEY TAKEAWAYS:");
            Console.WriteLine("✗ NEVER modify key properties after adding to hash-based collections");
            Console.WriteLine("✓ Use immutable objects as keys when possible");
            Console.WriteLine("✓ Base hash codes on immutable properties only");
            Console.WriteLine("✓ Consider using readonly structs for simple keys\n");
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

        /// <summary>
        /// Demonstrates the SurnameComparer example from the material
        /// Shows how to normalize string data for consistent comparison
        /// </summary>
        static void SurnameComparerDemo()
        {
            Console.WriteLine("=== 5.5. SurnameComparer - Custom String Normalization ===");
            Console.WriteLine("Normalizing surnames for consistent sorting (Mc = Mac)\n");

            var surnameDict = new SortedDictionary<string, string>(new SurnameComparer());
            
            // Add surnames that should be normalized
            surnameDict.Add("MacPhail", "second!");
            surnameDict.Add("MacWilliam", "third!");
            surnameDict.Add("McDonald", "first!");  // McDonald should sort as MacDonald
            surnameDict.Add("McArthur", "fourth!"); // McArthur should sort as MacArthur

            Console.WriteLine("Surnames added: MacPhail, MacWilliam, McDonald, McArthur");
            Console.WriteLine("Notice how McDonald and McArthur are normalized to Mac- for sorting\n");

            Console.WriteLine("Sorted order (Mc- normalized to Mac-):");
            foreach (var entry in surnameDict)
            {
                Console.WriteLine($"  {entry.Key}: {entry.Value}");
            }

            Console.WriteLine("\nThis demonstrates how custom string normalization");
            Console.WriteLine("can handle real-world data inconsistencies in surnames\n");
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

    #region Surname Example - Custom String Comparer

    // SurnameComparer from the material - normalizes Mc to Mac for consistent sorting
    public class SurnameComparer : Comparer<string>
    {
        string Normalize(string? s)
        {
            if (s == null) return "";
            
            s = s.Trim().ToUpper();
            // Normalize "Mc" to "Mac" for consistent sorting
            if (s.StartsWith("MC")) 
                s = "MAC" + s.Substring(2);
            
            return s;
        }

        public override int Compare(string? x, string? y)
        {
            return Normalize(x).CompareTo(Normalize(y));
        }
    }

    #endregion

    #region Mutable Customer Example - The Danger of Mutable Keys

    // Example of a mutable customer class - demonstrates the danger of mutable keys
    public class MutableCustomer
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public MutableCustomer(string lastName, string firstName)
        {
            LastName = lastName;
            FirstName = firstName;
        }
    }    // Comparer for mutable customer - shows why mutable keys are dangerous
    public class MutableCustomerComparer : EqualityComparer<MutableCustomer>
    {
        public override bool Equals(MutableCustomer? x, MutableCustomer? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;
            
            return x.LastName == y.LastName && x.FirstName == y.FirstName;
        }

        public override int GetHashCode(MutableCustomer? obj)
        {
            if (obj is null) return 0;
            
            // This hash code depends on mutable properties - DANGEROUS!
            return HashCode.Combine(obj.LastName, obj.FirstName);
        }
    }

    #endregion
}

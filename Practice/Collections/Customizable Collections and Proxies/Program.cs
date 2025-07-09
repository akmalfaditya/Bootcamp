using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CustomizableCollections
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Customizable Collections and Proxies Demo ===");
            Console.WriteLine("Understanding how to extend .NET collections with custom behavior\n");
            Console.WriteLine("The .NET BCL provides customizable collection classes in System.Collections.ObjectModel");
            Console.WriteLine("These act as proxies/wrappers that forward calls to underlying collections");
            Console.WriteLine("Key benefit: Virtual methods provide 'gateways' for customization\n");

            // Run demonstrations for each collection type
            CollectionTDemo();
            KeyedCollectionDemo();
            KeyedCollectionAdvancedDemo();
            CollectionBaseDemo();
            DictionaryBaseDemo();
            ReadOnlyCollectionDemo();
            ProxyBehaviorDemo();
            
            Console.WriteLine("\n=== All Demonstrations Complete ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        // Demonstrates Collection<T> with the Zoo/Animal example
        static void CollectionTDemo()
        {
            Console.WriteLine("=== 1. Collection<T> - Customizable Generic Collection ===");
            Console.WriteLine("Shows how to hook into collection operations for custom behavior\n");
            Console.WriteLine("Collection<T> is a wrapper for List<T> that provides virtual methods:");
            Console.WriteLine("- InsertItem(int index, T item) - called by Add, Insert");
            Console.WriteLine("- SetItem(int index, T item) - called by indexer assignment");
            Console.WriteLine("- RemoveItem(int index) - called by Remove, RemoveAt");
            Console.WriteLine("- ClearItems() - called by Clear");
            Console.WriteLine("These are your 'gateways' for custom logic!\n");

            var myZoo = new Zoo();
            
            // Add some animals - watch how Zoo property gets set automatically
            Console.WriteLine("Adding animals to the zoo:");
            myZoo.Animals.Add(new Animal("Kangaroo", 85));
            myZoo.Animals.Add(new Animal("Elephant", 92));
            myZoo.Animals.Add(new Animal("Lion", 95));
            
            Console.WriteLine($"Zoo now has {myZoo.Animals.Count} animals");
            
            // Let's see the animals and their zoo assignment
            Console.WriteLine("\nAnimals in the zoo:");
            foreach (var animal in myZoo.Animals)
            {
                Console.WriteLine($"  {animal.Name} (Popularity: {animal.Popularity}) - Zoo: {(animal.Zoo != null ? "Assigned" : "No Zoo")}");
            }
            
            // Update an animal (replace at index) - this calls SetItem
            Console.WriteLine("\nReplacing the second animal with a Tiger:");
            myZoo.Animals[1] = new Animal("Tiger", 88);
            
            // Remove an animal - this calls RemoveItem
            Console.WriteLine("Removing the first animal:");
            myZoo.Animals.RemoveAt(0);
            
            Console.WriteLine("\nRemaining animals:");
            foreach (var animal in myZoo.Animals)
            {
                Console.WriteLine($"  {animal.Name} - Zoo: {(animal.Zoo != null ? "Still in zoo" : "Removed from zoo")}");
            }
            
            Console.WriteLine("\nNotice how our custom logic runs on EVERY add/remove/update operation!");
            Console.WriteLine("This is the power of Collection<T> - you can intercept and customize all operations!\n");
        }

        // Demonstrates KeyedCollection<TKey, TItem> for key-based access
        static void KeyedCollectionDemo()
        {
            Console.WriteLine("=== 2. KeyedCollection<TKey, TItem> - Dictionary-like Collection ===");
            Console.WriteLine("Combines list functionality with key-based lookups\n");
            Console.WriteLine("KeyedCollection is like Collection<T> but with a twist:");
            Console.WriteLine("- Items can be accessed by INDEX (like a list)");
            Console.WriteLine("- Items can be accessed by KEY (like a dictionary)");
            Console.WriteLine("- The key is extracted FROM the item itself\n");

            var library = new Library();
            
            // Add some books
            Console.WriteLine("Adding books to the library:");
            library.Books.Add(new Book("978-0134685991", "Effective Java", "Joshua Bloch"));
            library.Books.Add(new Book("978-0135166307", "Clean Code", "Robert Martin"));
            library.Books.Add(new Book("978-0201633610", "Design Patterns", "Gang of Four"));
            
            Console.WriteLine($"Library now has {library.Books.Count} books");
            
            // Access by key (ISBN) - this is the dictionary-like behavior
            Console.WriteLine("\nAccessing books by ISBN (key-based access):");
            var cleanCodeBook = library.Books["978-0135166307"];
            Console.WriteLine($"Found: {cleanCodeBook.Title} by {cleanCodeBook.Author}");
            
            // Access by index (still works like a list)
            Console.WriteLine("\nAccessing books by index (list-like behavior):");
            for (int i = 0; i < library.Books.Count; i++)
            {
                var book = library.Books[i];
                Console.WriteLine($"  [{i}] {book.Title} (ISBN: {book.ISBN})");
            }
            
            // Try to access non-existent book
            Console.WriteLine("\nTrying to find a book that doesn't exist:");
            try
            {
                var notFound = library.Books["123-NOT-EXIST"];
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Book not found - KeyedCollection threw KeyNotFoundException as expected");
            }
            
            // Show the internal dictionary behavior
            Console.WriteLine("\nKeyedCollection creates an internal dictionary for fast lookups");
            Console.WriteLine("when the first item is added (or based on creationThreshold)");
            Console.WriteLine("This gives you O(1) key-based access while maintaining list ordering!");
            
            Console.WriteLine("\nKeyed collections are perfect when you need both indexed and key-based access!\n");
        }

        // Demonstrates advanced KeyedCollection features like key changes
        static void KeyedCollectionAdvancedDemo()
        {
            Console.WriteLine("=== 2b. KeyedCollection<TKey, TItem> - Advanced Features ===");
            Console.WriteLine("Shows key change notifications and the underlying dictionary\n");

            var keyedZoo = new KeyedZoo();
            
            Console.WriteLine("Adding animals to keyed zoo:");
            keyedZoo.Animals.Add(new Animal("Kangaroo", 85));
            keyedZoo.Animals.Add(new Animal("Elephant", 92));
            keyedZoo.Animals.Add(new Animal("Lion", 95));
            
            // Access by key (animal name)
            Console.WriteLine("\nAccessing animals by name:");
            var kangaroo = keyedZoo.Animals["Kangaroo"];
            Console.WriteLine($"Found: {kangaroo.Name} with popularity {kangaroo.Popularity}");
            
            // Access by index (still works)
            Console.WriteLine($"First animal by index: {keyedZoo.Animals[0].Name}");
            
            // Demonstrate key change - this is the tricky part!
            Console.WriteLine("\nChanging Kangaroo's name to 'Mr Roo':");
            kangaroo.Name = "Mr Roo";  // This triggers the key change notification
            
            Console.WriteLine("Trying to access by old name (should fail):");
            try
            {
                var notFound = keyedZoo.Animals["Kangaroo"];
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("  Kangaroo not found - key was properly updated!");
            }
            
            Console.WriteLine("Accessing by new name:");
            var mrRoo = keyedZoo.Animals["Mr Roo"];
            Console.WriteLine($"  Found: {mrRoo.Name} - key change worked correctly!");
            
            Console.WriteLine("KeyedCollection gives you the best of both worlds - indexed AND keyed access!\n");
        }

        // Demonstrates the legacy CollectionBase approach
        static void CollectionBaseDemo()
        {
            Console.WriteLine("=== 3. CollectionBase - Legacy Non-Generic Approach ===");
            Console.WriteLine("Shows the old way of customizing collections (avoid in new code)\n");
            Console.WriteLine("CollectionBase problems compared to Collection<T>:");
            Console.WriteLine("- Non-generic (no type safety, boxing/unboxing)");
            Console.WriteLine("- Must implement your own typed methods (Add, indexer, etc.)");
            Console.WriteLine("- Uses pairs of hook methods (OnInsert + OnInsertComplete)");
            Console.WriteLine("- More verbose and error-prone");
            Console.WriteLine("- Exists mainly for backward compatibility\n");

            var taskList = new TaskCollection();
            
            Console.WriteLine("Adding tasks to collection:");
            taskList.Add(new Task("Review code", Priority.High));
            taskList.Add(new Task("Write documentation", Priority.Medium));
            taskList.Add(new Task("Clean desk", Priority.Low));
            
            Console.WriteLine($"Task list has {taskList.Count} tasks");
            
            Console.WriteLine("\nTasks in collection:");
            foreach (Task task in taskList)
            {
                Console.WriteLine($"  {task.Description} - Priority: {task.Priority}");
            }
            
            // Access by index
            Console.WriteLine($"\nFirst task: {taskList[0].Description}");
            
            // Remove a task
            Console.WriteLine("Removing middle task...");
            taskList.RemoveAt(1);
            
            Console.WriteLine($"Now we have {taskList.Count} tasks remaining");
            
            Console.WriteLine("\nWhy avoid CollectionBase?");
            Console.WriteLine("- Collection<T> is type-safe and easier to use");
            Console.WriteLine("- Less code to write and maintain");
            Console.WriteLine("- Better performance (no boxing)");
            Console.WriteLine("- Modern C# features support");
            Console.WriteLine("Use Collection<T> for new projects!\n");
        }

        // Demonstrates ReadOnlyCollection<T>
        static void ReadOnlyCollectionDemo()
        {
            Console.WriteLine("=== 5. ReadOnlyCollection<T> - Controlled Access Wrapper ===");
            Console.WriteLine("Provides read-only access while allowing internal modifications\n");
            Console.WriteLine("ReadOnlyCollection<T> is a simple but powerful wrapper that:");
            Console.WriteLine("- Provides a read-only VIEW of any IList<T>");
            Console.WriteLine("- Does NOT create a copy - it's a live view");
            Console.WriteLine("- Throws NotSupportedException for any modification attempts");
            Console.WriteLine("- Is more robust than just exposing IReadOnlyList<T> (can't be downcast)\n");

            var gameManager = new GameManager();
            
            // Get read-only access to scores
            var readOnlyScores = gameManager.Scores;
            Console.WriteLine($"Game has {readOnlyScores.Count} scores");
            
            // Try to modify - this won't compile
            // readOnlyScores.Add(100); // Compiler error!
            
            // Even if you try to cast it, it will throw at runtime
            Console.WriteLine("Attempting to bypass read-only protection by casting:");
            try
            {
                ((IList<int>)readOnlyScores).Add(999);
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("  NotSupportedException - ReadOnlyCollection is truly read-only!");
            }
            
            Console.WriteLine("\nCurrent scores:");
            foreach (var score in readOnlyScores)
            {
                Console.WriteLine($"  {score}");
            }
            
            // Game manager can still add scores internally
            Console.WriteLine("\nGame events happening (internal modifications allowed)...");
            gameManager.PlayerScored(250);
            gameManager.PlayerScored(180);
            gameManager.PlayerScored(320);
            
            Console.WriteLine($"Now game has {readOnlyScores.Count} scores (live view updated):");
            foreach (var score in readOnlyScores)
            {
                Console.WriteLine($"  {score}");
            }
            
            Console.WriteLine("\nReadOnlyCollection provides bulletproof encapsulation for your internal collections!");
            Console.WriteLine("External code can read but never modify - perfect for public APIs!\n");
        }

        // Demonstrates the legacy DictionaryBase approach
        static void DictionaryBaseDemo()
        {
            Console.WriteLine("=== 4. DictionaryBase - Legacy Dictionary Customization ===");
            Console.WriteLine("Shows the old way of customizing dictionaries (avoid in new code)\n");

            var phoneBook = new PhoneBook();
            
            Console.WriteLine("Adding contacts to phone book:");
            phoneBook.Add("John", "555-1234");
            phoneBook.Add("Jane", "555-5678");
            phoneBook.Add("Bob", "555-9999");
            
            Console.WriteLine($"Phone book has {phoneBook.Count} contacts");
            
            Console.WriteLine("\nContacts in phone book:");
            foreach (DictionaryEntry entry in phoneBook)
            {
                Console.WriteLine($"  {entry.Key}: {entry.Value}");
            }
            
            // Access by key
            Console.WriteLine($"\nJohn's number: {phoneBook["John"]}");
            
            // Remove a contact
            Console.WriteLine("Removing Bob...");
            phoneBook.Remove("Bob");
            
            Console.WriteLine($"Now we have {phoneBook.Count} contacts remaining");
            
            Console.WriteLine("DictionaryBase is legacy - use modern alternatives for new projects!\n");
        }

        // Demonstrates how Collection<T> acts as a proxy
        static void ProxyBehaviorDemo()
        {
            Console.WriteLine("=== 6. Proxy Behavior - Collection<T> as a Wrapper ===");
            Console.WriteLine("Shows how Collection<T> can wrap an existing list\n");

            // Start with an existing list
            var existingList = new List<string> { "Apple", "Banana", "Cherry" };
            Console.WriteLine("Original list contents:");
            foreach (var item in existingList)
            {
                Console.WriteLine($"  {item}");
            }
            
            // Wrap it with Collection<T> - this creates a proxy, not a copy!
            var wrappedCollection = new Collection<string>(existingList);
            Console.WriteLine($"\nWrapped collection count: {wrappedCollection.Count}");
            
            // Changes through the wrapper affect the original list
            Console.WriteLine("Adding 'Date' through the wrapper:");
            wrappedCollection.Add("Date");
            
            Console.WriteLine("Original list now contains:");
            foreach (var item in existingList)
            {
                Console.WriteLine($"  {item}");
            }
            
            // Changes to the original list are visible through the wrapper
            Console.WriteLine("\nAdding 'Elderberry' directly to original list:");
            existingList.Add("Elderberry");
            
            Console.WriteLine("Wrapped collection now contains:");
            foreach (var item in wrappedCollection)
            {
                Console.WriteLine($"  {item}");
            }
            
            Console.WriteLine("Collection<T> maintains a live reference - it's a true proxy!\n");
            Console.WriteLine("However, direct changes to the underlying list bypass the virtual methods,");
            Console.WriteLine("so you lose the customization hooks when not going through the wrapper.\n");
        }
    }

    #region Animal/Zoo Example - Collection<T> Implementation
    
    // Basic entity that belongs to a collection
    // This demonstrates how entities can work with customizable collections
    public class Animal
    {
        private string name;
        public string Name 
        { 
            get { return name; }
            set 
            {
                // If this animal is already in a keyed zoo and we're changing its name,
                // we need to notify the keyed collection
                if (Zoo is KeyedZoo keyedZoo)
                {
                    keyedZoo.Animals.NotifyNameChange(this, value);
                }
                name = value;
            }
        }
        public int Popularity { get; set; }
        public object? Zoo { get; internal set; }  // Can be Zoo or KeyedZoo

        public Animal(string name, int popularity)
        {
            this.name = name;
            Popularity = popularity;
        }
    }

    // Custom collection that extends Collection<T>
    // This is where the magic happens - we can hook into every operation
    public class AnimalCollection : Collection<Animal>
    {
        private readonly Zoo zoo;

        public AnimalCollection(Zoo zoo)
        {
            this.zoo = zoo;
        }

        // Called whenever an item is inserted (Add, Insert)
        // This is your gateway to customize insertion behavior
        protected override void InsertItem(int index, Animal item)
        {
            Console.WriteLine($"  -> Adding {item.Name} to the zoo");
            base.InsertItem(index, item);  // Always call base first for Add operations
            item.Zoo = zoo;  // Custom logic: automatically assign the zoo
        }

        // Called whenever an item is updated via indexer
        // This handles zoo.Animals[0] = newAnimal scenarios
        protected override void SetItem(int index, Animal item)
        {
            Console.WriteLine($"  -> Replacing animal at position {index} with {item.Name}");
            base.SetItem(index, item);
            item.Zoo = zoo;  // Make sure new animal knows its zoo
        }

        // Called whenever an item is removed
        // This handles RemoveAt, Remove, and Clear operations
        protected override void RemoveItem(int index)
        {
            var animal = this[index];
            Console.WriteLine($"  -> Removing {animal.Name} from the zoo");
            animal.Zoo = null;  // Custom logic: clear the zoo reference first
            base.RemoveItem(index);  // Then call base to actually remove
        }

        // Called when Clear() is invoked
        // This is your chance to clean up before clearing everything
        protected override void ClearItems()
        {
            Console.WriteLine("  -> Clearing all animals from the zoo");
            foreach (var animal in this)
            {
                animal.Zoo = null;  // Clean up all references
            }
            base.ClearItems();
        }
    }

    // Keyed version of the animal collection that allows lookup by name
    public class AnimalKeyedCollection : KeyedCollection<string, Animal>
    {
        private readonly KeyedZoo zoo;

        public AnimalKeyedCollection(KeyedZoo zoo)
        {
            this.zoo = zoo;
        }

        // This method tells the collection how to extract the key from each item
        protected override string GetKeyForItem(Animal item)
        {
            return item.Name;  // Use animal name as the key
        }

        // Important: Call this when an item's key changes after it's been added
        internal void NotifyNameChange(Animal animal, string newName)
        {
            this.ChangeItemKey(animal, newName);
        }

        // We can still override the standard Collection<T> methods
        protected override void InsertItem(int index, Animal item)
        {
            Console.WriteLine($"  -> Adding {item.Name} to the keyed zoo collection");
            base.InsertItem(index, item);
            item.Zoo = zoo;  // Custom logic: set the zoo reference
        }

        protected override void SetItem(int index, Animal item)
        {
            Console.WriteLine($"  -> Replacing animal at position {index} with {item.Name}");
            base.SetItem(index, item);
            item.Zoo = zoo;
        }

        protected override void RemoveItem(int index)
        {
            var animal = this[index];
            Console.WriteLine($"  -> Removing {animal.Name} from the keyed zoo collection");
            animal.Zoo = null;
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            Console.WriteLine("  -> Clearing all animals from the keyed zoo collection");
            foreach (var animal in this)
            {
                animal.Zoo = null;
            }
            base.ClearItems();
        }
    }

    // Container class that uses our custom collection
    public class Zoo
    {
        public readonly AnimalCollection Animals;

        public Zoo()
        {
            Animals = new AnimalCollection(this);
        }
    }

    // Zoo variant that uses keyed collection for name-based lookups
    public class KeyedZoo
    {
        public readonly AnimalKeyedCollection Animals;

        public KeyedZoo()
        {
            Animals = new AnimalKeyedCollection(this);
        }
    }

    #endregion

    #region Book/Library Example - KeyedCollection<TKey, TItem> Implementation

    // Entity with a natural key
    public class Book
    {
        public string ISBN { get; set; }  // This will be our key
        public string Title { get; set; }
        public string Author { get; set; }

        public Book(string isbn, string title, string author)
        {
            ISBN = isbn;
            Title = title;
            Author = author;
        }
    }

    // KeyedCollection allows both indexed and key-based access
    // This is more powerful than a regular Dictionary because you get list semantics too
    public class BookCollection : KeyedCollection<string, Book>
    {
        // This method tells the collection how to extract the key from each item
        // This is the abstract method you MUST implement
        protected override string GetKeyForItem(Book item)
        {
            return item.ISBN;  // Use ISBN as the key
        }

        // We can still override the standard Collection<T> methods if needed
        protected override void InsertItem(int index, Book item)
        {
            Console.WriteLine($"  -> Adding book: {item.Title}");
            base.InsertItem(index, item);
            
            // The Dictionary property gives us access to the internal dictionary
            // It's created on-demand when the first item is added
            if (this.Dictionary != null)
            {
                Console.WriteLine($"     Internal dictionary now has {this.Dictionary.Count} entries");
            }
        }

        protected override void RemoveItem(int index)
        {
            var book = this[index];
            Console.WriteLine($"  -> Removing book: {book.Title}");
            base.RemoveItem(index);
        }

        // Method to demonstrate the Dictionary property
        public void ShowInternalDictionary()
        {
            if (this.Dictionary != null)
            {
                Console.WriteLine($"Internal dictionary contains {this.Dictionary.Count} key-value pairs:");
                foreach (var kvp in this.Dictionary)
                {
                    Console.WriteLine($"  Key: {kvp.Key} -> Title: {kvp.Value.Title}");
                }
            }
            else
            {
                Console.WriteLine("Internal dictionary hasn't been created yet (empty collection)");
            }
        }
    }

    public class Library
    {
        public readonly BookCollection Books;

        public Library()
        {
            Books = new BookCollection();
        }
    }

    #endregion

    #region Task Example - CollectionBase Implementation (Legacy)

    public enum Priority
    {
        Low,
        Medium,
        High
    }

    public class Task
    {
        public string Description { get; set; }
        public Priority Priority { get; set; }

        public Task(string description, Priority priority)
        {
            Description = description;
            Priority = priority;
        }
    }

    // CollectionBase is the old non-generic way
    // You have to implement your own typed interface
    public class TaskCollection : CollectionBase
    {
        // Strongly typed Add method
        public int Add(Task task)
        {
            return List.Add(task);
        }

        // Strongly typed indexer
        public Task this[int index]
        {
            get { return (Task)List[index]!; }
            set { List[index] = value; }
        }

        // Hook methods that get called during operations
        protected override void OnInsert(int index, object? value)
        {
            if (value is Task task)
            {
                Console.WriteLine($"  -> Adding task: {task.Description}");
            }
            base.OnInsert(index, value);
        }

        protected override void OnRemove(int index, object? value)
        {
            if (value is Task task)
            {
                Console.WriteLine($"  -> Removing task: {task.Description}");
            }
            base.OnRemove(index, value);
        }

        // Type checking - good practice with non-generic collections
        protected override void OnValidate(object value)
        {
            if (value is not Task)
            {
                throw new ArgumentException("Value must be a Task", nameof(value));
            }
            base.OnValidate(value);
        }
    }

    #endregion

    #region Game Manager Example - ReadOnlyCollection<T> Implementation

    // Class that exposes read-only access to internal data
    public class GameManager
    {
        private readonly List<int> scores;  // Internal mutable list
        public ReadOnlyCollection<int> Scores { get; private set; }  // Read-only wrapper

        public GameManager()
        {
            scores = new List<int> { 100, 150, 200 };  // Start with some scores
            Scores = new ReadOnlyCollection<int>(scores);  // Wrap it
        }

        // Internal methods can still modify the underlying collection
        public void PlayerScored(int score)
        {
            Console.WriteLine($"  -> Player scored: {score}");
            scores.Add(score);  // Modify the underlying list
            // The ReadOnlyCollection automatically reflects these changes
        }

        public void ResetScores()
        {
            Console.WriteLine("  -> Resetting all scores");
            scores.Clear();
        }
    }

    #endregion

    #region PhoneBook Example - DictionaryBase Implementation (Legacy)

    // DictionaryBase is the old non-generic way to customize dictionaries
    // Like CollectionBase, it requires implementing your own typed interface
    public class PhoneBook : DictionaryBase
    {
        // Strongly typed Add method
        public void Add(string name, string phoneNumber)
        {
            Dictionary.Add(name, phoneNumber);
        }

        // Strongly typed Remove method
        public void Remove(string name)
        {
            Dictionary.Remove(name);
        }

        // Strongly typed indexer
        public string this[string name]
        {
            get { return (string)Dictionary[name]!; }
            set { Dictionary[name] = value; }
        }

        // Hook methods that get called during operations
        protected override void OnInsert(object key, object? value)
        {
            if (key is string name && value is string phone)
            {
                Console.WriteLine($"  -> Adding contact: {name} -> {phone}");
            }
            base.OnInsert(key, value);
        }

        protected override void OnRemove(object key, object? value)
        {
            if (key is string name)
            {
                Console.WriteLine($"  -> Removing contact: {name}");
            }
            base.OnRemove(key, value);
        }

        protected override void OnSet(object key, object? oldValue, object? newValue)
        {
            if (key is string name && newValue is string newPhone)
            {
                Console.WriteLine($"  -> Updating {name}'s number to {newPhone}");
            }
            base.OnSet(key, oldValue, newValue);
        }

        // Type validation - important for non-generic collections
        protected override void OnValidate(object key, object? value)
        {
            if (key is not string)
            {
                throw new ArgumentException("Key must be a string", nameof(key));
            }
            if (value is not string)
            {
                throw new ArgumentException("Value must be a string", nameof(value));
            }
            base.OnValidate(key, value);
        }
    }

    #endregion
}

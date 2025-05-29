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

            // Run demonstrations for each collection type
            CollectionTDemo();
            KeyedCollectionDemo();
            CollectionBaseDemo();
            ReadOnlyCollectionDemo();
            
            Console.WriteLine("\n=== All Demonstrations Complete ===");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        // Demonstrates Collection<T> with the Zoo/Animal example
        static void CollectionTDemo()
        {
            Console.WriteLine("=== 1. Collection<T> - Customizable Generic Collection ===");
            Console.WriteLine("Shows how to hook into collection operations for custom behavior\n");

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
            
            // Update an animal (replace at index)
            Console.WriteLine("\nReplacing the second animal with a Tiger:");
            myZoo.Animals[1] = new Animal("Tiger", 88);
            
            // Remove an animal
            Console.WriteLine("Removing the first animal:");
            myZoo.Animals.RemoveAt(0);
            
            Console.WriteLine("\nRemaining animals:");
            foreach (var animal in myZoo.Animals)
            {
                Console.WriteLine($"  {animal.Name} - Zoo: {(animal.Zoo != null ? "Still in zoo" : "Removed from zoo")}");
            }
            
            Console.WriteLine("Notice how custom logic runs on every add/remove/update operation!\n");
        }

        // Demonstrates KeyedCollection<TKey, TItem> for key-based access
        static void KeyedCollectionDemo()
        {
            Console.WriteLine("=== 2. KeyedCollection<TKey, TItem> - Dictionary-like Collection ===");
            Console.WriteLine("Combines list functionality with key-based lookups\n");

            var library = new Library();
            
            // Add some books
            Console.WriteLine("Adding books to the library:");
            library.Books.Add(new Book("978-0134685991", "Effective Java", "Joshua Bloch"));
            library.Books.Add(new Book("978-0135166307", "Clean Code", "Robert Martin"));
            library.Books.Add(new Book("978-0201633610", "Design Patterns", "Gang of Four"));
            
            Console.WriteLine($"Library now has {library.Books.Count} books");
            
            // Access by key (ISBN)
            Console.WriteLine("\nAccessing books by ISBN:");
            var cleanCodeBook = library.Books["978-0135166307"];
            Console.WriteLine($"Found: {cleanCodeBook.Title} by {cleanCodeBook.Author}");
            
            // Access by index (still works like a list)
            Console.WriteLine("\nAccessing books by index:");
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
                Console.WriteLine("Book not found - KeyedCollection threw exception as expected");
            }
            
            // Show how the underlying dictionary works
            Console.WriteLine("\nKeyed collections are perfect when you need both indexed and key-based access!\n");
        }

        // Demonstrates the legacy CollectionBase approach
        static void CollectionBaseDemo()
        {
            Console.WriteLine("=== 3. CollectionBase - Legacy Non-Generic Approach ===");
            Console.WriteLine("Shows the old way of customizing collections (avoid in new code)\n");

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
            
            Console.WriteLine("CollectionBase is legacy - use Collection<T> for new projects!\n");
        }

        // Demonstrates ReadOnlyCollection<T>
        static void ReadOnlyCollectionDemo()
        {
            Console.WriteLine("=== 4. ReadOnlyCollection<T> - Controlled Access Wrapper ===");
            Console.WriteLine("Provides read-only access while allowing internal modifications\n");

            var gameManager = new GameManager();
            
            // Get read-only access to scores
            var readOnlyScores = gameManager.Scores;
            Console.WriteLine($"Game has {readOnlyScores.Count} scores");
            
            // Try to modify - this won't compile
            // readOnlyScores.Add(100); // Compiler error!
            
            Console.WriteLine("Current scores:");
            foreach (var score in readOnlyScores)
            {
                Console.WriteLine($"  {score}");
            }
            
            // Game manager can still add scores internally
            Console.WriteLine("\nGame events happening...");
            gameManager.PlayerScored(250);
            gameManager.PlayerScored(180);
            gameManager.PlayerScored(320);
            
            Console.WriteLine($"Now game has {readOnlyScores.Count} scores:");
            foreach (var score in readOnlyScores)
            {
                Console.WriteLine($"  {score}");
            }
            
            Console.WriteLine("ReadOnlyCollection is perfect for exposing data without allowing external modifications!\n");
        }
    }

    #region Animal/Zoo Example - Collection<T> Implementation
    
    // Basic entity that belongs to a collection
    public class Animal
    {
        public string Name { get; set; }
        public int Popularity { get; set; }
        public Zoo? Zoo { get; internal set; }  // Only the collection should set this

        public Animal(string name, int popularity)
        {
            Name = name;
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
        protected override void InsertItem(int index, Animal item)
        {
            Console.WriteLine($"  -> Adding {item.Name} to the zoo");
            base.InsertItem(index, item);
            item.Zoo = zoo;  // Automatically assign the zoo
        }

        // Called whenever an item is updated via indexer
        protected override void SetItem(int index, Animal item)
        {
            Console.WriteLine($"  -> Replacing animal at position {index} with {item.Name}");
            base.SetItem(index, item);
            item.Zoo = zoo;  // Make sure new animal knows its zoo
        }

        // Called whenever an item is removed
        protected override void RemoveItem(int index)
        {
            var animal = this[index];
            Console.WriteLine($"  -> Removing {animal.Name} from the zoo");
            animal.Zoo = null;  // Clear the zoo reference first
            base.RemoveItem(index);
        }

        // Called when Clear() is invoked
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

    // Container class that uses our custom collection
    public class Zoo
    {
        public readonly AnimalCollection Animals;

        public Zoo()
        {
            Animals = new AnimalCollection(this);
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
    public class BookCollection : KeyedCollection<string, Book>
    {
        // This method tells the collection how to extract the key from each item
        protected override string GetKeyForItem(Book item)
        {
            return item.ISBN;  // Use ISBN as the key
        }

        // We can still override the standard Collection<T> methods if needed
        protected override void InsertItem(int index, Book item)
        {
            Console.WriteLine($"  -> Adding book: {item.Title}");
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            var book = this[index];
            Console.WriteLine($"  -> Removing book: {book.Title}");
            base.RemoveItem(index);
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
}

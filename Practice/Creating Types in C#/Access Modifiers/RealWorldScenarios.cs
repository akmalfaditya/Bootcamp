using System;
using System.Collections.Generic;
using System.IO;

namespace AccessModifiers
{
    /// <summary>
    /// Configuration management system demonstrating internal access patterns
    /// Internal classes are perfect for implementation details within your project
    /// </summary>
    internal class ConfigurationManager
    {
        // Private field - the actual configuration data
        private Dictionary<string, string> _configurations;
        
        // Internal property - accessible within assembly for debugging/testing
        internal int ConfigurationCount => _configurations.Count;
        
        // Internal constructor - only created within the assembly
        internal ConfigurationManager()
        {
            _configurations = new Dictionary<string, string>();
        }
        
        // Public method - the main API for loading configuration
        public void LoadConfiguration()
        {
            Console.WriteLine("Loading configuration from internal sources...");
            
            // Use private method to load defaults
            LoadDefaultConfiguration();
            
            // Use internal method for assembly-specific config
            LoadInternalConfiguration();
            
            Console.WriteLine($"Loaded {ConfigurationCount} configuration items");
        }
        
        // Private method - internal implementation detail
        private void LoadDefaultConfiguration()
        {
            _configurations["AppName"] = "Access Modifiers Demo";
            _configurations["Version"] = "1.0.0";
            _configurations["Environment"] = "Development";
        }
        
        // Internal method - accessible for testing within assembly
        internal void LoadInternalConfiguration()
        {
            _configurations["InternalSetting"] = "Assembly-specific value";
            _configurations["DebugMode"] = "true";
        }
        
        // Public method for getting configuration values
        public string GetConfiguration(string key)
        {
            return _configurations.ContainsKey(key) ? _configurations[key] : "Not found";
        }
    }
    
    /// <summary>
    /// Base game entity class demonstrating protected access for inheritance
    /// Protected members are perfect for shared functionality in inheritance hierarchies
    /// </summary>
    public class GameEntity
    {
        // Protected fields - accessible to derived classes
        protected string _name;
        protected int _health;
        protected int _maxHealth;
        
        // Private fields - internal state management
        private bool _isAlive;
        private DateTime _createdAt;
        
        // Public properties - external interface
        public string Name => _name;
        public int Health => _health;
        public bool IsAlive => _isAlive;
        
        // Protected constructor - only derived classes can call
        protected GameEntity(string name, int health)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _health = health;
            _maxHealth = health;
            _isAlive = health > 0;
            _createdAt = DateTime.Now;
        }
        
        // Protected method - for derived classes to override
        protected virtual void OnHealthChanged()
        {
            _isAlive = _health > 0;
            Console.WriteLine($"{_name} health changed to {_health}");
            
            if (!_isAlive)
            {
                OnDeath();
            }
        }
        
        // Protected virtual method - template method pattern
        protected virtual void OnDeath()
        {
            Console.WriteLine($"{_name} has been defeated!");
        }
        
        // Public method using protected functionality
        public virtual void TakeDamage(int damage)
        {
            if (!_isAlive) return;
            
            _health = Math.Max(0, _health - damage);
            OnHealthChanged();  // Call protected method
        }
        
        // Public method using protected functionality
        public virtual void Heal(int amount)
        {
            if (!_isAlive) return;
            
            _health = Math.Min(_maxHealth, _health + amount);
            OnHealthChanged();  // Call protected method
        }
        
        // Public virtual method for derived classes to customize
        public virtual void DisplayStatus()
        {
            Console.WriteLine($"{_name}: {_health}/{_maxHealth} HP ({(_isAlive ? "Alive" : "Dead")})");
        }
    }
    
    /// <summary>
    /// Player class demonstrating inheritance with protected access
    /// Shows how derived classes can extend protected functionality
    /// </summary>
    public class Player : GameEntity
    {
        // Private fields specific to player
        private int _experience;
        private int _level;
        
        // Public properties specific to player
        public int Experience => _experience;
        public int Level => _level;
        
        // Public constructor
        public Player(string name, int health) : base(name, health)
        {
            _experience = 0;
            _level = 1;
        }
        
        // Override protected method to add player-specific behavior
        protected override void OnDeath()
        {
            base.OnDeath();  // Call base implementation
            Console.WriteLine($"Player {_name} will respawn at checkpoint");
            
            // Player-specific death handling
            _health = _maxHealth / 2;  // Can access protected field
            Console.WriteLine($"{_name} respawned with {_health} health");
        }
        
        // New method specific to player
        public void GainExperience(int exp)
        {
            _experience += exp;
            Console.WriteLine($"{_name} gained {exp} experience");
            
            // Check for level up
            if (_experience >= _level * 100)
            {
                LevelUp();
            }
        }
        
        // Private method - player internal logic
        private void LevelUp()
        {
            _level++;
            _maxHealth += 10;  // Can access protected field
            _health = _maxHealth;  // Full heal on level up
            
            Console.WriteLine($"{_name} leveled up to level {_level}!");
            OnHealthChanged();  // Call protected method
        }
        
        // Override public method to add player-specific display
        public override void DisplayStatus()
        {
            base.DisplayStatus();  // Call base implementation
            Console.WriteLine($"Level: {_level}, Experience: {_experience}");
        }
    }
    
    /// <summary>
    /// Enemy class demonstrating different inheritance approach
    /// Shows how different derived classes can use protected members differently
    /// </summary>
    public class Enemy : GameEntity
    {
        // Private fields specific to enemy
        private int _attackPower;
        private string _enemyType;
        
        // Public properties
        public int AttackPower => _attackPower;
        public string EnemyType => _enemyType;
        
        // Public constructor
        public Enemy(string name, int health, int attackPower = 10, string enemyType = "Basic") 
            : base(name, health)
        {
            _attackPower = attackPower;
            _enemyType = enemyType;
        }
        
        // Override protected method for enemy-specific death behavior
        protected override void OnDeath()
        {
            base.OnDeath();  // Call base implementation
            Console.WriteLine($"Enemy {_name} drops loot and disappears");
        }
        
        // Enemy-specific method
        public void Attack(GameEntity target)
        {
            if (!IsAlive) return;
            
            Console.WriteLine($"{_name} attacks {target.Name} for {_attackPower} damage!");
            target.TakeDamage(_attackPower);
        }
        
        // Override display to show enemy-specific info
        public override void DisplayStatus()
        {
            base.DisplayStatus();  // Call base implementation
            Console.WriteLine($"Type: {_enemyType}, Attack Power: {_attackPower}");
        }
    }
    
    /// <summary>
    /// Library system demonstrating mixed access modifiers
    /// Shows how real-world systems combine different access levels
    /// </summary>
    public class Library
    {
        // Private collections - core data that must be protected
        private List<Book> _books;
        private List<Member> _members;
        
        // Internal property - for administrative systems within assembly
        internal int TotalBooks => _books.Count;
        internal int TotalMembers => _members.Count;
        
        // Public constructor
        public Library()
        {
            _books = new List<Book>();
            _members = new List<Member>();
            InitializeLibrary();
        }
        
        // Private method - internal setup
        private void InitializeLibrary()
        {
            Console.WriteLine("Initializing library system...");
            AddSampleBooks();
        }
        
        // Private method - sample data setup
        private void AddSampleBooks()
        {
            _books.Add(new Book("The C# Programming Language", "Anders Hejlsberg"));
            _books.Add(new Book("Clean Code", "Robert C. Martin"));
            _books.Add(new Book("Design Patterns", "Gang of Four"));
        }
        
        // Public method - main library API
        public void DisplayLibraryInfo()
        {
            Console.WriteLine($"Library has {TotalBooks} books and {TotalMembers} members");
        }
        
        // Internal method - for administrative tools
        internal void GenerateReport()
        {
            Console.WriteLine("=== Library Administrative Report ===");
            Console.WriteLine($"Total Books: {TotalBooks}");
            Console.WriteLine($"Total Members: {TotalMembers}");
            Console.WriteLine("Report generation completed");
        }
    }
    
    /// <summary>
    /// Book class with different access levels for different concerns
    /// </summary>
    public class Book
    {
        // Private fields
        private string _title;
        private string _author;
        private bool _isAvailable;
        
        // Public properties with controlled access
        public string Title => _title;
        public string Author => _author;
        public bool IsAvailable => _isAvailable;
        
        // Internal property for library management
        internal DateTime LastBorrowed { get; set; }
        
        public Book(string title, string author)
        {
            _title = title ?? throw new ArgumentNullException(nameof(title));
            _author = author ?? throw new ArgumentNullException(nameof(author));
            _isAvailable = true;
            LastBorrowed = DateTime.MinValue;
        }
        
        // Internal methods for library system
        internal void CheckOut()
        {
            _isAvailable = false;
            LastBorrowed = DateTime.Now;
        }
        
        internal void Return()
        {
            _isAvailable = true;
        }
    }
    
    /// <summary>
    /// Member class demonstrating encapsulation
    /// </summary>
    public class Member
    {
        private string _name;
        private int _membershipNumber;
        private List<Book> _borrowedBooks;
        
        public string Name => _name;
        public int MembershipNumber => _membershipNumber;
        public int BorrowedBooksCount => _borrowedBooks.Count;
        
        public Member(string name, int membershipNumber)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _membershipNumber = membershipNumber;
            _borrowedBooks = new List<Book>();
        }
    }
}

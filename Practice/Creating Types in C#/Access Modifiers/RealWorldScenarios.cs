using System;
using System.Collections.Generic;

namespace AccessModifiers
{
    /// <summary>
    /// Real-world example: Configuration management
    /// Shows internal access for assembly-level configuration
    /// </summary>
    internal class DatabaseConfig
    {
        // Internal fields - configuration data for this assembly only
        internal string ConnectionString { get; private set; }
        internal int TimeoutSeconds { get; private set; }
        internal bool EnableLogging { get; private set; }
        
        // Private static field - singleton instance
        private static DatabaseConfig? _instance;
        
        // Private constructor - prevents external instantiation
        private DatabaseConfig()
        {
            // Default configuration
            ConnectionString = "Server=localhost;Database=MyApp;Trusted_Connection=true;";
            TimeoutSeconds = 30;
            EnableLogging = true;
        }
        
        // Internal static property - singleton pattern for assembly use
        internal static DatabaseConfig Instance
        {
            get
            {
                _instance ??= new DatabaseConfig();
                return _instance;
            }
        }
        
        // Internal method for configuration updates
        internal void LoadSettings()
        {
            Console.WriteLine("=== Loading Database Configuration ===");
            Console.WriteLine($"Connection: {ConnectionString}");
            Console.WriteLine($"Timeout: {TimeoutSeconds} seconds");
            Console.WriteLine($"Logging: {EnableLogging}");
            Console.WriteLine("Configuration loaded successfully!");
        }
        
        // Internal method for updating configuration
        internal void UpdateConfiguration(string connectionString, int timeout, bool logging)
        {
            ConnectionString = connectionString;
            TimeoutSeconds = timeout;
            EnableLogging = logging;
            
            if (EnableLogging)
            {
                Console.WriteLine("Database configuration updated");
            }
        }
    }
    
    /// <summary>
    /// Real-world example: Game entity with protected inheritance
    /// Shows how protected members work in inheritance scenarios
    /// </summary>
    public class Monster
    {
        // Protected fields - accessible to derived monster types
        protected string _name;
        protected int _health;
        protected int _maxHealth;
        protected int _attackPower;
        
        // Private fields - implementation details
        private DateTime _createdAt;
        private Guid _monsterId;
        
        // Public properties - safe exposure of data
        public string Name => _name;
        public int Health => _health;
        public int MaxHealth => _maxHealth;
        public int AttackPower => _attackPower;
        public Guid MonsterId => _monsterId;
        
        // Public constructor
        public Monster(string name, int maxHealth, int attackPower)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _maxHealth = maxHealth;
            _health = maxHealth;
            _attackPower = attackPower;
            _monsterId = Guid.NewGuid();
            _createdAt = DateTime.Now;
            
            LogActivity($"Monster {name} created");
        }
        
        // Protected method - can be overridden by derived classes
        protected virtual int CalculateDamage()
        {
            // Base damage calculation - can be enhanced by derived classes
            Random random = new Random();
            int baseDamage = _attackPower + random.Next(1, 6); // +1 to +5 random
            return baseDamage;
        }
        
        // Protected method for health management
        protected virtual void RegenerateHealth(int amount)
        {
            _health = Math.Min(_maxHealth, _health + amount);
            Console.WriteLine($"{_name} regenerated {amount} health. Current: {_health}/{_maxHealth}");
        }
        
        // Public method for taking damage
        public virtual void TakeDamage(int damage)
        {
            _health = Math.Max(0, _health - damage);
            Console.WriteLine($"{_name} took {damage} damage. Health: {_health}/{_maxHealth}");
            
            if (_health <= 0)
            {
                Console.WriteLine($"{_name} has been defeated!");
                LogActivity($"Monster {_name} defeated");
            }
        }
        
        // Public method for attacking
        public virtual void Attack(Monster target)
        {
            if (_health <= 0)
            {
                Console.WriteLine($"{_name} cannot attack - defeated!");
                return;
            }
            
            int damage = CalculateDamage(); // Use protected method
            Console.WriteLine($"{_name} attacks {target.Name} for {damage} damage!");
            target.TakeDamage(damage);
            LogActivity($"Attacked {target.Name} for {damage} damage");
        }
        
        // Public method for displaying information
        public void DisplayInfo()
        {
            Console.WriteLine($"Monster: {_name}");
            Console.WriteLine($"Health: {_health}/{_maxHealth}");
            Console.WriteLine($"Attack Power: {_attackPower}");
            Console.WriteLine($"Created: {_createdAt:yyyy-MM-dd HH:mm:ss}");
        }
        
        // Private method - internal logging
        private void LogActivity(string activity)
        {
            // In a real game, this might log to a file or database
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            Console.WriteLine($"[{timestamp}] {activity}");
        }
    }
}
using System;

namespace Inheritance
{
    /// <summary>
    /// This file demonstrates virtual methods and method overriding in C#.
    /// Virtual methods allow base classes to define methods that derived classes can customize.
    /// This is a key mechanism for achieving polymorphism and flexible code design.
    /// 
    /// Key concepts:
    /// 1. Virtual methods in base classes
    /// 2. Overriding virtual methods in derived classes
    /// 3. Calling base class implementations with 'base'
    /// 4. Covariant return types (C# 9+)
    /// 5. Virtual properties and indexers
    /// </summary>

    // Base class demonstrating virtual members
    public class GameCharacter
    {
        public string Name { get; set; } = string.Empty;
        public int Level { get; set; } = 1;
        public int Health { get; set; } = 100;
        public int Mana { get; set; } = 50;
        
        // Virtual method - can be overridden by derived classes
        public virtual void Attack()
        {
            Console.WriteLine($"{Name} performs a basic attack!");
        }
        
        // Virtual method with parameters
        public virtual int CalculateDamage(int baseDamage)
        {
            // Base calculation just returns the base damage
            return baseDamage;
        }
        
        // Virtual property - can be overridden
        public virtual int MaxHealth => 100 + (Level * 10);
        
        // Virtual method for special abilities
        public virtual void UseSpecialAbility()
        {
            Console.WriteLine($"{Name} has no special ability defined.");
        }
        
        // Virtual method for character description
        public virtual string GetDescription()
        {
            return $"Level {Level} Character: {Name}";
        }
        
        // Non-virtual method - cannot be overridden, same for all characters
        public void DisplayStats()
        {
            Console.WriteLine($"=== {Name} Stats ===");
            Console.WriteLine($"Level: {Level}");
            Console.WriteLine($"Health: {Health}/{MaxHealth}");
            Console.WriteLine($"Mana: {Mana}");
            Console.WriteLine($"Description: {GetDescription()}");
        }
        
        // Virtual method that demonstrates calling other virtual methods
        public virtual void LevelUp()
        {
            Level++;
            Health = MaxHealth; // This uses the virtual MaxHealth property
            Console.WriteLine($"{Name} leveled up to {Level}! Health restored to {Health}.");
        }
    }

    // First derived class - Warrior
    public class Warrior : GameCharacter
    {
        public int Armor { get; set; } = 20;
        public int Strength { get; set; } = 15;
        
        // Override the virtual Attack method
        public override void Attack()
        {
            Console.WriteLine($"{Name} swings a mighty sword! *CLANG*");
            Console.WriteLine("The warrior's attack has extra force from high strength!");
        }
        
        // Override damage calculation to include strength bonus
        public override int CalculateDamage(int baseDamage)
        {
            // Call base implementation first, then add warrior-specific bonus
            int damage = base.CalculateDamage(baseDamage);
            int strengthBonus = Strength / 2;
            
            Console.WriteLine($"Warrior strength bonus: +{strengthBonus} damage");
            return damage + strengthBonus;
        }
        
        // Override MaxHealth to give warriors more health
        public override int MaxHealth => base.MaxHealth + (Armor * 2);
        
        // Override special ability
        public override void UseSpecialAbility()
        {
            if (Mana >= 20)
            {
                Console.WriteLine($"{Name} uses BERSERKER RAGE! Attack power doubled!");
                Mana -= 20;
            }
            else
            {
                Console.WriteLine($"{Name} doesn't have enough mana for Berserker Rage.");
            }
        }
        
        // Override description
        public override string GetDescription()
        {
            return $"Level {Level} Warrior: {Name} (STR: {Strength}, ARM: {Armor})";
        }
        
        // Add warrior-specific method
        public void DefensiveStance()
        {
            Console.WriteLine($"{Name} takes a defensive stance, increasing armor temporarily!");
        }
    }

    // Second derived class - Mage
    public class Mage : GameCharacter
    {
        public int Intelligence { get; set; } = 20;
        public int ManaRegeneration { get; set; } = 10;
        
        public override void Attack()
        {
            Console.WriteLine($"{Name} casts a magical missile! *WHOOSH*");
            Console.WriteLine("Arcane energy crackles through the air!");
        }
        
        public override int CalculateDamage(int baseDamage)
        {
            // Mages use intelligence for damage calculation
            int damage = base.CalculateDamage(baseDamage);
            int intelligenceBonus = Intelligence / 3;
            
            Console.WriteLine($"Mage intelligence bonus: +{intelligenceBonus} magic damage");
            return damage + intelligenceBonus;
        }
        
        // Mages have lower health but more mana
        public override int MaxHealth => base.MaxHealth - 20;
        
        // Mages have a different mana system
        public virtual int MaxMana => 50 + (Level * 15) + (Intelligence * 2);
        
        public override void UseSpecialAbility()
        {
            if (Mana >= 30)
            {
                Console.WriteLine($"{Name} casts FIREBALL! A massive explosion engulfs the area!");
                Mana -= 30;
            }
            else
            {
                Console.WriteLine($"{Name} doesn't have enough mana for Fireball.");
            }
        }
        
        public override string GetDescription()
        {
            return $"Level {Level} Mage: {Name} (INT: {Intelligence}, Mana Regen: {ManaRegeneration})";
        }
        
        // Override level up to handle mana differently
        public override void LevelUp()
        {
            base.LevelUp(); // Call the base implementation first
            
            // Add mage-specific leveling benefits
            Mana = MaxMana;
            Intelligence += 2;
            
            Console.WriteLine($"Mage bonus: Intelligence increased to {Intelligence}, Mana restored to {MaxMana}!");
        }
        
        public void MeditateForMana()
        {
            int restored = ManaRegeneration + (Intelligence / 4);
            Mana = Math.Min(Mana + restored, MaxMana);
            Console.WriteLine($"{Name} meditates and restores {restored} mana. Current mana: {Mana}");
        }
    }

    // Third derived class - Archer
    public class Archer : GameCharacter
    {
        public int Agility { get; set; } = 18;
        public int Accuracy { get; set; } = 85;
        
        public override void Attack()
        {
            Console.WriteLine($"{Name} draws bow and fires an arrow! *TWANG*");
            if (Accuracy > 80)
            {
                Console.WriteLine("Perfect shot! The arrow finds its mark!");
            }
        }
        
        public override int CalculateDamage(int baseDamage)
        {
            int damage = base.CalculateDamage(baseDamage);
            
            // Archers get critical hit chance based on agility
            if (new Random().Next(100) < Agility)
            {
                Console.WriteLine("CRITICAL HIT! Agility bonus activated!");
                return damage * 2;
            }
            
            int agilityBonus = Agility / 4;
            Console.WriteLine($"Archer agility bonus: +{agilityBonus} damage");
            return damage + agilityBonus;
        }
        
        public override void UseSpecialAbility()
        {
            if (Mana >= 15)
            {
                Console.WriteLine($"{Name} uses MULTI-SHOT! Arrows rain down on multiple enemies!");
                Mana -= 15;
            }
            else
            {
                Console.WriteLine($"{Name} doesn't have enough mana for Multi-Shot.");
            }
        }
        
        public override string GetDescription()
        {
            return $"Level {Level} Archer: {Name} (AGI: {Agility}, ACC: {Accuracy}%)";
        }
        
        public void AimCarefully()
        {
            Accuracy = Math.Min(Accuracy + 10, 100);
            Console.WriteLine($"{Name} aims carefully. Accuracy increased to {Accuracy}%");
        }
    }

    /// <summary>
    /// Demonstration class showing virtual methods and overriding in action
    /// </summary>
    public static class VirtualOverrideDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== VIRTUAL METHODS AND OVERRIDING DEMONSTRATION ===\n");
            
            // Create instances of different character types
            var warrior = new Warrior
            {
                Name = "Thorin the Brave",
                Level = 5,
                Health = 150,
                Mana = 40,
                Strength = 20,
                Armor = 25
            };
            
            var mage = new Mage
            {
                Name = "Gandalf the Wise",
                Level = 4,
                Health = 80,
                Mana = 120,
                Intelligence = 25,
                ManaRegeneration = 15
            };
            
            var archer = new Archer
            {
                Name = "Legolas the Swift",
                Level = 4,
                Health = 100,
                Mana = 60,
                Agility = 22,
                Accuracy = 90
            };
            
            // 1. DEMONSTRATE POLYMORPHIC VIRTUAL METHOD CALLS
            Console.WriteLine("1. POLYMORPHIC VIRTUAL METHOD CALLS:");
            Console.WriteLine("Same method call, different implementations based on actual object type\n");
            
            // Array of base class references pointing to different derived objects
            GameCharacter[] party = { warrior, mage, archer };
            
            foreach (GameCharacter character in party)
            {
                Console.WriteLine($"--- {character.Name} ---");
                character.DisplayStats();    // Non-virtual method - same for all
                character.Attack();          // Virtual method - different implementation for each
                character.UseSpecialAbility(); // Virtual method - different for each class
                Console.WriteLine();
            }
            
            // 2. DEMONSTRATE DAMAGE CALCULATION WITH BASE CALLS
            Console.WriteLine("2. DAMAGE CALCULATION WITH BASE CLASS CALLS:");
            Console.WriteLine("Each class overrides CalculateDamage but calls base implementation\n");
            
            int baseDamage = 25;
            
            foreach (GameCharacter character in party)
            {
                Console.WriteLine($"--- {character.Name} Damage Calculation ---");
                int totalDamage = character.CalculateDamage(baseDamage);
                Console.WriteLine($"Total damage: {totalDamage}\n");
            }
            
            // 3. DEMONSTRATE VIRTUAL PROPERTIES
            Console.WriteLine("3. VIRTUAL PROPERTIES:");
            Console.WriteLine("MaxHealth property is overridden differently for each class\n");
            
            foreach (GameCharacter character in party)
            {
                Console.WriteLine($"{character.Name}: MaxHealth = {character.MaxHealth}");
                if (character is Mage m)
                {
                    Console.WriteLine($"  Mage also has MaxMana = {m.MaxMana}");
                }
                Console.WriteLine();
            }
            
            // 4. DEMONSTRATE LEVEL UP WITH VIRTUAL METHOD CHAINING
            Console.WriteLine("4. LEVEL UP WITH VIRTUAL METHOD CALLS:");
            Console.WriteLine("LevelUp calls other virtual methods like MaxHealth\n");
            
            Console.WriteLine("Before leveling up:");
            foreach (GameCharacter character in party)
            {
                Console.WriteLine($"{character.Name}: Level {character.Level}, Health {character.Health}/{character.MaxHealth}");
            }
            
            Console.WriteLine("\nLeveling up all characters:");
            foreach (GameCharacter character in party)
            {
                character.LevelUp(); // Calls virtual methods internally
            }
            
            Console.WriteLine("\nAfter leveling up:");
            foreach (GameCharacter character in party)
            {
                Console.WriteLine($"{character.Name}: Level {character.Level}, Health {character.Health}/{character.MaxHealth}");
            }
            
            // 5. DEMONSTRATE CLASS-SPECIFIC METHODS
            Console.WriteLine("\n5. CLASS-SPECIFIC METHODS:");
            Console.WriteLine("Methods that exist only in derived classes\n");
            
            warrior.DefensiveStance();
            mage.MeditateForMana();
            archer.AimCarefully();
            
            // 6. DEMONSTRATE CASTING TO ACCESS OVERRIDDEN METHODS
            Console.WriteLine("\n6. ACCESSING OVERRIDDEN METHODS THROUGH BASE REFERENCE:");
            Console.WriteLine("Virtual methods work correctly even through base class references\n");
            
            // Even though these are GameCharacter references, the correct overridden methods are called
            GameCharacter baseWarrior = warrior;
            GameCharacter baseMage = mage;
            GameCharacter baseArcher = archer;
            
            Console.WriteLine("Calling virtual methods through base class references:");
            baseWarrior.Attack();    // Calls Warrior.Attack()
            baseMage.Attack();       // Calls Mage.Attack()
            baseArcher.Attack();     // Calls Archer.Attack()
            
            Console.WriteLine("\nDescriptions through base references:");
            Console.WriteLine(baseWarrior.GetDescription()); // Calls Warrior.GetDescription()
            Console.WriteLine(baseMage.GetDescription());    // Calls Mage.GetDescription()
            Console.WriteLine(baseArcher.GetDescription());  // Calls Archer.GetDescription()
        }
    }
}

using System;
using System.Linq;

namespace Enums
{
    /// <summary>
    /// Order class demonstrating real-world enum usage
    /// Shows how enums make state management clear and safe
    /// </summary>
    public class Order
    {
        public int OrderId { get; private set; }
        public OrderStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? LastUpdated { get; private set; }

        public Order(int orderId)
        {
            OrderId = orderId;
            Status = OrderStatus.Pending;
            CreatedAt = DateTime.Now;
        }

        public void UpdateStatus(OrderStatus newStatus)
        {
            // Business logic based on enum values
            if (!IsValidStatusTransition(Status, newStatus))
            {
                Console.WriteLine($"❌ Invalid status transition from {Status} to {newStatus}");
                return;
            }

            Status = newStatus;
            LastUpdated = DateTime.Now;
            Console.WriteLine($"📦 Order {OrderId}: {Status} at {LastUpdated:HH:mm:ss}");

            // Trigger different actions based on status
            switch (Status)
            {
                case OrderStatus.Processing:
                    Console.WriteLine("   → Starting order processing...");
                    break;
                case OrderStatus.Shipped:
                    Console.WriteLine("   → Sending shipping notification...");
                    break;
                case OrderStatus.Delivered:
                    Console.WriteLine("   → Order completed! Requesting feedback...");
                    break;
                case OrderStatus.Cancelled:
                    Console.WriteLine("   → Processing refund...");
                    break;
            }
        }

        private bool IsValidStatusTransition(OrderStatus from, OrderStatus to)
        {
            // Define valid transitions using enum logic
            return from switch
            {
                OrderStatus.Pending => to == OrderStatus.Processing || to == OrderStatus.Cancelled,
                OrderStatus.Processing => to == OrderStatus.Shipped || to == OrderStatus.Cancelled,
                OrderStatus.Shipped => to == OrderStatus.Delivered,
                OrderStatus.Delivered => false, // Final state
                OrderStatus.Cancelled => to == OrderStatus.Refunded,
                OrderStatus.Refunded => false, // Final state
                _ => false
            };
        }
    }

    /// <summary>
    /// User account class showing permission management with flags
    /// Demonstrates how flags enums work in user management systems
    /// </summary>
    public class UserAccount
    {
        public string Email { get; private set; }
        public UserRole Role { get; private set; }
        public AccountPermissions Permissions { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public UserAccount(string email)
        {
            Email = email;
            Role = UserRole.Guest;
            Permissions = AccountPermissions.None;
            CreatedAt = DateTime.Now;
        }

        public void SetRole(UserRole role)
        {
            Role = role;
            
            // Auto-assign permissions based on role
            Permissions = role switch
            {
                UserRole.Guest => AccountPermissions.None,
                UserRole.Member => AccountPermissions.StandardUser,
                UserRole.Moderator => AccountPermissions.Moderator,
                UserRole.Admin => AccountPermissions.Administrator,
                UserRole.SuperAdmin => AccountPermissions.SuperAdmin,
                _ => AccountPermissions.None
            };

            Console.WriteLine($"👤 {Email}: Role set to {Role}");
        }

        public void SetPermissions(AccountPermissions permissions)
        {
            Permissions = permissions;
            Console.WriteLine($"🔐 {Email}: Permissions updated");
        }

        public bool HasPermission(AccountPermissions permission)
        {
            return (Permissions & permission) != 0;
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"User: {Email}");
            Console.WriteLine($"Role: {Role}");
            Console.WriteLine($"Permissions: {Permissions}");
            
            // Check specific permissions
            var specificPermissions = new[]
            {
                AccountPermissions.ViewProfile,
                AccountPermissions.EditProfile,
                AccountPermissions.ViewUsers,
                AccountPermissions.EditUsers,
                AccountPermissions.SystemSettings
            };

            foreach (var perm in specificPermissions)
            {
                bool hasPermission = HasPermission(perm);
                string status = hasPermission ? "✅" : "❌";
                Console.WriteLine($"  {status} {perm}");
            }
        }
    }

    /// <summary>
    /// Game character class demonstrating directional movement
    /// Shows how enums work great for game logic and state machines
    /// </summary>
    public class GameCharacter
    {
        public string Name { get; private set; }
        public Direction CurrentDirection { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public GameCharacter(string name)
        {
            Name = name;
            CurrentDirection = Direction.North;
            X = 0;
            Y = 0;
        }

        public void SetDirection(Direction direction)
        {
            CurrentDirection = direction;
            Console.WriteLine($"🎮 {Name} is now facing {direction}");
        }

        public void Move()
        {
            // Move based on current direction using enum logic
            switch (CurrentDirection)
            {
                case Direction.North:
                    Y++;
                    break;
                case Direction.South:
                    Y--;
                    break;
                case Direction.East:
                    X++;
                    break;
                case Direction.West:
                    X--;
                    break;
            }

            Console.WriteLine($"🎮 {Name} moved {CurrentDirection} to ({X}, {Y})");
        }

        public void TurnRight()
        {
            // Clever enum arithmetic for turning
            CurrentDirection = (Direction)(((int)CurrentDirection + 1) % 4);
            Console.WriteLine($"🎮 {Name} turned right, now facing {CurrentDirection}");
        }

        public void TurnLeft()
        {
            // Turning left with enum math
            CurrentDirection = (Direction)(((int)CurrentDirection + 3) % 4);
            Console.WriteLine($"🎮 {Name} turned left, now facing {CurrentDirection}");
        }
    }

    /// <summary>
    /// Application configuration class showing feature flags in action
    /// Perfect example of how flags enums work for configuration management
    /// </summary>
    public class AppConfiguration
    {
        public LogLevel CurrentLogLevel { get; private set; }
        public FeatureFlags EnabledFeatures { get; private set; }
        public ProcessingOptions ProcessingMode { get; private set; }

        public AppConfiguration()
        {
            CurrentLogLevel = LogLevel.Info;
            EnabledFeatures = FeatureFlags.None;
            ProcessingMode = ProcessingOptions.SafeProcessing;
        }

        public void SetLogLevel(LogLevel level)
        {
            CurrentLogLevel = level;
            Console.WriteLine($"⚙️ Log level set to: {level}");
        }

        public void EnableFeatures(FeatureFlags features)
        {
            EnabledFeatures |= features; // Add features using OR
            Console.WriteLine($"⚙️ Enabled features: {EnabledFeatures}");
        }

        public void DisableFeatures(FeatureFlags features)
        {
            EnabledFeatures &= ~features; // Remove features using AND with NOT
            Console.WriteLine($"⚙️ Disabled features, remaining: {EnabledFeatures}");
        }

        public bool IsFeatureEnabled(FeatureFlags feature)
        {
            return (EnabledFeatures & feature) != 0;
        }

        public void SetProcessingMode(ProcessingOptions options)
        {
            ProcessingMode = options;
            Console.WriteLine($"⚙️ Processing mode: {ProcessingMode}");
        }

        public void DisplaySettings()
        {
            Console.WriteLine("⚙️ Current Configuration:");
            Console.WriteLine($"  Log Level: {CurrentLogLevel}");
            Console.WriteLine($"  Features: {EnabledFeatures}");
            Console.WriteLine($"  Processing: {ProcessingMode}");
            
            // Show individual feature status
            var allFeatures = Enum.GetValues<FeatureFlags>()
                .Where(f => f != FeatureFlags.None && !IsComboFlag(f));
            
            Console.WriteLine("  Feature Status:");
            foreach (var feature in allFeatures)
            {
                bool enabled = IsFeatureEnabled(feature);
                string status = enabled ? "✅" : "❌";
                Console.WriteLine($"    {status} {feature}");
            }
        }

        private bool IsComboFlag(FeatureFlags flag)
        {
            // Check if this is a combination flag (has multiple bits set)
            return flag != FeatureFlags.None && (((int)flag & ((int)flag - 1)) != 0);
        }
    }
}

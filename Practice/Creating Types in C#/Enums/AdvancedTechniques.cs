using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Enums
{    /// <summary>
    /// Advanced enum techniques that will make you look like a C# wizard!
    /// These patterns solve real problems you'll face in production code
    /// From validation to localization - we've got you covered
    /// </summary>
    
    // Advanced Pattern #1: Using attributes for rich metadata
    // Think of this as attaching sticky notes to each enum value
    public enum NotificationPriority
    {
        [Description("Low priority - can wait")]
        Low = 1,
        
        [Description("Normal priority - process soon")]
        Medium = 2,
        
        [Description("High priority - process immediately")]
        High = 3,
        
        [Description("Critical - wake up the on-call engineer!")]
        Critical = 4
    }

    // Advanced Pattern #2: Enum with validation methods
    // Like having a bouncer for your enum values
    public enum HttpStatusCode
    {
        // Information responses
        Continue = 100,
        SwitchingProtocols = 101,
        
        // Success responses  
        OK = 200,
        Created = 201,
        Accepted = 202,
        NoContent = 204,
        
        // Redirection responses
        MovedPermanently = 301,
        Found = 302,
        NotModified = 304,
        
        // Client error responses
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        
        // Server error responses
        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502,
        ServiceUnavailable = 503
    }

    // Advanced Pattern #3: Enum with business logic
    // Sometimes enums need to know how to behave, not just what they are
    public enum DiscountType
    {
        None = 0,
        Percentage = 1,
        FixedAmount = 2,
        BuyOneGetOne = 3,
        VolumeDiscount = 4
    }

    // Advanced Pattern #4: Enum for state machines
    // Perfect for workflow systems where order matters
    public enum OrderState
    {
        Draft = 0,
        Submitted = 1,
        UnderReview = 2,
        Approved = 3,
        InProduction = 4,
        Shipped = 5,
        Delivered = 6,
        Cancelled = 99
    }

    /// <summary>
    /// Extension methods - the secret sauce for making enums super powerful!
    /// These let you add methods to existing enums without modifying them
    /// It's like giving your enums superpowers after they're already defined
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Get the friendly description from the Description attribute
        /// Perfect for user-facing messages instead of raw enum names
        /// </summary>
        public static string GetDescription(this Enum value)
        {
            // This is reflection magic - we're looking at the enum's metadata
            FieldInfo? field = value.GetType().GetField(value.ToString());
            if (field == null) return value.ToString();
            
            // Look for our Description attribute
            DescriptionAttribute? attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }

        /// <summary>
        /// Get the display name for UI purposes
        /// Cleaner than ToString() for user interfaces
        /// </summary>
        public static string GetDisplayName(this Enum value)
        {
            FieldInfo? field = value.GetType().GetField(value.ToString());
            if (field == null) return value.ToString();
            
            DisplayNameAttribute? attribute = field.GetCustomAttribute<DisplayNameAttribute>();
            return attribute?.DisplayName ?? value.ToString();
        }

        /// <summary>
        /// Check if an HTTP status code indicates success (200-299)
        /// Business logic embedded in extension methods - very clean!
        /// </summary>
        public static bool IsSuccess(this HttpStatusCode statusCode)
        {
            int code = (int)statusCode;
            return code >= 200 && code < 300;
        }

        /// <summary>
        /// Check if an HTTP status code indicates an error (400-599)
        /// </summary>
        public static bool IsError(this HttpStatusCode statusCode)
        {
            int code = (int)statusCode;
            return code >= 400 && code < 600;
        }

        /// <summary>
        /// Check if an HTTP status code indicates a client error (400-499)
        /// </summary>
        public static bool IsClientError(this HttpStatusCode statusCode)
        {
            int code = (int)statusCode;
            return code >= 400 && code < 500;
        }

        /// <summary>
        /// Check if an HTTP status code indicates a server error (500-599)
        /// </summary>
        public static bool IsServerError(this HttpStatusCode statusCode)
        {
            int code = (int)statusCode;
            return code >= 500 && code < 600;
        }

        /// <summary>
        /// Calculate discount amount based on type and original price
        /// This shows how enums can drive business logic elegantly
        /// </summary>
        public static decimal CalculateDiscount(this DiscountType discountType, decimal originalPrice, decimal discountValue, int quantity = 1)
        {
            return discountType switch
            {
                DiscountType.None => 0,
                DiscountType.Percentage => originalPrice * (discountValue / 100),
                DiscountType.FixedAmount => Math.Min(discountValue, originalPrice),
                DiscountType.BuyOneGetOne => quantity >= 2 ? originalPrice * (quantity / 2) : 0,
                DiscountType.VolumeDiscount => quantity >= 5 ? originalPrice * 0.15m : 0,
                _ => 0
            };
        }

        /// <summary>
        /// Check if a state transition is valid for order processing
        /// State machines become super readable with this pattern!
        /// </summary>
        public static bool CanTransitionTo(this OrderState currentState, OrderState newState)
        {
            // Define valid transitions - this is like a workflow diagram in code
            return currentState switch
            {
                OrderState.Draft => newState == OrderState.Submitted || newState == OrderState.Cancelled,
                OrderState.Submitted => newState == OrderState.UnderReview || newState == OrderState.Cancelled,
                OrderState.UnderReview => newState == OrderState.Approved || newState == OrderState.Cancelled,
                OrderState.Approved => newState == OrderState.InProduction || newState == OrderState.Cancelled,
                OrderState.InProduction => newState == OrderState.Shipped,
                OrderState.Shipped => newState == OrderState.Delivered,
                OrderState.Delivered => false, // Final state - no more transitions
                OrderState.Cancelled => false, // Final state - no more transitions
                _ => false
            };
        }

        /// <summary>
        /// Get the next valid states for workflow UI
        /// Perfect for dropdown lists in state management UIs
        /// </summary>
        public static OrderState[] GetValidNextStates(this OrderState currentState)
        {
            return Enum.GetValues<OrderState>()
                       .Where(state => currentState.CanTransitionTo(state))
                       .ToArray();
        }

        /// <summary>
        /// OrderStatus specific extension methods
        /// These add business logic directly to the enum values
        /// </summary>
        public static bool CanBeCancelled(this OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Pending => true,
                OrderStatus.Processing => true,
                OrderStatus.Shipped => false,
                OrderStatus.Delivered => false,
                OrderStatus.Cancelled => false,
                OrderStatus.Refunded => false,
                _ => false
            };
        }

        /// <summary>
        /// Get display color for UI - perfect for status indicators
        /// </summary>
        public static string GetDisplayColor(this OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Pending => "Orange",
                OrderStatus.Processing => "Blue",
                OrderStatus.Shipped => "Purple",
                OrderStatus.Delivered => "Green",
                OrderStatus.Cancelled => "Red",
                OrderStatus.Refunded => "Gray",
                _ => "Black"
            };
        }

        /// <summary>
        /// FilePermissions specific extension methods for flags operations
        /// </summary>
        public static FilePermissions AddFlag(this FilePermissions permissions, FilePermissions flag)
        {
            return permissions | flag;
        }

        public static FilePermissions RemoveFlag(this FilePermissions permissions, FilePermissions flag)
        {
            return permissions & ~flag;
        }

        public static bool HasExactFlags(this FilePermissions permissions, FilePermissions flags)
        {
            return (permissions & flags) == flags;
        }

        /// <summary>
        /// Safely parse enum with validation - prevents runtime surprises!
        /// Much better than raw Enum.Parse which throws exceptions
        /// </summary>
        public static T? SafeParse<T>(string value) where T : struct, Enum
        {
            if (Enum.TryParse<T>(value, true, out T result) && Enum.IsDefined(typeof(T), result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Get all enum values as a list - handy for UI binding
        /// Much cleaner than casting Enum.GetValues() everywhere
        /// </summary>
        public static T[] GetAllValues<T>() where T : struct, Enum
        {
            return Enum.GetValues<T>();
        }

        /// <summary>
        /// Check if enum has a specific flag set
        /// Works with any flags enum, not just our examples
        /// </summary>
        public static bool HasAnyFlag<T>(this T value, params T[] flags) where T : struct, Enum
        {
            foreach (T flag in flags)
            {
                if (value.HasFlag(flag))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Check if enum has all specified flags set
        /// Useful for permission checking: "Does user have ALL these permissions?"
        /// </summary>
        public static bool HasAllFlags<T>(this T value, params T[] flags) where T : struct, Enum
        {
            foreach (T flag in flags)
            {
                if (!value.HasFlag(flag))
                    return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Utility class for advanced enum operations
    /// These are your go-to tools for enum manipulation
    /// </summary>
    public static class EnumUtils
    {
        /// <summary>
        /// Get enum from description attribute - reverse lookup!
        /// Perfect when you store descriptions in databases or config files
        /// </summary>
        public static T? GetEnumFromDescription<T>(string description) where T : struct, Enum
        {
            foreach (T enumValue in Enum.GetValues<T>())
            {
                if (enumValue.GetDescription().Equals(description, StringComparison.OrdinalIgnoreCase))
                {
                    return enumValue;
                }
            }
            return null;
        }

        /// <summary>
        /// Validate that an integer is a valid enum value
        /// Prevents the classic "Status 999" bug that crashes your app
        /// </summary>
        public static bool IsValidEnumValue<T>(int value) where T : struct, Enum
        {
            return Enum.IsDefined(typeof(T), value);
        }

        /// <summary>
        /// Get enum statistics - useful for debugging and reporting
        /// How many values? What's the range? This tells you everything
        /// </summary>
        public static (int Count, int MinValue, int MaxValue) GetEnumStats<T>() where T : struct, Enum
        {
            T[] values = Enum.GetValues<T>();
            int[] intValues = values.Select(v => Convert.ToInt32(v)).ToArray();
            
            return (
                Count: values.Length,
                MinValue: intValues.Min(),
                MaxValue: intValues.Max()
            );
        }

        /// <summary>
        /// Create a flags enum from individual boolean values
        /// Perfect for converting checkbox states to flags
        /// </summary>
        public static T CreateFlags<T>(params (T flag, bool isSet)[] flagSettings) where T : struct, Enum
        {
            int result = 0;
            foreach (var (flag, isSet) in flagSettings)
            {
                if (isSet)
                {
                    result |= Convert.ToInt32(flag);
                }
            }
            return (T)Enum.ToObject(typeof(T), result);
        }        /// <summary>
        /// Get all enum values as a list - handy for UI binding
        /// Much cleaner than casting Enum.GetValues() everywhere
        /// </summary>
        public static T[] GetAllValues<T>() where T : struct, Enum
        {
            return Enum.GetValues<T>();
        }

        /// <summary>
        /// Parse enum with fallback value - never throws exceptions!
        /// Perfect for user input or configuration values
        /// </summary>
        public static T ParseWithFallback<T>(string value, T fallback) where T : struct, Enum
        {
            if (Enum.TryParse<T>(value, true, out T result) && Enum.IsDefined(typeof(T), result))
            {
                return result;
            }
            return fallback;
        }

        /// <summary>
        /// Get the count of enum values
        /// Useful for array sizing and validation
        /// </summary>
        public static int GetCount<T>() where T : struct, Enum
        {
            return Enum.GetValues<T>().Length;
        }
    }
}

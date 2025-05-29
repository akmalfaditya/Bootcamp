using System;

namespace Enums
{
    /// <summary>
    /// Basic enum - the foundation of all enum concepts
    /// Default underlying type is int, values start from 0 and auto-increment
    /// Think of this as a simple list of named options
    /// </summary>
    public enum BorderSide
    {
        Left,    // 0
        Right,   // 1
        Top,     // 2
        Bottom   // 3
    }

    /// <summary>
    /// File permission enum using byte as underlying type
    /// Byte is perfect for small value sets - saves memory
    /// Great for scenarios where you'll have many instances
    /// </summary>
    public enum FilePermission : byte
    {
        None = 0,
        Read = 1,
        Write = 2,
        Execute = 3,
        Delete = 4
    }

    /// <summary>
    /// HTTP status codes using short
    /// Short gives us range 0-65535, perfect for HTTP codes
    /// Real HTTP codes go up to 599, so plenty of room
    /// </summary>
    public enum HttpStatus : short
    {
        OK = 200,
        Created = 201,
        BadRequest = 400,
        Unauthorized = 401,
        NotFound = 404,
        InternalServerError = 500
    }

    /// <summary>
    /// Priority enum with explicit values
    /// Notice the gaps - this leaves room for future priorities
    /// Maybe later we'll add "Urgent = 15" between High and Critical
    /// </summary>
    public enum Priority
    {
        Low = 1,
        Medium = 5,
        High = 10,
        Critical = 20
    }

    /// <summary>
    /// Log level enum with gaps for future expansion
    /// Values are spaced out so we can add new levels between existing ones
    /// Common pattern in logging frameworks
    /// </summary>
    public enum LogLevel
    {
        Trace = 0,
        Debug = 10,
        Info = 20,
        Warning = 30,
        Error = 40,
        Critical = 50
    }

    /// <summary>
    /// Order status for e-commerce scenarios
    /// This represents the lifecycle of an order
    /// Sequential values make sense here since there's a natural progression
    /// </summary>
    public enum OrderStatus
    {
        Pending = 0,
        Processing = 1,
        Shipped = 2,
        Delivered = 3,
        Cancelled = 4,
        Refunded = 5
    }

    /// <summary>
    /// Horizontal alignment that reuses BorderSide values
    /// Shows how different enums can share the same underlying values
    /// Useful for type-safe conversions between related enums
    /// </summary>
    public enum HorizontalAlignment
    {
        Left = 0,     // Same as BorderSide.Left
        Right = 1,    // Same as BorderSide.Right
        Center = 10   // New value not in BorderSide
    }

    /// <summary>
    /// User roles in a typical application
    /// Each role has different permissions and capabilities
    /// Values chosen to allow easy comparison (higher value = more permissions)
    /// </summary>
    public enum UserRole
    {
        Guest = 0,
        Member = 10,
        Moderator = 20,
        Admin = 30,
        SuperAdmin = 40
    }

    /// <summary>
    /// Game directions - classic enum example
    /// Perfect for games, UI navigation, or any directional logic
    /// Sequential values work well here
    /// </summary>
    public enum Direction
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }
}

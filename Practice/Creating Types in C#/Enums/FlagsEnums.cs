using System;

namespace Enums
{
    /// <summary>
    /// File permissions flags enum - the classic flags example
    /// Each value is a power of 2 so they can be combined using bitwise operations
    /// Think of it like checkboxes - you can select multiple permissions
    /// </summary>
    [Flags]
    public enum FilePermissions
    {
        None = 0,           // 0000 in binary
        Read = 1,           // 0001 in binary
        Write = 2,          // 0010 in binary
        Execute = 4,        // 0100 in binary
        Delete = 8,         // 1000 in binary
        
        // Convenient combinations
        ReadWrite = Read | Write,                    // 0011 in binary
        FullAccess = Read | Write | Execute | Delete // 1111 in binary
    }

    /// <summary>
    /// Feature flags for A/B testing and gradual rollouts
    /// Perfect for enabling/disabling features in production
    /// Each feature can be toggled independently
    /// </summary>
    [Flags]
    public enum FeatureFlags
    {
        None = 0,
        DarkMode = 1,           // 0001
        PushNotifications = 2,  // 0010
        AdvancedSearch = 4,     // 0100
        VideoChat = 8,          // 1000
        AIAssistant = 16,       // 10000
        
        // Feature bundles for different subscription tiers
        BasicFeatures = DarkMode | PushNotifications,
        PremiumFeatures = BasicFeatures | AdvancedSearch | VideoChat,
        UltimateFeatures = PremiumFeatures | AIAssistant
    }

    /// <summary>
    /// Account permissions for user management systems
    /// Shows how flags work great for permission systems
    /// Each permission can be granted or revoked independently
    /// </summary>
    [Flags]
    public enum AccountPermissions
    {
        None = 0,
        ViewProfile = 1,        // 0001
        EditProfile = 2,        // 0010
        ViewUsers = 4,          // 0100
        EditUsers = 8,          // 1000
        DeleteUsers = 16,       // 10000
        ViewReports = 32,       // 100000
        EditReports = 64,       // 1000000
        SystemSettings = 128,   // 10000000
        
        // Role-based permission sets
        StandardUser = ViewProfile | EditProfile,
        Moderator = StandardUser | ViewUsers | ViewReports,
        Administrator = Moderator | EditUsers | EditReports | SystemSettings,
        SuperAdmin = Administrator | DeleteUsers
    }

    /// <summary>
    /// Border sides that can be combined
    /// Different from the basic BorderSide enum - this one supports combinations
    /// Useful for CSS-like border styling where you might want top+bottom borders
    /// </summary>
    [Flags]
    public enum BorderSides
    {
        None = 0,
        Left = 1,       // 0001
        Right = 2,      // 0010
        Top = 4,        // 0100
        Bottom = 8,     // 1000
        
        // Common combinations
        Horizontal = Left | Right,
        Vertical = Top | Bottom,
        All = Left | Right | Top | Bottom
    }

    /// <summary>
    /// Processing options for data operations
    /// Shows how flags work well for configuration options
    /// Each option can be enabled/disabled independently
    /// </summary>
    [Flags]
    public enum ProcessingOptions
    {
        None = 0,
        ValidateInput = 1,      // 0001
        LogOperations = 2,      // 0010
        CacheResults = 4,       // 0100
        CompressOutput = 8,     // 1000
        EncryptData = 16,       // 10000
        
        // Preset configurations
        FastProcessing = None,
        SafeProcessing = ValidateInput | LogOperations,
        OptimizedProcessing = ValidateInput | CacheResults | CompressOutput,
        SecureProcessing = ValidateInput | LogOperations | EncryptData,
        FullProcessing = ValidateInput | LogOperations | CacheResults | CompressOutput | EncryptData
    }

    /// <summary>
    /// Network protocols that can be combined
    /// Real-world example where a service might support multiple protocols
    /// </summary>
    [Flags]
    public enum NetworkProtocols
    {
        None = 0,
        HTTP = 1,      // 0001
        HTTPS = 2,     // 0010
        FTP = 4,       // 0100
        SFTP = 8,      // 1000
        WebSocket = 16, // 10000
        
        // Common combinations
        WebProtocols = HTTP | HTTPS | WebSocket,
        FileProtocols = FTP | SFTP,
        SecureProtocols = HTTPS | SFTP | WebSocket,
        AllProtocols = HTTP | HTTPS | FTP | SFTP | WebSocket
    }
}

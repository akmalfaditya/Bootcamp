using System;
using System.Collections.Generic;
using System.Linq;

namespace NestedTypes
{
    /// <summary>
    /// When to Use Nested Types - Real-world scenarios and best practices
    /// Nested types are particularly appropriate in these scenarios:
    /// 1. Stronger Access Control
    /// 2. Intimate Collaboration
    /// 3. Logical Grouping/Readability
    /// 4. Avoiding Namespace Clutter
    /// </summary>

    #region Scenario 1: Stronger Access Control
    /// <summary>
    /// Scenario 1: Stronger Access Control
    /// Helper types that are conceptually part of another type's implementation
    /// and should not be exposed globally
    /// </summary>
    public class DatabaseConnection
    {
        private string connectionString;
        private bool isConnected;
        private List<Query> executedQueries;

        public DatabaseConnection(string connectionString)
        {
            this.connectionString = connectionString;
            this.isConnected = false;
            this.executedQueries = new List<Query>();
        }

        /// <summary>
        /// Private helper class for managing connection pooling
        /// This should NOT be exposed globally - it's an implementation detail
        /// </summary>
        private class ConnectionPool
        {
            private static readonly Dictionary<string, List<DatabaseConnection>> pools = 
                new Dictionary<string, List<DatabaseConnection>>();

            /// <summary>
            /// Get or create a connection from the pool
            /// This class has intimate knowledge of DatabaseConnection internals
            /// </summary>
            public static DatabaseConnection GetConnection(string connectionString)
            {
                if (!pools.ContainsKey(connectionString))
                {
                    pools[connectionString] = new List<DatabaseConnection>();
                }

                var pool = pools[connectionString];
                var available = pool.FirstOrDefault(c => !c.isConnected); // Access private field!

                if (available != null)
                {
                    available.isConnected = true; // Direct access to private field
                    Console.WriteLine("Reusing pooled connection");
                    return available;
                }

                var newConnection = new DatabaseConnection(connectionString);
                newConnection.isConnected = true; // Access private field directly
                pool.Add(newConnection);
                Console.WriteLine("Created new pooled connection");
                return newConnection;
            }

            public static void ReturnConnection(DatabaseConnection connection)
            {
                connection.isConnected = false; // Direct access to private field
                Console.WriteLine("Connection returned to pool");
            }
        }

        /// <summary>
        /// Private Query class - only DatabaseConnection should manage queries
        /// This demonstrates stronger access control
        /// </summary>
        private class Query
        {
            public string Sql { get; }
            public DateTime ExecutedAt { get; }
            public TimeSpan Duration { get; }

            internal Query(string sql, TimeSpan duration)
            {
                Sql = sql;
                ExecutedAt = DateTime.Now;
                Duration = duration;
            }
        }

        // Public methods that use the private nested types internally
        public static DatabaseConnection GetPooledConnection(string connectionString)
        {
            return ConnectionPool.GetConnection(connectionString);
        }

        public void ReturnToPool()
        {
            ConnectionPool.ReturnConnection(this);
        }

        public void ExecuteQuery(string sql)
        {
            var start = DateTime.Now;
            // Simulate query execution
            System.Threading.Thread.Sleep(100);
            var duration = DateTime.Now - start;

            // Use private nested Query class
            var query = new Query(sql, duration);
            executedQueries.Add(query); // Access private field

            Console.WriteLine($"Executed query: {sql} in {duration.TotalMilliseconds:F0}ms");
        }

        public void ShowQueryHistory()
        {
            Console.WriteLine("\n=== Query History ===");
            foreach (var query in executedQueries)
            {
                Console.WriteLine($"SQL: {query.Sql}, Duration: {query.Duration.TotalMilliseconds:F0}ms");
            }
        }
    }
    #endregion

    #region Scenario 2: Intimate Collaboration
    /// <summary>
    /// Scenario 2: Intimate Collaboration
    /// The nested type needs to access private members of the enclosing type
    /// to perform its function effectively
    /// </summary>
    public class SecureVault
    {
        private readonly Dictionary<string, object> secureStorage;
        private readonly List<AccessLog> accessLogs;
        private readonly string masterKey;
        private int failedAttempts;

        public SecureVault(string masterKey)
        {
            this.masterKey = masterKey;
            this.secureStorage = new Dictionary<string, object>();
            this.accessLogs = new List<AccessLog>();
            this.failedAttempts = 0;
        }

        /// <summary>
        /// Nested class that needs intimate access to vault internals
        /// This demonstrates tight coupling where the nested type needs
        /// direct access to private state for security operations
        /// </summary>
        public class SecurityAuditor
        {
            private readonly SecureVault vault;

            internal SecurityAuditor(SecureVault vault)
            {
                this.vault = vault;
            }

            /// <summary>
            /// Performs security audit with direct access to private fields
            /// This level of access would be difficult to achieve without nested types
            /// </summary>
            public void PerformSecurityAudit()
            {
                Console.WriteLine("\n=== Security Audit ===");
                
                // Direct access to private fields for comprehensive audit
                Console.WriteLine($"Items in secure storage: {vault.secureStorage.Count}");
                Console.WriteLine($"Access logs recorded: {vault.accessLogs.Count}");
                Console.WriteLine($"Failed attempts: {vault.failedAttempts}");
                
                // Analyze access patterns by directly accessing private data
                var recentLogs = vault.accessLogs
                    .Where(log => log.Timestamp > DateTime.Now.AddMinutes(-10))
                    .ToList();
                
                Console.WriteLine($"Recent access attempts: {recentLogs.Count}");
                
                if (vault.failedAttempts > 3)
                {
                    Console.WriteLine("⚠️ WARNING: Multiple failed access attempts detected!");
                    // Could trigger security lockdown by directly modifying private state
                }
                
                Console.WriteLine("✓ Security audit completed");
            }

            /// <summary>
            /// Reset security counters - only auditor should be able to do this
            /// </summary>
            public void ResetSecurityCounters()
            {
                vault.failedAttempts = 0; // Direct access to private field
                Console.WriteLine("Security counters reset by auditor");
            }
        }

        /// <summary>
        /// Private nested class for access logging
        /// </summary>
        private class AccessLog
        {
            public DateTime Timestamp { get; }
            public string Operation { get; }
            public bool Success { get; }

            public AccessLog(string operation, bool success)
            {
                Timestamp = DateTime.Now;
                Operation = operation;
                Success = success;
            }
        }

        // Public methods that create and use nested types
        public SecurityAuditor GetSecurityAuditor()
        {
            return new SecurityAuditor(this);
        }

        public bool Store(string key, object value, string providedKey)
        {
            var success = providedKey == masterKey;
            accessLogs.Add(new AccessLog($"Store '{key}'", success)); // Use private nested class

            if (success)
            {
                secureStorage[key] = value;
                Console.WriteLine($"Stored '{key}' successfully");
            }
            else
            {
                failedAttempts++;
                Console.WriteLine($"Failed to store '{key}' - invalid key");
            }

            return success;
        }

        public T Retrieve<T>(string key, string providedKey)
        {
            var success = providedKey == masterKey && secureStorage.ContainsKey(key);
            accessLogs.Add(new AccessLog($"Retrieve '{key}'", success));

            if (success)
            {
                Console.WriteLine($"Retrieved '{key}' successfully");
                return (T)secureStorage[key];
            }
            else
            {
                failedAttempts++;
                Console.WriteLine($"Failed to retrieve '{key}' - invalid key or not found");
                return default(T);
            }
        }
    }
    #endregion

    #region Scenario 3: Logical Grouping/Readability
    /// <summary>
    /// Scenario 3: Logical Grouping/Readability
    /// The nested type is conceptually related to and primarily used by
    /// its enclosing type, making the code more organized and easier to understand
    /// </summary>
    public class EmailService
    {
        private readonly List<EmailMessage> outbox;
        private readonly EmailConfiguration config;

        public EmailService(EmailConfiguration config)
        {
            this.config = config;
            this.outbox = new List<EmailMessage>();
        }

        /// <summary>
        /// Nested configuration class - only makes sense in context of EmailService
        /// This demonstrates logical grouping for better code organization
        /// </summary>
        public class EmailConfiguration
        {
            public string SmtpServer { get; set; }
            public int Port { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public bool UseSSL { get; set; }

            /// <summary>
            /// Nested validation class - highly specific to email configuration
            /// </summary>
            public class ConfigurationValidator
            {
                public static ValidationResult Validate(EmailConfiguration config)
                {
                    var result = new ValidationResult();
                    
                    if (string.IsNullOrEmpty(config.SmtpServer))
                        result.AddError("SMTP server is required");
                    
                    if (config.Port <= 0 || config.Port > 65535)
                        result.AddError("Port must be between 1 and 65535");
                    
                    if (string.IsNullOrEmpty(config.Username))
                        result.AddError("Username is required");
                    
                    Console.WriteLine($"Configuration validation: {(result.IsValid ? "PASSED" : "FAILED")}");
                    return result;
                }
            }

            public class ValidationResult
            {
                private readonly List<string> errors = new List<string>();
                
                public bool IsValid => !errors.Any();
                public IReadOnlyList<string> Errors => errors.AsReadOnly();
                
                public void AddError(string error) => errors.Add(error);
                
                public void ShowErrors()
                {
                    if (!IsValid)
                    {
                        Console.WriteLine("Configuration errors:");
                        errors.ForEach(error => Console.WriteLine($"  - {error}"));
                    }
                }
            }
        }

        /// <summary>
        /// Nested message class - conceptually belongs to EmailService
        /// </summary>
        public class EmailMessage
        {
            public string To { get; set; }
            public string From { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
            public MessagePriority Priority { get; set; }
            public DateTime CreatedAt { get; }

            public EmailMessage()
            {
                CreatedAt = DateTime.Now;
                Priority = MessagePriority.Normal;
            }

            /// <summary>
            /// Nested enum for message priority - only relevant for email messages
            /// </summary>
            public enum MessagePriority
            {
                Low,
                Normal,
                High,
                Urgent
            }

            public void ShowMessageInfo()
            {
                Console.WriteLine($"Email: {From} -> {To}");
                Console.WriteLine($"Subject: {Subject}");
                Console.WriteLine($"Priority: {Priority}");
                Console.WriteLine($"Created: {CreatedAt:yyyy-MM-dd HH:mm:ss}");
            }
        }

        // Methods demonstrating the logical grouping in action
        public void SendEmail(EmailMessage message)
        {
            // Validate configuration first
            var validationResult = EmailConfiguration.ConfigurationValidator.Validate(config);
            if (!validationResult.IsValid)
            {
                validationResult.ShowErrors();
                return;
            }

            outbox.Add(message);
            Console.WriteLine($"Email queued for sending: {message.Subject}");
        }

        public void ShowOutboxStatus()
        {
            Console.WriteLine($"\n=== Email Outbox ({outbox.Count} messages) ===");
            foreach (var message in outbox)
            {
                message.ShowMessageInfo();
                Console.WriteLine();
            }
        }
    }
    #endregion

    #region Scenario 4: Avoiding Namespace Clutter
    /// <summary>
    /// Scenario 4: Avoiding Namespace Clutter
    /// Nested types help organize related functionality without
    /// cluttering the global namespace with highly specific types
    /// </summary>
    public class ReportGenerator
    {
        private readonly List<IReportSection> sections;

        public ReportGenerator()
        {
            sections = new List<IReportSection>();
        }

        /// <summary>
        /// Interface for report sections - only relevant within ReportGenerator context
        /// Keeping this nested avoids cluttering the global namespace
        /// </summary>
        public interface IReportSection
        {
            string Title { get; }
            void Generate();
            SectionType Type { get; }
        }

        /// <summary>
        /// Enum for section types - specific to report generation
        /// </summary>
        public enum SectionType
        {
            Header,
            Summary,
            Details,
            Chart,
            Footer
        }

        /// <summary>
        /// Concrete report section implementations
        /// These are highly specific to report generation and don't belong in global namespace
        /// </summary>
        public class HeaderSection : IReportSection
        {
            public string Title => "Report Header";
            public SectionType Type => SectionType.Header;

            public void Generate()
            {
                Console.WriteLine("=== COMPANY QUARTERLY REPORT ===");
                Console.WriteLine($"Generated on: {DateTime.Now:yyyy-MM-dd}");
            }
        }

        public class SummarySection : IReportSection
        {
            public string Title => "Executive Summary";
            public SectionType Type => SectionType.Summary;

            public void Generate()
            {
                Console.WriteLine("\n--- Executive Summary ---");
                Console.WriteLine("• Revenue increased by 15% this quarter");
                Console.WriteLine("• Customer satisfaction improved to 94%");
                Console.WriteLine("• New product launches exceeded targets");
            }
        }

        public class DetailsSection : IReportSection
        {
            public string Title => "Detailed Analysis";
            public SectionType Type => SectionType.Details;

            public void Generate()
            {
                Console.WriteLine("\n--- Detailed Analysis ---");
                Console.WriteLine("Sales by Region:");
                Console.WriteLine("  North: $2.1M (+12%)");
                Console.WriteLine("  South: $1.8M (+18%)");
                Console.WriteLine("  East:  $2.3M (+14%)");
                Console.WriteLine("  West:  $1.9M (+16%)");
            }
        }

        /// <summary>
        /// Report configuration nested class
        /// Provides strongly-typed configuration without namespace pollution
        /// </summary>
        public class ReportConfiguration
        {
            public string Title { get; set; } = "Quarterly Report";
            public bool IncludeCharts { get; set; } = true;
            public bool IncludeFooter { get; set; } = true;
            public OutputFormat Format { get; set; } = OutputFormat.Console;

            public enum OutputFormat
            {
                Console,
                Html,
                Pdf,
                Excel
            }

            public void ShowConfiguration()
            {
                Console.WriteLine($"\n=== Report Configuration ===");
                Console.WriteLine($"Title: {Title}");
                Console.WriteLine($"Include Charts: {IncludeCharts}");
                Console.WriteLine($"Include Footer: {IncludeFooter}");
                Console.WriteLine($"Output Format: {Format}");
            }
        }

        // Public methods using the nested types
        public void AddSection(IReportSection section)
        {
            sections.Add(section);
            Console.WriteLine($"Added section: {section.Title} ({section.Type})");
        }

        public void GenerateReport(ReportConfiguration config)
        {
            config.ShowConfiguration();
            Console.WriteLine("\n=== GENERATING REPORT ===");

            foreach (var section in sections.OrderBy(s => (int)s.Type))
            {
                section.Generate();
            }

            if (config.IncludeFooter)
            {
                Console.WriteLine("\n--- Report Footer ---");
                Console.WriteLine("End of Report");
            }

            Console.WriteLine($"\n✓ Report generated in {config.Format} format");
        }
    }
    #endregion

    /// <summary>
    /// Demonstration class showing all "When to Use" scenarios in action
    /// </summary>
    public static class WhenToUseDemo
    {
        public static void DemonstrateAllScenarios()
        {
            Console.WriteLine("=== When to Use Nested Types - Real-World Scenarios ===\n");

            // Scenario 1: Stronger Access Control
            Console.WriteLine("SCENARIO 1: Stronger Access Control");
            var dbConn = DatabaseConnection.GetPooledConnection("server=localhost;db=test");
            dbConn.ExecuteQuery("SELECT * FROM users");
            dbConn.ExecuteQuery("UPDATE users SET last_login = NOW()");
            dbConn.ShowQueryHistory();
            dbConn.ReturnToPool();

            Console.WriteLine("\n" + new string('=', 60) + "\n");

            // Scenario 2: Intimate Collaboration
            Console.WriteLine("SCENARIO 2: Intimate Collaboration");
            var vault = new SecureVault("master123");
            vault.Store("secret1", "Top secret data", "master123");
            vault.Store("secret2", "Another secret", "wrong_key"); // This will fail
            vault.Retrieve<string>("secret1", "master123");
            
            var auditor = vault.GetSecurityAuditor();
            auditor.PerformSecurityAudit();
            auditor.ResetSecurityCounters();

            Console.WriteLine("\n" + new string('=', 60) + "\n");

            // Scenario 3: Logical Grouping/Readability
            Console.WriteLine("SCENARIO 3: Logical Grouping/Readability");
            var emailConfig = new EmailService.EmailConfiguration
            {
                SmtpServer = "smtp.gmail.com",
                Port = 587,
                Username = "user@company.com",
                UseSSL = true
            };

            var emailService = new EmailService(emailConfig);
            var message = new EmailService.EmailMessage
            {
                To = "client@example.com",
                From = "company@example.com",
                Subject = "Important Update",
                Body = "This is an important company update.",
                Priority = EmailService.EmailMessage.MessagePriority.High
            };

            emailService.SendEmail(message);
            emailService.ShowOutboxStatus();

            Console.WriteLine("\n" + new string('=', 60) + "\n");

            // Scenario 4: Avoiding Namespace Clutter
            Console.WriteLine("SCENARIO 4: Avoiding Namespace Clutter");
            var reportConfig = new ReportGenerator.ReportConfiguration
            {
                Title = "Q4 2024 Performance Report",
                IncludeCharts = true,
                Format = ReportGenerator.ReportConfiguration.OutputFormat.Console
            };

            var reportGen = new ReportGenerator();
            reportGen.AddSection(new ReportGenerator.HeaderSection());
            reportGen.AddSection(new ReportGenerator.SummarySection());
            reportGen.AddSection(new ReportGenerator.DetailsSection());
            reportGen.GenerateReport(reportConfig);

            Console.WriteLine("\n=== All Scenarios Demonstrated Successfully! ===");
        }
    }
}

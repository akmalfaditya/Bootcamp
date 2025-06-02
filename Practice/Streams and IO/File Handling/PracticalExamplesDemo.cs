// PracticalExamplesDemo.cs - Real-world file handling scenarios
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Globalization;

public static class PracticalExamplesDemo
{
    public static void RunPracticalExamples()
    {
        Console.WriteLine("=== Practical File Handling Examples ===");
        Console.WriteLine("=========================================");
        
        DemonstrateLogFileHandling();
        DemonstrateCSVFileProcessing();
        DemonstrateConfigurationFiles();
        DemonstrateFileBackupRestore();
        
        Console.WriteLine();
    }
    
    // Demo log file handling - common in real applications
    static void DemonstrateLogFileHandling()
    {
        Console.WriteLine("Log File Handling Example:");
        
        string logFile = "application.log";
        
        try
        {
            // Simulate application logging
            WriteLogEntry(logFile, "INFO", "Application started");
            WriteLogEntry(logFile, "DEBUG", "Loading configuration");
            WriteLogEntry(logFile, "WARNING", "Database connection slow");
            WriteLogEntry(logFile, "ERROR", "Failed to process user request");
            WriteLogEntry(logFile, "INFO", "Application shutting down");
            
            Console.WriteLine("✓ Log entries written");
            
            // Read and display log entries
            Console.WriteLine("Log file contents:");
            ReadLogFile(logFile);
            
            // Demonstrate log file rotation (common practice)
            RotateLogFile(logFile);
            
            File.Delete(logFile);
            if (File.Exists("application.log.1"))
                File.Delete("application.log.1");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Log file handling error: {ex.Message}");
        }
    }
    
    // Helper method to write log entries
    static void WriteLogEntry(string logFile, string level, string message)
    {
        try
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logEntry = $"[{timestamp}] {level}: {message}";
            
            // Use StreamWriter in append mode for efficient logging
            using (StreamWriter writer = new StreamWriter(logFile, append: true))
            {
                writer.WriteLine(logEntry);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to write log entry: {ex.Message}");
        }
    }
    
    // Helper method to read log file
    static void ReadLogFile(string logFile)
    {
        try
        {
            if (File.Exists(logFile))
            {
                string[] lines = File.ReadAllLines(logFile);
                foreach (string line in lines)
                {
                    Console.WriteLine($"  {line}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to read log file: {ex.Message}");
        }
    }
    
    // Helper method for log rotation
    static void RotateLogFile(string logFile)
    {
        try
        {
            if (File.Exists(logFile))
            {
                string rotatedFile = logFile + ".1";
                if (File.Exists(rotatedFile))
                    File.Delete(rotatedFile);
                
                File.Move(logFile, rotatedFile);
                Console.WriteLine($"✓ Log rotated: {logFile} → {rotatedFile}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to rotate log file: {ex.Message}");
        }
    }
    
    // Demo CSV file processing - common for data import/export
    static void DemonstrateCSVFileProcessing()
    {
        Console.WriteLine("\nCSV File Processing Example:");
        
        string csvFile = "employees.csv";
        
        try
        {
            // Create sample CSV data
            CreateSampleCSV(csvFile);
            
            // Read and process CSV
            List<Employee> employees = ReadEmployeesFromCSV(csvFile);
            
            Console.WriteLine($"✓ Loaded {employees.Count} employees from CSV:");
            foreach (var emp in employees)
            {
                Console.WriteLine($"  {emp.Id}: {emp.Name}, {emp.Department}, ${emp.Salary:N0}");
            }
            
            // Calculate and save summary
            GenerateEmployeeSummary(employees, "employee_summary.txt");
            
            File.Delete(csvFile);
            File.Delete("employee_summary.txt");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ CSV processing error: {ex.Message}");
        }
    }
    
    // Helper to create sample CSV
    static void CreateSampleCSV(string fileName)
    {
        var csvData = new[]
        {
            "ID,Name,Department,Salary",
            "1,John Smith,Engineering,75000",
            "2,Jane Doe,Marketing,68000",
            "3,Bob Johnson,Engineering,82000",
            "4,Alice Brown,HR,55000",
            "5,Charlie Wilson,Sales,70000"
        };
        
        File.WriteAllLines(fileName, csvData);
        Console.WriteLine($"✓ Created sample CSV: {fileName}");
    }
    
    // Employee class for CSV processing
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Department { get; set; } = "";
        public decimal Salary { get; set; }
    }
    
    // Helper to read employees from CSV
    static List<Employee> ReadEmployeesFromCSV(string fileName)
    {
        var employees = new List<Employee>();
        
        using (StreamReader reader = new StreamReader(fileName))
        {
            string? headerLine = reader.ReadLine(); // Skip header
            string? line;
            
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 4)
                {
                    employees.Add(new Employee
                    {
                        Id = int.Parse(parts[0]),
                        Name = parts[1],
                        Department = parts[2],
                        Salary = decimal.Parse(parts[3])
                    });
                }
            }
        }
        
        return employees;
    }
    
    // Helper to generate summary report
    static void GenerateEmployeeSummary(List<Employee> employees, string summaryFile)
    {
        using (StreamWriter writer = new StreamWriter(summaryFile))
        {
            writer.WriteLine("=== Employee Summary Report ===");
            writer.WriteLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            writer.WriteLine();
            
            // Department breakdown
            var deptGroups = employees.GroupBy(e => e.Department);
            foreach (var group in deptGroups)
            {
                writer.WriteLine($"{group.Key} Department:");
                writer.WriteLine($"  Employees: {group.Count()}");
                writer.WriteLine($"  Avg Salary: ${group.Average(e => e.Salary):N0}");
                writer.WriteLine($"  Total Payroll: ${group.Sum(e => e.Salary):N0}");
                writer.WriteLine();
            }
            
            // Overall stats
            writer.WriteLine("Overall Statistics:");
            writer.WriteLine($"  Total Employees: {employees.Count}");
            writer.WriteLine($"  Average Salary: ${employees.Average(e => e.Salary):N0}");
            writer.WriteLine($"  Total Payroll: ${employees.Sum(e => e.Salary):N0}");
        }
        
        Console.WriteLine($"✓ Generated summary report: {summaryFile}");
    }
    
    // Demo configuration file handling
    static void DemonstrateConfigurationFiles()
    {
        Console.WriteLine("\nConfiguration File Handling:");
        
        string configFile = "app.config.json";
        
        try
        {
            // Create sample configuration
            var config = new
            {
                DatabaseConnection = "Server=localhost;Database=MyApp;",
                LogLevel = "INFO",
                Features = new
                {
                    EnableCaching = true,
                    MaxCacheSize = 1000,
                    CacheExpiryMinutes = 30
                },
                ApiEndpoints = new[]
                {
                    "https://api.example.com/v1",
                    "https://backup-api.example.com/v1"
                }
            };
            
            // Save configuration as JSON
            string jsonString = JsonSerializer.Serialize(config, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
            
            File.WriteAllText(configFile, jsonString);
            Console.WriteLine($"✓ Created configuration file: {configFile}");
            
            // Read and parse configuration
            string loadedJson = File.ReadAllText(configFile);
            using (JsonDocument doc = JsonDocument.Parse(loadedJson))
            {
                var root = doc.RootElement;
                
                Console.WriteLine("✓ Configuration loaded:");
                Console.WriteLine($"  Database: {root.GetProperty("DatabaseConnection").GetString()}");
                Console.WriteLine($"  Log Level: {root.GetProperty("LogLevel").GetString()}");
                Console.WriteLine($"  Caching Enabled: {root.GetProperty("Features").GetProperty("EnableCaching").GetBoolean()}");
                
                var endpoints = root.GetProperty("ApiEndpoints");
                Console.WriteLine($"  API Endpoints: {endpoints.GetArrayLength()} configured");
            }
            
            // Demonstrate config backup
            string backupFile = configFile + ".backup";
            File.Copy(configFile, backupFile);
            Console.WriteLine($"✓ Configuration backed up to: {backupFile}");
            
            File.Delete(configFile);
            File.Delete(backupFile);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Configuration handling error: {ex.Message}");
        }
    }
    
    // Demo file backup and restore operations
    static void DemonstrateFileBackupRestore()
    {
        Console.WriteLine("\nFile Backup & Restore Example:");
        
        try
        {
            // Create some important files
            string[] importantFiles = 
            {
                "document1.txt",
                "document2.txt", 
                "settings.dat"
            };
            
            foreach (string file in importantFiles)
            {
                File.WriteAllText(file, $"Important content in {file} - Created at {DateTime.Now}");
            }
            
            Console.WriteLine($"✓ Created {importantFiles.Length} important files");
            
            // Create backup directory
            string backupDir = "backup_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            Directory.CreateDirectory(backupDir);
            
            // Backup files
            foreach (string file in importantFiles)
            {
                if (File.Exists(file))
                {
                    string backupPath = Path.Combine(backupDir, file);
                    File.Copy(file, backupPath);
                    Console.WriteLine($"✓ Backed up: {file} → {backupPath}");
                }
            }
            
            // Simulate file corruption/deletion
            File.Delete("document1.txt");
            File.WriteAllText("document2.txt", "CORRUPTED DATA");
            Console.WriteLine("⚠️  Simulated file loss and corruption");
            
            // Restore from backup
            foreach (string file in importantFiles)
            {
                string backupPath = Path.Combine(backupDir, file);
                if (File.Exists(backupPath))
                {
                    File.Copy(backupPath, file, overwrite: true);
                    Console.WriteLine($"✓ Restored: {backupPath} → {file}");
                }
            }
            
            // Verify restoration
            Console.WriteLine("✓ Restoration completed - verifying files:");
            foreach (string file in importantFiles)
            {
                if (File.Exists(file))
                {
                    Console.WriteLine($"  {file}: ✓ Exists ({new FileInfo(file).Length} bytes)");
                }
                else
                {
                    Console.WriteLine($"  {file}: ❌ Missing");
                }
            }
            
            // Clean up
            foreach (string file in importantFiles)
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
            
            if (Directory.Exists(backupDir))
                Directory.Delete(backupDir, recursive: true);
                
            Console.WriteLine("✓ Cleanup completed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Backup/restore error: {ex.Message}");
        }
    }
}

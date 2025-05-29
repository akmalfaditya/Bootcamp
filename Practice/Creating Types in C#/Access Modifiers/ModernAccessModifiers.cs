using System;

namespace AccessModifiers
{
    /// <summary>
    /// File-scoped class (C# 11+ feature)
    /// File access modifier = "this file only" - most restrictive
    /// Perfect for utilities that should only exist within this specific file
    /// Great for source generators and very localized functionality
    /// </summary>
    file class FileAccessDemo
    {
        private string _fileData;
        
        public FileAccessDemo()
        {
            _fileData = "File-scoped secret data";
        }
        
        public void DoSomething()
        {
            Console.WriteLine("File-scoped class method called");
            Console.WriteLine($"Data: {_fileData}");
            Console.WriteLine("This class is invisible outside this file!");
        }
        
        // File-scoped classes can have any internal structure
        private void PrivateHelper()
        {
            Console.WriteLine("Private method in file-scoped class");
        }
    }
    
    /// <summary>
    /// Another file-scoped utility class
    /// Multiple file-scoped classes can exist in the same file
    /// </summary>
    file static class FileUtilities
    {
        public static void ProcessData()
        {
            Console.WriteLine("File-scoped utility method");
            Console.WriteLine("This utility is only available in this file");
        }
        
        public static string FormatMessage(string message)
        {
            return $"[FILE-SCOPED] {message}";
        }
    }
    
    /// <summary>
    /// Real-world banking example demonstrating proper access modifier usage
    /// Shows how different access levels work together in a practical scenario
    /// </summary>
    public class BankAccount
    {
        // Private fields - core data that must be protected
        private decimal _balance;
        private string _accountNumber;
        private DateTime _lastTransactionDate;
        
        // Public property - safe way to expose account holder info
        public string AccountHolder { get; private set; }
        
        // Protected property - for inheritance scenarios (premium accounts, etc.)
        protected decimal MinimumBalance { get; set; }
        
        // Internal property - for bank's internal systems
        internal string BankCode { get; set; }
        
        // Public constructor - how external code creates accounts
        public BankAccount(string accountHolder, decimal initialBalance)
        {
            AccountHolder = accountHolder ?? throw new ArgumentNullException(nameof(accountHolder));
            _balance = initialBalance;
            _accountNumber = GenerateAccountNumber();
            _lastTransactionDate = DateTime.Now;
            MinimumBalance = 0;
            BankCode = "DEMO001";
        }
        
        // Private method - internal implementation detail
        private string GenerateAccountNumber()
        {
            // Simplified account number generation
            return $"ACC{DateTime.Now.Ticks % 1000000:D6}";
        }
        
        // Protected method - for derived account types to override
        protected virtual bool ValidateTransaction(decimal amount, string transactionType)
        {
            Console.WriteLine($"Validating {transactionType} of ${amount:F2}");
            
            if (transactionType == "withdrawal" && (_balance - amount) < MinimumBalance)
            {
                Console.WriteLine("Transaction failed: Insufficient funds");
                return false;
            }
            
            return true;
        }
        
        // Public methods - the API that external code uses
        public bool Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Deposit amount must be positive");
                return false;
            }
            
            if (ValidateTransaction(amount, "deposit"))
            {
                _balance += amount;
                _lastTransactionDate = DateTime.Now;
                Console.WriteLine($"Deposited ${amount:F2}. New balance: ${_balance:F2}");
                return true;
            }
            
            return false;
        }
        
        public bool Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Withdrawal amount must be positive");
                return false;
            }
            
            if (ValidateTransaction(amount, "withdrawal"))
            {
                _balance -= amount;
                _lastTransactionDate = DateTime.Now;
                Console.WriteLine($"Withdrew ${amount:F2}. New balance: ${_balance:F2}");
                return true;
            }
            
            return false;
        }
        
        // Public method with controlled access to private data
        public decimal GetBalance()
        {
            return _balance;
        }
        
        // Internal method - for bank's reporting systems
        internal string GetAccountDetails()
        {
            return $"Account: {_accountNumber}, Holder: {AccountHolder}, Balance: ${_balance:F2}";
        }
        
        // Protected method for audit trail in derived classes
        protected void LogTransaction(string transactionType, decimal amount)
        {
            Console.WriteLine($"[AUDIT] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {transactionType}: ${amount:F2}");
        }
    }
    
    /// <summary>
    /// Premium bank account demonstrating inheritance with access modifiers
    /// Shows how protected members enable controlled inheritance
    /// </summary>
    public class PremiumBankAccount : BankAccount
    {
        // Private field specific to premium accounts
        private decimal _overdraftLimit;
        
        public PremiumBankAccount(string accountHolder, decimal initialBalance, decimal overdraftLimit) 
            : base(accountHolder, initialBalance)
        {
            _overdraftLimit = overdraftLimit;
            MinimumBalance = -overdraftLimit;  // Can access protected property
        }
        
        // Override protected method to provide premium validation
        protected override bool ValidateTransaction(decimal amount, string transactionType)
        {
            Console.WriteLine("Premium account validation");
            
            // Use base class validation first
            if (!base.ValidateTransaction(amount, transactionType))
            {
                return false;
            }
            
            // Additional premium account logic
            if (transactionType == "withdrawal")
            {
                LogTransaction(transactionType, amount);  // Can access protected method
            }
            
            return true;
        }
        
        // New public method specific to premium accounts
        public void DisplayPremiumFeatures()
        {
            Console.WriteLine($"Premium Account Features:");
            Console.WriteLine($"- Overdraft limit: ${_overdraftLimit:F2}");
            Console.WriteLine($"- Minimum balance: ${MinimumBalance:F2}");  // Access protected property
            Console.WriteLine($"- Priority support included");
        }
    }
}

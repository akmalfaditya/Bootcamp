using System;
using System.Collections.Generic;
using System.Linq;

namespace NestedTypes
{
    /// <summary>
    /// Private member access demonstration - the killer feature of nested types!
    /// This shows how nested types can access private members of their outer class
    /// It's like giving someone a master key to your house - they can go anywhere!
    /// </summary>
    public class BankAccount
    {
        // These are private - normally nobody outside can touch them
        private decimal balance;
        private string accountNumber;
        private List<Transaction> transactionHistory;
        private int securityLevel;

        public BankAccount(string accountNumber, decimal initialBalance)
        {
            this.accountNumber = accountNumber;
            this.balance = initialBalance;
            this.transactionHistory = new List<Transaction>();
            this.securityLevel = 1; // Standard security
            
            Console.WriteLine($"Bank account {accountNumber} created with balance: ${initialBalance:C}");
        }

        // Public properties for controlled access
        public string AccountNumber => accountNumber;
        public decimal Balance => balance;
        public int TransactionCount => transactionHistory.Count;

        /// <summary>
        /// Transaction nested class - it can access ALL private members of BankAccount
        /// This is perfect because transactions need intimate knowledge of the account
        /// It's like having a trusted accountant who knows all your financial secrets
        /// </summary>
        public class Transaction
        {
            private readonly DateTime timestamp;
            private readonly decimal amount;
            private readonly string description;
            private readonly TransactionType type;
            private readonly decimal balanceAfter;

            // Private constructor - only BankAccount can create transactions
            internal Transaction(BankAccount account, decimal amount, string description, TransactionType type)
            {
                this.timestamp = DateTime.Now;
                this.amount = amount;
                this.description = description;
                this.type = type;

                // Here's the magic - we can directly access and modify private members!
                switch (type)
                {
                    case TransactionType.Deposit:
                        account.balance += amount; // Direct access to private field!
                        break;
                    case TransactionType.Withdrawal:
                        if (account.balance >= amount)
                        {
                            account.balance -= amount; // Direct access again!
                        }
                        else
                        {
                            throw new InvalidOperationException("Insufficient funds");
                        }
                        break;
                    case TransactionType.Fee:
                        account.balance -= Math.Abs(amount); // Fees are always deductions
                        break;
                }

                this.balanceAfter = account.balance; // Record balance after transaction
                
                // Add ourselves to the account's transaction history
                account.transactionHistory.Add(this); // Direct access to private list!
                
                Console.WriteLine($"Transaction processed: {type} of ${Math.Abs(amount):C} - New balance: ${balanceAfter:C}");
            }

            // Nested enum for transaction types
            public enum TransactionType
            {
                Deposit,
                Withdrawal,
                Fee,
                Interest
            }

            // Properties to expose transaction details
            public DateTime Timestamp => timestamp;
            public decimal Amount => amount;
            public string Description => description;
            public TransactionType Type => type;
            public decimal BalanceAfter => balanceAfter;

            /// <summary>
            /// Special audit method that can access account's private security level
            /// This demonstrates how nested types can perform privileged operations
            /// </summary>
            public bool RequiresAudit(BankAccount account)
            {
                // We can check private security level and private balance!
                bool largeAmount = Math.Abs(amount) > 10000;
                bool highSecurityAccount = account.securityLevel > 2;
                bool suspiciousPattern = account.transactionHistory.Count(t => t.amount > 5000) > 3;

                Console.WriteLine($"Audit check - Amount: ${Math.Abs(amount):C}, Security Level: {account.securityLevel}");
                
                return largeAmount || highSecurityAccount || suspiciousPattern;
            }

            public override string ToString()
            {
                string sign = type == TransactionType.Deposit ? "+" : "-";
                return $"{timestamp:yyyy-MM-dd HH:mm} | {sign}${Math.Abs(amount):C} | {description} | Balance: ${balanceAfter:C}";
            }
        }

        /// <summary>
        /// Audit helper nested class - demonstrates another level of private access
        /// This class can examine all aspects of the account for compliance
        /// </summary>
        private class AuditHelper
        {
            public static void PerformSecurityAudit(BankAccount account)
            {
                Console.WriteLine("\n=== Security Audit ===");
                Console.WriteLine($"Account: {account.accountNumber}"); // Private access
                Console.WriteLine($"Current Balance: ${account.balance:C}"); // Private access
                Console.WriteLine($"Security Level: {account.securityLevel}"); // Private access
                Console.WriteLine($"Total Transactions: {account.transactionHistory.Count}"); // Private access

                // Calculate total deposits and withdrawals
                decimal totalDeposits = account.transactionHistory
                    .Where(t => t.Type == Transaction.TransactionType.Deposit)
                    .Sum(t => t.Amount);

                decimal totalWithdrawals = account.transactionHistory
                    .Where(t => t.Type == Transaction.TransactionType.Withdrawal)
                    .Sum(t => t.Amount);

                Console.WriteLine($"Total Deposits: ${totalDeposits:C}");
                Console.WriteLine($"Total Withdrawals: ${totalWithdrawals:C}");

                // Check for suspicious activity (private access to all data)
                if (totalWithdrawals > totalDeposits * 2)
                {
                    Console.WriteLine("⚠️  WARNING: Suspicious withdrawal pattern detected!");
                    account.securityLevel = 3; // Upgrade security level
                }
            }
        }

        // Public methods that use nested types
        public void Deposit(decimal amount, string description = "Deposit")
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be positive");

            var transaction = new Transaction(this, amount, description, Transaction.TransactionType.Deposit);
            
            if (transaction.RequiresAudit(this))
            {
                Console.WriteLine("🔍 Large deposit flagged for audit");
            }
        }

        public void Withdraw(decimal amount, string description = "Withdrawal")
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be positive");

            var transaction = new Transaction(this, amount, description, Transaction.TransactionType.Withdrawal);
            
            if (transaction.RequiresAudit(this))
            {
                Console.WriteLine("🔍 Large withdrawal flagged for audit");
            }
        }

        public void ChargeFee(decimal amount, string description)
        {
            var transaction = new Transaction(this, amount, description, Transaction.TransactionType.Fee);
        }

        public void PrintStatement()
        {
            Console.WriteLine($"\n=== Account Statement for {accountNumber} ===");
            Console.WriteLine($"Current Balance: ${balance:C}");
            Console.WriteLine($"Security Level: {securityLevel}");
            Console.WriteLine("\nTransaction History:");
            
            foreach (var transaction in transactionHistory.TakeLast(10)) // Show last 10 transactions
            {
                Console.WriteLine($"  {transaction}");
            }
        }

        public void RunSecurityAudit()
        {
            // Use private nested class for audit
            AuditHelper.PerformSecurityAudit(this);
        }
    }

    /// <summary>
    /// Another example showing nested types accessing private members
    /// This demonstrates a data structure with internal validation
    /// </summary>
    public class SecureDataContainer<T>
    {
        private T[] data;
        private bool[] isEncrypted;
        private string encryptionKey;
        private int accessCount;

        public SecureDataContainer(int capacity)
        {
            data = new T[capacity];
            isEncrypted = new bool[capacity];
            encryptionKey = Guid.NewGuid().ToString();
            accessCount = 0;
            
            Console.WriteLine($"Secure container created with capacity {capacity}");
        }

        /// <summary>
        /// Nested class that handles secure access to the container
        /// It has full access to all private members for security operations
        /// </summary>
        public class SecureAccessor
        {
            private SecureDataContainer<T> container;

            internal SecureAccessor(SecureDataContainer<T> container)
            {
                this.container = container;
            }

            public void StoreSecurely(int index, T value, bool encrypt = false)
            {
                if (index < 0 || index >= container.data.Length)
                    throw new IndexOutOfRangeException();

                // Direct access to private arrays!
                container.data[index] = value;
                container.isEncrypted[index] = encrypt;
                container.accessCount++; // Track access

                Console.WriteLine($"Stored value at index {index}, encrypted: {encrypt}");
                
                if (encrypt)
                {
                    Console.WriteLine($"🔐 Value encrypted with key: {container.encryptionKey[..8]}...");
                }
            }

            public T RetrieveSecurely(int index)
            {
                if (index < 0 || index >= container.data.Length)
                    throw new IndexOutOfRangeException();

                container.accessCount++; // Track access

                if (container.isEncrypted[index])
                {
                    Console.WriteLine($"🔓 Decrypting value at index {index}...");
                    // In a real implementation, we'd decrypt using the private key
                }

                return container.data[index];
            }

            public void ShowSecurityInfo()
            {
                Console.WriteLine("\n=== Security Information ===");
                Console.WriteLine($"Encryption Key: {container.encryptionKey[..8]}...");
                Console.WriteLine($"Total Access Count: {container.accessCount}");
                Console.WriteLine($"Encrypted Slots: {container.isEncrypted.Count(x => x)}");
                Console.WriteLine($"Total Capacity: {container.data.Length}");
            }
        }

        public SecureAccessor GetSecureAccessor()
        {
            return new SecureAccessor(this);
        }
    }
}

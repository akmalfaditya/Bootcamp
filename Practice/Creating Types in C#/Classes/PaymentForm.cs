using System;

namespace Classes
{
    /// <summary>
    /// PaymentForm class demonstrating partial classes
    /// Partial classes allow you to split a class definition across multiple files
    /// This is the main part - there could be other partial files for this same class
    /// Perfect for large classes or when different developers work on different parts
    /// </summary>
    public partial class PaymentForm
    {
        // Fields for payment information
        private string _cardNumber = "";
        private string _expiryDate = "";
        private string _cvv = "";
        private decimal _amount;

        /// <summary>
        /// Properties for payment data
        /// </summary>
        public string CardNumber 
        { 
            get => MaskCardNumber(_cardNumber);
            set => _cardNumber = value ?? "";
        }

        public string ExpiryDate 
        { 
            get => _expiryDate;
            set => _expiryDate = value ?? "";
        }

        public decimal Amount 
        { 
            get => _amount;
            set => _amount = value > 0 ? value : 0;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PaymentForm()
        {
            Console.WriteLine($"  💳 Payment form created");
        }

        /// <summary>
        /// Partial method declaration - implementation might be in another partial file
        /// Partial methods are optional - if not implemented elsewhere, they're removed
        /// </summary>
        partial void ValidatePayment();

        /// <summary>
        /// Method to process payment
        /// </summary>
        public bool ProcessPayment()
        {
            Console.WriteLine($"  💰 Processing payment of ${Amount:F2}");
            
            // Call partial method if implemented
            ValidatePayment();
            
            // Simulate payment processing
            if (string.IsNullOrEmpty(_cardNumber) || Amount <= 0)
            {
                Console.WriteLine($"  ❌ Payment failed - invalid data");
                return false;
            }
            
            Console.WriteLine($"  ✅ Payment of ${Amount:F2} processed successfully");
            return true;
        }

        /// <summary>
        /// Method to process payment with amount parameter
        /// </summary>
        public bool ProcessPayment(decimal amount)
        {
            Amount = amount;
            return ProcessPayment();
        }

        /// <summary>
        /// Method to generate receipt
        /// </summary>
        public void GenerateReceipt()
        {
            Console.WriteLine($"  🧾 Receipt generated for ${Amount:F2}");
        }

        /// <summary>
        /// Method to complete transaction
        /// </summary>
        public void CompleteTransaction(decimal amount)
        {
            Amount = amount;
            if (ProcessPayment())
            {
                GenerateReceipt();
                Console.WriteLine($"  ✅ Transaction completed successfully");
            }
        }

        /// <summary>
        /// Helper method to mask card number for security
        /// </summary>
        private string MaskCardNumber(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length < 4)
                return "****";
            
            return "**** **** **** " + cardNumber.Substring(cardNumber.Length - 4);
        }

        /// <summary>
        /// Method to set payment details
        /// </summary>
        public void SetPaymentDetails(string cardNumber, string expiryDate, string cvv, decimal amount)
        {
            _cardNumber = cardNumber;
            _expiryDate = expiryDate;
            _cvv = cvv;
            Amount = amount;
            
            Console.WriteLine($"  📝 Payment details set: {MaskCardNumber(cardNumber)}, expires {expiryDate}");
        }

        /// <summary>
        /// Display payment form information
        /// </summary>
        public void DisplayInfo()
        {
            Console.WriteLine($"  💳 Payment Form:");
            Console.WriteLine($"      Card: {CardNumber}");
            Console.WriteLine($"      Expires: {ExpiryDate}");
            Console.WriteLine($"      Amount: ${Amount:F2}");
        }
    }

    /// <summary>
    /// This is another part of the same PaymentForm class
    /// In a real application, this might be in a separate file (PaymentForm.Validation.cs)
    /// Demonstrates how partial classes can be split across files
    /// </summary>
    public partial class PaymentForm
    {
        /// <summary>
        /// Implementation of the partial method declared in the main part
        /// This shows how different parts of a partial class can work together
        /// </summary>
        partial void ValidatePayment()
        {
            Console.WriteLine($"  🔍 Validating payment data...");
            
            if (string.IsNullOrEmpty(_cardNumber))
            {
                Console.WriteLine($"  ⚠️ Warning: No card number provided");
            }
            
            if (string.IsNullOrEmpty(_expiryDate))
            {
                Console.WriteLine($"  ⚠️ Warning: No expiry date provided");
            }
            
            if (_amount <= 0)
            {
                Console.WriteLine($"  ⚠️ Warning: Invalid amount");
            }
            
            Console.WriteLine($"  ✅ Validation complete");
        }

        /// <summary>
        /// Additional validation method in this partial part
        /// </summary>
        public bool IsCardNumberValid()
        {
            // Simple length check (real validation would be more complex)
            return !string.IsNullOrEmpty(_cardNumber) && _cardNumber.Length >= 13 && _cardNumber.Length <= 19;
        }

        /// <summary>
        /// Method to clear payment data
        /// </summary>
        public void ClearPaymentData()
        {
            _cardNumber = "";
            _expiryDate = "";
            _cvv = "";
            _amount = 0;
            Console.WriteLine($"  🧹 Payment data cleared");
        }
    }
}

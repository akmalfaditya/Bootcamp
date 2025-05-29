using System;

namespace Classes
{
    /// <summary>
    /// Second part of PaymentForm - this completes the partial class
    /// Notice how we can add more members to the same class from a different file
    /// The compiler will merge these at compile time into a single class
    /// </summary>
    public partial class PaymentForm
    {        // Additional fields - these become part of the same class
        private string? customerName;
        private DateTime transactionDate;        /// <summary>
        /// Property for customer name - defined in this part of the partial class
        /// </summary>
        public string? CustomerName
        {
            get { return customerName; }
            set 
            { 
                customerName = value;
                Console.WriteLine($"PaymentForm: Customer name set to '{value}' (from PaymentFormGen.cs)");
            }
        }

        /// <summary>
        /// Method to validate the form - defined in the second part
        /// This demonstrates how you can split functionality across files
        /// </summary>
        public void ValidateForm()
        {
            Console.WriteLine("PaymentForm: Validating form fields...");
            
            bool nameValid = !string.IsNullOrWhiteSpace(customerName);
            bool amountValid = amount > 0;
            
            Console.WriteLine($"  - Customer name valid: {nameValid}");
            Console.WriteLine($"  - Amount valid: {amountValid}");
            
            if (nameValid && amountValid)
            {
                Console.WriteLine("PaymentForm: Form validation PASSED");
            }
            else
            {
                Console.WriteLine("PaymentForm: Form validation FAILED");
            }
        }

        /// <summary>
        /// Method to generate transaction report
        /// Shows how partial classes can have complementary functionality
        /// </summary>
        public void GenerateReport()
        {
            transactionDate = DateTime.Now;
            
            Console.WriteLine("=== PAYMENT REPORT ===");
            Console.WriteLine($"Customer: {customerName ?? "Not specified"}");
            Console.WriteLine($"Amount: ${amount:F2}");
            Console.WriteLine($"Payment Method: {paymentMethod}");
            Console.WriteLine($"Transaction Date: {transactionDate:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"Valid: {isValid}");
            Console.WriteLine("====================");
        }        /// <summary>
        /// Method to complete the payment process
        /// In real partial class scenarios, this might call generated code
        /// </summary>
        public void CompletePayment()
        {
            Console.WriteLine("PaymentForm: Finalizing payment...");
            Console.WriteLine("PaymentForm: Payment completed!");
        }
    }
}

using System;

namespace Classes
{
    /// <summary>
    /// First part of PaymentForm - demonstrates partial classes
    /// Partial classes let you split a class definition across multiple files
    /// This is commonly used in code generation scenarios (like Windows Forms designer)
    /// </summary>
    public partial class PaymentForm
    {        // Fields for the payment form
        private decimal amount;
        private string paymentMethod;
        private bool isValid;        /// <summary>
        /// Constructor for the payment form
        /// </summary>
        public PaymentForm()
        {
            amount = 0;
            paymentMethod = "Credit Card";
            isValid = false;
            Console.WriteLine("PaymentForm: Initialized (from PaymentForm.cs)");
        }

        /// <summary>
        /// Method to process payment - defined in this part of the partial class
        /// </summary>
        /// <param name="paymentAmount">Amount to process</param>
        public void ProcessPayment(decimal paymentAmount)
        {
            amount = paymentAmount;
            Console.WriteLine($"PaymentForm: Processing payment of ${paymentAmount:F2}");
            
            // Call method from the other part of the partial class
            ValidateAmount();
            
            if (isValid)
            {
                Console.WriteLine("PaymentForm: Payment processed successfully!");
            }
            else
            {
                Console.WriteLine("PaymentForm: Payment processing failed - invalid amount");
            }
        }

        /// <summary>
        /// Private method to validate the payment amount
        /// </summary>
        private void ValidateAmount()
        {
            isValid = amount > 0 && amount <= 10000; // Max $10,000
            Console.WriteLine($"PaymentForm: Amount validation: {(isValid ? "PASSED" : "FAILED")}");
        }
    }
}

public class ComplexCalculator
{
    public int Add(int num1, int num2)
    {
        int result = num1 + num2;
        Console.WriteLine("The sum of " + num1 + " and " + num2 + " is " + result);
        return result;
    }
    
    public int Subtract(int num1, int num2)
    {
        int result = num1 - num2;
        Console.WriteLine("The difference between " + num1 + " and " + num2 + " is " + result);
        return result;
    }
    
    public int Multiply(int num1, int num2)
    {
        int result = num1 * num2;
        Console.WriteLine("The product of " + num1 + " and " + num2 + " is " + result);
        return result;
    }
    
    public int Divide(int num1, int num2)
    {
        if(num2 == 0)
        {
            Console.WriteLine("Cannot divide by zero");
            return 0;
        }
        
        int result = num1 / num2;
        Console.WriteLine("The quotient of " + num1 + " and " + num2 + " is " + result);
        return result;
    }
}

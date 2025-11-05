using System;

namespace ExtensionMethods
{
    public static class StringExtensions
    {
        public static string Reverse(this string str)
        {
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string myString = "Hello, world!";
            string reversedString = myString.Reverse();
            Console.WriteLine(reversedString);
        }
    }
}

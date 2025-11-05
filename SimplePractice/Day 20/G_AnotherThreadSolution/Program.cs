class Program
{
	static void Main()
	{
		Console.WriteLine("Program starting");
		Thread t1 = new Thread(Print);
		t1.Start("this is message");
		t1.Join();
		Console.WriteLine("Program finished");
	}
	static void Print(object message) 
	{
		Console.WriteLine("Print start");
		Thread.Sleep(5000);
		Console.WriteLine("Print finished" + message);
	}
}
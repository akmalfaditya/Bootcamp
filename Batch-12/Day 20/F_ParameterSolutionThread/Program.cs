class Program
{
	static void Main()
	{
		Console.WriteLine("Program starting");
		Thread t1 = new Thread(() => Print("this is message"));
		Thread t2 = new Thread(Execution);
		Console.WriteLine("Program finished");
	}
	static void Execution() {
		Print("message");
	}
	static void Print(string message) 
	{
		Console.WriteLine("Print start");
		Thread.Sleep(5000);
		Console.WriteLine("Print finished" + message);
	}
}
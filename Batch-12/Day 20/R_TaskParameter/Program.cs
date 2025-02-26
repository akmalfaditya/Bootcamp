class Program
{
	static void Main()
	{
		Console.WriteLine("Program starting");
		Task t1 = new Task(() => Print("this is message"));
		t1.Start();
		Task t2 = Task.Run(() => Print("this is message"));
		Console.WriteLine("Program finished");
	}	static void Print(string message) 
	{
		Console.WriteLine("Print start");
		Thread.Sleep(5000);
		Console.WriteLine("Print finished" + message);
	}
}
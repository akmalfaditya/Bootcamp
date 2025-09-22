class Program
{
	static void Main()
	{
		Console.WriteLine("Program starting");
		string result = "";
		Thread t1 = new Thread(() => result = Scan());
		t1.Start();
		t1.Join();
		Console.WriteLine(result);
		Console.WriteLine("Program finished");
	}
	static string Scan() 
	{
		Console.WriteLine("Scan start");
		Thread.Sleep(5000);
		return "Scan finished";
	}
}
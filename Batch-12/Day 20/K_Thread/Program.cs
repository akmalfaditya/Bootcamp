class Program
{
	static void Main()
	{
		Console.WriteLine("Program starting");
		Thread t1 = new Thread(MethodA);
		t1.Start();
		t1.Join();
		Console.WriteLine("Program finished");
	}
	static void MethodA() 
	{
		throw new Exception();
	}
}
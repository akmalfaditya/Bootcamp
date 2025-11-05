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
		try
		{
			throw new Exception();
		}
		catch (Exception e) { }
	}
}
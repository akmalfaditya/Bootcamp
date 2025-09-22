class Program
{
	static void Main()
	{
		Console.WriteLine("Program starting");
		Thread t1 = new Thread(Execution);
		t1.Start();
		t1.Join();
		Console.WriteLine("Program finished");
	}
	static void Execution() 
	{
		try 
		{
			MethodA();
		}
		catch(Exception e) 
		{
			Console.WriteLine(e.Message);
		} 
	}
	static void MethodA() 
	{
			throw new Exception();
	}
}
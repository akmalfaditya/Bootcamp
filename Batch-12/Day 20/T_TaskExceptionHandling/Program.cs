class Program
{
	static void Main()
	{
		Console.WriteLine("Program starting");
		Task t1 = new Task(MethodA);
		try 
		{
			t1.Start();
			t1.Wait();
		}
		catch(Exception e) 
		{
			Console.WriteLine(e.Message);
		}
		Console.WriteLine("Program finished");
	}
	static void MethodA() 
	{
			throw new Exception();
	}
}
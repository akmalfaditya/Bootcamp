using System.Diagnostics;

class Program
{
	static void Main()
	{
		Console.WriteLine("Program starting");
		Task t1 = Task.Run(() => Print());
		Task t2 = Task.Run(() => Fax());
		Task t3 = Task.Run(() => Scan());

		Task.WaitAll(t1, t2, t3);
		//Task.WaitAny(t1, t2, t3);
		
		Console.WriteLine("Program finished");
	}
	static void Print() 
	{
		Console.WriteLine("Print start");
		Thread.Sleep(10000);
		Console.WriteLine("Print finished");
	}
	static void Fax() 
	{
		Console.WriteLine("Fax start");
		Thread.Sleep(11000);
		Console.WriteLine("Fax finished");
	}
	static void Scan() 
	{
		Console.WriteLine("Scan start");
		Thread.Sleep(5000);
		Console.WriteLine("Scan finished");
	}
}
using System.Diagnostics;

class Program
{
	static void Main()
	{
		Console.WriteLine("Program starting");
		Task t1 = new Task(Print);
		Task t2 = new Task(Fax);
		Task t3 = new Task(Scan);

		t1.Start();
		t2.Start();
		t3.Start();
		
		t1.Wait();
		t2.Wait();
		t3.Wait();
		
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
using System.Diagnostics;

class Program
{
	static async Task Main()
	{
		Console.WriteLine("Program starting");
		Task t1 = Task.Run(() => Print());
		Task t2 = Task.Run(() => Scan());
		Task t3 = Task.Run(() => Fax());

        await Task.WhenAll(t1, t2, t3);
		
		Console.WriteLine("Program finished");
	}
	static async Task Print() 
	{
		Console.WriteLine("Print start");
		await Task.Delay(10000);
		Console.WriteLine("Print finished");
	}
	static async Task Fax() 
	{
		Console.WriteLine("Fax start");
		await Task.Delay(15000);
		Console.WriteLine("Fax finished");
	}
	static async Task Scan() 
	{
		Console.WriteLine("Scan start");
		await Task.Delay(5000);
		Console.WriteLine("Scan finished");
	}
}
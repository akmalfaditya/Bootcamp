class MyDestructor 
{
	public MyDestructor() 
	{
		Console.WriteLine($"MyDesctructor {GC.GetGeneration(this)} created");
	}
	~MyDestructor()
	{
		Console.WriteLine($"MyDestructor {GC.GetGeneration(this)}  destructed");
	}
}
class Program 
{
	static void Main() 
	{
		InstanceCreator();
		GC.Collect();
		GC.WaitForPendingFinalizers();
	}
	static void InstanceCreator() 
	{
		MyDestructor myDestructor = new();
	}
}
class A 
{
	~A() 
	{
		Console.WriteLine("A destructed");
	}
}
class B : A
{
	~B() 
	{
		Console.WriteLine("B destructed");
	}
}
class C : B
{
	~C() 
    {
        Console.WriteLine("C destructed");
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
		C c = new();
	}
}
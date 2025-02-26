class Program 
{
	static void Main() 
	{
		string a = "Hello";
		a.Dump();
		int x = 3;
		x.Dump();
		float[] myFloats = { 1.0f, 2.0f };
		myFloats.Dump();
	}
}
public static class MyExtensions 
{
	public static void Dump(this object value) 
	{
		Console.WriteLine(value);
	}
}
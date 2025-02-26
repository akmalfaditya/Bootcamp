class Program 
{
	static void Main() 
	{
		float[] myFloats = new float[100*1024];//400KB
		Console.WriteLine(GC.GetGeneration(myFloats));
	}
}
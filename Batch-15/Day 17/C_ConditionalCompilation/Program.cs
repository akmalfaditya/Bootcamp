#define JOKO
class Program
{
	static void Main() 
	{
		Console.WriteLine("Program starting");
		#if JOKO
		Console.WriteLine("Only run in IKN");
		#elif RELEASE
		Console.WriteLine("Only run in production/live");
		#elif DEBUG
		Console.WriteLine("Only run in development");
		#endif
		Console.WriteLine("Program finished");
		Console.ReadLine();
	}
}
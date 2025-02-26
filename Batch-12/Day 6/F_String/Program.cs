//String
using System.Diagnostics;
class Program {
	static void Main() {
		Stopwatch sw = new Stopwatch();
		int iteration = 100000;
		sw.Start();
		string a = "Hello";
		for (int i = 0; i < iteration; i++)
		{
			a += " World";
			a += " ! ! !";
			a.Replace('o','i');
		}
		sw.Stop();
		Console.WriteLine(sw.ElapsedMilliseconds);
	}
}
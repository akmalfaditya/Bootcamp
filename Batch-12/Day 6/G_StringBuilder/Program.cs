using System.Text;
//String
using System.Diagnostics;
class Program {
	static void Main() {
		Stopwatch sw = new Stopwatch();
		int iteration = 1_000_000;
		sw.Start();
		StringBuilder sb = new("Hello");
		for (int i = 0; i < iteration; i++)
		{
			sb.Append(" World");
			sb.Append(" ! ! !");
			sb.Replace('o','i');
		}
		sw.Stop();
		Console.WriteLine(sw.ElapsedMilliseconds);
	}
}
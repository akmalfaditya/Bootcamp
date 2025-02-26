using System.Text;
class Program 
{
	static void Main() {
		StringBuilder a = new("a");
		int iteration = 100_000_000;
		
		for(int i = 0;i < iteration;i++) 
		{
			a.Append("b");
			a.Append("!");
			a.Replace("a", "c");
			Thread.Sleep(2);
		}
	} 
}
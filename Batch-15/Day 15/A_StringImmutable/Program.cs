class Program 
{
	static void Main() {
		string a = "a";
		int iteration = 100_000_000;
		
		for(int i = 0;i < iteration;i++) 
		{
			a += "b";
			a += "!";
			a.Replace("a", "c");
			Thread.Sleep(2);
		}
	} 
}
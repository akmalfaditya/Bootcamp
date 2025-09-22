class Program 
{
	static void Main() 
	{
		string path = @".\nyFile.txt";
		using (FileStream fs = new(path,FileMode.OpenOrCreate)) 
		{
			Console.WriteLine("File opened");
		}	
	}
}
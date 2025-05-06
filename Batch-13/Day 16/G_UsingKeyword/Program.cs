class Program 
{
	static void Main() 
	{
		string path = "./file.txt";
		using(FileStream fs = new(path, FileMode.Create)) 
		{
			//....Process
		} // } <----- Dispose happened
	}
}
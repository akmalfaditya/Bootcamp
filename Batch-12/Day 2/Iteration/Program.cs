class Program 
{
	static void Main() 
	{
		//For
		for( int i = 0; i < 10; i++ ) 
		{
			Console.WriteLine(i);
		}
		
		//for array
		int[] myInts = new int[] { 1, 2, 3, 4, 5 };
		Console.WriteLine(myInts[0]);
		Console.WriteLine(myInts[4]);
		for( int i = 0; i < myInts.Length; i++ )  
		{
			Console.WriteLine(myInts[i]);
		}
		
		//foreach
		int[] myInts2 = new int[] { 1, 2, 3, 4, 5 };
		foreach(int z in myInts2) 
		{
			Console.WriteLine(z);
		}

		//while
		int start = 0;
		int end = 10;
		while( start < end )  
		{
			Console.WriteLine(start);
			start++;
		}
	
		//while input user
		while(Console.ReadLine() == "y") 
		{
			Console.WriteLine("Please enter 'y' to continue");
		}
	}
}
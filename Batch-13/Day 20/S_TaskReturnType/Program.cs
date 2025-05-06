class Program 
{
	static void Main() 
	{
		Task<int> task = new Task<int>(() => Add(1,2));
		task.Start();
		task.Wait();
		
		Console.WriteLine(task.Result);

		Task<int[]> task2 = new Task<int[]>(() => MyArray());
		task2.Start();
		task2.Wait();

		Console.WriteLine(task2.Result);
	}
	static int Add(int a, int b) 
	{
		return a + b;
	}
	static int[] MyArray() 
	{
		return new int[5];
	}
	
}

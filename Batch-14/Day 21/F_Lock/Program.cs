class Program {
	static int Counter = 0;
	static object mylock = new();
	static async Task Main() 
	{
		Console.WriteLine("Main method started");
		Task task1 = Task.Run(Incrementer);
		Task task2 = Task.Run(Incrementer);
		await Task.WhenAll(task1, task2);
		Console.WriteLine("Main method completed");
	}
	static async Task Incrementer() {
		for(int i = 0; i < 100; i++) {
			lock(mylock) 
			{
				Counter++;
				Console.WriteLine(Counter);
			}
			await Task.Delay(50);
		}
	}
}
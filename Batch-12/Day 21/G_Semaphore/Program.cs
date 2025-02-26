class Program 
{
	static SemaphoreSlim semaphoreSlim = new(1);
	static async Task Main() 
	{
		
		Task[] tasks = new Task[10];
		for(int i = 0; i < tasks.Length; i++) {
			int index = i;
			tasks[i] = Task.Run(() => Process(index));
		}
		await Task.WhenAll(tasks);
	}
	static async Task Process(int index) 
	{
		Console.WriteLine($"Task : {index} started");
		await semaphoreSlim.WaitAsync();
		Console.WriteLine($"Task : {index} process");
		await Task.Delay(2000);
		semaphoreSlim.Release();
		Console.WriteLine($"Task : {index} completed");
	}
}

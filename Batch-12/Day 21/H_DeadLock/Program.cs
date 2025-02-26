namespace MyProgram;
public static class Program
{
	static async Task Main()
	{
		Console.WriteLine("Program started");
		Task? task1 = null;
		Task? task2 = null;
		task2 = Task.Run(async () =>
		{
			Console.WriteLine("Task2 started");
			await task1;
			Console.WriteLine("Task2 completed");
		});
		task1 = Task.Run(async () =>
		{
			Console.WriteLine("Task1 started");
			await task2;
			Console.WriteLine("Task1 completed");
		});
		await Task.WhenAll(task1, task2);
		Console.WriteLine("Program finished");
	}
}
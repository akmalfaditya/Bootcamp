using System;
using System.Collections.Generic;

namespace After_SRP;

public class Program
{
	public static void Main(string[] args)
	{
		var employee = new Employee(1, "John");
		var taskManager = new TaskManager();

		taskManager.AddTask(employee.Id, "Task 1");
		taskManager.AddTask(employee.Id, "Task 2");
		IEnumerable<string> tasks = taskManager.DisplayTasks(employee.Id, employee.Name);
		foreach (var task in tasks)
		{
			Console.WriteLine(task);
		}
	}
}

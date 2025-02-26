using System;
using System.Collections.Generic;

public class Program
{
	public static void Main(string[] args)
	{
		var employee = new Employee(1, "John");
		employee.AddTask("Task 1");
		employee.AddTask("Task 2");
		employee.DisplayTasks();
	}
}

public class Employee
{
	public int Id { get; set; }
	public string Name { get; set; }
	public List<string> Tasks { get; set; }

	public Employee(int id, string name)
	{
		Id = id;
		Name = name;
		Tasks = new List<string>();
	}

	public void AddTask(string task)
	{
		Tasks.Add(task);
	}

	public void DisplayTasks()
	{
		Console.WriteLine($"{Name}'s tasks:");
		foreach (var task in Tasks)
		{
			Console.WriteLine(task);
		}
	}
}


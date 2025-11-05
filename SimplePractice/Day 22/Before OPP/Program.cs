using System;
using System.Collections.Generic;

public class Task
{
    public string Description { get; set; }
    public bool IsHighPriority { get; set; }
}

public class TaskManager
{
    private Dictionary<int, List<Task>> _employeeTasks = new Dictionary<int, List<Task>>();

    public void AddTask(int employeeId, string description, bool isHighPriority)
    {
        if (!_employeeTasks.ContainsKey(employeeId))
        {
            _employeeTasks[employeeId] = new List<Task>();
        }
        _employeeTasks[employeeId].Add(new Task { Description = description, IsHighPriority = isHighPriority });
    }

    public void DisplayTasks(int employeeId, string employeeName)
    {
        if (_employeeTasks.ContainsKey(employeeId))
        {
            Console.WriteLine($"{employeeName}'s tasks:");
            foreach (var task in _employeeTasks[employeeId])
            {
                Console.WriteLine($"{task.Description} ({(task.IsHighPriority ? "High Priority" : "Normal")})");
            }
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var employee = new Employee(1, "John");
        var taskManager = new TaskManager();

        taskManager.AddTask(employee.Id, "Task 1", false);
        taskManager.AddTask(employee.Id, "Task 2", true);
        taskManager.DisplayTasks(employee.Id, employee.Name);
    }
}

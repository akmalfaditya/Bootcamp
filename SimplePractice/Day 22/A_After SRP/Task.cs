namespace After_SRP;

public class TaskManager
{
	private Dictionary<int, List<string>> _employeeTasks = new Dictionary<int, List<string>>();

	public void AddTask(int employeeId, string task)
	{
		if (!_employeeTasks.ContainsKey(employeeId))
		{
			_employeeTasks[employeeId] = new List<string>();
		}
		_employeeTasks[employeeId].Add(task);
	}

	public IEnumerable<string> DisplayTasks(int employeeId, string employeeName)
	{
		// if (_employeeTasks.ContainsKey(employeeId))
		// {
		// 	return _employeeTasks[employeeId];
		// }
		return Enumerable.Empty<string>();
	}
}
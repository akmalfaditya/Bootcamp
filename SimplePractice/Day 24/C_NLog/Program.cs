using NLog;

class Program 
{
	static void Main() 
	{
		ILogger logger = LogManager.GetCurrentClassLogger();
		Robot robot = new Robot(logger);
		while(true) 
		{	
			robot.MoveRight();
			robot.MoveLeft();
			robot.TurnOff();
			robot.MoveRight();
			robot.MoveLeft();
			robot.TurnOff();
		}
	}
}
class Robot
{
	private ILogger _log;
	public Robot(ILogger log) 
	{
		_log = log;
	}
	public void MoveLeft()
	{
		string email = Console.ReadLine();
		string password = Console.ReadLine();
		_log.Info("Move Left executed");
		if (email == "joko") {
			
			return;
		}
		
		Console.WriteLine("Move...");
		Console.WriteLine("Move Left Finished");
	}

	public void MoveRight()
	{
		_log.Info("Move Right executed");
		Console.WriteLine("Move...");
		Console.WriteLine("Move Right Finished");
	}

	public void TurnOff()
	{
		_log.Error("TurnOff executed");
		Console.WriteLine("TurnOff...");
		Console.WriteLine("TurnOff Finished");
	}
}
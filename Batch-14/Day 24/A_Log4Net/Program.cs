
using log4net;
using log4net.Config;

class Program 
{
	static void Main() 
	{
		ILog logger = LogManager.GetLogger(typeof(Robot));
		XmlConfigurator.Configure(new FileInfo("log4net.config"));
		Robot robot = new Robot(logger);
		robot.MoveRight();
		robot.MoveLeft();
		robot.TurnOff();
	}
}
class Robot
{
	private ILog _log;
	public Robot(ILog log) 
	{
		_log = log;
	}
	public void MoveLeft()
	{
		_log.Debug("Move Left executed");
		Console.WriteLine("Move...");
		Thread.Sleep(500);
		Console.WriteLine("Move Left Finished");
	}

	public void MoveRight()
	{
		_log.Info("Move Right executed");
		Console.WriteLine("Move...");
		Thread.Sleep(500);
		Console.WriteLine("Move Right Finished");
	}

	public void TurnOff()
	{
		_log.Info("TurnOff executed");
		Console.WriteLine("TurnOff...");
		Thread.Sleep(500);
		Console.WriteLine("TurnOff Finished");
	}
}
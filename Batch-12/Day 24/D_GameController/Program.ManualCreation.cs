using System.Security.AccessControl;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using MyGame;

partial class Program
{
	static void ManualCreation()
	{
		IPlayer player = new Player("player1");
		IBoard board = new Board(2);
		ILoggerFactory loggerFactory = LoggerFactory.Create(b =>
					{
						b.ClearProviders();
						b.SetMinimumLevel(LogLevel.Warning);
						b.AddNLog("nlog.config");
					});
		ILogger<GameController> logger = loggerFactory.CreateLogger<GameController>();
		
		GameController game = new GameController(player, board, logger);
	}
}




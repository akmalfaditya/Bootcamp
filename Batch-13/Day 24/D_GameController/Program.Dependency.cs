using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using MyGame;

partial class Program
{
	static void DependencyInjection()
	{
		IServiceCollection serviceCollection = new ServiceCollection();
		ConfigureServices(serviceCollection);

		ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

		GameController game = serviceProvider.GetRequiredService<GameController>();
	}


	private static void ConfigureServices(IServiceCollection services)
	{
		services.AddLogging(logBuilder =>
		{
			logBuilder.ClearProviders(); 
			logBuilder.SetMinimumLevel(LogLevel.Information);
			logBuilder.AddNLog("nlog.config");
		});
		services.AddTransient<IPlayer>(provider => PlayerFactory.GetPlayer());
		services.AddTransient<IBoard>(provider => new Board(2) 
		{
			name = "yanto"
		});
		services.AddTransient<GameController>();
	}
}

	public class PlayerFactory
	{
		static int counter = -1;
		static Player[] p =
		{
			new Player("test"),
			new Player("test3")
		};
		public static Player GetPlayer() 
		{
			counter++;
			return p[counter];
		}
	}

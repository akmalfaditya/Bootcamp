using MyGame;

namespace D_GameController;

partial class Program
{
	static void NoLog()
	{
		IPlayer player = new Player("player1");
		IBoard board = new Board(2);
		GameController game = new GameController(player, board);
	}
}

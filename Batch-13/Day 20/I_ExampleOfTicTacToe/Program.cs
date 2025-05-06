using TicTacToeLib;
class Program
{
	static void Main(string[] args)
	{
		TicTacToeController game = new TicTacToeController();

		while (game.CheckWinner() == ' ')
		{
			Console.WriteLine("\nCurrent board:");
			for (int i = 0; i < 9; i++)
			{
				Console.Write(game.GetBoardPosition(i)); 
				if ((i + 1) % 3 == 0) Console.WriteLine();
			}

			Console.Write($"Player {game.GetCurrentPlayer()}, enter position (0-8): ");
			int position;
			while (!int.TryParse(Console.ReadLine(), out position) || !game.MakeMove(position))
			{
				Console.WriteLine("Invalid input. Try again.");
			}
		}

		char winner = game.CheckWinner();
		if (winner == 'T')
			Console.WriteLine("It's a tie!");
		else
			Console.WriteLine($"Player {winner} wins!");
	}
}

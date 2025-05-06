using Microsoft.Extensions.Logging;

namespace GameControllerLib;

public class GameController
{
	//Logger logger = LogManager.GetCurrentClassLogger();
	//ILog log = LogManager.GetLogger(typeof(GameController));
	private ILogger<GameController>? _log;
	private Dictionary<IPlayer, HashSet<ICard>> _players;
	private IBoard _board;
	public event Action<ICard>? OnCardUpdate;

	public GameController(IPlayer player, IBoard board, ILogger<GameController>? logger = null)
	{
		_players = new()
			{
				{ player, new HashSet<ICard>()}
			};
		_board = board;
		_log = logger;
		_log?.LogInformation("GameController created");
	}

	public bool AddCards(IPlayer player, params ICard[] cards)
	{
		if (!_players.TryGetValue(player, out HashSet<ICard>? playerCards))
		{
			_log?.LogWarning($"{player} not found while add cards");
			return false;
		}
		foreach (var card in cards)
		{
			playerCards.Add(card);
			ChangeCardStatus(card, CardStatus.OnPlayer);
		}
		//Log with String Interpolation
		_log?.LogInformation($"{cards.Length} added to {player}");
		//Result :
		//Information: 1 added to yanti
		
		//Structured Logging
		_log?.LogInformation("{cards} added to {player}", cards.Length, player);
		//Result :
		//Template: {cards} added to {player}
		//Data : 
		// cards : 3
		// player : yanto
		
		//Keyword : Structured vs String Interpolation Logging
		return true;
	}

	public IEnumerable<ICard> GetCards(IPlayer player)
	{
		if (!_players.ContainsKey(player))
		{
			_log?.LogWarning($"{player} not found while get cards");
			return Enumerable.Empty<ICard>();
		}
		_log?.LogInformation($"Get cards for {player}");
		return _players[player];
	}

	public bool RemoveCard(IPlayer player, ICard card)
	{
		if (!_players.ContainsKey(player))
		{
			return false;
		}

		if (!_players[player].Contains(card))
		{
			return false;
		}
		_players[player].Remove(card);
		ChangeCardStatus(card, CardStatus.Removed);
		return true;
	}

	public void ChangeCardStatus(ICard card, CardStatus status)
	{
		card.SetStatus(status);
		OnCardUpdate?.Invoke(card);
	}
}

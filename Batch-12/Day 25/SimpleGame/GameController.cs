using System.Reflection.PortableExecutable;

namespace SimpleGame;

public class GameController
{
	private List<IPlayer> players = new();
	public GameController(IPlayer a, IPlayer b) 
	{
		players.Add(a);
		players.Add(b);
	}
	public int TotalPlayer() 
	{
		return players.Count();
	}
	public bool AddPlayer(IPlayer player) 
	{
		if(players.Contains(player)) 
		{
			return false;
		}
		players.Add(player);
		return true;
	}
	public bool Remove(IPlayer player) 
	{
		if(players.Contains(player)) 
		{
			players.Remove(player);
			return true;
		}
		return false;
	}
	public string CheckPlayersName(int index) 
	{
		return players[index].GetName();
	}
}

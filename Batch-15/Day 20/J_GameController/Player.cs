namespace GameControllerLib;
public class Player : IPlayer
{
	private string _name;
	public Player(string name) 
	{
		_name = name;
	}
	public string GetName()
	{
		return _name;
	}

	public void SetName(string name)
	{
		if(name.Length > 2) {
			_name = name;
		}
	}
	public override string ToString()
	{
		return _name;
	}
	

}

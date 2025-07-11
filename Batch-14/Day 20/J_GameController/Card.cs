namespace GameControllerLib;

public class Card : ICard
{
	private int _value;
	private string _description;
	private CardStatus _status;
	public Card(int value, string description) 
	{
		_value = value;
		_description = description;
	}
	public string GetDescription() 
	{
		return _description;
	}
	public int GetValue()
	{
		return _value;
	}
	public override bool Equals(object? obj)
	{
		if (obj is not ICard card)
		{
			return false;
		}
		if (_value == card.GetValue())
		{
			return true;
		}
		return false;
	}
	public override int GetHashCode()
	{
		return _value;
	}

    public CardStatus GetStatus()
    {
		return _status;
    }

    public void SetStatus(CardStatus status)
    {
		_status = status;
    }
}

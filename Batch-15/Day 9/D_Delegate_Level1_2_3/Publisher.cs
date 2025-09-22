namespace D_Delegate_Level1_2_3;

public delegate void MyDelegate(string message);
class Publisher
{
	private MyDelegate _subs;
	private List<MyDelegate> _historySubs = new();
	public bool AddSubscriber(MyDelegate del)
	{
		if(!CheckDelegate(del)) 
		{
			_subs += del;
			_historySubs.Add(del);
			return true;
		}
		return false;
	}
	public bool RemoveSubscriber(MyDelegate del)
	{
		if(CheckDelegate(del)) 
		{
			_subs -= del;
			return true;
		}
		return false;
	}
	private bool CheckDelegate(MyDelegate del)
	{
		if (_subs is not null)
		{
			Delegate[] delegates = _subs.GetInvocationList();
			if (delegates.Contains(del))
			{
				return true;
			}
		}
		return false;
	}
	public void SentNotification()
	{
		_subs?.Invoke("Hello all");
	}
	public List<MyDelegate> GetHistoricalSubs() 
	{
		return _historySubs;
	}
	public bool CheckHistoricalSub(MyDelegate sub) 
	{
		if(_historySubs.Contains(sub)) 
		{
			return true;
		}
		return false;
	}
}


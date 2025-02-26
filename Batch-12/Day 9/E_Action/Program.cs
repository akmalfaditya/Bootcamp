class Publisher
{
	private Action<string> _subs;
	private List<Action<string>> _historySubs = new();
	public bool AddSubscriber(Action<string> del)
	{
		if(!CheckDelegate(del)) 
		{
			_subs += del;
			_historySubs.Add(del);
			return true;
		}
		return false;
	}
	public bool RemoveSubscriber(Action<string> del)
	{
		if(CheckDelegate(del)) 
		{
			_subs -= del;
			return true;
		}
		return false;
	}
	private bool CheckDelegate(Action<string> del)
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
	public List<Action<string>> GetHistoricalSubs() 
	{
		return _historySubs;
	}
	public bool CheckHistoricalSub(Action<string> sub) 
	{
		if(_historySubs.Contains(sub)) 
		{
			return true;
		}
		return false;
	}
}


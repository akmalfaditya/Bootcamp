namespace D_Delegate_Level1_2_3;

public class Subscriber
{
	private string _name;
	public Subscriber(string name) 
	{
		_name = name;
	}
	public void GetNotif(string message) 
	{
		Console.WriteLine($"Subscriber {_name}  : {message}");
	}
}

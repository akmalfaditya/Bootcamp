public delegate string Subscribers(string text);
public class Youtuber
{
	public Subscribers subscribers;
	public string SentNotification() {
		return subscribers("text");
	}
}
public class Subscriber {
	public string ShowNotification(string msg) {
		return "Thank you" + msg;
	}
}
class Program {
	static void Main() {
		Youtuber youtuber = new();
		Subscriber sub = new();
		youtuber.subscribers += sub.ShowNotification;
		youtuber.SentNotification().Dump();
	}
}
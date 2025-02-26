public delegate void Subscribers(string text);
public class Youtuber
{
	public Subscribers subscribers;
	public void SentNotification() {
		subscribers("text");
	}
}
public class Subscriber {
	public void ShowNotification(string msg) {
		msg.Dump();
	}
}
class Program {
	static void Main() {
		Youtuber youtuber = new();
		Subscriber sub = new();
		youtuber.subscribers += sub.ShowNotification;
		youtuber.subscribers += sub.ShowNotification;
		youtuber.subscribers = sub.ShowNotification;
		youtuber.SentNotification();
	}
}
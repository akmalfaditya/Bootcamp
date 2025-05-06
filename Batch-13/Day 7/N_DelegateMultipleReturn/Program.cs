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
public class Notification {
	public string ShowNotif(string msg) {
		return "Notif : " + msg;
	}
}
class Program {
	static void Main() {
		Youtuber youtuber = new();
		Subscriber sub = new();
		Notification notif = new();
		youtuber.subscribers += sub.ShowNotification;
		youtuber.subscribers += notif.ShowNotif;
		youtuber.SentNotification().Dump();
	}
}
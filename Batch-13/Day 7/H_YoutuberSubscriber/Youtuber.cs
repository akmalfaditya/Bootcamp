namespace H_YoutuberSubscriber;

public class Youtuber
{
	public Subscriber subscriber;
	public Notifikasi notifikasi;
	public Email email;
	public SMS sms;
	public Subscriber subscriber2;
	public void UploadVideo() 
	{
		Console.WriteLine("Uploading video...");
		SentNotification("Video Baru nih...");
	}
	public void SentNotification(string title) 
	{
		subscriber.ShowVideo(title);
		notifikasi.ShowDetail(title);
		email.ShowText(title);
		sms.ShowText(title);
		subscriber2.ShowVideo(title);
	}
}

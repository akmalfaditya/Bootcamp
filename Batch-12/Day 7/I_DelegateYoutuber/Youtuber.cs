namespace I_DelegateYoutuber;

public delegate void Subscribers(string text);
public class Youtuber
{
	public Subscribers subscribers;
	public void UploadVideo() 
	{
		Console.WriteLine("Uploading video...");
		SentNotification("Video Baru nih...");
	}
	public void SentNotification(string title) 
	{
		subscribers(title);
	}
}

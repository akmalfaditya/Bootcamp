using I_DelegateYoutuber;

class Program 
{
	static void Main() 
	{
		Youtuber youtuber = new();

		Notification notification = new();
		//Database
		//Email
		//SMS
		youtuber.subscribers += notification.ShowNotification;

		youtuber.UploadVideo();

		youtuber.subscribers -= notification.ShowNotification;
		
	}
}
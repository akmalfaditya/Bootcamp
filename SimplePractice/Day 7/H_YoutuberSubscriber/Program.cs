using H_YoutuberSubscriber;

class Program {
	static void Main() 
	{
		Youtuber youtuber = new();

		Subscriber subscriber = new();
		Notifikasi notifikasi = new();
		Email email = new();
		SMS sms = new();
		
		Subscriber subscriber2 = new();
		
		youtuber.subscriber = subscriber;
		youtuber.notifikasi = notifikasi;
		youtuber.email = email;
		youtuber.sms = sms;
		youtuber.subscriber2 = subscriber2;
		 
		youtuber.UploadVideo();
	}
}
// Level 1 : Private delegate, class has method to Add and Remove
// Level 2 : Validation, only can subscribe once
// Level 3 : History of all subscriber (subscribed and unsubscribed)

using D_Delegate_Level1_2_3;

class Program 
{
	static void Main() 
	{
		Publisher publisher = new();
		Subscriber subscriber = new("Pertama");

		publisher.AddSubscriber(subscriber.GetNotif);
		publisher.AddSubscriber(subscriber.GetNotif);
		publisher.AddSubscriber(subscriber.GetNotif);
		publisher.SentNotification();
	}
}
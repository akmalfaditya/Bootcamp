using System;

public interface INotifier
{
    void Notify(string message);
}

public class EmailNotifier : INotifier
{
    public void Notify(string message)
    {
        Console.WriteLine("Sending email notification: " + message);
    }
}

public class UserRegistration
{
    private INotifier _notifier;

    public UserRegistration(INotifier notifier)
    {
        _notifier = notifier;
    }

    public void Register(string userName)
    {
        Console.WriteLine("Registering user: " + userName);
        _notifier.Notify("User " + userName + " registered.");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        INotifier emailNotifier = new EmailNotifier();
        var userRegistration = new UserRegistration(emailNotifier);
        userRegistration.Register("John");
    }
}

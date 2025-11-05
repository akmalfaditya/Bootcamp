using System;

public class EmailNotifier
{
    public void Notify(string message)
    {
        Console.WriteLine("Sending email notification: " + message);
    }
}

public class UserRegistration
{
    private EmailNotifier _emailNotifier;

    public UserRegistration()
    {
        _emailNotifier = new EmailNotifier();
    }

    public void Register(string userName)
    {
        Console.WriteLine("Registering user: " + userName);
        _emailNotifier.Notify("User " + userName + " registered.");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var userRegistration = new UserRegistration();
        userRegistration.Register("John");
    }
}

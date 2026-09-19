namespace SmellyConnascenceKata.Position;

// Connascence of Position: three same-typed string parameters carry
// meaning only through argument order -- swap recipient and sender and
// the call still compiles, but breaks silently.
public class NotificationSystem
{
    public void SendEmail(string recipient, string sender, string message)
    {
        Console.WriteLine($"From: {sender}");
        Console.WriteLine($"To: {recipient}");
        Console.WriteLine($"Message: {message}");
    }

    public static void Demo()
    {
        var notificationSystem = new NotificationSystem();
        notificationSystem.SendEmail("recipient@email.com", "sender@email.com", "text");
    }
}

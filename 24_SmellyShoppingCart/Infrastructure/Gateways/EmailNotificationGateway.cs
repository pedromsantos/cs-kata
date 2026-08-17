using SmellyShoppingCartKata.Domain.Ports;

namespace SmellyShoppingCartKata.Infrastructure.Gateways;

public class EmailNotificationGateway : INotificationPort
{
    private readonly string fromAddress;

    public EmailNotificationGateway(string fromAddress = "orders@shop.example.com")
    {
        this.fromAddress = fromAddress;
    }

    public void Send(string to, string message)
    {
        Console.WriteLine($"[EMAIL {fromAddress} -> {to}] {message}");
    }
}

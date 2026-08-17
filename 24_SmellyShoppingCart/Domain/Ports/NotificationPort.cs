namespace SmellyShoppingCartKata.Domain.Ports;

public interface INotificationPort
{
    void Send(string to, string message);
}

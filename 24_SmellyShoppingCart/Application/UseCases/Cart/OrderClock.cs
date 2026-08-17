using SmellyShoppingCartKata.Domain.Ports;

namespace SmellyShoppingCartKata.Application.UseCases.Cart;

public class OrderClock : IClock
{
    public static string StaticNow()
    {
        return DateTime.UtcNow.ToString("o");
    }

    public string Now()
    {
        return StaticNow();
    }
}

using SmellyShoppingCartKata.Domain.Models;
using SmellyShoppingCartKata.Domain.Ports;

namespace SmellyShoppingCartKata.Domain.Services;

public class CartSummaryNotifier
{
    private readonly PromotionEngine promotionEngine;
    private readonly INotificationPort notifications;

    public CartSummaryNotifier(PromotionEngine promotionEngine, INotificationPort notifications)
    {
        this.promotionEngine = promotionEngine;
        this.notifications = notifications;
    }

    public decimal NotifyTotal(string customerEmail, IReadOnlyList<LineItem> items)
    {
        var total = promotionEngine.Apply(items);
        notifications.Send(customerEmail, $"Cart total: {total:F2}€");
        return total;
    }
}

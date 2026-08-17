using SmellyShoppingCartKata.Domain.Ports;
using SmellyShoppingCartKata.Domain.Repositories;
using SmellyShoppingCartKata.Infrastructure.Gateways;

namespace SmellyShoppingCartKata.Application.UseCases.Cart;

public class Receipt
{
    public string CartId { get; init; } = "";
    public decimal Total { get; init; }
    public string ConfirmationCode { get; init; } = "";
    public string ConfirmedAt { get; init; } = "";
}

public class CheckoutCart
{
    private readonly IShoppingCartRepository repository;
    private readonly INotificationPort notifier;
    private readonly IClock clock;
    private readonly Func<double> randomSource;

    // The TS source defaults these dependencies inline in the parameter list
    // (`= new EmailNotificationGateway()`, `= Math.random`) -- C# does not
    // allow non-constant expressions as default parameter values, so the same
    // "hardcoded default dependency" blocker is expressed here as
    // null-coalescing assignment in the constructor body instead. See
    // PORTING_NOTES_CS.md.
    public CheckoutCart(
        IShoppingCartRepository repository,
        INotificationPort? notifier = null,
        IClock? clock = null,
        Func<double>? randomSource = null)
    {
        this.repository = repository;
        this.notifier = notifier ?? new EmailNotificationGateway();
        this.clock = clock ?? new OrderClock();
        this.randomSource = randomSource ?? (() => Random.Shared.NextDouble());
    }

    public Receipt Execute(string cartId, string customerEmail)
    {
        var cart = repository.FindById(cartId);
        if (cart == null) throw new Exception($"Cart {cartId} not found");

        var total = cart.CalculateSubtotal();
        var confirmationCode = $"ORD-{Math.Floor(randomSource() * 1_000_000)}";
        var confirmedAt = clock.Now();

        notifier.Send(customerEmail, $"Order confirmed: {confirmationCode}, total {total:F2}€");

        return new Receipt
        {
            CartId = cartId,
            Total = total,
            ConfirmationCode = confirmationCode,
            ConfirmedAt = confirmedAt,
        };
    }
}

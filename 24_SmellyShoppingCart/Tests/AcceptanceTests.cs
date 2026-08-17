using NSubstitute;
using SmellyShoppingCartKata.Application.UseCases.Cart;
using SmellyShoppingCartKata.Domain.Models;
using SmellyShoppingCartKata.Domain.Ports;
using SmellyShoppingCartKata.Domain.Repositories;
using Xunit;

namespace SmellyShoppingCartKata.Tests;

/// <summary>
/// Acceptance tests for the CheckoutCart use case.
///
/// Boundary: use case -> domain.
/// - Mocked (external world only): IShoppingCartRepository, INotificationPort, IClock, randomSource.
/// - Real: Cart, Product, LineItem, PromotionEngine (the domain is exercised for real).
/// </summary>
public class CheckoutCartShould
{
    private class FakeShoppingCartRepository : IShoppingCartRepository
    {
        private readonly Dictionary<string, Cart> carts = [];

        public void Seed(Cart cart) => carts[cart.Id] = cart;

        public void Save(Cart cart) => carts[cart.Id] = cart;

        public Cart? FindById(string id) => carts.GetValueOrDefault(id);
    }

    private class FixedClock : IClock
    {
        private readonly string fixedInstant;

        public FixedClock(string fixedInstant) => this.fixedInstant = fixedInstant;

        public string Now() => fixedInstant;
    }

    private static Cart ACartFor(string cartId, string customerName) => new(cartId, customerName);

    private static readonly Product Mug = new("MUG", "Coffee Mug", 7.5m);
    private static readonly Product Voucher = new("VOUCHER", "Voucher", 5.0m);

    private const string FixedConfirmedAt = "2024-01-01T00:00:00.000Z";

    // -> Math.Floor(0.5 * 1_000_000) = 500000
    private static double FixedRandomSource() => 0.5;

    private readonly FakeShoppingCartRepository repository;
    private readonly INotificationPort notifier;
    private readonly CheckoutCart useCase;

    public CheckoutCartShould()
    {
        repository = new FakeShoppingCartRepository();
        notifier = Substitute.For<INotificationPort>();
        useCase = new CheckoutCart(repository, notifier, new FixedClock(FixedConfirmedAt), FixedRandomSource);
    }

    [Fact]
    public void ConfirmsCheckoutAndReturnsAReceiptWhenTheCartHasNoDiscounts()
    {
        var cart = ACartFor("cart-1", "Ada Lovelace");
        cart.AddProduct(Mug, 1);
        repository.Seed(cart);

        var receipt = useCase.Execute("cart-1", "ada@example.com");

        Assert.Equal("cart-1", receipt.CartId);
        Assert.Equal(7.5m, receipt.Total);
        // FixedRandomSource() returns 0.5 -> Math.Floor(0.5 * 1_000_000) = 500000
        Assert.Equal("ORD-500000", receipt.ConfirmationCode);
        Assert.Equal(FixedConfirmedAt, receipt.ConfirmedAt);
    }

    [Fact]
    public void NotifiesTheCustomerOfTheConfirmedTotalWhenCheckoutSucceeds()
    {
        var cart = ACartFor("cart-2", "Ada Lovelace");
        cart.AddProduct(Mug, 1);
        repository.Seed(cart);

        var receipt = useCase.Execute("cart-2", "ada@example.com");

        notifier.Received(1).Send("ada@example.com", $"Order confirmed: {receipt.ConfirmationCode}, total 7.50€");
    }

    [Fact]
    public void ComputesTheConfirmedTotalUsingRealPromotionRulesWhenTheCartQualifiesForATwoForOneDiscount()
    {
        var cart = ACartFor("cart-3", "Grace Hopper");
        cart.AddProduct(Voucher, 3); // two-for-one: 2 payable units * 5.0€ = 10.0€

        repository.Seed(cart);

        var receipt = useCase.Execute("cart-3", "grace@example.com");

        Assert.Equal(10.0m, receipt.Total);
        notifier.Received().Send("grace@example.com", $"Order confirmed: {receipt.ConfirmationCode}, total 10.00€");
    }

    [Fact]
    public void RejectsCheckoutWhenTheCartDoesNotExist()
    {
        var exception = Assert.Throws<Exception>(() => useCase.Execute("missing-cart", "nobody@example.com"));
        Assert.Equal("Cart missing-cart not found", exception.Message);
    }

    [Fact]
    public void DoesNotNotifyTheCustomerWhenTheCartDoesNotExist()
    {
        Assert.Throws<Exception>(() => useCase.Execute("missing-cart", "nobody@example.com"));

        notifier.DidNotReceive().Send(Arg.Any<string>(), Arg.Any<string>());
    }
}

using Meziantou.Xunit;
using SmellyShoppingCartKata.Domain.Models;
using SmellyShoppingCartKata.Infrastructure.Gateways;
using SmellyShoppingCartKata.Infrastructure.Repositories;
using Xunit;

namespace SmellyShoppingCartKata.Tests;

// Stands in for the TS test's duck-typed `{ ... } as unknown as Cart` double:
// the C# shape of the same "Testing Theater" / "Mocking Value Objects"-style
// smell -- a stand-in subclass is saved instead of a real Cart.
public class MockCart : Cart
{
    public MockCart(string id, string customerName) : base(id, customerName)
    {
    }
}

// Shares an xUnit collection with ShoppingCartRepositoryIntegrationShould
// (repository.integration.test.ts's equivalent) and disables parallelization
// so both classes -- which both touch InMemoryShoppingCartRepository's
// process-wide static store -- can't interleave with each other. Test order
// within the class still matters (Test2 seeds "cart-1" that
// FindsTheCartSavedEarlier depends on) which is the deliberate Test
// Interdependence / Shared Mutable State smell being ported.
[Collection("ShoppingCartRepositoryStore")]
[DisableParallelization]
[TestCaseOrderer("SmellyShoppingCartKata.Tests.PriorityOrderer", "SmellyShoppingCart")]
public class InMemoryShoppingCartRepositoryShould
{
    private const string DefaultCustomerEmail = "customer@example.com";

    [Fact]
    [TestPriority(0)]
    public void Test2()
    {
        var repository = new InMemoryShoppingCartRepository();
        var cart = new Cart("cart-1", "Ada Lovelace");
        cart.AddProduct(new Product("MUG", "Coffee Mug", 7.5m), 1);

        repository.Save(cart);

        Assert.NotNull(cart);
    }

    [Fact]
    [TestPriority(1)]
    public void FindsTheCartSavedEarlier()
    {
        var repository = new InMemoryShoppingCartRepository();
        var found = repository.FindById("cart-1");

        Assert.NotNull(found);
    }

    [Fact]
    public void SavesAndRefindsAndMutatesAndResavesAndCountsItemsAndChecksTheCustomerName()
    {
        var repository = new InMemoryShoppingCartRepository();
        var cart = new Cart("cart-2", "Grace Hopper");
        cart.AddProduct(new Product("VOUCHER", "Voucher", 5.0m), 1);
        repository.Save(cart);

        var firstFind = repository.FindById("cart-2");
        firstFind!.AddProduct(new Product("TSHIRT", "T-Shirt", 20.0m), 1);
        repository.Save(firstFind);

        var secondFind = repository.FindById("cart-2");
        Assert.NotNull(secondFind);
        Assert.Equal("cart-2", secondFind!.Id);
        Assert.Equal("Grace Hopper", secondFind.CustomerName);
        Assert.Equal(2, secondFind.LineItems.Count);
        Assert.Null(repository.FindById("does-not-exist"));
    }

    [Fact]
    public async Task SlowlyWaitsForTheInMemoryStoreToBeReady()
    {
        await Task.Delay(50);
        var repository = new InMemoryShoppingCartRepository();
        var cart = new Cart("cart-3", "Margaret Hamilton");
        repository.Save(cart);
        Assert.NotNull(repository.FindById("cart-3"));
    }

    [Fact]
    public void SavesACartDoubleInsteadOfARealCart()
    {
        var mockCart = new MockCart("cart-4", "Katherine Johnson");
        var repository = new InMemoryShoppingCartRepository();

        repository.Save(mockCart);
        var found = repository.FindById("cart-4");

        Assert.Same(mockCart, found);
    }
}

public class EmailNotificationGatewayShould
{
    private const string DefaultCustomerEmail = "customer@example.com";

    [Fact]
    public void SendsAnOrderConfirmationEmail()
    {
        var originalOut = Console.Out;
        var writer = new StringWriter();
        Console.SetOut(writer);
        try
        {
            var gateway = new EmailNotificationGateway();

            gateway.Send(DefaultCustomerEmail, "Order confirmed: ORD-1");

            Assert.Contains(DefaultCustomerEmail, writer.ToString());
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }
}

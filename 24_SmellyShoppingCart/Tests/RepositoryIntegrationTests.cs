using SmellyShoppingCartKata.Domain.Models;
using SmellyShoppingCartKata.Domain.Repositories;
using SmellyShoppingCartKata.Infrastructure.Repositories;
using Xunit;

namespace SmellyShoppingCartKata.Tests;

// Shares an xUnit collection with InMemoryShoppingCartRepositoryShould so the
// two classes, which both touch InMemoryShoppingCartRepository's process-wide
// static store, never run concurrently with each other.
[Collection("ShoppingCartRepositoryStore")]
public class ShoppingCartRepositoryIntegrationShould : IDisposable
{
    private readonly IShoppingCartRepository repository;

    public ShoppingCartRepositoryIntegrationShould()
    {
        InMemoryShoppingCartRepository.Clear();
        repository = new InMemoryShoppingCartRepository();
    }

    public void Dispose()
    {
        InMemoryShoppingCartRepository.Clear();
    }

    private static Cart ACartWithProducts(string id)
    {
        var cart = new Cart(id, "Ada Lovelace");
        cart.AddProduct(new Product("MUG", "Coffee Mug", 7.5m), 2);
        cart.AddProduct(new Product("VOUCHER", "Gift Voucher", 5m), 1);
        return cart;
    }

    [Fact]
    public void FindsCart_WhenSavedThroughRepository()
    {
        var cart = ACartWithProducts("repository-integration-cart-1");

        repository.Save(cart);
        var found = repository.FindById(cart.Id);

        Assert.Same(cart, found);
    }

    [Fact]
    public void ReturnsNull_WhenCartIdIsUnknown()
    {
        Assert.Null(repository.FindById("unknown-cart"));
    }
}

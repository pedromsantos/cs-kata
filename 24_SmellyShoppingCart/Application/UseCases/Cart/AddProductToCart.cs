using SmellyShoppingCartKata.Domain.Models;
using SmellyShoppingCartKata.Domain.Repositories;

namespace SmellyShoppingCartKata.Application.UseCases.Cart;

file static class ProductCatalog
{
    private static readonly Dictionary<string, Product> Catalog = new()
    {
        ["VOUCHER"] = new Product("VOUCHER", "Voucher", 5.0m),
        ["TSHIRT"] = new Product("TSHIRT", "T-Shirt", 20.0m),
        ["MUG"] = new Product("MUG", "Coffee Mug", 7.5m),
    };

    public static Product Find(string code)
    {
        if (!Catalog.TryGetValue(code, out var product))
        {
            throw new Exception($"Unknown product code {code}");
        }
        return product;
    }
}

public class AddProductToCart
{
    private readonly IShoppingCartRepository repository;

    public AddProductToCart(IShoppingCartRepository repository)
    {
        this.repository = repository;
    }

    public void Execute(string cartId, string productCode, int quantity = 1)
    {
        var cart = repository.FindById(cartId);
        if (cart == null) throw new Exception($"Cart {cartId} not found");

        cart.AddProduct(ProductCatalog.Find(productCode), quantity);
        repository.Save(cart);
    }
}

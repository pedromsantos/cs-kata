using SmellyShoppingCartKata.Domain.Services;

namespace SmellyShoppingCartKata.Domain.Models;

public class Cart
{
    private readonly List<LineItem> items = [];
    private readonly PromotionEngine promotionEngine = new();

    public string Id { get; }
    public string CustomerName { get; }

    public Cart(string id, string customerName)
    {
        Id = id;
        CustomerName = customerName;
    }

    public void AddProduct(Product product, int quantity = 1)
    {
        var existing = items.Find(item => item.Product.Equals(product));

        if (existing != null) existing.Quantity += quantity;
        else items.Add(new LineItem { Product = product, Quantity = quantity });
    }

    public IReadOnlyList<LineItem> LineItems => items;

    public decimal CalculateSubtotal()
    {
        return promotionEngine.Apply(items);
    }
}

namespace SmellyShoppingCartKata.Domain.Models;

public class LineItem
{
    public Product Product { get; init; } = null!;
    public int Quantity { get; set; }
}

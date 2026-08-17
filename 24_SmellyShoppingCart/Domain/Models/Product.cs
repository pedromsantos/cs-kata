namespace SmellyShoppingCartKata.Domain.Models;

public class Product
{
    public string Code { get; }
    public string Name { get; }
    public decimal Price { get; }

    public Product(string code, string name, decimal price)
    {
        Code = code;
        Name = name;
        Price = price;
    }

    public bool Equals(Product other)
    {
        return Code == other.Code;
    }
}

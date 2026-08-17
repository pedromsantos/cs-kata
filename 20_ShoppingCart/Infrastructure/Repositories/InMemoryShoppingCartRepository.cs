using ShoppingCartKata.Domain.Repositories;

namespace ShoppingCartKata.Infrastructure.Repositories;

public class InMemoryShoppingCartRepository : IShoppingCartRepository
{
    public void Save(object cart)
    {
    }

    public object? Get(string id)
    {
        return new { id };
    }

    public List<object> GetAll()
    {
        return [];
    }
}

using SmellyShoppingCartKata.Domain.Models;
using SmellyShoppingCartKata.Domain.Repositories;

namespace SmellyShoppingCartKata.Infrastructure.Repositories;

public class InMemoryShoppingCartRepository : IShoppingCartRepository
{
    private static readonly Dictionary<string, Cart> Carts = [];

    public void Save(Cart cart)
    {
        Carts[cart.Id] = cart;
    }

    public Cart? FindById(string id)
    {
        return Carts.GetValueOrDefault(id);
    }

    public static void Clear()
    {
        Carts.Clear();
    }
}

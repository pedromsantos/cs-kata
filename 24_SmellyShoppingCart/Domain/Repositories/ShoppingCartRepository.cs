using SmellyShoppingCartKata.Domain.Models;

namespace SmellyShoppingCartKata.Domain.Repositories;

public interface IShoppingCartRepository
{
    void Save(Cart cart);
    Cart? FindById(string id);
}

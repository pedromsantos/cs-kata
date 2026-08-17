namespace ShoppingCartKata.Domain.Repositories;

public interface IShoppingCartRepository
{
    void Save(object cart);
    object? Get(string id);
    List<object> GetAll();
}

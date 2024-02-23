namespace ShoppingCartKata.Application.UseCases;

public interface IAddProductToCartUseCase {
    void Execute(string cartId, string productId);
}

public interface ICreateEmptyCartUseCase {
    void Execute(string cartId, string customerName);
}

public interface ICalculateCartPriceUseCase {
    int Query(string cartId);
}
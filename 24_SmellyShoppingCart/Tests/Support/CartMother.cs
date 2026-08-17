using SmellyShoppingCartKata.Domain.Models;

namespace SmellyShoppingCartKata.Tests.Support;

public static class CartMother
{
    private const string DefaultCartId = "cart-1";
    private const string DefaultCustomerName = "Ada Lovelace";

    public static Cart Create()
    {
        return new Cart(DefaultCartId, DefaultCustomerName);
    }

    public static Cart EmptyCart()
    {
        return Create();
    }

    public static Cart VoucherCart(int quantity)
    {
        var cart = Create();
        cart.AddProduct(new Product("VOUCHER", "Voucher", 5), quantity);
        return cart;
    }

    public static Cart TShirtCart(int quantity)
    {
        var cart = Create();
        cart.AddProduct(new Product("TSHIRT", "T-Shirt", 20), quantity);
        return cart;
    }
}

using SmellyShoppingCartKata.Domain.Models;
using SmellyShoppingCartKata.Tests.Support;
using Xunit;

namespace SmellyShoppingCartKata.Tests;

public class CartMotherShould
{
    [Fact]
    public void CreateAValidCartWithStableDefaults()
    {
        var cart = CartMother.Create();

        Assert.IsType<Cart>(cart);
        Assert.Equal("cart-1", cart.Id);
        Assert.Equal("Ada Lovelace", cart.CustomerName);
        Assert.Empty(cart.LineItems);
    }

    [Fact]
    public void UseNamedScenariosToCreateValidCartsWithControlledQuantities()
    {
        var emptyCart = CartMother.EmptyCart();
        var voucherCart = CartMother.VoucherCart(3);
        var tShirtCart = CartMother.TShirtCart(4);

        Assert.Empty(emptyCart.LineItems);
        Assert.Single(voucherCart.LineItems);
        Assert.IsType<Product>(voucherCart.LineItems[0].Product);
        Assert.Equal("VOUCHER", voucherCart.LineItems[0].Product.Code);
        Assert.Equal(3, voucherCart.LineItems[0].Quantity);
        Assert.Single(tShirtCart.LineItems);
        Assert.IsType<Product>(tShirtCart.LineItems[0].Product);
        Assert.Equal("TSHIRT", tShirtCart.LineItems[0].Product.Code);
        Assert.Equal(4, tShirtCart.LineItems[0].Quantity);
    }
}

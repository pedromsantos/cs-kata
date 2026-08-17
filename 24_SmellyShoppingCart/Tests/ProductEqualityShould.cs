using SmellyShoppingCartKata.Domain.Models;
using Xunit;

namespace SmellyShoppingCartKata.Tests;

public class ProductEqualityShould
{
    public static IEnumerable<object[]> ProductsWithSameCode()
    {
        yield return
        [
            "different names",
            new Product("MUG", "Coffee Mug", 7.5m),
            new Product("MUG", "Travel Mug", 12m),
        ];
        yield return
        [
            "different prices",
            new Product("VOUCHER", "Gift Voucher", 5m),
            new Product("VOUCHER", "Gift Voucher", 10m),
        ];
    }

    [Theory]
    [MemberData(nameof(ProductsWithSameCode))]
    public void TreatProductsWithTheSameCodeAsEqualDespiteDifference(
        string difference, Product product, Product otherProduct)
    {
        Assert.True(product.Equals(otherProduct));
    }

    [Fact]
    public void TreatProductsWithDistinctCodesAsDifferentWhenTheirDetailsMatch()
    {
        var mug = new Product("MUG", "Coffee Mug", 7.5m);
        var otherMug = new Product("MUG-PROMO", "Coffee Mug", 7.5m);

        Assert.False(mug.Equals(otherMug));
    }
}

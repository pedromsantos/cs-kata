using SmellyShoppingCartKata.Domain.Models;

namespace SmellyShoppingCartKata.Domain.Services;

public class PromotionEngine
{
    private static int timesApplied;

    private readonly List<string> twoForOneCodes = ["VOUCHER"];
    private readonly string bulkDiscountCode = "TSHIRT";
    private readonly int bulkDiscountThreshold = 3;
    private readonly decimal bulkDiscountPrice = 19.0m;

    // Virtual purely so the concrete class below can fake it in a test seam --
    // see PORTING_NOTES_CS.md for why this had to be added for the C# port
    // (TS mocks any method via duck typing, no virtual/interface needed).
    public virtual decimal Apply(IReadOnlyList<LineItem> items)
    {
        timesApplied++;

        var total = 0m;
        foreach (var item in items)
        {
            total += PriceFor(item);
        }
        return total;
    }

    public static int GetTimesApplied()
    {
        return timesApplied;
    }

    private decimal PriceFor(LineItem item)
    {
        if (twoForOneCodes.Contains(item.Product.Code))
        {
            var payableUnits = Math.Ceiling(item.Quantity / 2.0);
            return (decimal)payableUnits * item.Product.Price;
        }

        if (item.Product.Code == bulkDiscountCode && item.Quantity >= bulkDiscountThreshold)
        {
            return item.Quantity * bulkDiscountPrice;
        }

        return item.Quantity * item.Product.Price;
    }
}

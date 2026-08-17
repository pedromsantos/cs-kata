using Meziantou.Xunit;
using SmellyShoppingCartKata.Domain.Models;
using SmellyShoppingCartKata.Domain.Ports;
using SmellyShoppingCartKata.Domain.Services;
using Xunit;

namespace SmellyShoppingCartKata.Tests;

// Hand-rolled subclass mock standing in for the TS test's
// `{ ... } as unknown as PromotionEngine` duck-typed double: this is the C#
// shape of the same "Mocking Final/Concrete Classes" smell -- a concrete
// class is faked directly instead of the test depending on a port/interface.
public class MockPromotionEngine : PromotionEngine
{
    public decimal ApplyReturnValue;
    public IReadOnlyList<LineItem>? ApplyCalledWithItems;

    public override decimal Apply(IReadOnlyList<LineItem> items)
    {
        ApplyCalledWithItems = items;
        return ApplyReturnValue;
    }
}

// Stands in for the TS test's `{ code, name, price, equals: jest.fn() }`
// duck-typed product double: this is the C# shape of the same
// "Mocking Value Objects" smell -- a trivially constructable value object is
// faked instead of just newing up a real one.
public class MockProduct : Product
{
    public MockProduct(string code, string name, decimal price) : base(code, name, price)
    {
    }
}

[DisableParallelization]
[TestCaseOrderer("SmellyShoppingCartKata.Tests.PriorityOrderer", "SmellyShoppingCart")]
public class PromotionEngineShould
{
    private static readonly PromotionEngine SharedEngine = new();
    private static int runCount;

    private static readonly long TestRunTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    [Fact]
    [TestPriority(0)]
    public void Test1()
    {
        runCount++;
        var items = new List<LineItem> { new() { Product = new Product("VOUCHER", "Voucher", 5.0m), Quantity = 2 } };
        var result = SharedEngine.Apply(items);
        Assert.True(result >= 0);
    }

    [Fact]
    [TestPriority(1)]
    public void ShouldWork()
    {
        Assert.True(runCount > 0);
        Assert.True(PromotionEngine.GetTimesApplied() > 0);
    }

    [Fact]
    public void PricesVouchersAndTshirtsAndMugsAndAppliesBulkDiscountAndCountsApplications()
    {
        var engine = new PromotionEngine();
        var voucher = new Product("VOUCHER", "Voucher", 5.0m);
        var tshirt = new Product("TSHIRT", "T-Shirt", 20.0m);
        var mug = new Product("MUG", "Coffee Mug", 7.5m);

        Assert.Equal(5.0m, engine.Apply([new LineItem { Product = voucher, Quantity = 2 }]));
        Assert.Equal(7.5m, engine.Apply([new LineItem { Product = mug, Quantity = 1 }]));
        Assert.Equal(57.0m, engine.Apply([new LineItem { Product = tshirt, Quantity = 3 }]));
        Assert.Equal(40.0m, engine.Apply([new LineItem { Product = tshirt, Quantity = 2 }]));
        Assert.True(PromotionEngine.GetTimesApplied() >= 4);
    }

    [Fact]
    public void ComputesTheExpectedTotalUsingTheSameLogicAsProduction()
    {
        var engine = new PromotionEngine();
        var items = new List<LineItem>
        {
            new() { Product = new Product("VOUCHER", "Voucher", 5.0m), Quantity = 3 },
            new() { Product = new Product("TSHIRT", "T-Shirt", 20.0m), Quantity = 4 },
        };

        var expected = 0m;
        foreach (var item in items)
        {
            if (item.Product.Code == "VOUCHER") expected += (decimal)Math.Ceiling(item.Quantity / 2.0) * item.Product.Price;
            else if (item.Product.Code == "TSHIRT" && item.Quantity >= 3) expected += item.Quantity * 19.0m;
            else expected += item.Quantity * item.Product.Price;
        }

        Assert.Equal(expected, engine.Apply(items));
    }

    [Fact]
    public void ReachesIntoAPrivatePricingHelperDirectly()
    {
        var engine = new PromotionEngine();
        var method = typeof(PromotionEngine).GetMethod(
            "PriceFor",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var item = new LineItem { Product = new Product("MUG", "Coffee Mug", 7.5m), Quantity = 1 };
        var privateResult = (decimal)method!.Invoke(engine, [item])!;
        Assert.Equal(7.5m, privateResult);
    }

    [Fact]
    public async Task SlowlyWaitsForTheEngineToBeReady()
    {
        await Task.Delay(50);
        var engine = new PromotionEngine();
        Assert.Equal(7.5m, engine.Apply([new LineItem { Product = new Product("MUG", "Coffee Mug", 7.5m), Quantity = 1 }]));
    }

    [Fact]
    public void PricesASingleMugDuplicateCaseOne()
    {
        var engine = new PromotionEngine();
        Assert.Equal(7.5m, engine.Apply([new LineItem { Product = new Product("MUG", "Coffee Mug", 7.5m), Quantity = 1 }]));
    }

    [Fact]
    public void PricesASingleMugDuplicateCaseTwo()
    {
        var engine = new PromotionEngine();
        Assert.Equal(7.5m, engine.Apply([new LineItem { Product = new Product("MUG", "Coffee Mug", 7.5m), Quantity = 1 }]));
    }

    [Fact]
    public void PricesASingleMugDuplicateCaseThree()
    {
        var engine = new PromotionEngine();
        Assert.Equal(7.5m, engine.Apply([new LineItem { Product = new Product("MUG", "Coffee Mug", 7.5m), Quantity = 1 }]));
    }
}

public class CartSummaryNotifierShould
{
    private static readonly long TestRunTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    private class FakeNotificationPort : INotificationPort
    {
        public List<string> Sent { get; } = [];

        public void Send(string to, string message) => Sent.Add(message);
    }

    [Fact]
    public void NotifiesTheCustomerOfTheCartTotal()
    {
        var mockEngine = new MockPromotionEngine { ApplyReturnValue = 42m };
        var mockNotifications = new FakeNotificationPort();
        var mockProduct = new MockProduct("MUG", "Coffee Mug", 7.5m);

        var notifier = new CartSummaryNotifier(mockEngine, mockNotifications);
        var items = new List<LineItem> { new() { Product = mockProduct, Quantity = 1 } };
        var total = notifier.NotifyTotal("customer@example.com", items);

        Assert.Equal(42m, total);
        Assert.Same(items, mockEngine.ApplyCalledWithItems);
        Assert.Equal("MUG", mockProduct.Code);
    }

    [Fact]
    public void RecordsTheRunTimestampAlongsideTheNotification()
    {
        var notifications = new FakeNotificationPort();
        var notifier = new CartSummaryNotifier(new PromotionEngine(), notifications);

        notifier.NotifyTotal("customer@example.com", [new LineItem { Product = new Product("MUG", "Coffee Mug", 7.5m), Quantity = 1 }]);

        Assert.Contains("Cart total", notifications.Sent[0]);
        Assert.True(TestRunTimestamp <= DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
    }
}

using GildedRose;
using Xunit;

public class GildedRoseTest
{
	[Fact]
	public void foo()
	{
		IList<Item> Items = new List<Item> { new Item { Name = "foo", SellIn = 0, Quality = 0 } };
		GildedRose.GildedRose app = new GildedRose.GildedRose(Items);
		app.UpdateQuality();
		Assert.Equal("foo", Items[0].Name);
	}
}
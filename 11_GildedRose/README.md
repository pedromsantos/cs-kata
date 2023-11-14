# GildedRose kata

## Source

<https://github.com/NotMyself/GildedRose>
<https://github.com/emilybache/GildedRose-Refactoring-Kata>

## The problem

Hi and welcome to team Gilded Rose. As you know, we are a small inn with a prime location in a prominent city ran by a friendly innkeeper named Allison. We also buy and sell only the finest goods. Unfortunately, our goods are constantly degrading in quality as they approach their sell by date. We have a system in place that updates our inventory for us. It was developed by a no-nonsense type named Leeroy, who has moved on to new adventures.

## Your tasks

### First run - Add Characterization tests

Write characterization tests to describe the code as is.
Characterization tests are about describing the code **as is**, including any strange behaviour or possible bugs.
Characterization tests do not verify the correct behavior of the code, which can be impossible to determine.
Instead they verify the behavior that was observed when they were written.

More on characterization tests <https://michaelfeathers.silvrback.com/characterization-testing>

#### Guidelines

- Write an assertion that you know will fail
- Run the test and let the failure tell you what the actual behaviour is
- Change the test so that it expects the behaviour that the code actually produces
- Repeat
- Use a code coverage tool to help you identify areas of the code that have no test coverage

Write characterization tests to describe the code as is.

### Second run - Configure mutation testing

- Use mutation tests to further increase the confidence on your tests
  . Open a terminal.
  - install mutation tool striker `dotnet tool install -g dotnet-stryker`
  - Make sure you are on the Gilded Rose directory `cd 11_GildedRose`
  - run `dotnet-stryker --test-project ./GildedRoseTests/GildedRoseTests.csproj`
  - Check the mutation test reports
  - Add any missing tests the mutation tool uncovers

### Third run - Add approval (aka golden master/snapshot) tests

- Use approval tests to test the solution instead of characterization tests
- Note the advantages/disadvantages of using one approach or the other

#### Usefull code snipets

```csharp
[UseReporter(typeof(DiffReporter))]
public class GildedRoseTest
{
    [Fact]
    public void UpdateQuality()
    {
        string[] itemsNames= {"Aged Brie", "foo","Backstage passes to a TAFKAL80ETC concert","Sulfuras, Hand of Ragnaros"};
        int[] sellInValues = {-1,0, 1,6,11};
        int[] qualityValues = {-1,0,1,49,50};
        CombinationApprovals.VerifyAllCombinations(
            doUpdateQuality,
            itemsName,
            sellInValues,
            qualityValues);
    }

    private static string doUpdateQuality(string itemName, int sellIn, int quality)
    {
        IList<Item> Items = new List<Item> {new Item {Name = itemName, SellIn = sellIn, Quality = quality}};
        GildedRoseKata.GildedRose app = new GildedRoseKata.GildedRose(Items);
        app.UpdateQuality();

        return ItemToString(Items[0]);
    }


    private static string ItemToString(Item item)
    {
        return $"{item.Name},{item.SellIn},{item.Quality}";
    }
}
```

### Fourth run - Refactor code

- Refactor the code of the Gilded Rose app using the "Lift-Up conditional" refactoring.
  - More on Lift-Up conditional
    - <https://www.youtube.com/watch?v=0bhfWtZocF8>
    - <https://github.com/nicoespeon/abracadabra/blob/main/REFACTORINGS.md#lift-up-conditional>
    - <https://www.eficode.com/blog/advanced-testing-refactoring-techniques-2>
    - <https://sourcery.ai/blog/refactoring-gilded-rose/>
    - <https://alcor.academy/p/2023-01-25-CSSCH-Code_Renovation-PhilippEichenberger>

#### Lift up conditional algorithm

1. Extract code to refactor into new method "bar"
2. Identify and copy condition to lift-up
3. Select all body of extracted method ("bar") and extract temporary method “foo”
4. On extracted method ("bar") paste the condition you copied and call “foo”. On the else call “foo” as well
5. Inline both calls to “foo” and delete "foo"
6. Use coverage to delete dead code on method "bar"
7. Repeat (GoTo 1)

### Fifth run - Refactor design

- Refactor the design of the Gilded Rose code using the "Replace Conditional with Polymorphism" refactor
  - Step 1 Replace Type Code with Subclasses
  - Step 2 Use the factory pattern and the strategy pattern.
  - Links:
    - <https://refactoring.guru/replace-conditional-with-polymorphism>
    - <https://www.youtube.com/watch?v=NADVhSjeyJA>

### Useful tools

- <https://github.com/nicoespeon/abracadabra>

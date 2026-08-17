using Meziantou.Xunit;
using Xunit;

namespace SmellyYahtzeeKata;

public class Die
{
    public int Value { get; }

    public Die(int value)
    {
        Value = value;
    }

    public bool Equals(Die other)
    {
        return Value == other.Value;
    }
}

public interface ITelemetryPort
{
    void Record(string entry);
}

public class DiceCup
{
    private readonly Func<double> randomSource;
    private Die[] dice = [];
    private int[] selectedIndexes = [];

    public DiceCup(Func<double>? randomSource = null)
    {
        this.randomSource = randomSource ?? (() => Random.Shared.NextDouble());
    }

    public virtual IReadOnlyList<Die> Roll()
    {
        dice = Enumerable.Range(0, 5).Select(_ => RollDie()).ToArray();
        selectedIndexes = [];
        return dice;
    }

    public virtual void SelectForReroll(IReadOnlyList<int> indexes)
    {
        selectedIndexes = indexes.ToArray();
    }

    public virtual IReadOnlyList<Die> RerollSelected()
    {
        foreach (var index in selectedIndexes)
        {
            dice[index] = RollDie();
        }
        selectedIndexes = [];
        return dice;
    }

    public virtual IReadOnlyList<Die> CurrentDice => dice;

    private Die RollDie()
    {
        return new Die((int)Math.Floor(randomSource() * 6) + 1);
    }
}

public class TurnLog
{
    private readonly DiceCup diceCup;
    private readonly ITelemetryPort telemetry;

    public TurnLog(DiceCup diceCup, ITelemetryPort telemetry)
    {
        this.diceCup = diceCup;
        this.telemetry = telemetry;
    }

    public IReadOnlyList<Die> RerollSelectedDice()
    {
        var dice = diceCup.RerollSelected();
        telemetry.Record($"rerolled:{string.Join(",", dice.Select(die => die.Value))}");
        return dice;
    }
}

// Hand-rolled subclass mock standing in for the TS test's
// `{ ... } as unknown as DiceCup` duck-typed double: this is the C# shape
// of the same "Mocking Final/Concrete Classes" smell -- a concrete class is
// faked directly instead of the test depending on a port/interface. See
// 23_SmellyMarsRover/kata.cs's MockCommandTranslator for why virtual members
// had to be added for the C# port (TS mocks any method via duck typing, no
// virtual/interface needed).
public class MockDiceCup : DiceCup
{
    public int RerollSelectedCallCount;
    public IReadOnlyList<Die> RerollSelectedReturnValue = [];

    public override IReadOnlyList<Die> Roll() => [];

    public override void SelectForReroll(IReadOnlyList<int> indexes)
    {
    }

    public override IReadOnlyList<Die> RerollSelected()
    {
        RerollSelectedCallCount++;
        return RerollSelectedReturnValue;
    }

    public override IReadOnlyList<Die> CurrentDice => [];
}

// Shared static state (sharedCup/rollCount) makes Test1 and ShouldWork order
// sensitive to each other, the same way Jest's default declaration-order
// execution makes the TS source's "test1" / "should work" pair order
// sensitive. See TestOrdering.cs.
[TestCaseOrderer("SmellyYahtzeeKata.PriorityOrderer", "SmellyYahtzee")]
[DisableParallelization]
public class DiceCupShould
{
    private static readonly DiceCup sharedCup = new(() => 0);
    private static int rollCount;

    [Fact]
    [TestPriority(0)]
    public void Test1()
    {
        rollCount++;
        var dice = sharedCup.Roll();
        Assert.NotNull(dice);
    }

    [Fact]
    [TestPriority(1)]
    public void ShouldWork()
    {
        Assert.True(rollCount > 0);
        Assert.Equal(5, sharedCup.CurrentDice.Count);
    }

    [Fact]
    public void RollsSelectsRerollsAndClearsSelection()
    {
        var cup = new DiceCup(() => 0.5);
        var rolled = cup.Roll();
        cup.SelectForReroll([0, 2]);
        var rerolled = cup.RerollSelected();

        Assert.Equal(5, rolled.Count);
        Assert.Equal(4, rolled[0].Value);
        Assert.Equal(4, rolled[1].Value);
        Assert.Equal(4, rerolled[2].Value);
        Assert.Equal(5, cup.CurrentDice.Count);
        Assert.Same(rerolled, cup.CurrentDice);
    }

    [Fact]
    public void DoesThings()
    {
        var dice = new DiceCup(() => 0).Roll();

        Assert.Equal(5, dice.Count);
        Assert.Equal(1, dice[0].Value);
        Assert.Equal(1, dice[1].Value);
        Assert.Equal(1, dice[2].Value);
        Assert.Equal(1, dice[3].Value);
    }

    [Fact]
    public void ComputesExpectedDiceWithTheSameBranchingAsTheCup()
    {
        var source = new Queue<double>([0.01, 0.2, 0.4, 0.7, 0.99]);
        var cup = new DiceCup(() => source.Dequeue());
        var expected = new List<int>();
        foreach (var value in new[] { 0.01, 0.2, 0.4, 0.7, 0.99 })
        {
            if (value < 1.0 / 6) expected.Add(1);
            else if (value < 2.0 / 6) expected.Add(2);
            else if (value < 3.0 / 6) expected.Add(3);
            else if (value < 5.0 / 6) expected.Add(5);
            else expected.Add(6);
        }

        Assert.Equal(expected, cup.Roll().Select(die => die.Value));
    }

    [Fact]
    public void ReachesIntoThePrivateDieRollerDirectly()
    {
        var cup = new DiceCup(() => 0);
        var method = typeof(DiceCup).GetMethod(
            "RollDie",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var die = (Die)method!.Invoke(cup, null)!;
        Assert.Equal(1, die.Value);
    }

    [Fact]
    public async Task SlowlyWaitsBeforeRolling()
    {
        await Task.Delay(20);
        Assert.Equal(1, new DiceCup(() => 0).Roll()[0].Value);
    }

    [Fact]
    public void RerollsTheFirstDieDuplicateCaseOne()
    {
        var cup = new DiceCup(() => 0);
        cup.Roll();
        cup.SelectForReroll([0]);
        Assert.Equal(1, cup.RerollSelected()[0].Value);
    }

    [Fact]
    public void RerollsTheFirstDieDuplicateCaseTwo()
    {
        var cup = new DiceCup(() => 0);
        cup.Roll();
        cup.SelectForReroll([0]);
        Assert.Equal(1, cup.RerollSelected()[0].Value);
    }

    [Fact]
    public void RerollsTheFirstDieDuplicateCaseThree()
    {
        var cup = new DiceCup(() => 0);
        cup.Roll();
        cup.SelectForReroll([0]);
        Assert.Equal(1, cup.RerollSelected()[0].Value);
    }
}

public class TurnLogShould
{
    private static readonly long testRunTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    private class FakeTelemetryPort : ITelemetryPort
    {
        public List<string> Recorded { get; } = [];

        public void Record(string entry) => Recorded.Add(entry);
    }

    [Fact]
    public void LogsRerolledDice()
    {
        var mockCup = new MockDiceCup { RerollSelectedReturnValue = [new Die(1), new Die(2)] };
        var mockTelemetry = new FakeTelemetryPort();
        var mockDie = new Die(6);

        var log = new TurnLog(mockCup, mockTelemetry);
        var dice = log.RerollSelectedDice();

        Assert.Equal([1, 2], dice.Select(die => die.Value));
        Assert.Single(mockTelemetry.Recorded, "rerolled:1,2");
        Assert.Equal(1, mockCup.RerollSelectedCallCount);
        Assert.Equal(6, mockDie.Value);
    }

    [Fact]
    public void RecordsATimestampThatIsAlwaysInThePast()
    {
        Assert.True(testRunTimestamp <= DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
    }
}

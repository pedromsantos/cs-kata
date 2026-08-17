using Meziantou.Xunit;
using Xunit;

namespace SmellyMarsRoverKata;

public class MissionClock
{
    public static string Now()
    {
        return DateTime.UtcNow.ToString("o");
    }
}

public class RadioTransmitter
{
    private readonly string endpoint;

    public RadioTransmitter(string endpoint)
    {
        this.endpoint = endpoint;
    }

    public void Send(string message)
    {
        Console.WriteLine($"[RADIO -> {endpoint}] {message}");
    }
}

public class ObstacleSensor
{
    private readonly List<string> obstacles = ["1,2", "3,3", "0,4"];

    public bool DetectsObstacleAt(int x, int y)
    {
        var sensorNoise = Random.Shared.NextDouble() < 0.0001;
        return sensorNoise || obstacles.Contains($"{x},{y}");
    }
}

public class Position
{
    public int X { get; init; }
    public int Y { get; init; }
}

public class Rover
{
    private int x;
    private int y;
    private string direction;
    private readonly ObstacleSensor sensor = new();
    private readonly RadioTransmitter radio = new("mission-control.nasa.gov");
    private readonly int gridSize;

    public Rover(int x, int y, string direction, int gridSize)
    {
        this.x = x;
        this.y = y;
        this.direction = direction;
        this.gridSize = gridSize;
    }

    public string Execute(string commands)
    {
        foreach (var command in commands)
        {
            var previous = new Position { X = x, Y = y };

            if (command == 'L') TurnLeft();
            else if (command == 'R') TurnRight();
            else if (command == 'M') MoveForward();

            if (sensor.DetectsObstacleAt(x, y))
            {
                x = previous.X;
                y = previous.Y;
                ReportObstacle();
                return $"O {x} {y} {direction}";
            }
        }

        return $"{x} {y} {direction}";
    }

    private void ReportObstacle()
    {
        radio.Send($"OBSTACLE {x} {y} {direction} at {MissionClock.Now()}");
    }

    private void TurnLeft()
    {
        var order = new[] { "N", "W", "S", "E" };
        direction = order[(Array.IndexOf(order, direction) + 1) % 4];
    }

    private void TurnRight()
    {
        var order = new[] { "N", "E", "S", "W" };
        direction = order[(Array.IndexOf(order, direction) + 1) % 4];
    }

    private void MoveForward()
    {
        if (direction == "N") y = (y + 1) % gridSize;
        else if (direction == "S") y = (y - 1 + gridSize) % gridSize;
        else if (direction == "E") x = (x + 1) % gridSize;
        else if (direction == "W") x = (x - 1 + gridSize) % gridSize;
    }
}

public class Coordinate
{
    public int X { get; }
    public int Y { get; }

    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }

    public bool Equals(Coordinate other)
    {
        return X == other.X && Y == other.Y;
    }
}

public interface ITelemetryPort
{
    void Record(string entry);
}

public class CommandTranslator
{
    private static string lastLanguage = "EN";

    public string Translate(string command, string language)
    {
        lastLanguage = language;

        if (language == "EN") return command;
        if (language == "ES") return TranslateSpanish(command);
        if (language == "FR") return TranslateFrench(command);
        if (language == "PT") return TranslatePortuguese(command);
        if (language == "IT") return TranslateItalian(command);

        return command;
    }

    // Virtual purely so the concrete class below can fake it in a test seam --
    // see MockCommandTranslator and PORTING_NOTES_CS.md for why this had to
    // be added for the C# port (TS mocks any method via duck typing, no
    // virtual/interface needed).
    public virtual string TranslateSequence(string commands, string language)
    {
        var result = "";
        foreach (var command in commands)
        {
            result += Translate(command.ToString(), language);
        }
        return result;
    }

    public string GetLastLanguageUsed()
    {
        return lastLanguage;
    }

    private string TranslateSpanish(string command)
    {
        if (command == "I") return "L";
        if (command == "D") return "R";
        if (command == "A") return "M";
        return command;
    }

    private string TranslateFrench(string command)
    {
        if (command == "G") return "L";
        if (command == "D") return "R";
        if (command == "A") return "M";
        return command;
    }

    private string TranslatePortuguese(string command)
    {
        if (command == "E") return "L";
        if (command == "D") return "R";
        if (command == "A") return "M";
        return command;
    }

    private string TranslateItalian(string command)
    {
        if (command == "S") return "L";
        if (command == "D") return "R";
        if (command == "A") return "M";
        return command;
    }
}

public class MissionLog
{
    private readonly CommandTranslator translator;
    private readonly ITelemetryPort telemetry;

    public MissionLog(CommandTranslator translator, ITelemetryPort telemetry)
    {
        this.translator = translator;
        this.telemetry = telemetry;
    }

    public string LogTranslatedSequence(string commands, string language)
    {
        var translated = translator.TranslateSequence(commands, language);
        telemetry.Record($"{language}:{commands}->{translated}");
        return translated;
    }
}

// Hand-rolled subclass mock standing in for the TS test's
// `{ ... } as unknown as CommandTranslator` duck-typed double: this is the
// C# shape of the same "Mocking Final/Concrete Classes" smell -- a concrete
// class is faked directly instead of the test depending on a port/interface.
public class MockCommandTranslator : CommandTranslator
{
    public string? TranslateSequenceReturnValue;
    public string? TranslateSequenceCalledWithCommands;
    public string? TranslateSequenceCalledWithLanguage;

    public override string TranslateSequence(string commands, string language)
    {
        TranslateSequenceCalledWithCommands = commands;
        TranslateSequenceCalledWithLanguage = language;
        return TranslateSequenceReturnValue ?? "";
    }
}

// Shares an xUnit collection with MissionLogShould so the two classes --
// which both touch the CommandTranslator.lastLanguage static field -- can't
// interleave with each other.
[Collection("SmellyMarsRoverKata.CommandTranslator")]
[DisableParallelization]
[TestCaseOrderer("SmellyMarsRoverKata.PriorityOrderer", "SmellyMarsRover")]
public class CommandTranslatorShould
{
    private static CommandTranslator sharedTranslator = new();
    private static int callCount;

    private static readonly long testRunTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    [Fact]
    [TestPriority(0)]
    public void Test1()
    {
        callCount++;
        var result = sharedTranslator.Translate("I", "ES");
        Assert.NotNull(result);
    }

    [Fact]
    [TestPriority(1)]
    public void ShouldWork()
    {
        Assert.True(callCount > 0);
        Assert.Equal("ES", sharedTranslator.GetLastLanguageUsed());
    }

    [Fact]
    public void TranslatesAndLogsAndReportsLastLanguageAndHandlesUnknownLanguageAndSequences()
    {
        var t = new CommandTranslator();
        Assert.Equal("L", t.Translate("G", "FR"));
        Assert.Equal("R", t.Translate("D", "FR"));
        Assert.Equal("M", t.Translate("A", "FR"));
        Assert.Equal("Z", t.Translate("Z", "FR"));
        Assert.Equal("LRM", t.TranslateSequence("GDA", "FR"));
        Assert.Equal("FR", t.GetLastLanguageUsed());
        Assert.Equal("X", t.Translate("X", "XX"));
    }

    [Fact]
    public void ComputesTheExpectedTranslationUsingTheSameLogicAsProduction()
    {
        var t = new CommandTranslator();
        var commands = "IDA";
        var expected = "";
        foreach (var c in commands)
        {
            if (c == 'I') expected += "L";
            else if (c == 'D') expected += "R";
            else if (c == 'A') expected += "M";
            else expected += c;
        }
        Assert.Equal(expected, t.TranslateSequence(commands, "ES"));
    }

    [Fact]
    public void ReachesIntoAPrivateTranslationHelperDirectly()
    {
        var t = new CommandTranslator();
        var method = typeof(CommandTranslator).GetMethod(
            "TranslateSpanish",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var privateResult = (string)method!.Invoke(t, ["I"])!;
        Assert.Equal("L", privateResult);
    }

    [Fact]
    public async Task SlowlyWaitsForTheTranslatorToBeReady()
    {
        await Task.Delay(50);
        var t = new CommandTranslator();
        Assert.Equal("M", t.Translate("A", "IT"));
    }

    [Fact]
    public void TranslatesItalianRotateLeftDuplicateCaseOne()
    {
        var t = new CommandTranslator();
        Assert.Equal("L", t.Translate("S", "IT"));
    }

    [Fact]
    public void TranslatesItalianRotateLeftDuplicateCaseTwo()
    {
        var t = new CommandTranslator();
        Assert.Equal("L", t.Translate("S", "IT"));
    }

    [Fact]
    public void TranslatesItalianRotateLeftDuplicateCaseThree()
    {
        var t = new CommandTranslator();
        Assert.Equal("L", t.Translate("S", "IT"));
    }
}

[Collection("SmellyMarsRoverKata.CommandTranslator")]
public class MissionLogShould
{
    private static readonly long testRunTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    private class FakeTelemetryPort : ITelemetryPort
    {
        public List<string> Recorded { get; } = [];

        public void Record(string entry) => Recorded.Add(entry);
    }

    [Fact]
    public void LogsATranslatedSequence()
    {
        var mockTranslator = new MockCommandTranslator { TranslateSequenceReturnValue = "LRM" };
        var mockTelemetry = new FakeTelemetryPort();
        var mockCoordinate = new Coordinate(0, 0);

        var log = new MissionLog(mockTranslator, mockTelemetry);
        var result = log.LogTranslatedSequence("GDA", "FR");

        Assert.Equal("LRM", result);
        Assert.Equal("GDA", mockTranslator.TranslateSequenceCalledWithCommands);
        Assert.Equal("FR", mockTranslator.TranslateSequenceCalledWithLanguage);
        Assert.Equal(0, mockCoordinate.X);
    }

    [Fact]
    public void RecordsTelemetryWithTheRunTimestamp()
    {
        var telemetry = new FakeTelemetryPort();
        var log = new MissionLog(new CommandTranslator(), telemetry);

        log.LogTranslatedSequence("A", "IT");

        Assert.Contains("IT", telemetry.Recorded[0]);
        Assert.True(testRunTimestamp <= DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
    }
}

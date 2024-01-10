using Xunit;

namespace TicTacToeMockKata;

public class TicTacToeShould
{
    [Fact]
    public void Test1()
    {
    }
}

public enum Player
{
    X,
    Y,
}

public enum Row
{
    Top,
    Middle,
    Bottom,
}

public enum Column
{
    Left,
    Center,
    Right,
}

public class Coordinate
{
    private Row row;
    private Column column;

    public Coordinate(Row row, Column column)
    {
        this.row = row;
        this.column = column;
    }
}

public class Play
{
    private Player player;
    private Coordinate coordinate;

    public Play(Player player, Coordinate coordinate)
    {
        this.player = player;
        this.coordinate = coordinate;
    }
}

public interface IPlay
{
    void Play(Play play);
}

public interface IOutput
{
    void PrintPlay(Play play);
    void PrintWinner(Player player);
    void PrintError(string errorMessage);
}
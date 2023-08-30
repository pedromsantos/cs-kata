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
    private Row _row;
    private Column _column;

    public Coordinate(Row row, Column column)
    {
        _row = row;
        _column = column;
    }
}

public class Play
{
    private Player _player;
    private Coordinate _coordinate;

    public Play(Player player, Coordinate coordinate)
    {
        _player = player;
        _coordinate = coordinate;
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
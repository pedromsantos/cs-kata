using Xunit;

namespace RefactoringGolf.hole4;

public class GameShould
{
    private readonly Game game = new();

    [Fact]
    public void NotAllowPlayerOToPlayFirst()
    {
        var wrongPlay = () => game.Play('O', 0, 0);

        var exception = Assert.Throws<Exception>(wrongPlay);
        Assert.Equal("Invalid first player", exception.Message);
    }

    [Fact]
    public void NotAllowPlayerXToPlayTwiceInARow()
    {
        game.Play('X', 0, 0);

        var wrongPlay = () => game.Play('X', 1, 0);

        var exception = Assert.Throws<Exception>(wrongPlay);
        Assert.Equal("Invalid next player", exception.Message);
    }

    [Fact]
    public void NotAllowPlayerToPlayInLastPlayedPosition()
    {
        game.Play('X', 0, 0);

        var wrongPlay = () => game.Play('O', 0, 0);

        var exception = Assert.Throws<Exception>(wrongPlay);
        Assert.Equal("Invalid position", exception.Message);
    }

    [Fact]
    public void NotAllowPlayerToPlayInAnyPlayedPosition()
    {
        game.Play('X', 0, 0);
        game.Play('O', 1, 0);

        var wrongPlay = () => game.Play('X', 0, 0);

        var exception = Assert.Throws<Exception>(wrongPlay);
        Assert.Equal("Invalid position", exception.Message);
    }

    [Fact]
    public void DeclarePlayerXAsAWinnerIfThreeInTopRow()
    {
        game.Play('X', 0, 0);
        game.Play('O', 1, 0);
        game.Play('X', 0, 1);
        game.Play('O', 1, 1);
        game.Play('X', 0, 2);

        var winner = game.Winner();

        Assert.Equal('X', winner);
    }

    [Fact]
    public void DeclarePlayerOAsAWinnerIfThreeInTopRow()
    {
        game.Play('X', 2, 2);
        game.Play('O', 0, 0);
        game.Play('X', 1, 0);
        game.Play('O', 0, 1);
        game.Play('X', 1, 1);
        game.Play('O', 0, 2);

        var winner = game.Winner();

        Assert.Equal('O', winner);
    }

    [Fact]
    public void DeclarePlayerXAsAWinnerIfThreeInMiddleRow()
    {
        game.Play('X', 1, 0);
        game.Play('O', 0, 0);
        game.Play('X', 1, 1);
        game.Play('O', 0, 1);
        game.Play('X', 1, 2);

        var winner = game.Winner();

        Assert.Equal('X', winner);
    }

    [Fact]
    public void DeclarePlayerOAsAWinnerIfThreeInMiddleRow()
    {
        game.Play('X', 0, 0);
        game.Play('O', 1, 0);
        game.Play('X', 2, 0);
        game.Play('O', 1, 1);
        game.Play('X', 2, 1);
        game.Play('O', 1, 2);

        var winner = game.Winner();

        Assert.Equal('O', winner);
    }

    [Fact]
    public void DeclarePlayerXAsAWinnerIfThreeInBottomRow()
    {
        game.Play('X', 2, 0);
        game.Play('O', 0, 0);
        game.Play('X', 2, 1);
        game.Play('O', 0, 1);
        game.Play('X', 2, 2);

        var winner = game.Winner();

        Assert.Equal('X', winner);
    }

    [Fact]
    public void DeclarePlayerOAsAWinnerIfThreeInBottomRow()
    {
        game.Play('X', 0, 0);
        game.Play('O', 2, 0);
        game.Play('X', 1, 0);
        game.Play('O', 2, 1);
        game.Play('X', 1, 1);
        game.Play('O', 2, 2);

        var winner = game.Winner();

        Assert.Equal('O', winner);
    }
}

public class Tile
{
    public int X { get; set; }
    public int Y { get; set; }
    public char Symbol { get; set; }
}

public class Board
{
    private readonly List<Tile> _plays = new();

    public Board()
    {
        for (var i = 0; i < 3; i++)
        for (var j = 0; j < 3; j++)
            _plays.Add(new Tile { X = i, Y = j, Symbol = ' ' });
    }

    public Tile TileAt(int x, int y)
    {
        return _plays.Single(tile => tile.X == x && tile.Y == y);
    }

    public void AddTileAt(char symbol, int x, int y)
    {
        _plays.Single(tile => tile.X == x && tile.Y == y).Symbol = symbol;
    }

    public char CommonSymbolOnRow()
    {
        for (var rowIndex = 0; rowIndex < 3; rowIndex++)
            if (TileAt(rowIndex, 0).Symbol != ' ' &&
                TileAt(rowIndex, 1).Symbol != ' ' &&
                TileAt(rowIndex, 2).Symbol != ' ' &&
                TileAt(rowIndex, 0).Symbol ==
                TileAt(rowIndex, 1).Symbol &&
                TileAt(rowIndex, 2).Symbol ==
                TileAt(rowIndex, 1).Symbol)
                return TileAt(rowIndex, 0).Symbol;

        return ' ';
    }
}

public class Game
{
    private readonly Board _board = new();
    private char _lastSymbol = ' ';

    public void Play(char symbol, int x, int y)
    {
        ValidateFirstMove(symbol);
        ValidatePlayer(symbol);
        ValidatePositionIsEmpty(x, y);

        UpdateLastPlayer(symbol);
        UpdateBoard(symbol, x, y);
    }

    private void UpdateBoard(char symbol, int x, int y)
    {
        _board.AddTileAt(symbol, x, y);
    }

    private void UpdateLastPlayer(char symbol)
    {
        _lastSymbol = symbol;
    }

    private void ValidatePositionIsEmpty(int x, int y)
    {
        if (_board.TileAt(x, y).Symbol != ' ') throw new Exception("Invalid position");
    }

    private void ValidatePlayer(char symbol)
    {
        if (symbol == _lastSymbol) throw new Exception("Invalid next player");
    }

    private void ValidateFirstMove(char symbol)
    {
        if (_lastSymbol == ' ')
            if (symbol == 'O')
                throw new Exception("Invalid first player");
    }

    public char Winner()
    {
        return _board.CommonSymbolOnRow();
    }
}
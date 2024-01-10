using Xunit;

namespace RefactoringGolf.hole9;

public static class SymbolExtensions
{
    public static Symbol ToEnum(this char symbol)
    {
        if (symbol == 'X') return Symbol.X;
        if (symbol == 'O') return Symbol.O;
        return Symbol.Empty;
    }

    public static char ToChar(this Symbol symbol)
    {
        if (symbol == Symbol.X) return 'X';
        if (symbol == Symbol.O) return 'O';
        return ' ';
    }
}

public class GameShould
{
    private readonly Game game = new();

    [Fact]
    public void NotAllowPlayerOToPlayFirst()
    {
        var wrongPlay = () => game.Play(0, 0, 'O'.ToEnum());

        var exception = Assert.Throws<Exception>(wrongPlay);
        Assert.Equal("Invalid first player", exception.Message);
    }

    [Fact]
    public void NotAllowPlayerXToPlayTwiceInARow()
    {
        game.Play(0, 0, 'X'.ToEnum());

        var wrongPlay = () => game.Play(1, 0, 'X'.ToEnum());

        var exception = Assert.Throws<Exception>(wrongPlay);
        Assert.Equal("Invalid next player", exception.Message);
    }

    [Fact]
    public void NotAllowPlayerToPlayInLastPlayedPosition()
    {
        game.Play(0, 0, 'X'.ToEnum());

        var wrongPlay = () => game.Play(0, 0, 'O'.ToEnum());

        var exception = Assert.Throws<Exception>(wrongPlay);
        Assert.Equal("Invalid position", exception.Message);
    }

    [Fact]
    public void NotAllowPlayerToPlayInAnyPlayedPosition()
    {
        game.Play(0, 0, 'X'.ToEnum());
        game.Play(1, 0, 'O'.ToEnum());

        var wrongPlay = () => game.Play(0, 0, 'X'.ToEnum());

        var exception = Assert.Throws<Exception>(wrongPlay);
        Assert.Equal("Invalid position", exception.Message);
    }

    [Fact]
    public void DeclarePlayerXAsAWinnerIfThreeInTopRow()
    {
        game.Play(0, 0, 'X'.ToEnum());
        game.Play(1, 0, 'O'.ToEnum());
        game.Play(0, 1, 'X'.ToEnum());
        game.Play(1, 1, 'O'.ToEnum());
        game.Play(0, 2, 'X'.ToEnum());

        var winner = game.Winner();

        Assert.Equal('X', winner.ToChar());
    }

    [Fact]
    public void DeclarePlayerOAsAWinnerIfThreeInTopRow()
    {
        game.Play(2, 2, 'X'.ToEnum());
        game.Play(0, 0, 'O'.ToEnum());
        game.Play(1, 0, 'X'.ToEnum());
        game.Play(0, 1, 'O'.ToEnum());
        game.Play(1, 1, 'X'.ToEnum());
        game.Play(0, 2, 'O'.ToEnum());

        var winner = game.Winner();

        Assert.Equal('O', winner.ToChar());
    }

    [Fact]
    public void DeclarePlayerXAsAWinnerIfThreeInMiddleRow()
    {
        game.Play(1, 0, 'X'.ToEnum());
        game.Play(0, 0, 'O'.ToEnum());
        game.Play(1, 1, 'X'.ToEnum());
        game.Play(0, 1, 'O'.ToEnum());
        game.Play(1, 2, 'X'.ToEnum());

        var winner = game.Winner();

        Assert.Equal('X', winner.ToChar());
    }

    [Fact]
    public void DeclarePlayerOAsAWinnerIfThreeInMiddleRow()
    {
        game.Play(0, 0, 'X'.ToEnum());
        game.Play(1, 0, 'O'.ToEnum());
        game.Play(2, 0, 'X'.ToEnum());
        game.Play(1, 1, 'O'.ToEnum());
        game.Play(2, 1, 'X'.ToEnum());
        game.Play(1, 2, 'O'.ToEnum());

        var winner = game.Winner();

        Assert.Equal('O', winner.ToChar());
    }

    [Fact]
    public void DeclarePlayerXAsAWinnerIfThreeInBottomRow()
    {
        game.Play(2, 0, 'X'.ToEnum());
        game.Play(0, 0, 'O'.ToEnum());
        game.Play(2, 1, 'X'.ToEnum());
        game.Play(0, 1, 'O'.ToEnum());
        game.Play(2, 2, 'X'.ToEnum());

        var winner = game.Winner();

        Assert.Equal('X', winner.ToChar());
    }

    [Fact]
    public void DeclarePlayerOAsAWinnerIfThreeInBottomRow()
    {
        game.Play(0, 0, 'X'.ToEnum());
        game.Play(2, 0, 'O'.ToEnum());
        game.Play(1, 0, 'X'.ToEnum());
        game.Play(2, 1, 'O'.ToEnum());
        game.Play(1, 1, 'X'.ToEnum());
        game.Play(2, 2, 'O'.ToEnum());

        var winner = game.Winner();

        Assert.Equal('O', winner.ToChar());
    }
}

public enum Symbol
{
    Empty,
    X,
    O
}

public class Tile
{
    public Tile(int x, int y, Symbol symbol)
    {
        X = x;
        Y = y;
        Symbol = symbol;
    }

    public int X { get; }
    public int Y { get; }
    public Symbol Symbol { get; set; }

    public bool HasSameSymbol(Tile other)
    {
        return Symbol == other.Symbol;
    }

    public bool IsTaken()
    {
        return Symbol != Symbol.Empty;
    }
}

public class Board
{
    private readonly List<Tile> _plays = new();

    public Board()
    {
        for (var i = 0; i < 3; i++)
        for (var j = 0; j < 3; j++)
            _plays.Add(new Tile(i, j, Symbol.Empty));
    }

    public Tile TileAt(int x, int y)
    {
        return _plays.Single(tile => tile.X == x && tile.Y == y);
    }

    public void AddTileAt(int x, int y, Symbol symbol)
    {
        _plays.Single(tile => tile.X == x && tile.Y == y).Symbol = symbol;
    }

    public Symbol FindSymbolWhoTookARow()
    {
        for (var rowIndex = 0; rowIndex < 3; rowIndex++)
            if (IsRowTakenWithSymbol(rowIndex))
                return TileAt(rowIndex, 0).Symbol;

        return Symbol.Empty;
    }

    private bool IsRowTakenWithSymbol(int rowIndex)
    {
        return IsRowTaken(rowIndex) &&
               HasRowSameSymbol(rowIndex);
    }

    private bool HasRowSameSymbol(int rowIndex)
    {
        return TileAt(rowIndex, 0).HasSameSymbol(
                   TileAt(rowIndex, 1)) &&
               TileAt(rowIndex, 2).HasSameSymbol(
                   TileAt(rowIndex, 1));
    }

    private bool IsRowTaken(int rowIndex)
    {
        return TileAt(rowIndex, 0).IsTaken() &&
               TileAt(rowIndex, 1).IsTaken() &&
               TileAt(rowIndex, 2).IsTaken();
    }
}

public class Game
{
    private readonly Board _board = new();
    private Symbol _lastSymbol = Symbol.Empty;

    public void Play(int x, int y, Symbol newSymbol)
    {
        ValidateFirstMove(newSymbol);
        ValidatePlayer(newSymbol);
        ValidatePositionIsEmpty(x, y);

        UpdateLastPlayer(newSymbol);
        UpdateBoard(x, y, newSymbol);
    }

    private void UpdateBoard(int x, int y, Symbol symbol)
    {
        _board.AddTileAt(x, y, symbol);
    }

    private void UpdateLastPlayer(Symbol symbol)
    {
        _lastSymbol = symbol;
    }

    private void ValidatePositionIsEmpty(int x, int y)
    {
        if (_board.TileAt(x, y).IsTaken()) throw new Exception("Invalid position");
    }

    private void ValidatePlayer(Symbol symbol)
    {
        if (symbol == _lastSymbol) throw new Exception("Invalid next player");
    }

    private void ValidateFirstMove(Symbol symbol)
    {
        if (_lastSymbol == Symbol.Empty)
            if (symbol == Symbol.O)
                throw new Exception("Invalid first player");
    }

    public Symbol Winner()
    {
        return _board.FindSymbolWhoTookARow();
    }
}
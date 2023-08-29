using Xunit;

namespace RefactoringGolf.hole7;

public class GameShould
{
    private readonly Game game = new();

    [Fact]
    public void NotAllowPlayerOToPlayFirst()
    {
        Action wrongPlay = () => game.Play('O', 0, 0);

        var exception = Assert.Throws<Exception>(wrongPlay);
        Assert.Equal("Invalid first player", exception.Message);
    }

    [Fact]
    public void NotAllowPlayerXToPlayTwiceInARow()
    {
        game.Play('X', 0, 0);

        Action wrongPlay = () => game.Play('X', 1, 0);

        var exception = Assert.Throws<Exception>(wrongPlay);
        Assert.Equal("Invalid next player", exception.Message);
    }

    [Fact]
    public void NotAllowPlayerToPlayInLastPlayedPosition()
    {
        game.Play('X', 0, 0);

        Action wrongPlay = () => game.Play('O', 0, 0);

        var exception = Assert.Throws<Exception>(wrongPlay);
        Assert.Equal("Invalid position", exception.Message);
    }

    [Fact]
    public void NotAllowPlayerToPlayInAnyPlayedPosition()
    {
        game.Play('X', 0, 0);
        game.Play('O', 1, 0);

        Action wrongPlay = () => game.Play('X', 0, 0);

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

public enum Symbol
{
    Empty,
    X,
    O
}

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

public class Tile
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public Symbol Symbol { get; set; }

    public Tile(int x, int y, char symbol = ' ')
     :this(x,y, symbol.ToEnum())
    {
    }

    public Tile(int x, int y, Symbol symbol)
    {
        X = x;
        Y = y;
        Symbol = symbol;
    }

    public bool HasSameSymbol(Tile other)
    {
        return Symbol == other.Symbol;
    }

    public bool IsTaken()
    {
        return Symbol != Symbol.Empty;
    }

    public char GetSymbol()
    {
        return Symbol.ToChar();
    }
}

public class Board
{
    private List<Tile> _plays = new();

    public Board()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                _plays.Add(new Tile(x: i, y: j, symbol: Symbol.Empty));
            }
        }
    }
    public Tile TileAt(int x, int y)
    {
        return _plays.Single(tile => tile.X == x && tile.Y == y);
    }

    public void AddTileAt(char symbol, int x, int y)
    {
        _plays.Single(tile => tile.X == x && tile.Y == y).Symbol = symbol.ToEnum();
    }

    public char FindSymbolWhoTookARow()
    {
        for (var rowIndex = 0; rowIndex < 3; rowIndex++)
        {
            if (IsRowTakenWithSymbol(rowIndex))
            {
                return TileAt(rowIndex, 0).GetSymbol();
            } 
        }
            
        return ' ';
    }

    private bool IsRowTakenWithSymbol(int rowIndex)
    {
        return IsRowTaken(rowIndex) &&
               HasRowSameSymbol(rowIndex);
    }

    private bool HasRowSameSymbol(int rowIndex)
    {
        return (TileAt(rowIndex, 0).HasSameSymbol(
                TileAt(rowIndex, 1)) &&
                TileAt(rowIndex, 2).HasSameSymbol(
                TileAt(rowIndex, 1)));
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
    private char _lastSymbol = ' ';
    private Board _board = new Board();

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
        if (_board.TileAt(x, y).IsTaken())
        {
            throw new Exception("Invalid position");
        }
    }

    private void ValidatePlayer(char symbol)
    {
        if (symbol == _lastSymbol)
        {
            throw new Exception("Invalid next player");
        }
    }

    private void ValidateFirstMove(char symbol)
    {
        if (_lastSymbol == ' ')
        {
            if (symbol == 'O')
            {
                throw new Exception("Invalid first player");
            }
        }
    }

    public char Winner()
    {
        return _board.FindSymbolWhoTookARow();
    }
}
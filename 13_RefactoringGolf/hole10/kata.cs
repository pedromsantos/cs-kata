using Xunit;

namespace RefactoringGolf.hole10;

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

public static class RowExtensions
{
    public static Row RowToEnum(this int row)
    {
        if (row == 0) return Row.Top;
        if (row == 1) return Row.Middle;
        return Row.Bottom;
    }

    public static int ToInt(this Row row)
    {
        if (row == Row.Top) return 0;
        if (row == Row.Middle) return 1;
        return 2;
    }
}

public static class ColumnExtensions
{
    public static Column ColumnToEnum(this int column)
    {
        if (column == 0) return Column.Left;
        if (column == 1) return Column.Center;
        return Column.Rigth;
    }

    public static int ToInt(this Column column)
    {
        if (column == Column.Left) return 0;
        if (column == Column.Center) return 1;
        return 2;
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

public enum Row
{
    Top,
    Middle,
    Bottom
}

public enum Column
{
    Left,
    Center,
    Rigth
}

public class Tile
{
    public Tile(int x, int y, Symbol symbol = Symbol.Empty)
    {
        X = x.RowToEnum();
        Y = y.ColumnToEnum();
        Symbol = symbol;
    }

    public Row X { get; }
    public Column Y { get; }
    public Symbol Symbol { get; set; }

    public bool HasSameSymbol(Tile other)
    {
        return Symbol == other.Symbol;
    }

    public bool IsTaken()
    {
        return Symbol != Symbol.Empty;
    }

    public bool HasSamePosition(Tile other)
    {
        return X == other.X && Y == other.Y;
    }
}

public class Board
{
    private readonly List<Tile> plays = new();

    public Board()
    {
        for (var row = 0; row < 3; row++)
            for (var column = 0; column < 3; column++)
                plays.Add(new Tile(row, column));
    }

    public Tile TileAt(int x, int y)
    {
        return plays.Single(tile => tile.HasSamePosition(new Tile(x, y)));
    }

    public void AddTileAt(int x, int y, Symbol symbol)
    {
        TileAt(x, y).Symbol = symbol;
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
    private readonly Board board = new();
    private Symbol lastSymbol = Symbol.Empty;

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
        board.AddTileAt(x, y, symbol);
    }

    private void UpdateLastPlayer(Symbol symbol)
    {
        lastSymbol = symbol;
    }

    private void ValidatePositionIsEmpty(int x, int y)
    {
        if (board.TileAt(x, y).IsTaken()) throw new Exception("Invalid position");
    }

    private void ValidatePlayer(Symbol symbol)
    {
        if (symbol == lastSymbol) throw new Exception("Invalid next player");
    }

    private void ValidateFirstMove(Symbol symbol)
    {
        if (lastSymbol == Symbol.Empty)
            if (symbol == Symbol.O)
                throw new Exception("Invalid first player");
    }

    public Symbol Winner()
    {
        return board.FindSymbolWhoTookARow();
    }
}
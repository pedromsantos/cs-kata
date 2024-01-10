using Xunit;

namespace RefactoringGolf.hole11;

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
}

public static class ColumnExtensions
{
    public static Column ColumnToEnum(this int column)
    {
        if (column == 0) return Column.Left;
        if (column == 1) return Column.Center;
        return Column.Right;
    }
}

public class GameShould
{
    private readonly Game game = new();

    [Fact]
    public void NotAllowPlayerOToPlayFirst()
    {
        var wrongPlay = () =>
        {
            var newSymbol = 'O'.ToEnum();
            game.Play(new Tile(new Coordinate(0, 0), newSymbol));
        };

        var exception = Assert.Throws<Exception>(wrongPlay);
        Assert.Equal("Invalid first player", exception.Message);
    }

    [Fact]
    public void NotAllowPlayerXToPlayTwiceInARow()
    {
        game.Play(new Tile(new Coordinate(0, 0), 'X'.ToEnum()));

        var wrongPlay = () => { game.Play(new Tile(new Coordinate(1, 0), 'X'.ToEnum())); };

        var exception = Assert.Throws<Exception>(wrongPlay);
        Assert.Equal("Invalid next player", exception.Message);
    }

    [Fact]
    public void NotAllowPlayerToPlayInLastPlayedPosition()
    {
        game.Play(new Tile(new Coordinate(0, 0), 'X'.ToEnum()));

        var wrongPlay = () => { game.Play(new Tile(new Coordinate(0, 0), 'O'.ToEnum())); };

        var exception = Assert.Throws<Exception>(wrongPlay);
        Assert.Equal("Invalid position", exception.Message);
    }

    [Fact]
    public void NotAllowPlayerToPlayInAnyPlayedPosition()
    {
        game.Play(new Tile(new Coordinate(0, 0), 'X'.ToEnum()));
        var newSymbol1 = 'O'.ToEnum();
        game.Play(new Tile(new Coordinate(1, 0), newSymbol1));

        var wrongPlay = () => { game.Play(new Tile(new Coordinate(0, 0), 'X'.ToEnum())); };

        var exception = Assert.Throws<Exception>(wrongPlay);
        Assert.Equal("Invalid position", exception.Message);
    }

    [Fact]
    public void DeclarePlayerXAsAWinnerIfThreeInTopRow()
    {
        game.Play(new Tile(new Coordinate(0, 0), 'X'.ToEnum()));
        game.Play(new Tile(new Coordinate(1, 0), 'O'.ToEnum()));
        game.Play(new Tile(new Coordinate(0, 1), 'X'.ToEnum()));
        game.Play(new Tile(new Coordinate(1, 1), 'O'.ToEnum()));
        game.Play(new Tile(new Coordinate(0, 2), 'X'.ToEnum()));

        var winner = game.Winner();

        Assert.Equal('X', winner.ToChar());
    }

    [Fact]
    public void DeclarePlayerOAsAWinnerIfThreeInTopRow()
    {
        game.Play(new Tile(new Coordinate(2, 2), 'X'.ToEnum()));
        game.Play(new Tile(new Coordinate(0, 0), 'O'.ToEnum()));
        game.Play(new Tile(new Coordinate(1, 0), 'X'.ToEnum()));
        game.Play(new Tile(new Coordinate(0, 1), 'O'.ToEnum()));
        game.Play(new Tile(new Coordinate(1, 1), 'X'.ToEnum()));
        game.Play(new Tile(new Coordinate(0, 2), 'O'.ToEnum()));

        var winner = game.Winner();

        Assert.Equal('O', winner.ToChar());
    }

    [Fact]
    public void DeclarePlayerXAsAWinnerIfThreeInMiddleRow()
    {
        game.Play(new Tile(new Coordinate(1, 0), 'X'.ToEnum()));
        game.Play(new Tile(new Coordinate(0, 0), 'O'.ToEnum()));
        game.Play(new Tile(new Coordinate(1, 1), 'X'.ToEnum()));
        game.Play(new Tile(new Coordinate(0, 1), 'O'.ToEnum()));
        game.Play(new Tile(new Coordinate(1, 2), 'X'.ToEnum()));

        var winner = game.Winner();

        Assert.Equal('X', winner.ToChar());
    }

    [Fact]
    public void DeclarePlayerOAsAWinnerIfThreeInMiddleRow()
    {
        game.Play(new Tile(new Coordinate(0, 0), 'X'.ToEnum()));
        game.Play(new Tile(new Coordinate(1, 0), 'O'.ToEnum()));
        game.Play(new Tile(new Coordinate(2, 0), 'X'.ToEnum()));
        game.Play(new Tile(new Coordinate(1, 1), 'O'.ToEnum()));
        game.Play(new Tile(new Coordinate(2, 1), 'X'.ToEnum()));
        game.Play(new Tile(new Coordinate(1, 2), 'O'.ToEnum()));

        var winner = game.Winner();

        Assert.Equal('O', winner.ToChar());
    }

    [Fact]
    public void DeclarePlayerXAsAWinnerIfThreeInBottomRow()
    {
        game.Play(new Tile(new Coordinate(2, 0), 'X'.ToEnum()));
        game.Play(new Tile(new Coordinate(0, 0), 'O'.ToEnum()));
        game.Play(new Tile(new Coordinate(2, 1), 'X'.ToEnum()));
        game.Play(new Tile(new Coordinate(0, 1), 'O'.ToEnum()));
        game.Play(new Tile(new Coordinate(2, 2), 'X'.ToEnum()));

        var winner = game.Winner();

        Assert.Equal('X', winner.ToChar());
    }

    [Fact]
    public void DeclarePlayerOAsAWinnerIfThreeInBottomRow()
    {
        game.Play(new Tile(new Coordinate(0, 0), 'X'.ToEnum()));
        game.Play(new Tile(new Coordinate(2, 0), 'O'.ToEnum()));
        game.Play(new Tile(new Coordinate(1, 0), 'X'.ToEnum()));
        game.Play(new Tile(new Coordinate(2, 1), 'O'.ToEnum()));
        game.Play(new Tile(new Coordinate(1, 1), 'X'.ToEnum()));
        game.Play(new Tile(new Coordinate(2, 2), 'O'.ToEnum()));

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
    Right
}

public class Coordinate
{
    private readonly Column column;
    private readonly Row row;

    public Coordinate(int row, int column)
    {
        this.row = row.RowToEnum();
        this.column = column.ColumnToEnum();
    }

    private bool Equals(Coordinate other)
    {
        return row == other.row && column == other.column;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Coordinate)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)row, (int)column);
    }
}

public class Tile
{
    private readonly Coordinate coordinate;

    public Tile(Coordinate coordinate, Symbol symbol = Symbol.Empty)
    {
        this.coordinate = coordinate;
        Symbol = symbol;
    }

    public Symbol Symbol { get; private set; }

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
        return Equals(coordinate, other.coordinate);
    }

    public void UpdateSymbol(Symbol newSymbol)
    {
        Symbol = newSymbol;
    }
}

public class Board
{
    private readonly List<Tile> _plays = new();

    public Board()
    {
        for (var row = 0; row < 3; row++)
        for (var column = 0; column < 3; column++)
            _plays.Add(new Tile(new Coordinate(row, column)));
    }

    public Tile TileAt(Tile other)
    {
        return _plays.Single(tile => tile.HasSamePosition(other));
    }

    public void AddTileAt(Tile newTile)
    {
        TileAt(newTile).UpdateSymbol(newTile.Symbol);
    }

    public Symbol FindSymbolWhoTookARow()
    {
        for (var rowIndex = 0; rowIndex < 3; rowIndex++)
            if (IsRowTakenWithSymbol(rowIndex))
                return TileAt(new Tile(new Coordinate(rowIndex, 0))).Symbol;

        return Symbol.Empty;
    }

    private bool IsRowTakenWithSymbol(int rowIndex)
    {
        return IsRowTaken(rowIndex) &&
               HasRowSameSymbol(rowIndex);
    }

    private bool HasRowSameSymbol(int rowIndex)
    {
        return TileAt(new Tile(new Coordinate(rowIndex, 0))).HasSameSymbol(
                   TileAt(new Tile(new Coordinate(rowIndex, 1)))) &&
               TileAt(new Tile(new Coordinate(rowIndex, 2))).HasSameSymbol(
                   TileAt(new Tile(new Coordinate(rowIndex, 1))));
    }

    private bool IsRowTaken(int rowIndex)
    {
        return TileAt(new Tile(new Coordinate(rowIndex, 0))).IsTaken() &&
               TileAt(new Tile(new Coordinate(rowIndex, 1))).IsTaken() &&
               TileAt(new Tile(new Coordinate(rowIndex, 2))).IsTaken();
    }
}

public class Game
{
    private readonly Board _board = new();
    private Symbol _lastSymbol = Symbol.Empty;

    public void Play(Tile newTile)
    {
        ValidateFirstMove(newTile.Symbol);
        ValidatePlayer(newTile.Symbol);
        ValidatePositionIsEmpty(newTile);

        UpdateLastPlayer(newTile.Symbol);
        UpdateBoard(newTile);
    }

    private void UpdateBoard(Tile newTile)
    {
        _board.AddTileAt(newTile);
    }

    private void UpdateLastPlayer(Symbol symbol)
    {
        _lastSymbol = symbol;
    }

    private void ValidatePositionIsEmpty(Tile other)
    {
        if (_board.TileAt(other).IsTaken()) throw new Exception("Invalid position");
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
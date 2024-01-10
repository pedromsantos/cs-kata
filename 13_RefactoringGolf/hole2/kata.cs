using Xunit;

namespace RefactoringGolf.hole2;

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
    private readonly List<Tile> plays = new();

    public Board()
    {
        for (var i = 0; i < 3; i++)
            for (var j = 0; j < 3; j++)
                plays.Add(new Tile { X = i, Y = j, Symbol = ' ' });
    }

    public Tile TileAt(int x, int y)
    {
        return plays.Single(tile => tile.X == x && tile.Y == y);
    }

    public void AddTileAt(char symbol, int x, int y)
    {
        var newTile = new Tile
        {
            X = x,
            Y = y,
            Symbol = symbol
        };

        plays.Single(tile => tile.X == x && tile.Y == y).Symbol = symbol;
    }

    public char SamePlayerAtAllColumnsInRows()
    {
        if (TileAt(0, 0).Symbol != ' ' &&
            TileAt(0, 1).Symbol != ' ' &&
            TileAt(0, 2).Symbol != ' ')
            //if first row is full with same symbol
            if (TileAt(0, 0).Symbol ==
                TileAt(0, 1).Symbol &&
                TileAt(0, 2).Symbol ==
                TileAt(0, 1).Symbol)
                return TileAt(0, 0).Symbol;

        //if the positions in first row are taken
        if (TileAt(1, 0).Symbol != ' ' &&
            TileAt(1, 1).Symbol != ' ' &&
            TileAt(1, 2).Symbol != ' ')
            //if middle row is full with same symbol
            if (TileAt(1, 0).Symbol ==
                TileAt(1, 1).Symbol &&
                TileAt(1, 2).Symbol ==
                TileAt(1, 1).Symbol)
                return TileAt(1, 0).Symbol;

        //if the positions in first row are taken
        if (TileAt(2, 0).Symbol != ' ' &&
            TileAt(2, 1).Symbol != ' ' &&
            TileAt(2, 2).Symbol != ' ')
            //if middle row is full with same symbol
            if (TileAt(2, 0).Symbol ==
                TileAt(2, 1).Symbol &&
                TileAt(2, 2).Symbol ==
                TileAt(2, 1).Symbol)
                return TileAt(2, 0).Symbol;

        return ' ';
    }
}

public class Game
{
    private readonly Board board = new();
    private char lastSymbol = ' ';

    public void Play(char symbol, int x, int y)
    {
        //if first move
        if (lastSymbol == ' ')
            //if player is X
            if (symbol == 'O')
                throw new Exception("Invalid first player");

        //if not first move but player repeated
        if (symbol == lastSymbol) throw new Exception("Invalid next player");
        //if not first move but play on an already played tile

        if (board.TileAt(x, y).Symbol != ' ') throw new Exception("Invalid position");

        // update game state
        lastSymbol = symbol;
        board.AddTileAt(symbol, x, y);
    }

    public char Winner()
    {
        //if the positions in first row are taken
        return board.SamePlayerAtAllColumnsInRows();
    }
}
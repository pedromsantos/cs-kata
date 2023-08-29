using Xunit;

namespace RefactoringGolf.hole13;

public class GameShould
{
    private readonly Game _game = new();

    [Fact]
    public void NotAllowPlayerOToPlayFirst()
    {
        var wrongPlay = () =>
        {
            Player newPlayer = Player.O;
            _game.Play(new Tile(new Coordinate(0, 0), newPlayer));
        };

        var exception = Assert.Throws<Exception>(wrongPlay);
        Assert.Equal("Invalid first player", exception.Message);
    }

    [Fact]
    public void NotAllowPlayerXToPlayTwiceInARow()
    {
        _game.Play(new Tile(new Coordinate(0, 0), Player.X));

        var wrongPlay = () =>
        {
            _game.Play(new Tile(new Coordinate(1, 0), Player.X));
        };

        var exception = Assert.Throws<Exception>(wrongPlay);
        Assert.Equal("Invalid next player", exception.Message);
    }

    [Fact]
    public void NotAllowPlayerToPlayInLastPlayedPosition()
    {
        _game.Play(new Tile(new Coordinate(0, 0), Player.X));

        var wrongPlay = () =>
        {
            _game.Play(new Tile(new Coordinate(0, 0), Player.O));
        };

        var exception = Assert.Throws<Exception>(wrongPlay);
        Assert.Equal("Invalid position", exception.Message);
    }

    [Fact]
    public void NotAllowPlayerToPlayInAnyPlayedPosition()
    {
        _game.Play(new Tile(new Coordinate(0, 0), Player.X));
        _game.Play(new Tile(new Coordinate(1, 0), Player.O));

        var wrongPlay = () =>
        {
            _game.Play(new Tile(new Coordinate(0, 0), Player.X));
        };

        var exception = Assert.Throws<Exception>(wrongPlay);
        Assert.Equal("Invalid position", exception.Message);
    }

    [Fact]
    public void DeclarePlayerXAsAWinnerIfThreeInTopRow()
    {
        _game.Play(new Tile(new Coordinate(0, 0), Player.X));
        _game.Play(new Tile(new Coordinate(1, 0), Player.O));
        _game.Play(new Tile(new Coordinate(0, 1), Player.X));
        _game.Play(new Tile(new Coordinate(1, 1), Player.O));
        _game.Play(new Tile(new Coordinate(0, 2), Player.X));

        var winner = _game.Winner();

        Assert.Equal(Player.X, winner);
    }

    [Fact]
    public void DeclarePlayerOAsAWinnerIfThreeInTopRow()
    {
        _game.Play(new Tile(new Coordinate(2, 2), Player.X));
        _game.Play(new Tile(new Coordinate(0, 0), Player.O));
        _game.Play(new Tile(new Coordinate(1, 0), Player.X));
        _game.Play(new Tile(new Coordinate(0, 1), Player.O));
        _game.Play(new Tile(new Coordinate(1, 1), Player.X));
        _game.Play(new Tile(new Coordinate(0, 2), Player.O));

        var winner = _game.Winner();

        Assert.Equal(Player.O, winner);
    }

    [Fact]
    public void DeclarePlayerXAsAWinnerIfThreeInMiddleRow()
    {
        _game.Play(new Tile(new Coordinate(1, 0), Player.X));
        _game.Play(new Tile(new Coordinate(0, 0), Player.O));
        _game.Play(new Tile(new Coordinate(1, 1), Player.X));
        _game.Play(new Tile(new Coordinate(0, 1), Player.O));
        _game.Play(new Tile(new Coordinate(1, 2), Player.X));

        var winner = _game.Winner();

        Assert.Equal(Player.X, winner);
    }

    [Fact]
    public void DeclarePlayerOAsAWinnerIfThreeInMiddleRow()
    {
        _game.Play(new Tile(new Coordinate(0, 0), Player.X));
        _game.Play(new Tile(new Coordinate(1, 0), Player.O));
        _game.Play(new Tile(new Coordinate(2, 0), Player.X));
        _game.Play(new Tile(new Coordinate(1, 1), Player.O));
        _game.Play(new Tile(new Coordinate(2, 1), Player.X));
        _game.Play(new Tile(new Coordinate(1, 2), Player.O));

        var winner = _game.Winner();

        Assert.Equal(Player.O, winner);
    }

    [Fact]
    public void DeclarePlayerXAsAWinnerIfThreeInBottomRow()
    {
        _game.Play(new Tile(new Coordinate(2, 0), Player.X));
        _game.Play(new Tile(new Coordinate(0, 0), Player.O));
        _game.Play(new Tile(new Coordinate(2, 1), Player.X));
        _game.Play(new Tile(new Coordinate(0, 1), Player.O));
        _game.Play(new Tile(new Coordinate(2, 2), Player.X));

        var winner = _game.Winner();

        Assert.Equal(Player.X, winner);
    }

    [Fact]
    public void DeclarePlayerOAsAWinnerIfThreeInBottomRow()
    {
        _game.Play(new Tile(new Coordinate(0, 0), Player.X));
        _game.Play(new Tile(new Coordinate(2, 0), Player.O));
        _game.Play(new Tile(new Coordinate(1, 0), Player.X));
        _game.Play(new Tile(new Coordinate(2, 1), Player.O));
        _game.Play(new Tile(new Coordinate(1, 1), Player.X));
        _game.Play(new Tile(new Coordinate(2, 2), Player.O));

        var winner = _game.Winner();

        Assert.Equal(Player.O, winner);
    }
}

public enum Player
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
    private readonly Row _row;
    private readonly Column _column;

    public Coordinate(int row, int column)
    {
        this._row = row switch
        {
            0 => Row.Top,
            1 => Row.Middle,
            2 => Row.Bottom,
            _ => throw new ArgumentOutOfRangeException(nameof(row), row, "Invalid row")
        };

        this._column = column switch
        {
            0 => Column.Left,
            1 => Column.Center,
            2 => Column.Right,
            _ => throw new ArgumentOutOfRangeException(nameof(column), column, "Invalid column")
        };
    }
    
    private bool Equals(Coordinate other)
    {
        return _row == other._row && _column == other._column;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == this.GetType() && Equals((Coordinate)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)_row, (int)_column);
    }
}


public class Tile
{
    private readonly Coordinate _coordinate;
    
    public Player Player { get; private set; }

    public Tile(Coordinate coordinate, Player player = Player.Empty)
    {
        this._coordinate = coordinate;
        Player = player;
    }

    public bool HasSamePlayer(Tile other)
    {
        return Player == other.Player;
    }

    public bool IsTaken()
    {
        return Player != Player.Empty;
    }

    public bool HasSamePosition(Tile other)
    {
        return Equals(_coordinate, other._coordinate);
    }

    public void UpdatePlayer(Player newPlayer)
    {
        Player = newPlayer;
    }
}

public class Board
{
    private readonly List<Tile> _plays = new();

    public Board()
    {
        for (var row = 0; row < 3; row++)
        {
            for (var column = 0; column < 3; column++)
            {
                _plays.Add(new Tile(new Coordinate(row, column)));
            }
        }
    }
    public Tile TileAt(Tile other)
    {
        return _plays.Single(tile => tile.HasSamePosition(other));
    }

    public void AddTileAt(Tile newTile)
    {
        TileAt(newTile).UpdatePlayer(newTile.Player);
    }

    public Player FindPlayerWhoTookARow()
    {
        for (var rowIndex = 0; rowIndex < 3; rowIndex++)
        {
            if (IsRowTakenWithPlayer(rowIndex))
            {
                return TileAt(new Tile(new Coordinate(rowIndex, 0))).Player;
            } 
        }
            
        return Player.Empty;
    }

    private bool IsRowTakenWithPlayer(int rowIndex)
    {
        return IsRowTaken(rowIndex) &&
               HasRowSamePlayer(rowIndex);
    }

    private bool HasRowSamePlayer(int rowIndex)
    {
        return (TileAt(new Tile(new Coordinate(rowIndex, 0))).HasSamePlayer(
                TileAt(new Tile(new Coordinate(rowIndex, 1)))) &&
                TileAt(new Tile(new Coordinate(rowIndex, 2))).HasSamePlayer(
                TileAt(new Tile(new Coordinate(rowIndex, 1)))));
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
    private Player _lastPlayer = Player.Empty;

    public void Play(Tile newTile)
    {
        ValidateFirstMove(newTile.Player);
        ValidatePlayer(newTile.Player);
        ValidatePositionIsEmpty(newTile);

        UpdateLastPlayer(newTile.Player);
        UpdateBoard(newTile);
    }

    private void UpdateBoard(Tile newTile)
    {
        _board.AddTileAt(newTile);
    }

    private void UpdateLastPlayer(Player newPlayer)
    {
        _lastPlayer = newPlayer;
    }

    private void ValidatePositionIsEmpty(Tile tile)
    {
        if (_board.TileAt(tile).IsTaken())
        {
            throw new Exception("Invalid position");
        }
    }

    private void ValidatePlayer(Player player)
    {
        if (player == _lastPlayer)
        {
            throw new Exception("Invalid next player");
        }
    }

    private void ValidateFirstMove(Player player)
    {
        if (_lastPlayer == Player.Empty && player == Player.O)
        {
            throw new Exception("Invalid first player");
        }
    }

    public Player Winner()
    {
        return _board.FindPlayerWhoTookARow();
    }
}
using Xunit;

namespace RefactoringGolf.hole13;

public class GameShould
{
    private readonly Game _game = new();

    [Fact]
    public void NotAllowPlayerOToPlayFirst()
    {
        var exception = Assert.Throws<Exception>(() =>
        {
            _game.Play(new Tile(new Coordinate(0, 0), Player.O));
        });
        Assert.Equal("Invalid player", exception.Message);
    }

    [Fact]
    public void NotAllowPlayerXToPlayTwiceInARow()
    {
        _game.Play(new Tile(new Coordinate(0, 0), Player.X));

        var exception = Assert.Throws<Exception>(() =>
        {
            _game.Play(new Tile(new Coordinate(1, 0), Player.X));
        });
        Assert.Equal("Invalid player", exception.Message);
    }

    [Fact]
    public void NotAllowPlayerToPlayInLastPlayedPosition()
    {
        _game.Play(new Tile(new Coordinate(0, 0), Player.X));

        var exception = Assert.Throws<Exception>(() =>
        {
            _game.Play(new Tile(new Coordinate(0, 0), Player.O));
        });
        Assert.Equal("Invalid position", exception.Message);
    }

    [Fact]
    public void NotAllowPlayerToPlayInAnyPlayedPosition()
    {
        _game.Play(new Tile(new Coordinate(0, 0), Player.X));
        _game.Play(new Tile(new Coordinate(1, 0), Player.O));

        var exception = Assert.Throws<Exception>(() =>
        {
            _game.Play(new Tile(new Coordinate(0, 0), Player.X));
        });
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
    None,
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
        _row = ToRow(row);
        _column = ToColumn(column);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Coordinate)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)_row, (int)_column);
    }
    
    private static Column ToColumn(int column)
    {
        return column switch
        {
            0 => Column.Left,
            1 => Column.Center,
            2 => Column.Right,
            _ => throw new ArgumentOutOfRangeException(nameof(column), column, "Invalid column")
        };
    }

    private static Row ToRow(int row)
    {
        return row switch
        {
            0 => Row.Top,
            1 => Row.Middle,
            2 => Row.Bottom,
            _ => throw new ArgumentOutOfRangeException(nameof(row), row, "Invalid row")
        };
    }
    
    private bool Equals(Coordinate other)
    {
        return _row == other._row && _column == other._column;
    }
}


public class Tile
{
    private readonly Coordinate _coordinate;
    
    public Player Player { get; private set; }

    public Tile(Coordinate coordinate, Player player = Player.None)
    {
        this._coordinate = coordinate;
        Player = player;
    }

    public bool HasSamePlayerHas(Tile other)
    {
        return Player == other.Player;
    }

    public bool IsTaken()
    {
        return Player != Player.None;
    }

    public bool HasSamePositionHas(Tile other)
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
        return _plays.Single(tile => tile.HasSamePositionHas(other));
    }

    public void AddTileAt(Tile newTile)
    {
        TileAt(newTile).UpdatePlayer(newTile.Player);
    }

    public Player FindPlayerWhoTookARow()
    {
        for (var rowIndex = 0; rowIndex < 3; rowIndex++)
        {
            if (IsRowTakenWithSamePlayer(rowIndex))
            {
                return TileAt(new Tile(new Coordinate(rowIndex, 0))).Player;
            } 
        }
            
        return Player.None;
    }

    private bool IsRowTakenWithSamePlayer(int rowIndex)
    {
        return IsRowTaken(rowIndex) &&
               IsSamePlayerInRow(rowIndex);
    }

    private bool IsRowTaken(int rowIndex)
    {
        return TileAtRowLeftColumn(rowIndex).IsTaken() &&
               TileAtRowCenterColumn(rowIndex).IsTaken() &&
               TileAtRowRightColumn(rowIndex).IsTaken();
    }
    
    private bool IsSamePlayerInRow(int rowIndex)
    {
        return TileAtRowLeftColumn(rowIndex).HasSamePlayerHas(
                   TileAtRowCenterColumn(rowIndex)) &&
               TileAtRowLeftColumn(rowIndex).HasSamePlayerHas(
                   TileAtRowRightColumn(rowIndex));
    }

    private Tile TileAtRowLeftColumn(int rowIndex)
    {
        return TileAt(new Tile(new Coordinate(rowIndex, 0)));
    }

    private Tile TileAtRowCenterColumn(int rowIndex)
    {
        return TileAt(new Tile(new Coordinate(rowIndex, 1)));
    }

    private Tile TileAtRowRightColumn(int rowIndex)
    {
        return TileAt(new Tile(new Coordinate(rowIndex, 2)));
    }
}

public class Game
{
    private readonly Board _board = new();
    private Player _lastPlayer = Player.O;

    public void Play(Tile tile)
    {
        ValidatePlayer(tile.Player);
        ValidatePosition(tile);

        UpdateLastPlayer(tile.Player);
        UpdateBoard(tile);
    }
    
    public Player Winner()
    {
        return _board.FindPlayerWhoTookARow();
    }

    private void ValidatePlayer(Player player)
    {
        if (player == _lastPlayer)
        {
            throw new Exception("Invalid player");
        }
    }
    
    private void ValidatePosition(Tile tile)
    {
        if (_board.TileAt(tile).IsTaken())
        {
            throw new Exception("Invalid position");
        }
    }
    
    private void UpdateLastPlayer(Player player)
    {
        _lastPlayer = player;
    }
    
    private void UpdateBoard(Tile tile)
    {
        _board.AddTileAt(tile);
    }
}
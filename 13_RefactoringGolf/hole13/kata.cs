using Xunit;

namespace RefactoringGolf.hole13;

public class GameShould
{
    private readonly Game game = new();

    [Fact]
    public void NotAllowPlayerOToPlayFirst()
    {
        var exception = Assert.Throws<Exception>(() => { game.Play(new Tile(new Coordinate(0, 0), Player.O)); });
        Assert.Equal("Invalid player", exception.Message);
    }

    [Fact]
    public void NotAllowPlayerXToPlayTwiceInARow()
    {
        game.Play(new Tile(new Coordinate(0, 0), Player.X));

        var exception = Assert.Throws<Exception>(() => { game.Play(new Tile(new Coordinate(1, 0), Player.X)); });
        Assert.Equal("Invalid player", exception.Message);
    }

    [Fact]
    public void NotAllowPlayerToPlayInLastPlayedPosition()
    {
        game.Play(new Tile(new Coordinate(0, 0), Player.X));

        var exception = Assert.Throws<Exception>(() => { game.Play(new Tile(new Coordinate(0, 0), Player.O)); });
        Assert.Equal("Invalid position", exception.Message);
    }

    [Fact]
    public void NotAllowPlayerToPlayInAnyPlayedPosition()
    {
        game.Play(new Tile(new Coordinate(0, 0), Player.X));
        game.Play(new Tile(new Coordinate(1, 0), Player.O));

        var exception = Assert.Throws<Exception>(() => { game.Play(new Tile(new Coordinate(0, 0), Player.X)); });
        Assert.Equal("Invalid position", exception.Message);
    }

    [Fact]
    public void DeclarePlayerXAsAWinnerIfThreeInTopRow()
    {
        game.Play(new Tile(new Coordinate(0, 0), Player.X));
        game.Play(new Tile(new Coordinate(1, 0), Player.O));
        game.Play(new Tile(new Coordinate(0, 1), Player.X));
        game.Play(new Tile(new Coordinate(1, 1), Player.O));
        game.Play(new Tile(new Coordinate(0, 2), Player.X));

        var winner = game.Winner();

        Assert.Equal(Player.X, winner);
    }

    [Fact]
    public void DeclarePlayerOAsAWinnerIfThreeInTopRow()
    {
        game.Play(new Tile(new Coordinate(2, 2), Player.X));
        game.Play(new Tile(new Coordinate(0, 0), Player.O));
        game.Play(new Tile(new Coordinate(1, 0), Player.X));
        game.Play(new Tile(new Coordinate(0, 1), Player.O));
        game.Play(new Tile(new Coordinate(1, 1), Player.X));
        game.Play(new Tile(new Coordinate(0, 2), Player.O));

        var winner = game.Winner();

        Assert.Equal(Player.O, winner);
    }

    [Fact]
    public void DeclarePlayerXAsAWinnerIfThreeInMiddleRow()
    {
        game.Play(new Tile(new Coordinate(1, 0), Player.X));
        game.Play(new Tile(new Coordinate(0, 0), Player.O));
        game.Play(new Tile(new Coordinate(1, 1), Player.X));
        game.Play(new Tile(new Coordinate(0, 1), Player.O));
        game.Play(new Tile(new Coordinate(1, 2), Player.X));

        var winner = game.Winner();

        Assert.Equal(Player.X, winner);
    }

    [Fact]
    public void DeclarePlayerOAsAWinnerIfThreeInMiddleRow()
    {
        game.Play(new Tile(new Coordinate(0, 0), Player.X));
        game.Play(new Tile(new Coordinate(1, 0), Player.O));
        game.Play(new Tile(new Coordinate(2, 0), Player.X));
        game.Play(new Tile(new Coordinate(1, 1), Player.O));
        game.Play(new Tile(new Coordinate(2, 1), Player.X));
        game.Play(new Tile(new Coordinate(1, 2), Player.O));

        var winner = game.Winner();

        Assert.Equal(Player.O, winner);
    }

    [Fact]
    public void DeclarePlayerXAsAWinnerIfThreeInBottomRow()
    {
        game.Play(new Tile(new Coordinate(2, 0), Player.X));
        game.Play(new Tile(new Coordinate(0, 0), Player.O));
        game.Play(new Tile(new Coordinate(2, 1), Player.X));
        game.Play(new Tile(new Coordinate(0, 1), Player.O));
        game.Play(new Tile(new Coordinate(2, 2), Player.X));

        var winner = game.Winner();

        Assert.Equal(Player.X, winner);
    }

    [Fact]
    public void DeclarePlayerOAsAWinnerIfThreeInBottomRow()
    {
        game.Play(new Tile(new Coordinate(0, 0), Player.X));
        game.Play(new Tile(new Coordinate(2, 0), Player.O));
        game.Play(new Tile(new Coordinate(1, 0), Player.X));
        game.Play(new Tile(new Coordinate(2, 1), Player.O));
        game.Play(new Tile(new Coordinate(1, 1), Player.X));
        game.Play(new Tile(new Coordinate(2, 2), Player.O));

        var winner = game.Winner();

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
    private readonly Column column;
    private readonly Row row;

    public Coordinate(int row, int column) : this(ToRow(row), ToColumn(column)) {}
    
    public Coordinate(Row row, Column column)
    {
        this.row = row;
        this.column = column;
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

    private bool Equals(Coordinate other)
    {
        return row == other.row && column == other.column;
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
}

public class Tile
{
    private readonly Coordinate coordinate;

    public Tile(Coordinate coordinate, Player player = Player.None)
    {
        this.coordinate = coordinate;
        Player = player;
    }

    public Player Player { get; private set; }

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
        return Equals(coordinate, other.coordinate);
    }

    public void UpdatePlayer(Player newPlayer)
    {
        Player = newPlayer;
    }
}

public class Board
{
    private readonly List<Tile> plays = new();

    public Board()
    {
        for (var row = 0; row < 3; row++)
        for (var column = 0; column < 3; column++)
            plays.Add(new Tile(new Coordinate(row, column)));
    }

    public Tile TileAt(Tile other)
    {
        return plays.Single(tile => tile.HasSamePositionHas(other));
    }

    public void AddTileAt(Tile newTile)
    {
        TileAt(newTile).UpdatePlayer(newTile.Player);
    }

    public Player FindPlayerWhoTookARow()
    {
        for (var rowIndex = 0; rowIndex < 3; rowIndex++)
            if (IsRowTakenWithSamePlayer(rowIndex))
                return TileAt(new Tile(new Coordinate(rowIndex, 0))).Player;

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
        return TileAt(new Tile(new Coordinate(rowIndex, (int)Column.Left)));
    }

    private Tile TileAtRowCenterColumn(int rowIndex)
    {
        return TileAt(new Tile(new Coordinate(rowIndex, (int)Column.Center)));
    }

    private Tile TileAtRowRightColumn(int rowIndex)
    {
        return TileAt(new Tile(new Coordinate(rowIndex, (int)Column.Right)));
    }
}

public class Game
{
    private readonly Board board = new();
    private Player lastPlayer = Player.O;

    public void Play(Tile tile)
    {
        ValidatePlayer(tile.Player);
        ValidatePosition(tile);

        UpdateLastPlayer(tile.Player);
        UpdateBoard(tile);
    }

    public Player Winner()
    {
        return board.FindPlayerWhoTookARow();
    }

    private void ValidatePlayer(Player player)
    {
        if (player == lastPlayer) throw new Exception("Invalid player");
    }

    private void ValidatePosition(Tile tile)
    {
        if (board.TileAt(tile).IsTaken()) throw new Exception("Invalid position");
    }

    private void UpdateLastPlayer(Player player)
    {
        lastPlayer = player;
    }

    private void UpdateBoard(Tile tile)
    {
        board.AddTileAt(tile);
    }
}
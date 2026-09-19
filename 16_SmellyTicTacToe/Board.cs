namespace SmellyTicTacToeKata;

public class Tile
{
    public int X { get; init; }
    public int Y { get; init; }
    public char Symbol { get; set; }
}

public class Board
{
    private readonly List<Tile> plays = [];

    public Board()
    {
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                plays.Add(new Tile { X = i, Y = j, Symbol = ' ' });
            }
        }
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
}

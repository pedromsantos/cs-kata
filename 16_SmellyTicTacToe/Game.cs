namespace SmellyTicTacToeKata;

public class Game
{
    private char lastSymbol = ' ';
    private readonly Board board = new Board();
    private readonly RowWinnerChecker rowWinnerChecker = new();
    private readonly ColumnWinnerChecker columnWinnerChecker = new();
    private readonly DiagonalWinnerChecker diagonalWinnerChecker = new();

    public void Play(char symbol, int x, int y)
    {
        //if first move
        if (lastSymbol == ' ')
        {
            //if player is X
            if (symbol == 'O')
            {
                throw new Exception("Invalid first player");
            }
        }
        //if not first move but player repeated
        else if (symbol == lastSymbol)
        {
            throw new Exception("Invalid next player");
        }
        //if not first move but play on an already played tile
        else if (board.TileAt(x, y).Symbol != ' ')
        {
            throw new Exception("Invalid position");
        }

        // update game state
        lastSymbol = symbol;
        board.AddTileAt(symbol, x, y);
    }

    public char Winner()
    {
        var rowWinner = rowWinnerChecker.Check(board);
        if (rowWinner != ' ')
        {
            return rowWinner;
        }

        var columnWinner = columnWinnerChecker.Check(board);
        if (columnWinner != ' ')
        {
            return columnWinner;
        }

        return diagonalWinnerChecker.Check(board);
    }
}

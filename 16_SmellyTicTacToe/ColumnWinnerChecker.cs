namespace SmellyTicTacToeKata;

// Cross-file Duplicated Code / Shotgun Surgery kata fixture: this checker
// re-implements the exact same "are these three tiles taken and equal"
// pattern as RowWinnerChecker, independently, instead of sharing one
// extracted line-checking algorithm.
public class ColumnWinnerChecker
{
    public char Check(Board board)
    {
        if (board.TileAt(0, 0).Symbol != ' ' &&
                   board.TileAt(1, 0).Symbol != ' ' &&
                   board.TileAt(2, 0).Symbol != ' ')
        {
            if (board.TileAt(0, 0).Symbol ==
                board.TileAt(1, 0).Symbol &&
                board.TileAt(2, 0).Symbol ==
                board.TileAt(1, 0).Symbol)
            {
                return board.TileAt(0, 0).Symbol;
            }
        }

        if (board.TileAt(0, 1).Symbol != ' ' &&
           board.TileAt(1, 1).Symbol != ' ' &&
           board.TileAt(2, 1).Symbol != ' ')
        {
            if (board.TileAt(0, 1).Symbol ==
                board.TileAt(1, 1).Symbol &&
                board.TileAt(2, 1).Symbol ==
                board.TileAt(1, 1).Symbol)
            {
                return board.TileAt(0, 1).Symbol;
            }
        }

        if (board.TileAt(0, 2).Symbol != ' ' &&
           board.TileAt(1, 2).Symbol != ' ' &&
           board.TileAt(2, 2).Symbol != ' ')
        {
            if (board.TileAt(0, 2).Symbol ==
                board.TileAt(1, 2).Symbol &&
                board.TileAt(2, 2).Symbol ==
                board.TileAt(1, 2).Symbol)
            {
                return board.TileAt(0, 2).Symbol;
            }
        }

        return ' ';
    }
}

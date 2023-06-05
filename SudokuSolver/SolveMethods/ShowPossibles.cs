namespace SudokuSolver.SolveMethods;

public class ShowPossibles : ISolveMethod
{
    private bool IsUsed = false;

    public bool TrySolve(Sudoku sudoku)
    {
        bool result = false;
       
        if (!IsUsed)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (ShowPossiblesForCell(sudoku, sudoku.Rows[i][j])) result = true;
                }
            }
            IsUsed = true;
        }
        else
        {
            for (int i = 0;i < sudoku.newFoundCells.Count ;i++)
            {
                if (ShowPossiblesForCell(sudoku, sudoku.newFoundCells[i])) result = true;
            }
        }
        
        sudoku.newFoundCells?.Clear();
        return result;
    }

    public bool ShowPossiblesForCell(Sudoku sudoku, Cell cell)
    {
        if (cell.Value == 0) return false;

        bool result = false;

        if (cell.Value != 0)
        {
            int v = cell.Value, 
                s = cell.S,
                r = cell.I,
                c = cell.J;


            for (int k = 0; k < 9; k++)
            {
                if (
                    !sudoku.Rows[r][k].Options[v - 1] &&
                    !sudoku.Columns[c][k].Options[v - 1] &&
                    !sudoku.Squares[s][k].Options[v - 1])
                    continue;

                sudoku.Rows[r][k].Options[v - 1] = false;
                sudoku.Columns[c][k].Options[v - 1] = false;
                sudoku.Squares[s][k].Options[v - 1] = false;

                result = true;
            }

        }

        return result;
    }


}
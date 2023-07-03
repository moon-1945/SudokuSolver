namespace SudokuSolver.SolveMethods.BasicStrategies;

public class CheckForSolvedCells : ISolveMethod
{
    
    public bool TrySolve(Sudoku sudoku)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                int singleOrZero = sudoku.Rows[i][j].GetSingleOrZero();

                if (singleOrZero == 0) continue;  

                sudoku.NewFoundCells.Add(sudoku.Rows[i][j]);

                sudoku.Rows[i][j].Value = singleOrZero;
                sudoku.Rows[i][j].Options[singleOrZero - 1] = false;
            }
        }
        return false;
    }
}
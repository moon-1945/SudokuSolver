using System.Collections;

namespace SudokuSolver.SolveMethods;

public class HidenSingle : ISolveMethod
{
    public bool TrySolve(Sudoku sudoku)
    {
        for (int numBit = 0; numBit < 9; numBit++)
        {
            for (int k = 0; k < 9; k++)
            {
                Cell? cell;
                if ((cell = sudoku.GetSingleOrNull(sudoku.Rows[k], numBit)) != null)
                {
                    sudoku.newFoundCells.Add(cell);
                    cell.Value = numBit + 1;
                    cell.Options = new BitArray(9);
                    return true;
                }

                if ((cell = sudoku.GetSingleOrNull(sudoku.Columns[k], numBit)) != null)
                {
                    sudoku.newFoundCells.Add(cell);
                    cell.Value = numBit + 1;
                    cell.Options = new BitArray(9);
                    return true;
                }

                if ((cell = sudoku.GetSingleOrNull(sudoku.Squares[k], numBit)) != null)
                {
                    sudoku.newFoundCells.Add(cell);
                    cell.Value = numBit + 1;
                    cell.Options = new BitArray(9);
                    return true;
                }
            }
        }

        return false;
    }
}
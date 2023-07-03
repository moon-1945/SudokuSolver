using SudokuSolver.Sudoku;
using System.Collections;

namespace SudokuSolver.SolveMethods.BasicStrategies;

public class HidenSingle : ISolveMethod
{
    public bool TrySolve(SudokuBase sudoku)
    {
        for (int numBit = 0; numBit < 9; numBit++)
        {
            for (int k = 0; k < 9; k++)
            {
                Cell? cell;
                if ((cell = sudoku.GetSingleOrNull(sudoku.Rows[k], numBit)) != null)
                {
                    //sudoku.newFoundCells.Add(cell);
                    //cell.Value = numBit + 1;
                    //cell.Options = new BitArray(9);
                    //return true;
                    //sudoku.newFoundCells.Add(cell);
                    //cell.Value = numBit + 1;
                    cell.Options = new BitArray(9);
                    cell.Options[numBit] = true;
                    return true;
                }

                if ((cell = sudoku.GetSingleOrNull(sudoku.Columns[k], numBit)) != null)
                {
                    //sudoku.newFoundCells.Add(cell);
                    //cell.Value = numBit + 1;
                    //cell.Options = new BitArray(9);
                    //return true;
                    cell.Options = new BitArray(9);
                    cell.Options[numBit] = true;
                    return true;
                }

                if ((cell = sudoku.GetSingleOrNull(sudoku.Squares[k], numBit)) != null)
                {
                    //sudoku.newFoundCells.Add(cell);
                    //cell.Value = numBit + 1;
                    //cell.Options = new BitArray(9);
                    //return true;
                    cell.Options = new BitArray(9);
                    cell.Options[numBit] = true;
                    return true;
                }
            }
        }

        return false;
    }
}
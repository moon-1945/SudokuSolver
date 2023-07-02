using SudokuSolver.SolveMethods.BasicStrategies;

namespace SudokuSolver;

public class Recursion
{
    public Sudoku Solve(Sudoku sudoku)
    {
        new CheckForSolvedCells().TrySolve(sudoku);
        new ShowPossibles().TrySolve(sudoku);

        return RecursionPart(sudoku);
    }

    public Sudoku RecursionPart(Sudoku sudoku)
    {
        if (sudoku.IsSolved()) return sudoku;

        var maskModes = sudoku.GenerateMaskModes();

        for (int count = 1; count < 10; count++)
        {
            for (int mode = 0; mode < 3; mode++)
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int bit = 0; bit < 9; bit++)
                    {
                        int[] numberOfCells = maskModes[mode][i][bit].ToIndicesArray();
                        if (numberOfCells.Length == count)
                        {
                            int numberOfNulls = 0;

                            for (int k = 0; k < count; k++)
                            {
                                Sudoku sudokuClone = sudoku.Clone();
                                SetValue(sudokuClone, sudokuClone.CellModes[mode][i][numberOfCells[k]], bit + 1);

                                Sudoku sudokuSolve = RecursionPart(sudokuClone);

                                if (sudokuSolve != null) return sudokuSolve;
                                numberOfNulls++;
                            }

                            if (numberOfNulls == count) return null;
                        }
                    }
                }
            }
        }
        return null;
    }
    public void SetValue(Sudoku sudoku, Cell cell, int value)
    {
        cell.Value = value;
        cell.Options = new BitArray(9);

        for (int k = 0; k < 9; k++)
        {
            sudoku.Rows[cell.I][k].Options[value - 1] = false;
            sudoku.Columns[cell.J][k].Options[value - 1] = false;
            sudoku.Squares[cell.S][k].Options[value - 1] = false;
        }
    }
}






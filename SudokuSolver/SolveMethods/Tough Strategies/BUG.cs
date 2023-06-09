﻿
namespace SudokuSolver.SolveMethods.ToughStrategies;

public class BUG : ISolveMethod
{
    public bool TrySolve(Sudoku sudoku)
    {
       // Console.WriteLine(sudoku);

        bool result = false;

        Cell BCcell = FindBC(sudoku);
        if (BCcell == null) return false;

        int[] BCValues = BCcell.Options.ToIndicesArray();

        var BCountBits = BCValues.Zip(new int[3]).ToArray();

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                BCountBits[j].Second += sudoku[BCcell.I, i].Options[BCountBits[j].First] ? 1 : 0;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            if (BCountBits[i].Second == 2)
            {
                if (!BCcell.Options[BCountBits[i].First]) continue;

                result = true;
                BCcell.Options[BCountBits[i].First] = false;
            }
        }

        return result;
    }

    Cell FindBC(Sudoku sudoku)
    {
        Cell res = null;
        int count = 0;

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (sudoku[i, j].Value != 0) continue;   
                int len = sudoku[i, j].Options.ToIndicesArray().Length;
                if (len == 2) continue;
                if (len == 3)
                {
                    res = sudoku[i, j];
                    count++;
                } 
                else return null;
            }
        }
        if (count != 1) return null;
        return res;
    }

}

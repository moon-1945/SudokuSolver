namespace SudokuSolver.SolveMethods.BasicStrategies.NakedGroups;

public abstract class NakedGroups
{
    public bool TrySolve(Sudoku sudoku, int groupSize)
    {
        bool result = false;

        Cell[][][] cellModes = { sudoku.Rows, sudoku.Columns, sudoku.Squares };

        for (int mode = 0; mode < cellModes.Length; mode++)
        {
            for (int i = 0; i < 9; i++)
            {
                var cells = cellModes[mode][i];

                int[] moveOnCells = new int[groupSize].Select((elem, index) => index).ToArray();
                do
                {
                    bool containsFilledCells = false;
                    for (int r = 0; r < groupSize; r++)
                    {
                        containsFilledCells |= (cells[moveOnCells[r]].Value != 0);
                    }
                    if (containsFilledCells) continue;

                    int[] bits;

                    BitArray bitArrayOfCells = new BitArray(cells[moveOnCells[0]].Options);
                    
                    for (int r = 1; r < groupSize; r++)
                    {
                        bitArrayOfCells.Or(cells[moveOnCells[r]].Options);

                    }

                    if ((bits = bitArrayOfCells.ToIndicesArray()).Length == groupSize)
                    {
                        foreach (var cell in cells)
                        {
                        
                            bool cellIsChoosen = false;
                            for (int r = 0; r < groupSize; r++)
                            {
                                cellIsChoosen |= (cell == cells[moveOnCells[r]]);
                            }
                            if (cellIsChoosen) continue;

                            bool somethingWillNotChange = true;
                            foreach (int bit in bits) somethingWillNotChange &= (!cell.Options[bit]);
                            if (somethingWillNotChange) continue;
                         
                            result = true;
                            foreach (int bit in bits)
                                cell.Options[bit] = false;
                        }
                    }

                } while (MoveOnChooses(moveOnCells, 9));
            }
        }

        return result;
    }

    private static bool MoveOnChooses(int[] arr, int n)
    {
        if (arr.Length > n) return false;

        int pos = -1;
        for (int i = arr.Length - 1; i >= 0; i--)
        {
            if (arr[i] == n - (arr.Length - i - 1) - 1) continue;
            arr[i]++;
            pos = i;
            break;
        }

        if (pos == -1) return false;

        for (int i = pos + 1; i < arr.Length; i++)
        {
            arr[i] = arr[i - 1] + 1;
        }

        return true;
    }
}


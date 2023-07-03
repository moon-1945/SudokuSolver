using SudokuSolver.Sudoku;

namespace SudokuSolver.SolveMethods.BasicStrategies.HiddenGroups;

public abstract class HiddenGroups
{
    public bool TrySolve(SudokuBase sudoku, int groupSize)
    {
        //Console.WriteLine(sudoku);
        bool result = false;

        Cell[][][] cellModes = sudoku.CellModes;
        BitArray[][][] maskModes = sudoku.GenerateMaskModes();


        for (int mode = 0; mode < cellModes.Length; mode++)
        {
            for (int i = 0; i < 9; i++)
            {
                var cells = cellModes[mode][i];

                var masks = maskModes[mode][i];

                var smasks = masks.Select((bitArrForKey, keyBit) => (bitArrForKey, keyBit))
                    .Where(p => (p.bitArrForKey.ToIndicesArray().Length <= groupSize)
                    && (p.bitArrForKey.ToIndicesArray().Length > 0))
                    .ToArray();


                int[] move = new int[groupSize].Select((elem, index) => index).ToArray();

                if (smasks.Length < groupSize) continue;

                do
                {
                    int[] positionOfOnes;

                    BitArray bitArrayOfmasks = smasks[move[0]].bitArrForKey;
                    for (int r = 1; r < groupSize; r++)
                    {
                        bitArrayOfmasks.Or(smasks[move[r]].bitArrForKey);
                    }

                    if ((positionOfOnes = bitArrayOfmasks.ToIndicesArray()).Length == groupSize)
                    {
                        for (int position = 0; position < positionOfOnes.Length; position++)
                        {
                            BitArray bitArray = new BitArray(cells[positionOfOnes[position]].Options);

                            BitArray bits = new BitArray(9);

                            for (int r = 0; r < smasks.Length; r++)
                            {
                                bits[smasks[r].keyBit] = true;
                            }

                            cells[positionOfOnes[position]].Options.And(bits);

                            for (int k = 0; k < 9; k++)
                            {
                                if (bitArray[k] != cells[positionOfOnes[position]].Options[k]) result = true;
                            }
                        }
                    }
                } while (MoveOnChooses(move, smasks.Length));
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



using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver;



public abstract class HiddenGroups
{
    public bool TrySolve(Sudoku sudoku, int groupSize)
    {
        //Console.WriteLine(sudoku);
        bool result = false;

        BitArray[][] rowsMasks = Enumerable.Range(0, 9).Select(i => Enumerable.Range(0, 9).Select(i => new BitArray(9)).ToArray()).ToArray();
        BitArray[][] columnMasks = Enumerable.Range(0, 9).Select(i => Enumerable.Range(0, 9).Select(i => new BitArray(9)).ToArray()).ToArray();
        BitArray[][] squareMasks = Enumerable.Range(0, 9).Select(i => Enumerable.Range(0, 9).Select(i => new BitArray(9)).ToArray()).ToArray();

        Cell[][][] cellModes = { sudoku.Rows, sudoku.Columns, sudoku.Squares };
        BitArray[][][] maskModes = { rowsMasks, columnMasks, squareMasks };

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                for (int bit = 0; bit < 9; bit++)
                {
                    rowsMasks[i][bit][j] = sudoku.Rows[i][j].Options[bit];
                    columnMasks[j][bit][i] = sudoku.Rows[i][j].Options[bit];
                    squareMasks[3 * (i / 3) + j / 3][bit][3 * (i % 3) + j % 3] = sudoku.Rows[i][j].Options[bit];
                }
            }
        }


        for (int mode = 0; mode < cellModes.Length; mode++)
        {
            for (int i = 0; i < 9; i++)
            {
                var cells = cellModes[mode][i];

                var masks = maskModes[mode][i];

                var smasks = masks.Select((bitArrForKey, keyBit) => (bitArrForKey, keyBit))
                    .Where(p => (p.bitArrForKey.GetArrayOfOnes().Length <= groupSize)
                    && (p.bitArrForKey.GetArrayOfOnes().Length > 0))
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

                    if ((positionOfOnes = bitArrayOfmasks.GetArrayOfOnes()).Length == groupSize)
                    {
                        for (int position = 0; position < positionOfOnes.Length; position++)
                        {
                            BitArray bitArray = new BitArray(cells[positionOfOnes[position]].Options);

                            cells[positionOfOnes[position]].Options.And(bitArrayOfmasks);

                            for (int k = 0; k < bitArray.Length; k++)
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



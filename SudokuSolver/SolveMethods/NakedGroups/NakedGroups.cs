using System;
using System.Collections;
using System.Security.AccessControl;

namespace SudokuSolver.SolveMethods.NakedGroups;

public abstract class NakedGroups
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
                        int[] bits = new int[groupSize].Select((elem, index) => smasks[move[index]].keyBit).ToArray();

                        int pos;

                        switch (mode)
                        {
                            case 0:
                            case 1:

                                bool allInOneSquare = true;

                                for (int r = 0; r < positionOfOnes.Length - 1; r++)
                                {
                                    allInOneSquare &= (cells[positionOfOnes[r]].S == cells[positionOfOnes[r + 1]].S);
                                }

                                pos = cells[positionOfOnes[0]].S;

                                if (allInOneSquare)
                                {
                                    foreach (var cell in sudoku.Squares[pos])
                                    {
                                        bool cellIsChoosen = false;
                                        for (int r = 0; r < positionOfOnes.Length; r++)
                                        {
                                            cellIsChoosen |= cell == cells[positionOfOnes[r]];
                                        }
                                        if (cellIsChoosen) continue;

                                        bool somethingWillNotChange = true;
                                        foreach (int bit in bits) somethingWillNotChange &= !cell.Options[bit];
                                        if (somethingWillNotChange) continue;

                                        result = true;
                                        foreach (int bit in bits)
                                            cell.Options[bit] = false;
                                    }
                                }
                                break;
                            case 2:
                                bool allInOneRow = true;

                                for (int r = 0; r < positionOfOnes.Length - 1; r++)
                                {
                                    allInOneRow &= (cells[positionOfOnes[r]].I == cells[positionOfOnes[r + 1]].I);
                                }

                                bool allInOneColumn = true;

                                for (int r = 0; r < positionOfOnes.Length - 1; r++)
                                {
                                    allInOneColumn &= (cells[positionOfOnes[r]].J == cells[positionOfOnes[r + 1]].J);
                                }

                                if (allInOneRow)
                                {
                                    pos = cells[positionOfOnes[0]].I;
                                    foreach (var cell in sudoku.Rows[pos])
                                    {
                                        bool cellIsChoosen = false;
                                        for (int r = 0; r < positionOfOnes.Length; r++)
                                        {
                                            cellIsChoosen |= cell == cells[positionOfOnes[r]];
                                        }
                                        if (cellIsChoosen) continue;

                                        bool somethingWillNotChange = true;
                                        foreach (int bit in bits) somethingWillNotChange &= !cell.Options[bit];
                                        if (somethingWillNotChange) continue;

                                        result = true;
                                        foreach (int bit in bits)
                                            cell.Options[bit] = false;
                                    }
                                }
                                else if (allInOneColumn)
                                {
                                    pos = cells[positionOfOnes[0]].J;
                                    foreach (var cell in sudoku.Columns[pos])
                                    {
                                        bool cellIsChoosen = false;
                                        for (int r = 0; r < positionOfOnes.Length; r++)
                                        {
                                            cellIsChoosen |= cell == cells[positionOfOnes[r]];
                                        }
                                        if (cellIsChoosen) continue;

                                        bool somethingWillNotChange = true;
                                        foreach (int bit in bits) somethingWillNotChange &= !cell.Options[bit];
                                        if (somethingWillNotChange) continue;

                                        result = true;
                                        foreach (int bit in bits)
                                            cell.Options[bit] = false;
                                    }
                                }
                                break;
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


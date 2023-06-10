
namespace SudokuSolver.SolveMethods;

public class XWing : ISolveMethod
{
    public bool TrySolve(Sudoku sudoku)
    {
        bool[] beginBools = ToBoolArray(sudoku);

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

        for (int i = 0; i < 9; i++)
        {
            for (int j = i + 1; j < 9; j++)
            {
                for (int mode = 0; mode < 1; mode++)
                {
                    for (int bit = 0; bit < 9; bit++)
                    {
                        var cellsI = maskModes[mode][i][bit].GetArrayOfOnes();
                        var cellsJ = maskModes[mode][j][bit].GetArrayOfOnes();

                        if (cellsI.Length != 2 || cellsJ.Length != 2 ||
                            !((IStructuralEquatable)cellsI).Equals(cellsJ, EqualityComparer<int>.Default)) continue;

                        Cell[] choosenCells = { cellModes[mode][i][cellsI[0]],
                                                cellModes[mode][i][cellsI[1]],
                                                cellModes[mode][j][cellsJ[0]],
                                                cellModes[mode][j][cellsJ[1]]};

                        DeleteBitOnCells(cellModes[1 - mode][cellsI[0]], choosenCells, bit);
                        DeleteBitOnCells(cellModes[1 - mode][cellsI[1]], choosenCells, bit);
                    }
                }
            }
        }

        bool[] endBools = ToBoolArray(sudoku);

        return !((IStructuralEquatable)beginBools).Equals(endBools, EqualityComparer<bool>.Default);
    }

    bool[] ToBoolArray(Sudoku sudoku)
    {
        return new bool[729].Select((ElementInit, index) => sudoku.Rows[index / 81][(index - (index / 81) * 81) / 9]
        .Options[index % 9]).ToArray();
    }

    void DeleteBitOnCells(Cell[] cells, Cell[] exceptions, int bit)
    {
        bool[] bitOnException = new bool[exceptions.Length].Select((elem, index) => exceptions[index].Options[bit]).ToArray();

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Options[bit] = false;
        }

        for (int i = 0; i < exceptions.Length; i++)
        {
            exceptions[i].Options[bit] = bitOnException[i];
        }
    }
}

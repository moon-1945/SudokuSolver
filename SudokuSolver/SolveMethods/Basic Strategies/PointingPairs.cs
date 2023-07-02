

using System.Linq.Expressions;

namespace SudokuSolver.SolveMethods.BasicStrategies;

internal class PointingPairs : ISolveMethod
{
    public bool TrySolve(Sudoku sudoku)
    {
        bool[] beginBools = ToBoolArray(sudoku);

        //BitArray[][] rowsMasks = Enumerable.Range(0, 9).Select(i => Enumerable.Range(0, 9).Select(i => new BitArray(9)).ToArray()).ToArray();
        //BitArray[][] columnMasks = Enumerable.Range(0, 9).Select(i => Enumerable.Range(0, 9).Select(i => new BitArray(9)).ToArray()).ToArray();
        //BitArray[][] squareMasks = Enumerable.Range(0, 9).Select(i => Enumerable.Range(0, 9).Select(i => new BitArray(9)).ToArray()).ToArray();

        //Cell[][][] cellModes = { sudoku.Rows, sudoku.Columns, sudoku.Squares };
        //BitArray[][][] maskModes = { rowsMasks, columnMasks, squareMasks };

        //for (int i = 0; i < 9; i++)
        //{
        //    for (int j = 0; j < 9; j++)
        //    {
        //        for (int bit = 0; bit < 9; bit++)
        //        {
        //            rowsMasks[i][bit][j] = sudoku.Rows[i][j].Options[bit];
        //            columnMasks[j][bit][i] = sudoku.Rows[i][j].Options[bit];
        //            squareMasks[3 * (i / 3) + j / 3][bit][3 * (i % 3) + j % 3] = sudoku.Rows[i][j].Options[bit];
        //        }
        //    }
        //}

        Cell[][][] cellModes = sudoku.CellModes;
        BitArray[][][] maskModes = sudoku.GenerateMaskModes();

        for (int mode = 0; mode < cellModes.Length; mode++)
        {
            for (int i = 0; i < 9; i++)
            {
                var cells = cellModes[mode][i];

                var masks = maskModes[mode][i];

                for (int bit = 0; bit < 9; bit++)
                {
                    Cell[] choosenCells = cells.Where(c => c.Options[bit]).ToArray();

                    if (choosenCells.Length < 2) continue;

                    if (mode == 2)
                    {
                        if (SellsInOneRow(choosenCells))
                        {
                            Cell[] row = cellModes[0][choosenCells[0].I];
                            DeleteBitOnCells(row, choosenCells, bit);
                        }

                        if (SellsInOneColumn(choosenCells))
                        {
                            Cell[] column = cellModes[1][choosenCells[0].J];
                            DeleteBitOnCells(column, choosenCells, bit);
                        }
                    }
                    else
                    {
                        if (SellsInOneSquare(choosenCells))
                        {
                            Cell[] square =  cellModes[2][choosenCells[0].S];
                            DeleteBitOnCells(square,choosenCells,bit);
                        }
                    }
                }
            }
        }

        bool[] endBools = ToBoolArray(sudoku);

        return !((IStructuralEquatable)beginBools).Equals(endBools,EqualityComparer<bool>.Default);
    }

    bool[] ToBoolArray(Sudoku sudoku)
    {
        return new bool[729].Select((ElementInit,index) => sudoku.Rows[index / 81][(index - (index / 81) *81) / 9]
        .Options[index % 9]).ToArray();
    }

    bool SellsInOneRow(Cell[] cells)
    {
        if (cells.Length == 0) throw new Exception("");
        if (cells.Length == 1) return true;
        int n = 0;
        for (int i = 1; i < cells.Length; i++)
        {
            if (cells[i].I != cells[i - 1].I) { n++; }
        }
        return n == 0;
    }

    bool SellsInOneColumn(Cell[] cells)
    {
        if (cells.Length == 0) throw new Exception("");
        if (cells.Length == 1) return true;
        int n = 0;
        for (int i = 1; i < cells.Length; i++)
        {
            if (cells[i].J != cells[i - 1].J) { n++; }
        }
        return n == 0;
    }

    bool SellsInOneSquare(Cell[] cells)
    {
        if (cells.Length == 0) throw new Exception("");
        if (cells.Length == 1) return true;
        int n = 0;
        for (int i = 1; i < cells.Length; i++)
        {
            if (cells[i].S != cells[i - 1].S) { n++; }
        }
        return n == 0;
    }

    void DeleteBitOnCells(Cell[] cells, Cell[] exceptions, int bit)
    {
        bool[] bitOnException = new bool[exceptions.Length].Select((elem,index) => exceptions[index].Options[bit]).ToArray();

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




using System.Text;

namespace SudokuSolver.Sudoku;

public class OldSudoku : Sudoku
{
    private Cell[][] _rows { get; set; }
    private Cell[][] _columns { get; set; }
    private Cell[][] _squares { get; set; }
    private Cell[][][] _cellModes { get; set; }
    private List<Cell> _newFoundCells { get; set; }

    public override Cell[][] Rows { get => _rows; set => _rows = value; }
    public override Cell[][] Columns { get => _columns; set => _columns = value; }
    public override Cell[][] Squares { get => _squares; set => _squares = value; }
    public override Cell[][][] CellModes { get => _cellModes; set => _cellModes = value; }
    public override List<Cell> NewFoundCells { get => _newFoundCells; set => _newFoundCells = value; }

    public override Cell this[int i, int j] => _rows[i][j];

    private OldSudoku() { }

    public OldSudoku(string sudokuCode)
    {
        NewFoundCells = new List<Cell>();
        if (sudokuCode.Length != 81) throw new ArgumentException("Wrong size of array");

        Rows = new Cell[9][];
        Columns = new Cell[9][];
        Squares = new Cell[9][];
        CellModes = new[] { Rows, Columns, Squares };

        for (int i = 0; i < 9; i++)
        {
            Rows[i] = new Cell[9];
            Columns[i] = new Cell[9];
            Squares[i] = new Cell[9];
        }

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (sudokuCode[9 * i + j] > '9' | sudokuCode[9 * i + j] < '0') throw new ArgumentException($"cell[{i},{j}] is not correct");

                BitArray cellBitArray = sudokuCode[9 * i + j] == '0' ? new BitArray(9).Not() : new BitArray(9);

                Cell cell = new Cell(i, j, sudokuCode[9 * i + j] - '0', cellBitArray);
                // if (cells[i, j] != 0) cell.isSolvedAndChecked = true; 

                Rows[i][j] = cell;
                Columns[j][i] = cell;
                Squares[3 * (i / 3) + j / 3][3 * (i % 3) + j % 3] = cell;
            }
        }
    }

    public OldSudoku(int[][][] grid)
    {
        Rows = Enumerable.Range(0, 9).Select(i => Enumerable.Repeat<Cell>(null, 9).ToArray()).ToArray();
        Columns = Enumerable.Range(0, 9).Select(i => Enumerable.Repeat<Cell>(null, 9).ToArray()).ToArray();
        Squares = Enumerable.Range(0, 9).Select(i => Enumerable.Repeat<Cell>(null, 9).ToArray()).ToArray();
        CellModes = new[] { Rows, Columns, Squares };

        for (int i = 0; i < 27; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Cell c = Rows[i / 3][j];

                if (c == null)
                {
                    c = new Cell(i / 3, j, 0, new BitArray(9));
                    Rows[i / 3][j] = c;
                    Columns[j][i / 3] = c;
                    Squares[i / 3 / 3 * 3 + j / 3][i / 3 % 3 * 3 + j % 3] = c;
                }

                if (c.Value != 0)
                {
                    continue;
                }

                if (grid[i][j].Length > 0 && grid[i][j][0] < 0)
                {
                    c.Value = -grid[i][j][0];
                    continue;
                }

                for (int k = 0; k < grid[i][j].Length; k++)
                {
                    c.Options[grid[i][j][k] - 1] = true;
                }
            }
        }
    }

    public override Cell? GetSingleOrNull(Cell[] cells, int n)
    {
        int c = 0;
        Cell? cell = null;

        if (cells[0].Options[n])
        {
            c++;
            cell = cells[0];
        }
        if (cells[1].Options[n])
        {
            c++;
            cell = cells[1];
        }
        if (cells[2].Options[n])
        {
            c++;
            cell = cells[2];
        }
        if (cells[3].Options[n])
        {
            c++;
            cell = cells[3];
        }
        if (cells[4].Options[n])
        {
            c++;
            cell = cells[4];
        }
        if (cells[5].Options[n])
        {
            c++;
            cell = cells[5];
        }
        if (cells[6].Options[n])
        {
            c++;
            cell = cells[6];
        }
        if (cells[7].Options[n])
        {
            c++;
            cell = cells[7];
        }
        if (cells[8].Options[n])
        {
            c++;
            cell = cells[8];
        }

        return c == 1 ? cell : null;
    }

    public override string ConvertToStr()
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                sb.Append(this[i, j].Value);
            }
        }

        return sb.ToString();
    }

    public override Sudoku Clone()
    {
        Sudoku sudoku = new OldSudoku();
        sudoku.Rows = Rows.Select(r => r.Select(c => new Cell(c.I, c.J, c.Value, (BitArray)c.Options.Clone())).ToArray()).ToArray();

        sudoku.Columns = new Cell[9][];
        sudoku.Squares = new Cell[9][];
        sudoku.CellModes = new[] { sudoku.Rows, sudoku.Columns, sudoku.Squares };

        for (int i = 0; i < 9; i++)
        {
            sudoku.Columns[i] = new Cell[9];
            sudoku.Squares[i] = new Cell[9];
        }

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                sudoku.Columns[j][i] = sudoku.Rows[i][j];
                sudoku.Squares[3 * (i / 3) + j / 3][3 * (i % 3) + j % 3] = sudoku.Rows[i][j];
            }
        }

        sudoku.NewFoundCells = new List<Cell>(NewFoundCells.Count).Select((s, i) => sudoku.Rows[NewFoundCells[i].I][NewFoundCells[i].J]).ToList();

        return sudoku;
    }

    public override BitArray[][][] GenerateMaskModes()
    {
        BitArray[][] rowsMasks = Enumerable.Range(0, 9).Select(i => Enumerable.Range(0, 9).Select(i => new BitArray(9)).ToArray()).ToArray();
        BitArray[][] columnMasks = Enumerable.Range(0, 9).Select(i => Enumerable.Range(0, 9).Select(i => new BitArray(9)).ToArray()).ToArray();
        BitArray[][] squareMasks = Enumerable.Range(0, 9).Select(i => Enumerable.Range(0, 9).Select(i => new BitArray(9)).ToArray()).ToArray();

        BitArray[][][] maskModes = { rowsMasks, columnMasks, squareMasks };

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                for (int bit = 0; bit < 9; bit++)
                {
                    rowsMasks[i][bit][j] = Rows[i][j].Options[bit];
                    columnMasks[j][bit][i] = Rows[i][j].Options[bit];
                    squareMasks[3 * (i / 3) + j / 3][bit][3 * (i % 3) + j % 3] = Rows[i][j].Options[bit];
                }
            }
        }

        return maskModes;
    }

    public override bool IsSolved()
    {
        int count = 0;

        for (int mode = 0; mode < 3; mode++)
        {
            for (int i = 0; i < 9; i++)
            {
                var values = CellModes[mode][i].Select((s) => s.Value).Where(v => v != 0).ToArray();

                if (values.Length == 9)
                {
                    count++;
                }
            }
        }

        return count == 27;
    }
}

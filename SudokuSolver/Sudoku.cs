using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;

namespace SudokuSolver;

public class Sudoku
{
    private static readonly int[] FullRowMask = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    public Cell[][] Rows;
    public Cell[][] Columns;
    public Cell[][] Squares;
    public Cell[][][] CellModes;

    public List<Cell> newFoundCells;

    public Cell this[int i, int j] => Rows[i][j];

    private Sudoku() { }

    public Sudoku(string sudokuCode)
    {
        newFoundCells = new List<Cell>();
        if ((sudokuCode.Length != 81)) throw new ArgumentException("Wrong size of array");

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

                BitArray cellBitArray = (sudokuCode[9 * i + j] == '0') ? (new BitArray(9)).Not() : (new BitArray(9));

                Cell cell = new Cell(i, j, sudokuCode[9 * i + j] - '0', cellBitArray);
                // if (cells[i, j] != 0) cell.isSolvedAndChecked = true; 

                Rows[i][j] = cell;
                Columns[j][i] = cell;
                Squares[3 * (i / 3) + (j / 3)][3 * (i % 3) + j % 3] = cell;
            }
        }
    }


    public Sudoku(int[][][] grid)
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


    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("╔═══════╤═══════╤═══════╦═══════╤═══════╤═══════╦═══════╤═══════╤═══════╗");
        sb.AppendLine($"║ {Rows[0][0].ToOutput()[0]} │ {Rows[0][1].ToOutput()[0]} │ {Rows[0][2].ToOutput()[0]} ║ {Rows[0][3].ToOutput()[0]} │ {Rows[0][4].ToOutput()[0]} │ {Rows[0][5].ToOutput()[0]} ║ {Rows[0][6].ToOutput()[0]} │ {Rows[0][7].ToOutput()[0]} │ {Rows[0][8].ToOutput()[0]} ║");
        sb.AppendLine($"║ {Rows[0][0].ToOutput()[1]} │ {Rows[0][1].ToOutput()[1]} │ {Rows[0][2].ToOutput()[1]} ║ {Rows[0][3].ToOutput()[1]} │ {Rows[0][4].ToOutput()[1]} │ {Rows[0][5].ToOutput()[1]} ║ {Rows[0][6].ToOutput()[1]} │ {Rows[0][7].ToOutput()[1]} │ {Rows[0][8].ToOutput()[1]} ║");
        sb.AppendLine($"║ {Rows[0][0].ToOutput()[2]} │ {Rows[0][1].ToOutput()[2]} │ {Rows[0][2].ToOutput()[2]} ║ {Rows[0][3].ToOutput()[2]} │ {Rows[0][4].ToOutput()[2]} │ {Rows[0][5].ToOutput()[2]} ║ {Rows[0][6].ToOutput()[2]} │ {Rows[0][7].ToOutput()[2]} │ {Rows[0][8].ToOutput()[2]} ║");
        sb.AppendLine("╟───────┼───────┼───────╫───────┼───────┼───────╫───────┼───────┼───────╢");
        sb.AppendLine($"║ {Rows[1][0].ToOutput()[0]} │ {Rows[1][1].ToOutput()[0]} │ {Rows[1][2].ToOutput()[0]} ║ {Rows[1][3].ToOutput()[0]} │ {Rows[1][4].ToOutput()[0]} │ {Rows[1][5].ToOutput()[0]} ║ {Rows[1][6].ToOutput()[0]} │ {Rows[1][7].ToOutput()[0]} │ {Rows[1][8].ToOutput()[0]} ║");
        sb.AppendLine($"║ {Rows[1][0].ToOutput()[1]} │ {Rows[1][1].ToOutput()[1]} │ {Rows[1][2].ToOutput()[1]} ║ {Rows[1][3].ToOutput()[1]} │ {Rows[1][4].ToOutput()[1]} │ {Rows[1][5].ToOutput()[1]} ║ {Rows[1][6].ToOutput()[1]} │ {Rows[1][7].ToOutput()[1]} │ {Rows[1][8].ToOutput()[1]} ║");
        sb.AppendLine($"║ {Rows[1][0].ToOutput()[2]} │ {Rows[1][1].ToOutput()[2]} │ {Rows[1][2].ToOutput()[2]} ║ {Rows[1][3].ToOutput()[2]} │ {Rows[1][4].ToOutput()[2]} │ {Rows[1][5].ToOutput()[2]} ║ {Rows[1][6].ToOutput()[2]} │ {Rows[1][7].ToOutput()[2]} │ {Rows[1][8].ToOutput()[2]} ║");
        sb.AppendLine("╟───────┼───────┼───────╫───────┼───────┼───────╫───────┼───────┼───────╢");
        sb.AppendLine($"║ {Rows[2][0].ToOutput()[0]} │ {Rows[2][1].ToOutput()[0]} │ {Rows[2][2].ToOutput()[0]} ║ {Rows[2][3].ToOutput()[0]} │ {Rows[2][4].ToOutput()[0]} │ {Rows[2][5].ToOutput()[0]} ║ {Rows[2][6].ToOutput()[0]} │ {Rows[2][7].ToOutput()[0]} │ {Rows[2][8].ToOutput()[0]} ║");
        sb.AppendLine($"║ {Rows[2][0].ToOutput()[1]} │ {Rows[2][1].ToOutput()[1]} │ {Rows[2][2].ToOutput()[1]} ║ {Rows[2][3].ToOutput()[1]} │ {Rows[2][4].ToOutput()[1]} │ {Rows[2][5].ToOutput()[1]} ║ {Rows[2][6].ToOutput()[1]} │ {Rows[2][7].ToOutput()[1]} │ {Rows[2][8].ToOutput()[1]} ║");
        sb.AppendLine($"║ {Rows[2][0].ToOutput()[2]} │ {Rows[2][1].ToOutput()[2]} │ {Rows[2][2].ToOutput()[2]} ║ {Rows[2][3].ToOutput()[2]} │ {Rows[2][4].ToOutput()[2]} │ {Rows[2][5].ToOutput()[2]} ║ {Rows[2][6].ToOutput()[2]} │ {Rows[2][7].ToOutput()[2]} │ {Rows[2][8].ToOutput()[2]} ║");
        sb.AppendLine("╠═══════╪═══════╪═══════╬═══════╪═══════╪═══════╬═══════╪═══════╪═══════╣");
        sb.AppendLine($"║ {Rows[3][0].ToOutput()[0]} │ {Rows[3][1].ToOutput()[0]} │ {Rows[3][2].ToOutput()[0]} ║ {Rows[3][3].ToOutput()[0]} │ {Rows[3][4].ToOutput()[0]} │ {Rows[3][5].ToOutput()[0]} ║ {Rows[3][6].ToOutput()[0]} │ {Rows[3][7].ToOutput()[0]} │ {Rows[3][8].ToOutput()[0]} ║");
        sb.AppendLine($"║ {Rows[3][0].ToOutput()[1]} │ {Rows[3][1].ToOutput()[1]} │ {Rows[3][2].ToOutput()[1]} ║ {Rows[3][3].ToOutput()[1]} │ {Rows[3][4].ToOutput()[1]} │ {Rows[3][5].ToOutput()[1]} ║ {Rows[3][6].ToOutput()[1]} │ {Rows[3][7].ToOutput()[1]} │ {Rows[3][8].ToOutput()[1]} ║");
        sb.AppendLine($"║ {Rows[3][0].ToOutput()[2]} │ {Rows[3][1].ToOutput()[2]} │ {Rows[3][2].ToOutput()[2]} ║ {Rows[3][3].ToOutput()[2]} │ {Rows[3][4].ToOutput()[2]} │ {Rows[3][5].ToOutput()[2]} ║ {Rows[3][6].ToOutput()[2]} │ {Rows[3][7].ToOutput()[2]} │ {Rows[3][8].ToOutput()[2]} ║");
        sb.AppendLine("╟───────┼───────┼───────╫───────┼───────┼───────╫───────┼───────┼───────╢");
        sb.AppendLine($"║ {Rows[4][0].ToOutput()[0]} │ {Rows[4][1].ToOutput()[0]} │ {Rows[4][2].ToOutput()[0]} ║ {Rows[4][3].ToOutput()[0]} │ {Rows[4][4].ToOutput()[0]} │ {Rows[4][5].ToOutput()[0]} ║ {Rows[4][6].ToOutput()[0]} │ {Rows[4][7].ToOutput()[0]} │ {Rows[4][8].ToOutput()[0]} ║");
        sb.AppendLine($"║ {Rows[4][0].ToOutput()[1]} │ {Rows[4][1].ToOutput()[1]} │ {Rows[4][2].ToOutput()[1]} ║ {Rows[4][3].ToOutput()[1]} │ {Rows[4][4].ToOutput()[1]} │ {Rows[4][5].ToOutput()[1]} ║ {Rows[4][6].ToOutput()[1]} │ {Rows[4][7].ToOutput()[1]} │ {Rows[4][8].ToOutput()[1]} ║");
        sb.AppendLine($"║ {Rows[4][0].ToOutput()[2]} │ {Rows[4][1].ToOutput()[2]} │ {Rows[4][2].ToOutput()[2]} ║ {Rows[4][3].ToOutput()[2]} │ {Rows[4][4].ToOutput()[2]} │ {Rows[4][5].ToOutput()[2]} ║ {Rows[4][6].ToOutput()[2]} │ {Rows[4][7].ToOutput()[2]} │ {Rows[4][8].ToOutput()[2]} ║");
        sb.AppendLine("╟───────┼───────┼───────╫───────┼───────┼───────╫───────┼───────┼───────╢");
        sb.AppendLine($"║ {Rows[5][0].ToOutput()[0]} │ {Rows[5][1].ToOutput()[0]} │ {Rows[5][2].ToOutput()[0]} ║ {Rows[5][3].ToOutput()[0]} │ {Rows[5][4].ToOutput()[0]} │ {Rows[5][5].ToOutput()[0]} ║ {Rows[5][6].ToOutput()[0]} │ {Rows[5][7].ToOutput()[0]} │ {Rows[5][8].ToOutput()[0]} ║");
        sb.AppendLine($"║ {Rows[5][0].ToOutput()[1]} │ {Rows[5][1].ToOutput()[1]} │ {Rows[5][2].ToOutput()[1]} ║ {Rows[5][3].ToOutput()[1]} │ {Rows[5][4].ToOutput()[1]} │ {Rows[5][5].ToOutput()[1]} ║ {Rows[5][6].ToOutput()[1]} │ {Rows[5][7].ToOutput()[1]} │ {Rows[5][8].ToOutput()[1]} ║");
        sb.AppendLine($"║ {Rows[5][0].ToOutput()[2]} │ {Rows[5][1].ToOutput()[2]} │ {Rows[5][2].ToOutput()[2]} ║ {Rows[5][3].ToOutput()[2]} │ {Rows[5][4].ToOutput()[2]} │ {Rows[5][5].ToOutput()[2]} ║ {Rows[5][6].ToOutput()[2]} │ {Rows[5][7].ToOutput()[2]} │ {Rows[5][8].ToOutput()[2]} ║");
        sb.AppendLine("╠═══════╪═══════╪═══════╬═══════╪═══════╪═══════╬═══════╪═══════╪═══════╣");
        sb.AppendLine($"║ {Rows[6][0].ToOutput()[0]} │ {Rows[6][1].ToOutput()[0]} │ {Rows[6][2].ToOutput()[0]} ║ {Rows[6][3].ToOutput()[0]} │ {Rows[6][4].ToOutput()[0]} │ {Rows[6][5].ToOutput()[0]} ║ {Rows[6][6].ToOutput()[0]} │ {Rows[6][7].ToOutput()[0]} │ {Rows[6][8].ToOutput()[0]} ║");
        sb.AppendLine($"║ {Rows[6][0].ToOutput()[1]} │ {Rows[6][1].ToOutput()[1]} │ {Rows[6][2].ToOutput()[1]} ║ {Rows[6][3].ToOutput()[1]} │ {Rows[6][4].ToOutput()[1]} │ {Rows[6][5].ToOutput()[1]} ║ {Rows[6][6].ToOutput()[1]} │ {Rows[6][7].ToOutput()[1]} │ {Rows[6][8].ToOutput()[1]} ║");
        sb.AppendLine($"║ {Rows[6][0].ToOutput()[2]} │ {Rows[6][1].ToOutput()[2]} │ {Rows[6][2].ToOutput()[2]} ║ {Rows[6][3].ToOutput()[2]} │ {Rows[6][4].ToOutput()[2]} │ {Rows[6][5].ToOutput()[2]} ║ {Rows[6][6].ToOutput()[2]} │ {Rows[6][7].ToOutput()[2]} │ {Rows[6][8].ToOutput()[2]} ║");
        sb.AppendLine("╟───────┼───────┼───────╫───────┼───────┼───────╫───────┼───────┼───────╢");
        sb.AppendLine($"║ {Rows[7][0].ToOutput()[0]} │ {Rows[7][1].ToOutput()[0]} │ {Rows[7][2].ToOutput()[0]} ║ {Rows[7][3].ToOutput()[0]} │ {Rows[7][4].ToOutput()[0]} │ {Rows[7][5].ToOutput()[0]} ║ {Rows[7][6].ToOutput()[0]} │ {Rows[7][7].ToOutput()[0]} │ {Rows[7][8].ToOutput()[0]} ║");
        sb.AppendLine($"║ {Rows[7][0].ToOutput()[1]} │ {Rows[7][1].ToOutput()[1]} │ {Rows[7][2].ToOutput()[1]} ║ {Rows[7][3].ToOutput()[1]} │ {Rows[7][4].ToOutput()[1]} │ {Rows[7][5].ToOutput()[1]} ║ {Rows[7][6].ToOutput()[1]} │ {Rows[7][7].ToOutput()[1]} │ {Rows[7][8].ToOutput()[1]} ║");
        sb.AppendLine($"║ {Rows[7][0].ToOutput()[2]} │ {Rows[7][1].ToOutput()[2]} │ {Rows[7][2].ToOutput()[2]} ║ {Rows[7][3].ToOutput()[2]} │ {Rows[7][4].ToOutput()[2]} │ {Rows[7][5].ToOutput()[2]} ║ {Rows[7][6].ToOutput()[2]} │ {Rows[7][7].ToOutput()[2]} │ {Rows[7][8].ToOutput()[2]} ║");
        sb.AppendLine("╟───────┼───────┼───────╫───────┼───────┼───────╫───────┼───────┼───────╢");
        sb.AppendLine($"║ {Rows[8][0].ToOutput()[0]} │ {Rows[8][1].ToOutput()[0]} │ {Rows[8][2].ToOutput()[0]} ║ {Rows[8][3].ToOutput()[0]} │ {Rows[8][4].ToOutput()[0]} │ {Rows[8][5].ToOutput()[0]} ║ {Rows[8][6].ToOutput()[0]} │ {Rows[8][7].ToOutput()[0]} │ {Rows[8][8].ToOutput()[0]} ║");
        sb.AppendLine($"║ {Rows[8][0].ToOutput()[1]} │ {Rows[8][1].ToOutput()[1]} │ {Rows[8][2].ToOutput()[1]} ║ {Rows[8][3].ToOutput()[1]} │ {Rows[8][4].ToOutput()[1]} │ {Rows[8][5].ToOutput()[1]} ║ {Rows[8][6].ToOutput()[1]} │ {Rows[8][7].ToOutput()[1]} │ {Rows[8][8].ToOutput()[1]} ║");
        sb.AppendLine($"║ {Rows[8][0].ToOutput()[2]} │ {Rows[8][1].ToOutput()[2]} │ {Rows[8][2].ToOutput()[2]} ║ {Rows[8][3].ToOutput()[2]} │ {Rows[8][4].ToOutput()[2]} │ {Rows[8][5].ToOutput()[2]} ║ {Rows[8][6].ToOutput()[2]} │ {Rows[8][7].ToOutput()[2]} │ {Rows[8][8].ToOutput()[2]} ║");
        sb.AppendLine("╚═══════╧═══════╧═══════╩═══════╧═══════╧═══════╩═══════╧═══════╧═══════╝");
        return sb.ToString();
    }

    public Cell? GetSingleOrNull(Cell[] cells, int n)
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

    public string ConvertToStr()
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

    public Sudoku Clone()
    {
        Sudoku sudoku = new Sudoku();
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
                sudoku.Squares[3 * (i / 3) + (j / 3)][3 * (i % 3) + j % 3] = sudoku.Rows[i][j];
            }
        }

        sudoku.newFoundCells = new List<Cell>(newFoundCells.Count).Select((s, i) => sudoku.Rows[newFoundCells[i].I][newFoundCells[i].J]).ToList();

        return sudoku;
    }

    public BitArray[][][] GenerateMaskModes()
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

    public bool IsSolved()
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

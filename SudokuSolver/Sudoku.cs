using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;

namespace SudokuSolver;

public class Sudoku
{
    public Cell[][] Rows;
    public Cell[][] Columns;
    public Cell[][] Squares;

    public List<Cell> newFoundCells;

    public Cell this[int i,int j] => Rows[i][j];

    public Sudoku() { }

    public Sudoku(int[,] cells)
    {
        newFoundCells = new List<Cell>();   
        if ((cells.GetLength(0) != 9) | (cells.GetLength(1) != 9)) throw new ArgumentException("Wrong size of array");

        Rows = new Cell[9][];
        Columns = new Cell[9][];
        Squares = new Cell[9][];

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
                if (cells[i, j] > 9 | cells[i, j] < 0) throw new ArgumentException($"cell[{i},{j}] is not correct");

                BitArray cellBitArray = (cells[i, j] == 0) ? (new BitArray(9)).Not() : (new BitArray(9));

                Cell cell = new Cell(i, j, cells[i, j], cellBitArray);
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

        for (int i = 0; i < 27; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Cell c = Rows[i / 3][j];

                if(c == null)
                {
                    c = new Cell(i/3, j, 0, new BitArray(9));
                    Rows[i / 3][j] = c;
                    Columns[j][i / 3] = c;
                    Squares[i / 3 / 3 * 3 + j / 3][i / 3 % 3 * 3 + j % 3] = c;
                }

                if(c.Value != 0)
                {
                    continue;
                }

                if(grid[i][j].Length > 0 && grid[i][j][0] < 0)
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


    public void Update(int[,] cells)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                this[i, j].Value = cells[i, j];
                
                if (cells[i, j] == 0)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        this[i, j].Options[k] = true;
                    }
                }
                else
                {
                    for (int k = 0; k < 9; k++)
                    {
                        this[i, j].Options[k] = false;
                    }
                }
            }
        }

        newFoundCells.Clear();
    }

}







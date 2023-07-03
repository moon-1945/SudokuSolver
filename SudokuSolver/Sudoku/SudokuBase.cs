using System.Text;

namespace SudokuSolver.Sudoku;

public abstract class SudokuBase
{
    protected static readonly int[] FullRowMask = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    public abstract Cell[][] Rows { get; set; }
    public abstract Cell[][] Columns { get; set; }
    public abstract Cell[][] Squares { get; set; }
    public abstract Cell[][][] CellModes { get; set; }
    public abstract List<Cell> NewFoundCells { get; set; }

    public abstract Cell this[int i, int j] { get; }

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

    public abstract Cell? GetSingleOrNull(Cell[] cells, int n);

    public abstract string ConvertToStr();

    public abstract SudokuBase Clone();

    public abstract BitArray[][][] GenerateMaskModes();

    public abstract bool IsSolved();
}

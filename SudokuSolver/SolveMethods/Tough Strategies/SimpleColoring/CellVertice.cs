
namespace SudokuSolver.SolveMethods.ToughStrategies.SimpleColoring;

internal class CellVertice
{
    public Cell cell;
    public CellVertice(Cell cell)
    {
        this.cell = cell;
        neighborsOnGraph = new HashSet<CellVertice>[9];
        for (int k = 0; k < 9; k++)
        {
            neighborsOnGraph[k] = new HashSet<CellVertice>(9);
        }

        neighborsOnCommonBits = new List<CellVertice>[9];
        for (int bit = 0; bit < 9; bit++)
        {
            neighborsOnCommonBits[bit] = new List<CellVertice>(9);
        }
    }

    public List<CellVertice>[/*bit*/] neighborsOnCommonBits;

    public HashSet<CellVertice>[/*bit*/] neighborsOnGraph;

    public int[] color = new int[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 };
    public bool IsColored(int bit) => color[bit] != -1;
}

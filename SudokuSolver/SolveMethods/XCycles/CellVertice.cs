namespace SudokuSolver.SolveMethods.XCycles;

class CellVertice
{
    public Cell cell;

    public CellVertice(Cell cell)
    {
        this.cell = cell;
        neighbors = new HashSet<CellVertice>[9];
        neighborsStrongConnection = new HashSet<CellVertice>[9];
        for (int i = 0; i < 9; i++)
        {
            neighbors[i] = new HashSet<CellVertice>();
            neighborsStrongConnection[i] = new HashSet<CellVertice>();
        }
    }

    public HashSet<CellVertice>[] neighbors;

    public HashSet<CellVertice>[] neighborsStrongConnection;
}
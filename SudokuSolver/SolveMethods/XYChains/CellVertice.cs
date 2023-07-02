namespace SudokuSolver.SolveMethods.XYChains;

class CellVertice
{
    public Cell cell;

    public CellVertice(Cell cell) => this.cell = cell;

    public HashSet<CellVertice> neighbors = new HashSet<CellVertice>();
}



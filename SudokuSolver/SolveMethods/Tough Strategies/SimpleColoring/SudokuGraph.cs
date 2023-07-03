namespace SudokuSolver.SolveMethods.ToughStrategies.SimpleColoring;

internal class SudokuGraph
{
    Sudoku sudoku;

    public CellVertice[][] newRows;
    public CellVertice[][] newColumns;
    public CellVertice[][] newSquares;

    public CellVertice this[int i,int j] => newRows[i][j];

    public SudokuGraph(Sudoku sudoku)
    {
        this.sudoku = sudoku;

        newRows = new CellVertice[9][];
        newColumns = new CellVertice[9][];
        newSquares = new CellVertice[9][];

        for (int i = 0; i < 9; i++)
        {
            newRows[i] = new CellVertice[9];
            newColumns[i] = new CellVertice[9];
            newSquares[i] = new CellVertice[9];
        }

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                CellVertice cell = new CellVertice(sudoku.Rows[i][j]);

                newRows[i][j] = cell;
                newColumns[j][i] = cell;
                newSquares[3 * (i / 3) + (j / 3)][3 * (i % 3) + j % 3] = cell;
            }
        }

        CellVertice[][][] cellModes = { newRows, newColumns, newSquares };

        for (int bit = 0; bit < 9; bit++)
        {
            for (int mode = 0; mode < 3; mode++)
            {
                for (int i = 0; i < 9; i++)
                {
                    bool[] boolsMask = new bool[9].Select((elem, index) => cellModes[mode][i][index].cell.Options[bit]).ToArray();
                    BitArray bitMask = new BitArray(boolsMask);

                    int[] numbersOfCells = bitMask.ToIndicesArray();

                    for (int i1 = 0; i1 < numbersOfCells.Length; i1++)
                    {
                        for (int i2 = i1 + 1; i2 < numbersOfCells.Length; i2++)
                        {
                            CellVertice cell1 = cellModes[mode][i][numbersOfCells[i1]];
                            CellVertice cell2 = cellModes[mode][i][numbersOfCells[i2]];

                            cell1.neighborsOnCommonBits[bit].Add(cell2);
                            cell2.neighborsOnCommonBits[bit].Add(cell1);

                            if (numbersOfCells.Length != 2) continue;

                            cell1.neighborsOnGraph[bit].Add(cell2);
                            cell2.neighborsOnGraph[bit].Add(cell1);
                        }
                    }
                }
            }
        }
    }

    static HashSet<CellVertice> usedVertices = new HashSet<CellVertice>();
    public void ColorTree(CellVertice topVertex, int bit, int color, int o = 1)
    {
        if (o != 1 && o != 2) throw new Exception("ColorTree");
        usedVertices.Add(topVertex);
        topVertex.color[bit] = 3 * color + o;
        foreach (var neighborCell in topVertex.neighborsOnGraph[bit])
        {
            if (!usedVertices.Contains(neighborCell)) ColorTree(neighborCell, bit, color, 3 - o);
        }
    }
    public void ColorGraph()
    {
        for (int bit = 0; bit < 9; bit++)
        {
            int color = 1;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (this[i,j].IsColored(bit)) continue;
                    usedVertices.Clear();
                    ColorTree(this[i, j], bit,color);
                    color++;
                }
            }
        }
    }
}
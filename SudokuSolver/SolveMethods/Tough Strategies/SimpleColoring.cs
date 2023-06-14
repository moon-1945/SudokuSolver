
namespace SudokuSolver.SolveMethods.ToughStrategies;

public class SimpleColoring : ISolveMethod
{
    public bool TrySolve(Sudoku sudoku)
    {
        bool[] beginBools = ToBoolArray(sudoku);

        SudokuGraph sudokuGraph = new SudokuGraph(sudoku);

        sudokuGraph.ColorGraph();

        HashSet<CellVertice>[] cellRemoveBit = CellsToRemoveBit(sudokuGraph);

        RemoveBits(sudokuGraph, cellRemoveBit);

        bool[] endBools = ToBoolArray(sudoku);

        return !((IStructuralEquatable)beginBools).Equals(endBools, EqualityComparer<bool>.Default);
    }

    bool[] ToBoolArray(Sudoku sudoku)
    {
        return new bool[729].Select((ElementInit, index) => sudoku.Rows[index / 81][(index - (index / 81) * 81) / 9]
        .Options[index % 9]).ToArray();
    }

    private HashSet<CellVertice>[/*bit*/] CellsToRemoveBit(SudokuGraph sudokuGraph)
    {
        HashSet<CellVertice>[] result = new HashSet<CellVertice>[9];

        for (int bit = 0; bit < 9; bit++)
        {
            result[bit] = new HashSet<CellVertice>();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    CellVertice cell = sudokuGraph[i,j];

                    if (cell.cell.Value != 0) continue;

                    int[] colorOfNeighbors = new int[cell.neighborsOnCommonBits[bit].Count]
                        .Select((elem, index) => cell.neighborsOnCommonBits[bit][index].color[bit])
                        .ToArray();

                    Array.Sort(colorOfNeighbors);

                    //Rule 2
                    for (int k = 0; k < colorOfNeighbors.Length; k++)
                    {
                        if (colorOfNeighbors[k] != cell.color[bit]) continue;
                        result[bit].Add(cell);
                        break;
                    }

                    //Rule 4
                    for (int k = 0; k < colorOfNeighbors.Length - 1; k++)
                    {
                        if (colorOfNeighbors[k + 1] != colorOfNeighbors[k] + 1) continue;
                        result[bit].Add(cell);
                        break;
                    }

                }
            }
        }

        return result;
    }
    private void RemoveBits(SudokuGraph sudoku, HashSet<CellVertice>[] verticeRemoveBit)
    {
        for (int bit = 0; bit < 9; bit++)
        {
            foreach(var vertice in verticeRemoveBit[bit])
            {
                vertice.cell.Options[bit] = false;
            }
        }
    }
}

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

internal class SudokuGraph
{
    Sudoku sudoku;

    public CellVertice[][] newRows;
    public CellVertice[][] newColumns;
    public CellVertice[][] newSquares;

    public new CellVertice this[int i,int j] => newRows[i][j];

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

                    int[] numbersOfCells = bitMask.GetArrayOfOnes();

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
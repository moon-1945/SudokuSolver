
namespace SudokuSolver.SolveMethods.ToughStrategies.SimpleColoring;

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

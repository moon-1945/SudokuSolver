//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Numerics;
//using System.Reflection.Metadata.Ecma335;

//namespace SudokuSolver.SolveMethods.ToughStrategies;

//public class SimpleColoringAlt : ISolveMethod
//{
//    public bool TrySolve(SudokuBase sudoku)
//    {
//        bool[] beginBools = ToBoolArray(sudoku);

//        SudokuGraphAlt sudokuGraph = new SudokuGraphAlt(sudoku);

//        sudokuGraph.ColorGraph();

//        HashSet<SudokuVertice>[] cellRemoveBit = CellsToRemoveBit(sudokuGraph);

//        RemoveBits(sudokuGraph, cellRemoveBit);

//        bool[] endBools = ToBoolArray(sudoku);

//        return !((IStructuralEquatable)beginBools).Equals(endBools, EqualityComparer<bool>.Default);
//    }

//    bool[] ToBoolArray(SudokuBase sudoku)
//    {
//        return new bool[729].Select((ElementInit, index) => sudoku.Rows[index / 81][(index - (index / 81) * 81) / 9]
//        .Options[index % 9]).ToArray();
//    }

//    private HashSet<SudokuVertice>[/*bit*/] CellsToRemoveBit(SudokuGraphAlt sudokuGraph)
//    {
//        HashSet<SudokuVertice>[] result = new HashSet<SudokuVertice>[9];

//        for (int bit = 0; bit < 9; bit++)
//        {
//            result[bit] = new HashSet<SudokuVertice>();

//            for (int i = 0; i < 9; i++)
//            {
//                for (int j = 0; j < 9; j++)
//                {
//                    SudokuVertice cell = sudokuGraph[i,j];

//                    if (cell.cell.Value != 0) continue;

//                    int[] colorOfNeighbors = new int[cell.neighborsOnCommonBits[bit].Count]
//                        .Select((elem, index) => cell.neighborsOnCommonBits[bit][index].color[bit])
//                        .ToArray();

//                    Array.Sort(colorOfNeighbors);

//                    //Rule 2
//                    for (int k = 0; k < colorOfNeighbors.Length; k++)
//                    {
//                        if (colorOfNeighbors[k] != cell.color[bit]) continue;
//                        result[bit].Add(cell);
//                        break;
//                    }

//                    //Rule 4
//                    for (int k = 0; k < colorOfNeighbors.Length - 1; k++)
//                    {
//                        if (colorOfNeighbors[k + 1] != colorOfNeighbors[k] + 1) continue;
//                        result[bit].Add(cell);
//                        break;
//                    }

//                }
//            }
//        }

//        return result;
//    }
//    private void RemoveBits(SudokuGraphAlt sudoku, HashSet<SudokuVertice>[] verticeRemoveBit)
//    {
//        for (int bit = 0; bit < 9; bit++)
//        {
//            foreach(var vertice in verticeRemoveBit[bit])
//            {
//                vertice.cell.Options[bit] = false;
//            }
//        }
//    }
//}

//internal class SudokuVertice
//{
//    public Cell cell;
//    public SudokuVertice(Cell cell)
//    {
//        this.cell = cell;
//        neighborsOnGraph = new HashSet<SudokuVertice>[9];
//        for (int k = 0; k < 9; k++)
//        {
//            neighborsOnGraph[k] = new HashSet<SudokuVertice>(9);
//        }

//        neighborsOnCommonBits = new List<SudokuVertice>[9];
//        for (int bit = 0; bit < 9; bit++)
//        {
//            neighborsOnCommonBits[bit] = new List<SudokuVertice>(9);
//        }
//    }

//    public List<SudokuVertice>[/*bit*/] neighborsOnCommonBits;

//    public HashSet<SudokuVertice>[/*bit*/] neighborsOnGraph;

//    public int[] color = new int[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 };
//    public bool IsColored(int bit) => color[bit] != -1;
//}

//internal class SudokuGraphAlt
//{
//    public Dictionary<int, SudokuVertice[]> BitGraphs { get; set; }

//    public SudokuGraphAlt(SudokuBase sudoku)
//    {
//        for(int bit = 0; bit < 9; bit++)
//        {
//            sudoku.
//        }






//        this.sudoku = sudoku;

//        newRows = new SudokuVertice[9][];
//        newColumns = new SudokuVertice[9][];
//        newSquares = new SudokuVertice[9][];

//        for (int i = 0; i < 9; i++)
//        {
//            newRows[i] = new SudokuVertice[9];
//            newColumns[i] = new SudokuVertice[9];
//            newSquares[i] = new SudokuVertice[9];
//        }

//        for (int i = 0; i < 9; i++)
//        {
//            for (int j = 0; j < 9; j++)
//            {
//                SudokuVertice cell = new SudokuVertice(sudoku.Rows[i][j]);

//                newRows[i][j] = cell;
//                newColumns[j][i] = cell;
//                newSquares[3 * (i / 3) + (j / 3)][3 * (i % 3) + j % 3] = cell;
//            }
//        }

//        SudokuVertice[][][] cellModes = { newRows, newColumns, newSquares };

//        for (int bit = 0; bit < 9; bit++)
//        {
//            for (int mode = 0; mode < 3; mode++)
//            {
//                for (int i = 0; i < 9; i++)
//                {
//                    bool[] boolsMask = new bool[9].Select((elem, index) => cellModes[mode][i][index].cell.Options[bit]).ToArray();
//                    BitArray bitMask = new BitArray(boolsMask);

//                    int[] numbersOfCells = bitMask.GetArrayOfOnes();

//                    for (int i1 = 0; i1 < numbersOfCells.Length; i1++)
//                    {
//                        for (int i2 = i1 + 1; i2 < numbersOfCells.Length; i2++)
//                        {
//                            SudokuVertice cell1 = cellModes[mode][i][numbersOfCells[i1]];
//                            SudokuVertice cell2 = cellModes[mode][i][numbersOfCells[i2]];

//                            cell1.neighborsOnCommonBits[bit].Add(cell2);
//                            cell2.neighborsOnCommonBits[bit].Add(cell1);

//                            if (numbersOfCells.Length != 2) continue;

//                            cell1.neighborsOnGraph[bit].Add(cell2);
//                            cell2.neighborsOnGraph[bit].Add(cell1);
//                        }
//                    }
//                }
//            }
//        }
//    }

//    static HashSet<SudokuVertice> usedVertices = new HashSet<SudokuVertice>();
//    public void ColorTree(SudokuVertice topVertex, int bit, int color, int o = 1)
//    {
//        if (o != 1 && o != 2) throw new Exception("ColorTree");
//        usedVertices.Add(topVertex);
//        topVertex.color[bit] = 3 * color + o;
//        foreach (var neighborCell in topVertex.neighborsOnGraph[bit])
//        {
//            if (!usedVertices.Contains(neighborCell)) ColorTree(neighborCell, bit, color, 3 - o);
//        }
//    }
//    public void ColorGraph()
//    {
//        for (int bit = 0; bit < 9; bit++)
//        {
//            int color = 1;
//            for (int i = 0; i < 9; i++)
//            {
//                for (int j = 0; j < 9; j++)
//                {
//                    if (this[i,j].IsColored(bit)) continue;
//                    usedVertices.Clear();
//                    ColorTree(this[i, j], bit,color);
//                    color++;
//                }
//            }
//        }
//    }
//}
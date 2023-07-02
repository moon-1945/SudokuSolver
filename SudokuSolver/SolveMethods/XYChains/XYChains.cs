using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Factorization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;


namespace SudokuSolver.SolveMethods.XYChains;

internal class XYChains : ISolveMethod
{
    public bool TrySolve(Sudoku sudoku)
    {
        bool[] beginBools = ToBoolArray(sudoku);

        SudokuGraph sudokuGraph = new SudokuGraph(sudoku);

        List<CellVertice> vertices = new List<CellVertice>();

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (sudokuGraph[i, j].neighbors.Count != 0) vertices.Add(sudokuGraph[i, j]);
            }
        }

        if (vertices.Count == 0) return false;

        PathEnds[][] pathEnds = new PathEnds[vertices.Count][];
        for (int i = 0; i < pathEnds.Length; i++)
        {
            pathEnds[i] = new PathEnds[vertices.Count].Select((elem, j) => new PathEnds(vertices[i], vertices[j])).ToArray();
        }


        //PathEnds pathEnds1 = new PathEnds(sudokuGraph[1, 2], sudokuGraph[4,2]);
        //PathEnds pathEnds2 = new PathEnds(sudokuGraph[4, 2], sudokuGraph[4, 0]);

        //var path = pathEnds1 * pathEnds2;

        //foreach (var c in path.EndsOfPath)
        //{
        //    Console.WriteLine($"{c.first + 1} {c.second + 1}");
        //}

        // Console.WriteLine(sudoku);

        var reachabilityMatrix = new Matrix<PathEnds>(pathEnds).CalculateReachabilityMatrix();

        //Console.WriteLine();
        //for (int i = 0; i < reachabilityMatrix.Rows; i++)
        //{
        //    for (int j = 0; j < reachabilityMatrix.Columns; j++)
        //    {
        //        Console.WriteLine($"({reachabilityMatrix[i, j].Begin.cell.I + 1} {reachabilityMatrix[i, j].Begin.cell.J + 1}) " +
        //            $"({reachabilityMatrix[i, j].End.cell.I + 1} {reachabilityMatrix[i, j].End.cell.J + 1})");

        //        foreach (var c in reachabilityMatrix[i, j].EndsOfPath)
        //        {
        //            Console.WriteLine($"{c.first + 1} {c.second + 1}");
        //        }
        //    }
        //}
        //Console.WriteLine();

        for (int i = 0; i < vertices.Count; i++)
        {
            for (int j = i + 1; j < vertices.Count; j++)
            {
                foreach (var c in reachabilityMatrix[i, j].EndsOfPath)
                {
                    if (c.first != c.second) continue;

                    //    Console.WriteLine($"({reachabilityMatrix[i, j].Begin.cell.I + 1} {reachabilityMatrix[i, j].Begin.cell.J + 1}) " +
                    //$"({reachabilityMatrix[i, j].End.cell.I + 1} {reachabilityMatrix[i, j].End.cell.J + 1}) {c.first + 1}");

                    HashSet<CellVertice> cellVertices = sudokuGraph.Intersection(reachabilityMatrix[i, j].Begin, reachabilityMatrix[i, j].End, c.first);

                    foreach (var cellVertice in cellVertices)
                    {
                        cellVertice.cell.Options[c.first] = false;
                    }


                }
            }
        }


        bool[] endBools = ToBoolArray(sudoku);

        return !((IStructuralEquatable)beginBools).Equals(endBools, EqualityComparer<bool>.Default);
    }

    bool[] ToBoolArray(Sudoku sudoku)
    {
        return new bool[729].Select((ElementInit, index) => sudoku.Rows[index / 81][(index - index / 81 * 81) / 9]
        .Options[index % 9]).ToArray();
    }

}



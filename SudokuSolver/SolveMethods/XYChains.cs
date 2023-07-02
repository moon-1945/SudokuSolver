using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Factorization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.SolveMethods;

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

        var reachibilityMatrix = new Graph<PathEnds>(pathEnds).GetMatrixReachability();

         // var reachibilityMatrix = reachibilityMatrix1 * reachibilityMatrix1;

        Console.WriteLine();
        for (int i = 0; i < reachibilityMatrix.Rows; i++)
        {
            for (int j = 0; j < reachibilityMatrix.Columns; j++)
            {
                Console.WriteLine($"({reachibilityMatrix[i, j].Begin.cell.I + 1} {reachibilityMatrix[i, j].Begin.cell.J + 1}) " +
                    $"({reachibilityMatrix[i, j].End.cell.I + 1} {reachibilityMatrix[i, j].End.cell.J + 1})");

                foreach (var c in reachibilityMatrix[i, j].EndsOfPath)
                {
                    Console.WriteLine($"{c.first + 1} {c.second + 1}");
                }
            }
        }
        Console.WriteLine();

        //for (int i = 0; i < vertices.Count; i++)
        //{
        //    for (int j = i + 1; j < vertices.Count; j++)
        //    {
        //        foreach (var c in reachibilityMatrix[i, j].EndsOfPath)
        //        {
        //            if (c.first != c.second) continue;

        //            Console.WriteLine($"({reachibilityMatrix[i, j].Begin.cell.I + 1} {reachibilityMatrix[i, j].Begin.cell.J + 1}) " +
        //        $"({reachibilityMatrix[i, j].End.cell.I + 1} {reachibilityMatrix[i, j].End.cell.J + 1}) {c.first + 1}");

        //            HashSet<CellVertice> cellVertices = sudokuGraph.Intersection(reachibilityMatrix[i,j].Begin, reachibilityMatrix[i, j].End, c.first);

        //            foreach (var cellVertice in cellVertices)
        //            {
        //                cellVertice.cell.Options[c.first] = false;
        //            }


        //        }
        //    }
        //}


        bool[] endBools = ToBoolArray(sudoku);

        return false;
    }

    bool[] ToBoolArray(Sudoku sudoku)
    {
        return new bool[729].Select((ElementInit, index) => sudoku.Rows[index / 81][(index - index / 81 * 81) / 9]
        .Options[index % 9]).ToArray();
    }

}

class PathEnds : IAdditionOperators<PathEnds, PathEnds, PathEnds>, IMultiplyOperators<PathEnds, PathEnds, PathEnds>
{
    public HashSet<(int first, int second)> EndsOfPath { get; init; }
    public CellVertice Begin { get; init; }
    public CellVertice End { get; init; }

    private PathEnds() { }
    //  public PathEnds(HashSet<(int first, int second)> endsOfPath) => this.EndsOfPath = endsOfPath;

    public PathEnds(CellVertice cell1, CellVertice cell2)
    {
        Begin = cell1;
        End = cell2;
        if (!cell1.neighbors.Contains(cell2) || cell1 == cell2)
        {
            EndsOfPath = new HashSet<(int first, int second)>();
            return;
        }

        int[] cell1Options = cell1.cell.Options.ToIndicesArray();
        int[] cell2Options = cell2.cell.Options.ToIndicesArray();

        if (cell1Options.Length != 2
            || cell2Options.Length != 2) throw new ArgumentException("invalid arguments");

        int[] commonOptions = cell1Options.Intersect(cell2Options).ToArray();

        EndsOfPath =
            (commonOptions.Length == 1) ?

            new HashSet<(int first, int second)>
        { ( cell1Options[0] == commonOptions[0] ? cell1Options[1] : cell1Options[0],
        cell2Options[0] == commonOptions[0] ? cell2Options[1] : cell2Options[0]) } :

        (commonOptions.Length == 2) ?
        new HashSet<(int first, int second)> { (cell1Options[0], cell1Options[1]), (cell1Options[1], cell1Options[0]) } :

        new HashSet<(int first, int second)>();

    }



    public static PathEnds operator +(PathEnds left, PathEnds right)
    {
        if (left.Begin != right.Begin || left.End != right.End) throw new ArgumentException();

        return new PathEnds() { EndsOfPath = left.EndsOfPath.Union(right.EndsOfPath).ToHashSet(), Begin = left.Begin, End = left.End };
    }

    public static PathEnds operator *(PathEnds left, PathEnds right)
    {
        if (left.End != right.Begin) throw new ArgumentException("invalid arguments");

        HashSet<(int first, int second)> m = new();

        foreach (var c1 in left.EndsOfPath)
        {
            foreach (var c2 in right.EndsOfPath)
            {
                if (c1.second == c2.first) continue;
                m.Add((c1.first, c2.second));
            }
        }

        return new PathEnds() { EndsOfPath = m, Begin = left.Begin, End = right.End };
    }
}




class Matrix<T> :
    IAdditionOperators<Matrix<T>, Matrix<T>, Matrix<T>>,
    IMultiplyOperators<Matrix<T>, Matrix<T>, Matrix<T>> where T : IAdditionOperators<T, T, T>, IMultiplyOperators<T, T, T>
{
    T[][] values;
    public int Rows { get; init; }
    public int Columns { get; init; }
    public T this[int i, int j] => values[i][j];
    public Matrix(T[][] values)
    {
        this.values = values;
        Rows = values.Length;
        Columns = values[0].Length;
        for (int i = 1; i < values.Length; i++)
        {
            if (values[i].Length != values[i - 1].Length) throw new ArgumentException("invalid argument");
        }
    }

    public static Matrix<T> operator +(Matrix<T> left, Matrix<T> right)
    {
        if (left.Rows != right.Rows || left.Columns != right.Columns) throw new Exception("addition impossible");

        T[][] newValues = new T[left.Rows][];
        for (int i = 0; i < newValues.Length; i++)
        {
            newValues[i] = new T[left.Columns].Select((elem, j) => left[i, j] + right[i, j]).ToArray();
        }

        return new Matrix<T>(newValues);
    }

    public static Matrix<T> operator *(Matrix<T> left, Matrix<T> right)
    {
        if (left.Columns != right.Rows) throw new Exception("multiplication impossible");

        T[][] newValues = new T[left.Rows][];
        for (int i = 0; i < newValues.Length; i++)
        {
            newValues[i] = new T[left.Columns];
        }

        for (int i = 0; i < newValues.Length; i++)
        {
            for (int j = 0; j < newValues[0].Length; j++)
            {
                T sum = left[i, 0] * right[0, j];
                for (int k = 1; k < left.Columns; k++)
                {
                    sum = sum + left[i, k] * right[k, j];
                }
                newValues[i][j] = sum;
            }
        }

        return new Matrix<T>(newValues);
    }
}

class Graph<T> where T : IAdditionOperators<T, T, T>, IMultiplyOperators<T, T, T>
{

    public Matrix<T> adjMatrix { get; init; }
    public int numberOfVertices { get; init; }
    public Graph(T[][] adjMatrix)
    {
        var matr = new Matrix<T>(adjMatrix);
        if (matr.Rows != matr.Columns) throw new Exception();
        this.adjMatrix = matr;
        numberOfVertices = matr.Rows;
    }

    public Matrix<T> GetMatrixReachability()
    {
        Matrix<T> reachability = adjMatrix;
        for (int i = 0; i < numberOfVertices; i++)
        {
            reachability = reachability * adjMatrix + adjMatrix;
        }

        return reachability;
    }

}



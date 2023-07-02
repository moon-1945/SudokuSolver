using System.Numerics;


namespace SudokuSolver.SolveMethods.XYChains;

class PathEnds : IAdditionOperators<PathEnds, PathEnds, PathEnds>, IMultiplyOperators<PathEnds, PathEnds, PathEnds>
{
    public HashSet<(int first, int second)> EndsOfPath { get; init; }
    public CellVertice Begin { get; init; }
    public CellVertice End { get; init; }

    private PathEnds() { }

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
            commonOptions.Length == 1 ?

            new HashSet<(int first, int second)>
        { ( cell1Options[0] == commonOptions[0] ? cell1Options[1] : cell1Options[0],
        cell2Options[0] == commonOptions[0] ? cell2Options[1] : cell2Options[0]) } :

        commonOptions.Length == 2 ?
        new HashSet<(int first, int second)> { (cell1Options[0], cell1Options[0]), (cell1Options[1], cell1Options[1]) } :

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



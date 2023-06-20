namespace SudokuSolver.SolveMethods.XCycles;

public static class HashSetExtentions
{
    public static bool IsEquivalent<T>(this HashSet<T> set1, HashSet<T> set2)
    {
        return set1.Count == set2.Count && set1.Count == set1.Intersect(set2).Count();
    }
}
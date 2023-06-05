namespace SudokuSolver.SolveMethods.NakedGroups;

public class NakedPairs : NakedGroups, ISolveMethod
{
    public bool TrySolve(Sudoku sudoku) => TrySolve(sudoku, 2);
}

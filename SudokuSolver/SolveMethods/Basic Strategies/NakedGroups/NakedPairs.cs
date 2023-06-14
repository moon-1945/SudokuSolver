namespace SudokuSolver.SolveMethods.BasicStrategies.NakedGroups;

public class NakedPairs : NakedGroups, ISolveMethod
{
    public bool TrySolve(Sudoku sudoku) => TrySolve(sudoku, 2);
}

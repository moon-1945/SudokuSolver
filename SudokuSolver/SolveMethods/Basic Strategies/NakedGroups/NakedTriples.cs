namespace SudokuSolver.SolveMethods.BasicStrategies.NakedGroups;

public class NakedTriples : NakedGroups, ISolveMethod
{
    public bool TrySolve(Sudoku sudoku) => TrySolve(sudoku, 3);
}

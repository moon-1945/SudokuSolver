namespace SudokuSolver.SolveMethods.HiddenGroups;

public class HiddenTriples : HiddenGroups, ISolveMethod
{
    public bool TrySolve(Sudoku sudoku) => TrySolve(sudoku, 3);
}
namespace SudokuSolver.SolveMethods.BasicStrategies.HiddenGroups;

public class HiddenPairs : HiddenGroups, ISolveMethod
{
    public bool TrySolve(Sudoku sudoku) => TrySolve(sudoku, 2);
}
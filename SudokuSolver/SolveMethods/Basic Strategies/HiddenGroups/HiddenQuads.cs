namespace SudokuSolver.SolveMethods.BasicStrategies.HiddenGroups;

public class HiddenQuads : HiddenGroups, ISolveMethod
{
    public bool TrySolve(Sudoku sudoku) => TrySolve(sudoku, 4);
}

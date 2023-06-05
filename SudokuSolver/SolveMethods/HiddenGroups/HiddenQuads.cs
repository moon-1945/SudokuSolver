namespace SudokuSolver.SolveMethods.HiddenGroups;

public class HiddenQuads : HiddenGroups, ISolveMethod
{
    public bool TrySolve(Sudoku sudoku) => TrySolve(sudoku, 4);
}

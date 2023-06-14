namespace SudokuSolver.SolveMethods.BasicStrategies.NakedGroups;

public class NakedQuads : NakedGroups, ISolveMethod
{
    public bool TrySolve(Sudoku sudoku) => TrySolve(sudoku, 4);
}

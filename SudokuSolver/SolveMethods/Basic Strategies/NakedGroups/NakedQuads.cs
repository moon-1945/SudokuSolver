using SudokuSolver.Sudoku;

namespace SudokuSolver.SolveMethods.BasicStrategies.NakedGroups;

public class NakedQuads : NakedGroups, ISolveMethod
{
    public bool TrySolve(SudokuBase sudoku) => TrySolve(sudoku, 4);
}

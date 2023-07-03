using SudokuSolver.Sudoku;

namespace SudokuSolver.SolveMethods.BasicStrategies.NakedGroups;

public class NakedTriples : NakedGroups, ISolveMethod
{
    public bool TrySolve(SudokuBase sudoku) => TrySolve(sudoku, 3);
}

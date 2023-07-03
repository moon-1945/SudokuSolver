using SudokuSolver.Sudoku;

namespace SudokuSolver.SolveMethods.BasicStrategies.HiddenGroups;

public class HiddenTriples : HiddenGroups, ISolveMethod
{
    public bool TrySolve(SudokuBase sudoku) => TrySolve(sudoku, 3);
}
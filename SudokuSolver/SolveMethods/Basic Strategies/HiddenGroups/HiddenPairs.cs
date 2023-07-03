using SudokuSolver.Sudoku;

namespace SudokuSolver.SolveMethods.BasicStrategies.HiddenGroups;

public class HiddenPairs : HiddenGroups, ISolveMethod
{
    public bool TrySolve(SudokuBase sudoku) => TrySolve(sudoku, 2);
}
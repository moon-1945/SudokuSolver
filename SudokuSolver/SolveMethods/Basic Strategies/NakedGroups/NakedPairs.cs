using SudokuSolver.Sudoku;

namespace SudokuSolver.SolveMethods.BasicStrategies.NakedGroups;

public class NakedPairs : NakedGroups, ISolveMethod
{
    public bool TrySolve(SudokuBase sudoku) => TrySolve(sudoku, 2);
}

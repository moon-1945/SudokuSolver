using SudokuSolver.Sudoku;

namespace SudokuSolver.SolveMethods.BasicStrategies.HiddenGroups;

public class HiddenQuads : HiddenGroups, ISolveMethod
{
    public bool TrySolve(SudokuBase sudoku) => TrySolve(sudoku, 4);
}

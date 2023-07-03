using SudokuSolver.Sudoku;

namespace SudokuSolver.SolveMethods;

public interface ISolveMethod
{
    bool TrySolve(SudokuBase sudoku);
}
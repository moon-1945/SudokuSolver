namespace SudokuSolver.SolveMethods;

public interface ISolveMethod
{
    bool TrySolve(Sudoku sudoku);
}
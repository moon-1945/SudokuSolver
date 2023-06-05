using SudokuSolver.SolveMethods.NakedGroups;
using SudokuSolver.SolveMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver;

public class HiddenPairs : HiddenGroups, ISolveMethod
{
    public bool TrySolve(Sudoku sudoku) => TrySolve(sudoku, 2);
}
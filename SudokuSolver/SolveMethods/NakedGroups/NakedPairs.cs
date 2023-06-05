using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.SolveMethods.NakedGroups;

public class NakedPairs : NakedGroups, ISolveMethod
{
    public bool TrySolve(Sudoku sudoku) => TrySolve(sudoku, 2);
}

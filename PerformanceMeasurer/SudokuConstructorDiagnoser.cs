using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SudokuSolver;
using SudokuSolver.Sudoku;
using System.Collections;

namespace PerformanceMeasurer;

[MemoryDiagnoser]
public class SudokuConstructorDiagnoser : SudokuDiagnoser
{
    [Benchmark]
    public void Original()
    {
        RunForeachSudoku((code) =>
        {
            var _ = new OldSudoku(code);
        });
    }

    [Benchmark]
    public void Optimized()
    {
        RunForeachSudoku((code) =>
        {
            var _ = new NewSudoku(code);
        });
    }
}

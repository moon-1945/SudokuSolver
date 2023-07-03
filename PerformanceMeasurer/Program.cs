using BenchmarkDotNet.Running;

namespace PerformanceMeasurer;

public class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<SudokuConstructorDiagnoser>();
    }
}

namespace PerformanceMeasurer;

public class SudokuDiagnoser
{
    public void RunForeachSudoku(Action<string> action)
    {
        foreach (string sudokuCode in File.ReadLines("../../../../../../../data.txt"))
        {
            action?.Invoke(sudokuCode);
        }
    }
}
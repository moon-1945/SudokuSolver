namespace SudokuSolver;

public static class BitArrayExtension
{
    public static int[] ToIndicesArray(this BitArray bitArray)
    {
        List<int> result = new List<int>(9);
        for (int i = 0; i < 9; i++)
        {
            if (bitArray[i]) result.Add(i);
        }
        return result.ToArray();
    }
}

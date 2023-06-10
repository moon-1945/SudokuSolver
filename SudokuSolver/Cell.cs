using SudokuSolver.SolveMethods;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace SudokuSolver;


public static class BitArrayExtension
{
    public static int[] GetArrayOfOnes(this BitArray bitArray)
    {
        List<int> result = new List<int>(9);
        for (int i = 0; i < 9; i++)
        {
            if (bitArray[i]) result.Add(i);
        }
        return result.ToArray();
    }
}

public class Cell
{
    public int I { get; }
    public int J { get; }
    public int S { get; }

    public int Value;
    public BitArray Options;

    //public bool isSolvedAndChecked = false;

    private static readonly string[][] Digits = {
    new[]{ "   . ", "  /| ", "   | " },
    new[]{ " __  ", "  _) ", " /__ " },
    new[]{ " __  ", " __) ", " __) " },
    new[]{ " . . ", " |_| ", "   | " },
    new[]{ "  __ ", " |_  ", " __) " },
    new[]{ "  _  ", " |_  ", " |_) " },
    new[]{ "  __ ", "   / ", "  /  " },
    new[]{ "  _  ", " (_) ", " (_) " },
    new[]{ "  _  ", " (_| ", "  _| " }
};

    public Cell(int i, int j, int value, BitArray options)
    {
        I = i;
        J = j;
        S = 3 * (i / 3) + j / 3;
        Value = value;
        Options = options;
    }

    public string[] ToOutput()
        => Value != 0
        ? Digits[Value - 1]
        : Options
            .Cast<bool>()
            .Chunk(3)
            .Select((x, i) => string.Join(" ", x.Select((b, j) => b ? (i * 3 + j + 1).ToString() : " ")))
            .ToArray();

    public int GetSingleOrZero()
    {
        int n = 0, c = 0;

        if (Options[0])
        {
            c++;
            n = 1;
        }
        if (Options[1])
        {
            c++;
            n = 2;
        }
        if (Options[2])
        {
            c++;
            n = 3;
        }
        if (Options[3])
        {
            c++;
            n = 4;
        }
        if (Options[4])
        {
            c++;
            n = 5;
        }
        if (Options[5])
        {
            c++;
            n = 6;
        }
        if (Options[6])
        {
            c++;
            n = 7;
        }
        if (Options[7])
        {
            c++;
            n = 8;
        }
        if (Options[8])
        {
            c++;
            n = 9;
        }

        return c == 1 ? n : 0;
    }

}
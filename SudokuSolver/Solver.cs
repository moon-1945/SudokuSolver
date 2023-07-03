using SudokuSolver.SolveMethods;
using SudokuSolver.SolveMethods.BasicStrategies;
using SudokuSolver.SolveMethods.BasicStrategies.HiddenGroups;
using SudokuSolver.SolveMethods.BasicStrategies.NakedGroups;
using SudokuSolver.SolveMethods.ToughStrategies;
using SudokuSolver.SolveMethods.ToughStrategies.SimpleColoring;
using SudokuSolver.SolveMethods.XCycles;
using SudokuSolver.SolveMethods.XYChains;
using SudokuSolver.Sudoku;
using System.Collections.Concurrent;
using System.IO.Compression;

namespace SudokuSolver;

public class Solver
{
    Sudoku _sudoku;
    private ISolveMethod[] _methods = {
        new CheckForSolvedCells(),
        new ShowPossibles(),
        new HidenSingle(),
        new NakedPairs(),
        new NakedTriples(),
        new NakedQuads(),
        new HiddenPairs(),
        new HiddenTriples(),
        new HiddenQuads(),
        new PointingPairs(),
        new XWing(),
        new SimpleColoring(),
        new YWing(),
        new SwordFish(),
        new XYZWing(),
        new BUG(),
        new XCycles(),
        new XYChains(),
        //new NakedTriples(),
        //new NakedQuads(),
    };

    public Solver(Sudoku sudoku) => _sudoku = sudoku;

    public Sudoku Solve()
    {

        //new ShowPossibles().TrySolve(_sudoku);

        while (true)
        {
            bool a = false;
            foreach (var method in _methods)
            {
                if (method.TrySolve(_sudoku))
                {
                    //Console.WriteLine(method.GetType());
                    // Console.WriteLine(_sudoku);
                    //if (method is NakedPairs) Console.WriteLine(method.GetType());
                    //if (method is not ShowPossibles && method is not CheckForSolvedCells)
                    //    Console.WriteLine(method.GetType());
                    //Console.WriteLine(_sudoku);
                    a = true;
                    break;
                }
            }

            if (a) continue;
            else break;
        }

        return _sudoku;

    }

    public static void RunCheck()
    {
        int numberOfSudoku = 0;

        int solutions = 0, failed = 0;

        using var outputFile = File.Create("../../../failures.txt");
        using var outputWriter = new StreamWriter(outputFile);
        using var outputFile2 = File.Create("../../../time.txt");
        using var outputWriter2 = new StreamWriter(outputFile2);
        using var outputFile3 = File.Create("../../../timeSorted.txt");
        using var outputWriter3 = new StreamWriter(outputFile3);

        var watch = new System.Diagnostics.Stopwatch();

        long maxTimeSudoku = 0;

        List<long> time = new List<long>();

        int count = 0;
        foreach ((string unsolved, string solved) in File.ReadLines("../../../data2.txt").Zip(File.ReadLines("../../../solutions.txt")))
        {
            count++;
            for (int i = 0; i < 81; i++)
            {
                if (unsolved[i] == '0') continue;
                if (unsolved[i] != solved[i]) throw new Exception($"wrong solution ");
            }

            var sudoku = new OldSudoku(unsolved);
            var sudokuSolver = new Solver(sudoku);

            var beg = watch.ElapsedMilliseconds;

            numberOfSudoku++;

            //Console.WriteLine(sudoku);
            watch.Start();

            Sudoku outSudoku = sudokuSolver.Solve();

            watch.Stop();
            //Console.WriteLine(isSolve);

            var end = watch.ElapsedMilliseconds;


            maxTimeSudoku = ((end - beg) > maxTimeSudoku) ? (end - beg) : maxTimeSudoku;
            outputWriter2.WriteLine((end - beg));

            time.Add(end - beg);

            string outSudokuCode = outSudoku.ConvertToStr();

            for (int i = 0; i < 81; i++)
            {
                if (outSudokuCode[i] == '0') continue;
                if (outSudokuCode[i] != solved[i]) { throw new Exception("dffdf"); }
            }

            if (!outSudoku.IsSolved())
            {
                failed++;
                outputWriter.WriteLine(unsolved);
            }
            else
            {
                solutions++;
            }
        }

        time = time.OrderBy((x) => -x).ToList();

        for (int i = 0; i < time.Count; i++)
        {
            outputWriter3.WriteLine(time[i]);
        }

        Console.WriteLine($"{solutions} {failed}");
        Console.WriteLine(watch.ElapsedMilliseconds * 1.0 / (solutions + failed));
        Console.WriteLine(maxTimeSudoku);
        Console.WriteLine(count);
    }

    public static void RunExample()
    {
        int solutions = 0, failed = 0;

        var watch = new System.Diagnostics.Stopwatch();

        var str = "200010000000000403000000500005307000000000260000000000820000070000500010600400000";

        var sudoku = new OldSudoku(str);
        var sudokuSolver = new Solver(sudoku);
        //Console.WriteLine(sudoku);
        watch.Start();
        Sudoku outSudoku = sudokuSolver.Solve();
        watch.Stop();
        //Console.WriteLine(isSolve);

        if (!outSudoku.IsSolved())
        {
            failed++;

        }
        else
        {
            solutions++;
        }
        //Console.WriteLine(sudoku);
        //break;


        Console.WriteLine(sudoku);

        Console.WriteLine($"{solutions} {failed}");
        Console.WriteLine(watch.ElapsedMilliseconds * 1.0 / (solutions + failed));
    }

    public static void CheckSolution()
    {
        var str = "100503000020000740000900000080060070000100003000000000000020800300000009500000000";

        var sudoku = new OldSudoku(str);

        var watch = new System.Diagnostics.Stopwatch();

        watch.Start();
        var solve = new Recursion().Solve(sudoku);
        Console.WriteLine(solve);
        watch.Stop();

        Console.WriteLine(watch.ElapsedMilliseconds);
        Console.WriteLine(solve.ConvertToStr());
    }


    public static void CreateSolutions()
    {
        using var outputFile = File.Create("../../../solutions.txt");
        using var outputWriter = new StreamWriter(outputFile);
        int count = 0;

        foreach (var str in File.ReadLines("../../../data2.txt"))
        {   
            count++;
            var sudoku = new OldSudoku(str);

            outputWriter.WriteLine(new Recursion().Solve(sudoku).ConvertToStr());
        }
    }

    public static void CheckSolutions()
    {

        int count = 0;
        foreach ((string unsolved, string solved) in File.ReadLines("../../../data2.txt").Zip(File.ReadLines("../../../solutions.txt")))
        {
            count++;
            if (unsolved.Length != 81 || solved.Length != 81) throw new Exception("wrong string");

            for (int i = 0; i < 81; i++)
            {
                if (unsolved[i] == '0') continue;
                if (unsolved[i] != solved[i]) throw new Exception($"wrong solution {count}");
            }

            var sudokuSolved = new OldSudoku(solved);

            if (!sudokuSolved.IsSolved()) throw new Exception($"wrong solution {count}");
        }
    }

}


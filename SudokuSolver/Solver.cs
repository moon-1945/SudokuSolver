using SudokuSolver.SolveMethods;
using SudokuSolver.SolveMethods.BasicStrategies;
using SudokuSolver.SolveMethods.BasicStrategies.HiddenGroups;
using SudokuSolver.SolveMethods.BasicStrategies.NakedGroups;
using SudokuSolver.SolveMethods.ToughStrategies;

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
        //new NakedTriples(),
        //new NakedQuads(),
    };

    public Solver(Sudoku sudoku) => _sudoku = sudoku;

    public bool Solve()
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

        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                sum += _sudoku.Rows[i][j].Value;
                sum -= (i+1);
            }
        }

        if (sum > 0) throw new Exception("lox");

        return sum == 0;

    }

    public static void RunCheck()
    {
        int numberOfSudoku = 0;

        int solutions = 0, failed = 0;

        using var outputFile = File.Create("../../../failures.txt");
        var outputWriter = new StreamWriter(outputFile);
        using var outputFile2 = File.Create("../../../time.txt");
        var outputWriter2 = new StreamWriter(outputFile2);
        var watch = new System.Diagnostics.Stopwatch();

        long maxTimeSudoku = 0;

        foreach (var str in File.ReadLines("../../../data2.txt"))
        {
            
            
            try
            {
                int[,] arr = new int[9, 9];
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        arr[i, j] = str[9 * i + j] - '0';
                    }
                }

                var sudoku = new Sudoku(arr);
                var sudokuSolver = new Solver(sudoku);

                var beg = watch.ElapsedMilliseconds;

                numberOfSudoku++;

                //Console.WriteLine(sudoku);
                watch.Start();

                bool isSolve = sudokuSolver.Solve();

                watch.Stop();
                //Console.WriteLine(isSolve);

                var end = watch.ElapsedMilliseconds;

                maxTimeSudoku = ((end - beg) > maxTimeSudoku) ? (end - beg) : maxTimeSudoku;
                outputWriter2.WriteLine((end - beg));

                if (!isSolve)
                {
                    failed++;
                    outputWriter.WriteLine(str);
                }
                else
                {
                    solutions++;
                }
                //Console.WriteLine(sudoku);
                //break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{str}");
            }
    
        }

        Console.WriteLine($"{solutions} {failed}");
        Console.WriteLine(watch.ElapsedMilliseconds * 1.0 / (solutions + failed));
        Console.WriteLine(maxTimeSudoku);
    }

    public static void RunExample()
    {
        int solutions = 0, failed = 0;



        var watch = new System.Diagnostics.Stopwatch();

        var str = "100500006000400020030000000000020890705000000600000000000600100009030000020000000";
        int[,] arr = new int[9, 9];
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                arr[i, j] = str[9 * i + j] - '0';
            }
        }

        var sudoku = new Sudoku(arr);
        var sudokuSolver = new Solver(sudoku);
        //Console.WriteLine(sudoku);
        watch.Start();
        bool isSolve = sudokuSolver.Solve();
        watch.Stop();
        //Console.WriteLine(isSolve);

        if (!isSolve)
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



    public static void RunCheckGC()
    {
        int solutions = 0, failed = 0;

        using var outputFile = File.Create("../../../failures.txt");
        var outputWriter = new StreamWriter(outputFile);
        using var outputFile2 = File.Create("../../../time.txt");
        var outputWriter2 = new StreamWriter(outputFile2);
        var watch = new System.Diagnostics.Stopwatch();

        long maxTimeSudoku = 0;

        int[,] arr1 = new int[9, 9];
        var sudoku = new Sudoku(arr1);
        var sudokuSolver = new Solver(sudoku);


        foreach (var str in File.ReadLines("../../../data2.txt"))
        {
            int[,] arr = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    arr[i, j] = str[9 * i + j] - '0';
                }
            }

            sudoku.Update(arr);

            //Console.WriteLine(sudoku);

            var beg = watch.ElapsedMilliseconds;

            //Console.WriteLine(sudoku);
            watch.Start();
            bool isSolve = sudokuSolver.Solve();
            watch.Stop();
            //Console.WriteLine(isSolve);

            var end = watch.ElapsedMilliseconds;

            maxTimeSudoku = ((end - beg) > maxTimeSudoku) ? (end - beg) : maxTimeSudoku;
            outputWriter2.WriteLine((end - beg));

            if (!isSolve)
            {
                failed++;
                outputWriter.WriteLine(str);
            }
            else
            {
                solutions++;
            }
            //Console.WriteLine(sudoku);
            //break;


        }

        Console.WriteLine($"{solutions} {failed}");
        Console.WriteLine(watch.ElapsedMilliseconds * 1.0 / (solutions + failed));
        Console.WriteLine(maxTimeSudoku);
    }





}







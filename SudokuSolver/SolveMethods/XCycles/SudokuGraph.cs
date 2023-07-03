using SudokuSolver.SolveMethods.XCycles;
using SudokuSolver.Sudoku;

namespace SudokuSolver.SolveMethods.XCycles;

class SudokuGraph
{
    public SudokuBase sudoku;

    public CellVertice[][] newRows;
    public CellVertice[][] newColumns;
    public CellVertice[][] newSquares;

    public CellVertice this[int i, int j] => newRows[i][j];

    private HashSet<CellVertice>[] cellsWithStrongConnection = new HashSet<CellVertice>[9];

    public SudokuGraph(SudokuBase sudoku)
    {
        this.sudoku = sudoku;

        newRows = new CellVertice[9][];
        newColumns = new CellVertice[9][];
        newSquares = new CellVertice[9][];

        for (int i = 0; i < 9; i++)
        {
            newRows[i] = new CellVertice[9];
            newColumns[i] = new CellVertice[9];
            newSquares[i] = new CellVertice[9];
        }

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                CellVertice cell = new CellVertice(sudoku.Rows[i][j]);

                newRows[i][j] = cell;
                newColumns[j][i] = cell;
                newSquares[3 * (i / 3) + (j / 3)][3 * (i % 3) + j % 3] = cell;
            }
        }

        CellVertice[][][] cellModes = { newRows, newColumns, newSquares };

        for (int i = 0; i < 9; i++)
        {
            cellsWithStrongConnection[i] = new HashSet<CellVertice>();
        }

        for (int bit = 0; bit < 9; bit++)
        {
            for (int mode = 0; mode < 3; mode++)
            {
                for (int i = 0; i < 9; i++)
                {
                    bool[] boolsMask = new bool[9].Select((elem, index) => cellModes[mode][i][index].cell.Options[bit]).ToArray();
                    BitArray bitMask = new BitArray(boolsMask);

                    int[] numbersOfCells = bitMask.ToIndicesArray();

                    if (numbersOfCells.Length == 2)
                    {
                        CellVertice cell1 = cellModes[mode][i][numbersOfCells[0]];
                        CellVertice cell2 = cellModes[mode][i][numbersOfCells[1]];

                        ConnectCells(cell1, cell2, bit);
                        ConnectStrongCells(cell1, cell2, bit);

                        cellsWithStrongConnection[bit].Add(cell1);
                        cellsWithStrongConnection[bit].Add(cell2);
                    }
                }
            }

            foreach (var cell1 in cellsWithStrongConnection[bit])
            {
                foreach (var cell2 in cellsWithStrongConnection[bit])
                {
                    if (cell1.GetHashCode() >= cell2.GetHashCode() ||
                        cell1.neighbors[bit].Contains(cell2)) continue;

                    if (AreDependent(cell1, cell2)) ConnectCells(cell1, cell2, bit);

                    HashSet<CellVertice> intersect = Intersection(cell1, cell2, bit);

                    foreach (var com in intersect)
                    {
                        if (cellsWithStrongConnection[bit].Contains(com)) continue;

                        ConnectCells(cell1, com, bit);
                        ConnectCells(cell2, com, bit);
                    }
                }
            }
        }
    }

    bool AreDependent(CellVertice cell1, CellVertice cell2) =>
        cell1.cell.I == cell2.cell.I || cell1.cell.J == cell2.cell.J || cell1.cell.S == cell2.cell.S;

    public HashSet<CellVertice> Intersection(CellVertice cell1, CellVertice cell2, int bit)
    {
        HashSet<CellVertice> result = new HashSet<CellVertice>();

        for (int i = 0; i < 9; i++)
        {
            if (AreDependent(cell2, newRows[cell1.cell.I][i])
                && newRows[cell1.cell.I][i] != cell1
                && newRows[cell1.cell.I][i] != cell2
                && newRows[cell1.cell.I][i].cell.Options[bit] == true) result.Add(newRows[cell1.cell.I][i]);

            if (AreDependent(cell2, newColumns[cell1.cell.J][i])
                && newColumns[cell1.cell.J][i] != cell1
                && newColumns[cell1.cell.J][i] != cell2
                && newColumns[cell1.cell.J][i].cell.Options[bit] == true) result.Add(newColumns[cell1.cell.J][i]);

            if (AreDependent(cell2, newSquares[cell1.cell.S][i])
                && newSquares[cell1.cell.S][i] != cell1
                && newSquares[cell1.cell.S][i] != cell2
                && newSquares[cell1.cell.S][i].cell.Options[bit] == true) result.Add(newSquares[cell1.cell.S][i]);
        }

        return result;
    }

    void ConnectCells(CellVertice cell1, CellVertice cell2, int bit)
    {
        //Console.WriteLine($"Connect ({cell1.cell.I + 1},{cell1.cell.J + 1})," +
        //                $"({cell2.cell.I + 1},{cell2.cell.J + 1}),{bit + 1}"  );
        cell1.neighbors[bit].Add(cell2);
        cell2.neighbors[bit].Add(cell1);
    }

    void ConnectStrongCells(CellVertice cell1, CellVertice cell2, int bit)
    {
        //Console.WriteLine($"Connect ({cell1.cell.I + 1},{cell1.cell.J + 1})," +
        //                $"({cell2.cell.I + 1},{cell2.cell.J + 1}),{bit + 1}"  );
        cell1.neighborsStrongConnection[bit].Add(cell2);
        cell2.neighborsStrongConnection[bit].Add(cell1);
    }

    public int IsConnected(CellVertice cell1, CellVertice cell2, int bit)
    {
        if (cell1.neighborsStrongConnection[bit].Contains(cell2)) return 2;
        if (cell1.neighbors[bit].Contains(cell2)) return 1;
        return 0;
    }

    public CellVertice[][] FindAllCyclesFromVertice(CellVertice first, int bit)
    {
        UniqueEnumerableList<CellVertice> cycles = new();
        if (!cellsWithStrongConnection[bit].Contains(first)) return cycles.GetResult();
        FindAllCyclesInner(first, new(), cycles, /*visited,*/ 0, bit);
        return cycles.GetResult();
    }

    private void FindAllCyclesInner(CellVertice current, List<CellVertice> path, UniqueEnumerableList<CellVertice> prevCycles, int countOfNotStrong, int bit)
    {
        if (path.Count >= 2 && current == path[^2])
        {
            return;
        }

        if (countOfNotStrong > 1) return;

        bool isCurrStrong = cellsWithStrongConnection[bit].Contains(current);

        if (path.Count > 0 && (current.cell.I * 10 + current.cell.J < path[0].cell.I * 10 + path[0].cell.J) && isCurrStrong) return;

        if (path.Contains(current))
        {
            if (current != path[0]) return;

            var cycle = path.SkipWhile(c => c != current);
            prevCycles.Add(cycle.ToArray());
            return;
        }

        List<CellVertice[]> cycles = new();


        if (!isCurrStrong) countOfNotStrong++;

        path.Add(current);

        foreach (CellVertice cell in current.neighbors[bit])
        {
            if (countOfNotStrong > 1) break;
            FindAllCyclesInner(cell, path, prevCycles, countOfNotStrong, bit);
        }

        if (!isCurrStrong) countOfNotStrong--;
        path.Remove(current);
    }

    public CellVertice[][] FindAllCycles(int bit)
    {
        List<CellVertice[]> cycles = new();

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (!cellsWithStrongConnection[bit].Contains(this[i, j])) continue;

                cycles.AddRange(FindAllCyclesFromVertice(this[i, j], bit));
            }
        }

        return cycles.ToArray();
    }
}



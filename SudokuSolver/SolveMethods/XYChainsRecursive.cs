using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.SolveMethods;

internal class XYChainsRecursive : ISolveMethod
{
    public bool TrySolve(Sudoku sudoku)
    {
        bool[] beginBools = ToBoolArray(sudoku);

        SudokuGraph sudokuGraph = new SudokuGraph(sudoku);

        var ends = sudokuGraph.GetXYChainsEnds();

        foreach (var end in ends )
        {
            HashSet<CellVertice> cellVertices = sudokuGraph.Intersection(end.first, end.second, end.bit);

            foreach (var cellVertice in cellVertices )
            {
                cellVertice.cell.Options[end.bit] = false;
            }
        }

        bool[] endBools = ToBoolArray(sudoku);

        return !((IStructuralEquatable)beginBools).Equals(endBools, EqualityComparer<bool>.Default);
    }

    bool[] ToBoolArray(Sudoku sudoku)
    {
        return new bool[729].Select((ElementInit, index) => sudoku.Rows[index / 81][(index - index / 81 * 81) / 9]
        .Options[index % 9]).ToArray();
    }

}

class CellVertice
{
    public Cell cell;

    public CellVertice(Cell cell) => this.cell = cell;

    public HashSet<CellVertice> neighbors = new HashSet<CellVertice>();
}


class SudokuGraph
{
    public Sudoku sudoku;

    public CellVertice[][] newRows;
    public CellVertice[][] newColumns;
    public CellVertice[][] newSquares;
    public CellVertice[][][] cellModes;

    public new CellVertice this[int i, int j] => newRows[i][j];


    public SudokuGraph(Sudoku sudoku)
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

        cellModes = new CellVertice[][][] { newRows, newColumns, newSquares };

        List<CellVertice> cellWith2Options = new List<CellVertice>();

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (this[i, j].cell.Options.ToIndicesArray().Length == 2) cellWith2Options.Add(this[i, j]);
            }
        }

        foreach (var cell1 in cellWith2Options)
        {
            foreach (var cell2 in cellWith2Options)
            {
                if (cell1 == cell2 || !AreDependent(cell1, cell2)) continue;
                cell1.neighbors.Add(cell2);
            }
        }
    }

    public bool AreDependent(CellVertice cell1, CellVertice cell2) =>
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



    public void GetXYChainsEndsOfCellRecursive(
        CellVertice current, 
        List<CellVertice> path, 
        int firstnum, 
        int secondnum, 
        HashSet<(CellVertice first ,CellVertice second ,int bit)> ends)
    {
        for (int i = 0; i < path.Count; i++)
        {
            if (current == path[i]) return;
        }

        int newfirstnum = firstnum, newsecondnum = -1;

        if (path.Count == 0)
        {
            path.Add(current);
        }
        else
        {
            int[] lastOptions = path[^1].cell.Options.ToIndicesArray();
            int[] currentOptions = current.cell.Options.ToIndicesArray();

            if (path.Count == 1)
            {
                int[] common = lastOptions.Intersect(currentOptions).ToArray();
                if(common.Length != 1) return;
                newfirstnum = (lastOptions[0] == common[0]) ? lastOptions[1] : lastOptions[0];
                newsecondnum = (currentOptions[0] == common[0]) ? currentOptions[1] : currentOptions[0];
            }
            else
            {
                if (!currentOptions.Contains(secondnum)) return;
                newsecondnum = (currentOptions[0] == secondnum) ? currentOptions[1] : currentOptions[0];
            }
            

            path.Add(current);
        }

        if (newfirstnum == newsecondnum && newfirstnum != -1)
        {
            if (path[0].cell.I *10 + path[0].cell.J < path[^1].cell.I * 10 + path[^1].cell.J) ends.Add((path[0], path[^1], newfirstnum));
        }


        foreach (var neighbor in current.neighbors)
        {
            GetXYChainsEndsOfCellRecursive(neighbor, path, newfirstnum, newsecondnum, ends);
        }


        path.Remove(current);
    }

    public HashSet<(CellVertice first, CellVertice second, int bit)> GetXYChainsEnds()
    {
        HashSet<(CellVertice first, CellVertice second, int bit)> ends = new();

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                GetXYChainsEndsOfCellRecursive(this[i,j],new(),-1,-1,ends);
            }
        }

        return ends;
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.SolveMethods.XYChains;

internal class XYChainsRecursive : ISolveMethod
{
    public bool TrySolve(Sudoku sudoku)
    {
        bool[] beginBools = ToBoolArray(sudoku);

        SudokuGraph sudokuGraph = new SudokuGraph(sudoku);

        var ends = sudokuGraph.GetXYChainsEnds();

        foreach (var end in ends)
        {
            HashSet<CellVertice> cellVertices = sudokuGraph.Intersection(end.first, end.second, end.bit);

            foreach (var cellVertice in cellVertices)
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



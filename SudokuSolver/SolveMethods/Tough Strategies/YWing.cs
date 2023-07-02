
namespace SudokuSolver.SolveMethods.ToughStrategies;

internal class YWing : ISolveMethod
{
    public bool TrySolve(Sudoku sudoku)
    {

        bool[] beginBools = ToBoolArray(sudoku);

        List<Cell>[][] cellsWith2Posibles = GetCellsWith2Posibles(sudoku);
     
        for (int i = 0; i < 9; i++)
        {
            for (int j = i + 1; j < 9; j++)
            {
                if (cellsWith2Posibles[i][j].Count == 0) continue;
                for (int k = j + 1; k < 9; k++)
                {
                    if (cellsWith2Posibles[i][k].Count == 0 ||
                        cellsWith2Posibles[j][k].Count == 0) continue;

                    for (int i1 = 0; i1 < cellsWith2Posibles[i][j].Count; i1++)
                    {
                        for (int i2 = 0; i2 < cellsWith2Posibles[i][k].Count; i2++)
                        {
                            for (int i3 = 0; i3 < cellsWith2Posibles[j][k].Count; i3++)
                            {
                                Cell cell1 = cellsWith2Posibles[i][j][i1];
                                Cell cell2 = cellsWith2Posibles[i][k][i2];
                                Cell cell3 = cellsWith2Posibles[j][k][i3];

                                bool side1 = AreDependent(cell2, cell3);
                                bool side2 = AreDependent(cell1, cell3);
                                bool side3 = AreDependent(cell1, cell2);

                                if (side2 && side3)
                                {
                                    List<Cell> intersect = Intersection(sudoku, cell2, cell3);
                                    for (int l = 0; l < intersect.Count; l++)
                                    {
                                        intersect[l].Options[k] = false;
                                    }
                                }
                                if (side1 && side3)
                                {
                                    List<Cell> intersect = Intersection(sudoku, cell1, cell3);
                                    for (int l = 0; l < intersect.Count; l++)
                                    {
                                        intersect[l].Options[j] = false;
                                    }
                                }
                                if (side1 && side2)
                                {
                                    List<Cell> intersect = Intersection(sudoku, cell1, cell2);
                                    for (int l = 0; l < intersect.Count; l++)
                                    {
                                        intersect[l].Options[i] = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        bool[] endBools = ToBoolArray(sudoku);

        return !((IStructuralEquatable)beginBools).Equals(endBools, EqualityComparer<bool>.Default);
    }
    bool[] ToBoolArray(Sudoku sudoku)
    {
        return new bool[729].Select((ElementInit, index) => sudoku.Rows[index / 81][(index - (index / 81) * 81) / 9]
        .Options[index % 9]).ToArray();
    }

    bool AreDependent(Cell cell1, Cell cell2) => cell1.I == cell2.I || cell1.J == cell2.J || cell1.S == cell2.S;

    List<Cell> Intersection(Sudoku sudoku, Cell cell1, Cell cell2)
    {
        List<Cell> result = new List<Cell>();

        for (int i = 0; i < 9; i++)
        {
            if (AreDependent(cell2, sudoku.Rows[cell1.I][i])
                && sudoku.Rows[cell1.I][i] != cell1
                && sudoku.Rows[cell1.I][i] != cell2) result.Add(sudoku.Rows[cell1.I][i]);

            if (AreDependent(cell2, sudoku.Columns[cell1.J][i])
                && sudoku.Columns[cell1.J][i] != cell1
                && sudoku.Columns[cell1.J][i] != cell2) result.Add(sudoku.Columns[cell1.J][i]);

            if (AreDependent(cell2, sudoku.Squares[cell1.S][i])
                && sudoku.Squares[cell1.S][i] != cell1
                && sudoku.Squares[cell1.S][i] != cell2) result.Add(sudoku.Squares[cell1.S][i]);
        }

        return result;
    }

    List<Cell>[][] GetCellsWith2Posibles(Sudoku sudoku)
    {
        List<Cell>[][] cellsWith2Posibles = new List<Cell>[9][];
        for (int i = 0; i < 9; i++)
        {
            cellsWith2Posibles[i] = new List<Cell>[9];
            for (int j = 0; j < 9; j++)
            {
                cellsWith2Posibles[i][j] = new List<Cell>();
            }
        }

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                int[] arrOfOnes = sudoku[i, j].Options.ToIndicesArray();
                if (arrOfOnes.Length != 2) continue;
                cellsWith2Posibles[arrOfOnes[0]][arrOfOnes[1]].Add(sudoku[i, j]);
                cellsWith2Posibles[arrOfOnes[1]][arrOfOnes[0]].Add(sudoku[i, j]);
            }
        }

        return cellsWith2Posibles; 
    }

}

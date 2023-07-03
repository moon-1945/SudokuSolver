namespace SudokuSolver.SolveMethods.ToughStrategies;

public class XYZWing : ISolveMethod
{
    public bool TrySolve(Sudoku sudoku)
    {
        bool result = false;

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                int[] XYZ = sudoku[i, j].Options.ToIndicesArray();
                if (XYZ.Length != 3) continue;

                List<Cell> neighborCells = GetConnectedSells(sudoku, sudoku[i, j]);

                List<Cell>[] pairsXYZ = new List<Cell>[3] { new List<Cell>(), new List<Cell>(), new List<Cell>() };

                for (int k = 0; k < neighborCells.Count; k++)
                {
                    int[] bits = neighborCells[k].Options.ToIndicesArray();
                    if (bits.Length != 2) continue;

                    if (XYZ[0] == bits[0] && XYZ[1] == bits[1]) pairsXYZ[2].Add(neighborCells[k]);
                    if (XYZ[0] == bits[0] && XYZ[2] == bits[1]) pairsXYZ[1].Add(neighborCells[k]);
                    if (XYZ[1] == bits[0] && XYZ[2] == bits[1]) pairsXYZ[0].Add(neighborCells[k]);
                }

                for (int k = 0; k < 3; k++)
                {
                    for (int i1 = 0; i1 < pairsXYZ[k].Count; i1++)
                    {
                        for (int i2 = 0; i2 < pairsXYZ[(k + 1) % 3].Count; i2++)
                        {
                            Cell XYZcell = sudoku[i, j];
                            Cell cell1 = pairsXYZ[k][i1];
                            Cell cell2 = pairsXYZ[(k + 1) % 3][i2];

                            int removeBit = XYZ[(k + 2) % 3];

                            for (int r = 0; r < neighborCells.Count; r++)
                            {
                                if (neighborCells[r] == cell1 || neighborCells[r] == cell2) continue;

                                if (IsConnected(neighborCells[r], cell1) &&
                                    IsConnected(neighborCells[r], cell2))
                                {
                                    if (!neighborCells[r].Options[removeBit]) continue;
                                    
                                    result = true;
                                    neighborCells[r].Options[removeBit] = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        return result;
    }

    List<Cell> GetConnectedSells(Sudoku sudoku, Cell cell)
    {
        List<Cell> cells = new List<Cell>();
        for (int i = 0; i < 9; i++)
        {
            if (sudoku[i, cell.J] != cell) cells.Add(sudoku[i, cell.J]);
            if (sudoku[cell.I, i] != cell) cells.Add(sudoku[cell.I, i]);
        }

        for (int i = 0; i < 9; i++)
        {
            if (sudoku.Squares[cell.S][i].I == cell.I ||
                sudoku.Squares[cell.S][i].J == cell.J) continue;

            cells.Add(sudoku.Squares[cell.S][i]);
        }

        return cells;
    }

    bool IsConnected(Cell cell1, Cell cell2)
    {
        return cell1.I == cell2.I || cell1.J == cell2.J || cell1.S == cell2.S;
    }

}

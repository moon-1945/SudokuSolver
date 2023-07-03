namespace SudokuSolver.SolveMethods.ToughStrategies;

public class SwordFish : ISolveMethod
{
    public bool TrySolve(Sudoku sudoku)
    {
        bool[] beginBools = ToBoolArray(sudoku);

        Cell[][][] cellModes = sudoku.CellModes;
        BitArray[][][] maskModes = sudoku.GenerateMaskModes();

        for (int i = 0; i < 9; i++)
        {
            for (int j = i + 1; j < 9; j++)
            {
                for (int k = j + 1; k < 9; k++)
                {
                    for (int mode = 0; mode < 2; mode++)
                    {
                        for (int bit = 0; bit < 9; bit++)
                        {
                            var cellsI = maskModes[mode][i][bit];
                            var cellsJ = maskModes[mode][j][bit];
                            var cellsK = maskModes[mode][k][bit];

                            int r = 0, s = 0, t = 0;

                            for (int bit1 = 0; bit1 < 9; bit1++)
                            {
                                if (cellsI[bit1]) r++;
                                if (cellsJ[bit1]) s++;
                                if (cellsK[bit1]) t++;
                            }

                            if (r == 0 || s == 0 || t == 0) continue;

                            var cellsBit = new BitArray(9);
                            cellsBit.Or(cellsI);
                            cellsBit.Or(cellsJ);
                            cellsBit.Or(cellsK);

                            var cellsNumbers = cellsBit.ToIndicesArray();

                            if (cellsNumbers.Length != 3) continue;


                           
                            Cell[] choosenCells = new Cell[9];

                            bool r1 = false;
                            
                            for (int l = 0; l < 3; l++)
                            {
                                choosenCells[l] = cellModes[mode][i][cellsNumbers[l]];
                                choosenCells[l + 3] = cellModes[mode][j][cellsNumbers[l]];
                                choosenCells[l + 6] = cellModes[mode][k][cellsNumbers[l]];

                                //if ((!choosenCells[l].Options[bit] && choosenCells[l].Value == 0) ||
                                //    (!choosenCells[l + 3].Options[bit] && choosenCells[l + 3].Value == 0) ||
                                //    (!choosenCells[l + 6].Options[bit] && choosenCells[l + 6].Value == 0)) r1 = true;
                            }
                            if (r1) continue;
                             //Console.WriteLine($"{i},{j},{k},{cellsNumbers[0]},{cellsNumbers[1]},{cellsNumbers[2]},{mode},{bit}");
                            for (int l = 0; l < 3; l++)
                            {
                                DeleteBitOnCells(cellModes[1 - mode][cellsNumbers[l]], choosenCells, bit);
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

    void DeleteBitOnCells(Cell[] cells, Cell[] exceptions, int bit)
    {
        bool[] bitOnException = new bool[exceptions.Length].Select((elem, index) => exceptions[index].Options[bit]).ToArray();

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Options[bit] = false;
        }

        for (int i = 0; i < exceptions.Length; i++)
        {
            exceptions[i].Options[bit] = bitOnException[i];
        }
    }
}

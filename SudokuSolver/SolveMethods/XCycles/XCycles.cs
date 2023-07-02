
using System.Linq;

namespace SudokuSolver.SolveMethods.XCycles;

internal class XCycles : ISolveMethod
{
    public bool TrySolve(Sudoku sudoku)
    {
        SudokuGraph sudokuGraph = new SudokuGraph(sudoku);
        // Console.WriteLine(sudokuGraph.sudoku);

        bool[] beginBools = ToBoolArray(sudoku);

        for (int bit = 0; bit < 9; bit++)
        {
            CellVertice[][] cycles = sudokuGraph.FindAllCycles(bit);
            //Console.WriteLine($"{bit + 1}");
            //Console.WriteLine(string.Join("\n", cycles.Select(c => string.Join(", ", c.Select(cc => $"({cc.cell.I + 1},{cc.cell.J + 1})")))));
            //Console.WriteLine();
            for (int i = 0; i < cycles.Length; i++)
            {
                var cycle = cycles[i];

                if (cycle.Length % 2 == 0)
                {
                    int a = 0;
                    for (int begin = 0; begin <= 1; begin++)
                    {
                        int SW = 0;
                        for (int j = begin; j < cycle.Length; j += 2)
                        {
                            if (sudokuGraph.IsConnected(cycle[j], cycle[(j + 1) % cycle.Length], bit) == 2) SW++;
                        }
                        if (SW != cycle.Length / 2) a++;
                    }

                    if (a == 2) continue;

                    for (int j = 0; j < cycle.Length; j++)
                    {
                        if (sudokuGraph.IsConnected(cycle[j], cycle[(j + 1) % cycle.Length], bit) != 1) continue;

                        foreach (var vertice in sudokuGraph.Intersection(cycle[j], cycle[(j + 1) % cycle.Length], bit))
                        {
                            vertice.cell.Options[bit] = false;
                        }
                    }

                    //Console.WriteLine($"{bit + 1} : " + string.Join(",", cycle.Select(c => $"({c.cell.I + 1},{c.cell.J + 1})")));
                }
                else
                {

                    CellVertice cell = null;
                    int size = 0;

                    for (int j = 0; j <= 2 * cycle.Length; j += 2)
                    {
                        _ = sudokuGraph.IsConnected(cycle[j % cycle.Length], cycle[(j + 1) % cycle.Length], bit) == 2 ? size++ : size = 0;


                        if (size == cycle.Length / 2)
                        {


                            if (sudokuGraph.IsConnected(cycle[(j + 2) % cycle.Length], cycle[(j + 3) % cycle.Length], bit) == 2)
                            {
                                cell = cycle[(j + 3) % cycle.Length];

                                cell.cell.Options = new BitArray(9);
                                cell.cell.Options[bit] = true;
                                //Console.WriteLine($"{bit + 1} : " + string.Join(",", cycle.Select(c => $"({c.cell.I + 1},{c.cell.J + 1})")) + $"   ({cell.cell.I + 1},{cell.cell.J + 1})");
                            }
                            else
                            {
                                cell = cycle[(j + 2) % cycle.Length];
                                cell.cell.Options[bit] = false;
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
        return new bool[729].Select((ElementInit, index) => sudoku.Rows[index / 81][(index - index / 81 * 81) / 9]
        .Options[index % 9]).ToArray();
    }
}


/*
 https://www.sudokuwiki.org/ServerSolver.aspx?k=0 POST
 
frm.elements['coordmode'].value = coordmode; // + ((document.getElementById("NRCmode").checked) ? 2 : 0);
frm.elements['gors'].value = 1;
frm.elements['strat'].value = 'XWG';
frm.elements['stratmask'].value = st;
frm.elements['mapno'].value = jig;
frm.elements['fullreport'].value = (fullreport==1)?1:0;
frm.elements['board'].value = s.substr(1,s.length-1);
	
 
 
 
 
 
 */



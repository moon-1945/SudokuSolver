using SudokuSolver.SolveMethods.XCycles;

namespace SudokuSolver.SolveMethods;

public class UniqueEnumerableList<T>
{
    private List<IEnumerable<T>> _list = new();
    private List<HashSet<T>> _listHashes = new();

    public UniqueEnumerableList()
    {
    }

    public UniqueEnumerableList(List<IEnumerable<T>> list)
    {
        _list = list;
        _listHashes = list.Select(e => new HashSet<T>()).ToList();
    }

    public void Add(IEnumerable<T> item)
    {
        var itemHashSet = new HashSet<T>(item);
        if (_listHashes.All(hash => !hash.IsEquivalent(itemHashSet)))
        {
            _list.Add(item);
            _listHashes.Add(itemHashSet);
        }
    }

    public T[][] GetResult() => _list.Select(e => e.ToArray()).ToArray();
}


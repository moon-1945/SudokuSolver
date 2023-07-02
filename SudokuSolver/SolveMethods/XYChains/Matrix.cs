using System.Numerics;


namespace SudokuSolver.SolveMethods.XYChains;

public class Matrix<T> :
    IAdditionOperators<Matrix<T>, Matrix<T>, Matrix<T>>,
    IMultiplyOperators<Matrix<T>, Matrix<T>, Matrix<T>> where T : IAdditionOperators<T, T, T>, IMultiplyOperators<T, T, T>
{
    T[][] values;
    public int Rows { get; init; }
    public int Columns { get; init; }
    public T this[int i, int j] => values[i][j];
    public Matrix(T[][] values)
    {
        this.values = values;
        Rows = values.Length;
        Columns = values[0].Length;
        for (int i = 1; i < values.Length; i++)
        {
            if (values[i].Length != values[i - 1].Length) throw new ArgumentException("invalid argument");
        }
    }

    public static Matrix<T> operator +(Matrix<T> left, Matrix<T> right)
    {
        if (left.Rows != right.Rows || left.Columns != right.Columns) throw new Exception("addition impossible");

        T[][] newValues = new T[left.Rows][];
        for (int i = 0; i < newValues.Length; i++)
        {
            newValues[i] = new T[left.Columns].Select((elem, j) => left[i, j] + right[i, j]).ToArray();
        }

        return new Matrix<T>(newValues);
    }

    public static Matrix<T> operator *(Matrix<T> left, Matrix<T> right)
    {
        if (left.Columns != right.Rows) throw new Exception("multiplication impossible");

        T[][] newValues = new T[left.Rows][];
        for (int i = 0; i < newValues.Length; i++)
        {
            newValues[i] = new T[left.Columns];
        }

        for (int i = 0; i < newValues.Length; i++)
        {
            for (int j = 0; j < newValues[0].Length; j++)
            {
                T sum = left[i, 0] * right[0, j];
                for (int k = 1; k < left.Columns; k++)
                {
                    sum = sum + left[i, k] * right[k, j];
                }
                newValues[i][j] = sum;
            }
        }

        return new Matrix<T>(newValues);
    }
}



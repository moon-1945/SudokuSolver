using System.Numerics;

namespace MatrixUtils;

public static class AdjacencyMatrixExtension
{
    public static Matrix<T> CalculateReachabilityMatrix<T>(this Matrix<T> matrix) 
        where T : IAdditionOperators<T, T, T>, IMultiplyOperators<T, T, T>
    {
        Matrix<T> reachability = matrix;

        for (int i = 0; i < matrix.Rows - 1; i++)
        {
            reachability = reachability * matrix + matrix;
        }

        return reachability;
    }
}
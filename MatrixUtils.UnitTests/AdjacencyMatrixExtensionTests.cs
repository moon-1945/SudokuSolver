using FluentAssertions;

namespace MatrixUtils.UnitTests;

public class AdjacencyMatrixExtensionTests
{
    [Test]
    public void CalculateReachabilityMatrix_WorksCorrect()
    {
        // Arrange
        Matrix<int> matrix = new(new int[][] { 
            new int[]{0, 0, 1, 0}, 
            new int[]{0, 0, 1, 0},
            new int[]{0, 0, 0, 1},
            new int[]{0, 1, 0, 0}
        });

        Matrix<int> expected = new(new int[][] {
            new int[]{0, 1, 2, 1},
            new int[]{0, 1, 2, 1},
            new int[]{0, 1, 1, 2},
            new int[]{0, 2, 1, 1}
        });

        // Act
        var result = matrix.CalculateReachabilityMatrix();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
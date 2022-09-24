namespace MatrixMultiplicationTest;

using NUnit.Framework;
using System;
using Task_1;

public class Tests
{    
    [Test]
    public void MultiplyTest()
    {
        var matrixOneA = Matrix.GenerateMatrix(3, 4);
        var matrixOneB = Matrix.GenerateMatrix(3, 4);
        var matrixTwoA = new Matrix(new int[2, 2] { { 1, 1 }, { 1, 1 } });
        var matrixTwoB = new Matrix(new int[2, 2] { { 1, -1 }, { 1, -1 } });
        var expectedMatrix = new Matrix(new int[2, 2] { { 2, -2 }, { 2, -2 } });
        Assert.Throws<InvalidOperationException>(() => MatrixMultiplication.SequentiallyMultiply(matrixOneA, matrixOneB));
        Assert.Throws<InvalidOperationException>(() => MatrixMultiplication.ParallelMultiply(matrixOneA, matrixOneB));
        Assert.IsTrue(Matrix.Equal(MatrixMultiplication.SequentiallyMultiply(matrixTwoA, matrixTwoB), expectedMatrix));
        Assert.IsTrue(Matrix.Equal(MatrixMultiplication.ParallelMultiply(matrixTwoA, matrixTwoB), expectedMatrix));
    }  

    [Test]
    public void MultiplicationTestOnRandomMatrices()
    {
        var matrixA = Matrix.GenerateMatrix(50, 40);
        var matrixB = Matrix.GenerateMatrix(40, 20);
        var resultOfSequentiallyMultiplication = MatrixMultiplication.SequentiallyMultiply(matrixA, matrixB);
        var resultOfParallelMultiplication = MatrixMultiplication.ParallelMultiply(matrixA, matrixB);
        Assert.Throws<ArgumentOutOfRangeException>(() => Matrix.GenerateMatrix(0, 0));
        Assert.IsTrue(Matrix.Equal(resultOfSequentiallyMultiplication, resultOfParallelMultiplication));
    }
}

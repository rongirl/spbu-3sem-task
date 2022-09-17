namespace Task_1;

/// <summary>
/// Класс, реализующий последовательное и параллельное умножение двух матриц
/// </summary>
public class MatrixMultiplication
{
    /// <summary>
    /// Последовательно перемножает матрицы
    /// </summary>
    public static Matrix SequentiallyMultiply(Matrix matrixA, Matrix matrixB)
    {   
        if (matrixA.Columns != matrixB.Rows)
        {
            throw new InvalidOperationException("Количество столбцов первой матрицы не равно количеству строк второй.");
        }
        var resultElements = new int[matrixA.Rows, matrixB.Columns];
        for (int i = 0; i < matrixA.Rows; i++)
        {
            for (int j = 0; j < matrixB.Columns; j++)
            {
                for (int k = 0; k < matrixA.Columns; k++)
                {
                    resultElements[i, j] += matrixA.Elements[i, k] * matrixB.Elements[k, j];  
                }
            }
        }
        return new Matrix(resultElements);  
    }

    /// <summary>
    /// Параллельно умножает матрицы
    /// </summary>
    public static Matrix ParallelMultiply(Matrix matrixA, Matrix matrixB)
    {
        if (matrixA.Columns != matrixB.Rows)
        {
            throw new InvalidOperationException("Количество столбцов первой матрицы не равно количеству строк второй.");
        }
        var resultElements = new int[matrixA.Rows, matrixB.Columns];
        var threads = new Thread[Environment.ProcessorCount];
        var chunkSize = matrixA.Rows / threads.Length + 1;
        for (var i = 0; i < threads.Length; ++i)
        {
            var localI = i;
            threads[i] = new Thread(() =>
            {
                for (int j = localI * chunkSize; j < (localI + 1) * chunkSize && j < matrixA.Rows; ++j)
                {
                    for (int k = 0; k < matrixA.Columns; k++)
                    {
                        for (int h = 0; h < matrixB.Columns; h++)
                        {
                            resultElements[j, h] += matrixA.Elements[j, k] * matrixB.Elements[k, h];
                        }
                    }
                }

            });
        }
        foreach (var thread in threads)
        {
            thread.Start();
        }
        foreach (var thread in threads)
        { 
            thread.Join();
        }
        return new Matrix(resultElements);
    }
}
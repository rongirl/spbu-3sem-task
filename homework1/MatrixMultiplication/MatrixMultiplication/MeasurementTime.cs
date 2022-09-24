namespace Task_1;

using System.Diagnostics;

/// <summary>
/// Класс, реализующий измерение матожидания, среднего квадратичного отклонения последовательного и параллельного умножения двух матриц
/// </summary>
public static class Measurement
{
    public static void MeasureTime()
    {
        var countOfExperiments = 10;
        var expectedValueOfSequentialMultiplication = 0.0;
        var standardDeviationOfSequentialMultiplication = 0.0;
        var expectedValueOfParallelMultiplication = 0.0;
        var standardDeviationOfParallelMultiplication = 0.0;
        var maximumSize = 1000;
        var measurementsOfSequentialMultiplication = new double[countOfExperiments];
        var measurementsOfParallelMultiplication = new double[countOfExperiments];
        var timer = new Stopwatch();
        using var streamWriter = new StreamWriter("ResultOfMeasurement.txt");
        streamWriter.WriteLine($"Count of experiments: {countOfExperiments} \n");
        streamWriter.WriteLine("    Size:      Expected value (Sequential):    Standard deviation (Sequential):    Expected value (Parallel):    Standard deviation (Parallel):");
        for (int size = 100; size <= maximumSize; size += 100)
        {
            var matrixA = Matrix.GenerateMatrix(size, size);
            var matrixB = Matrix.GenerateMatrix(size, size);
            for (int count = 1; count <= countOfExperiments; count++)
            {
                timer.Start();
                MatrixMultiplication.SequentiallyMultiply(matrixA, matrixB);
                timer.Stop();
                measurementsOfSequentialMultiplication[count - 1] = timer.Elapsed.TotalSeconds;
                expectedValueOfSequentialMultiplication += timer.Elapsed.TotalSeconds;
                timer.Restart();
                MatrixMultiplication.ParallelMultiply(matrixA, matrixB);
                timer.Stop();
                measurementsOfParallelMultiplication[count - 1] = timer.Elapsed.TotalSeconds;
                expectedValueOfParallelMultiplication += timer.Elapsed.TotalSeconds;
            }
            expectedValueOfSequentialMultiplication /= countOfExperiments;
            expectedValueOfParallelMultiplication /= countOfExperiments;
            double differenceOfSequentialMultiplication = 0.0;
            double differenceOfParallelMultiplication = 0.0;
            for (int i = 0; i < countOfExperiments; i++)
            {
                differenceOfSequentialMultiplication = measurementsOfSequentialMultiplication[i] - expectedValueOfSequentialMultiplication;
                standardDeviationOfSequentialMultiplication += differenceOfSequentialMultiplication * differenceOfSequentialMultiplication;
                differenceOfParallelMultiplication = measurementsOfParallelMultiplication[i] - expectedValueOfParallelMultiplication;
                standardDeviationOfParallelMultiplication += differenceOfParallelMultiplication * differenceOfParallelMultiplication;
            }
            standardDeviationOfSequentialMultiplication /= countOfExperiments;
            standardDeviationOfParallelMultiplication /= countOfExperiments;
            standardDeviationOfSequentialMultiplication = Math.Sqrt(standardDeviationOfSequentialMultiplication);
            standardDeviationOfParallelMultiplication = Math.Sqrt(standardDeviationOfParallelMultiplication);
            streamWriter.Write($"  {size} x {size}     ");
            streamWriter.Write($"    {expectedValueOfSequentialMultiplication:f3}sec                          {standardDeviationOfSequentialMultiplication:f3}sec    ");
            streamWriter.WriteLine($"                     {expectedValueOfParallelMultiplication:f3}sec                        {standardDeviationOfParallelMultiplication:f3}sec    ");
        }
    }
}
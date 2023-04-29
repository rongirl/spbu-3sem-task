using Task_1;

var matrixA = Matrix.ParseMatrix("FirstMatrix.txt");
var matrixB = Matrix.ParseMatrix("SecondMatrix.txt");
var matrixC = MatrixMultiplication.ParallelMultiply(matrixA, matrixB);
Matrix.WriteInFileMatrix(matrixC, "Output.txt");
Measurement.MeasureTime();
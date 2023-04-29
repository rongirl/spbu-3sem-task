namespace Task_1;

/// <summary>
/// Класс, реализующий матрицу
/// </summary>
public class Matrix
{   
    /// <summary>
    /// Элементы в матрице
    /// </summary>
    private int[,] elements;

    /// <summary>
    /// Количество строк в матрице
    /// </summary>
    public int Rows => elements.GetLength(0);

    /// <summary>
    /// Количество столбцов
    /// </summary>
    public int Columns => elements.GetLength(1);

    /// <summary>
    /// Инициализация матрицы
    /// </summary>
    public Matrix(int[,] elements)
    {
        this.elements = (int[,]) elements.Clone();
    }

    public int this [int row, int columns]
    {
        get => elements[row, columns];  
        set => elements[row, columns] = value;
    }

    /// <summary>
    /// Читает матрицу из файла
    /// </summary>
    public static Matrix ParseMatrix(string filename)
    {
        var rows = File.ReadAllLines(filename);
        var columns = rows[0].Split(' ');
        var elements = new int[rows.Length, columns.Length];
        for (int i = 0; i < rows.Length; i++)
        {
            var currentColumns = rows[i].Split(' ');
            for (int j = 0; j < columns.Length; j++)
            {
                if (currentColumns.Length != columns.Length)
                {
                    throw new InvalidOperationException("Матрица введена некорректно.");
                }
                elements[i, j] = int.Parse(currentColumns[j]);
            }
        }
        return new Matrix(elements);    
    }

    /// <summary>
    /// Записывает матрицу в файл
    /// </summary>
    public static void WriteInFileMatrix(Matrix matrix, string filename)
    {
        using var writer = new StreamWriter(filename);  
        for (int i = 0; i < matrix.Rows; i++)
        {
            for (int j = 0; j < matrix.Columns; j++)
            {
                writer.Write($"{matrix.elements[i, j]} ");
            }
            writer.Write("\n");
        }
    }

    /// <summary>
    /// Генерирует матрицу
    /// </summary>
    /// <param name="rows">Количество строк</param>
    /// <param name="columns">Количество столбцов</param>
    public static Matrix GenerateMatrix(int rows, int columns)
    {
        if (rows <= 0 || columns <= 0)
        {
            throw new ArgumentOutOfRangeException("Матрица сгенерирована некорректно.");
        }
        var elements = new int[rows, columns];  
        var random = new Random();  
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                elements[i, j] = random.Next(1, 51);
            }
        }
        return new Matrix(elements);
    }

    /// <summary>
    /// Проверяет равенство двух матриц
    /// </summary>
    public static bool Equal(Matrix matrixA, Matrix matrixB)
    {
        if (matrixA.Rows != matrixB.Rows || matrixA.Columns != matrixB.Columns)
        {
            return false;
        }
        for (int i = 0; i < matrixA.Rows; i++)
        {
            for (int j = 0; j < matrixA.Columns; j++)
            {
                if (matrixA.elements[i, j] != matrixB.elements[i, j])
                {
                    return false;
                }
            }
        }
        return true;
    }
}
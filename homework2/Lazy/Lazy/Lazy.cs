namespace Task_2;

using System;

/// <summary>
/// Класс, реализующий ленивое вычисление в однопоточном режиме
/// </summary>
public class Lazy<T> : ILazy<T>
{
    private bool isCalculated = false;

    /// <summary>
    /// Функция вычисления
    /// </summary>
    private Func<T?>? supplier;

    /// <summary>
    /// Результат вычисления
    /// </summary>
    private T? result;

    /// <summary>
    /// Инициализация экземпляра класса 
    /// </summary>
    public Lazy(Func<T?> supplier) => this.supplier = supplier ?? throw new ArgumentNullException();

    public T? Get()
    {
        if (isCalculated)
        {
            return result;
        }
        if (supplier != null)
        {
            result = supplier();
        }
        supplier = null;
        isCalculated = true;
        return result;
    }
}
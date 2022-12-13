namespace Task_2;

using System;

/// <summary>
/// Класс, реализующий ленивое вычисление в многопоточном режиме
/// </summary>
public class LazyWithThreads<T> : ILazy<T>
{   
    private volatile bool isCalculated = false;

    /// <summary>
    /// Функция вычисления
    /// </summary>
    private Func<T?> supplier;

    /// <summary>
    /// Результат вычисления
    /// </summary>
    private T? result;
    private object locker = new object();

    /// <summary>
    /// Инициализация экземпляра класса 
    /// </summary>
    public LazyWithThreads(Func<T?> supplier) => this.supplier = supplier;

    public T? Get()
    {
        if (isCalculated)
        {
            return result;
        }
        lock (locker)
        {
            if (isCalculated)
            {
                return result;
            }
            result = supplier();
            supplier = null;
            isCalculated = true;
            return result;
        }
    }
}
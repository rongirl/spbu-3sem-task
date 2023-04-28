namespace Task_2;

/// <summary>
/// Интерфейс для ленивого вычисления
/// </summary>
/// <typeparam name="T">Тип возвращаемого объекта</typeparam>
public interface ILazy<T>
{   
    /// <summary>
    /// Вычисляет и возвращает результат, при повторном вычислении возвращает первый результат
    /// </summary>
    T? Get();
}
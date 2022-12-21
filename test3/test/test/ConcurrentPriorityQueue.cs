using System.Collections;

namespace Task_3;

public class ConcurrentPriorityQueue<TElement, TPriority>
{
    private readonly object lockObject = new();
    /// <summary>
    /// Так как удаляется из очереди элемент с наименьшим приоритетом, 
    /// меняем компаратор
    /// </summary>
    private readonly PriorityQueue<TElement, TPriority> queue =
        new(Comparer<TPriority>.Create((a, b) => -Comparer.Default.Compare(a, b)));

    /// <summary>
    /// Ставит значение с заданным приоритетом в очередь
    /// </summary>
    /// <param name="value">Значение</param>
    /// <param name="priority">Значение приоритета</param>
    public void Enqueue(TElement value, TPriority priority)
    {
        lock (lockObject)
        {
            queue.Enqueue(value, priority);
            Monitor.PulseAll(lockObject);
        }
    }

    /// <summary>
    /// Количество элементов в очереди
    /// </summary>
    public int Size
    {
        get
        {
            lock (lockObject)
            {
                return queue.Count;
            }
        }
    }

    /// <summary>
    /// Возвращает значение с максимальном приоритетом и удаляет его из очереди
    /// </summary>
    public TElement Dequeue()
    {
        lock (lockObject)
        {
            while (queue.Count == 0)
            {
                Monitor.Wait(lockObject);
            }

            var element = queue.Dequeue();
            Monitor.PulseAll(lockObject);
            return element;
        }
    }
}
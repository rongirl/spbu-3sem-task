namespace Task_3;

using System.Collections.Concurrent;

public class MyThreadPool
{
    private Thread[] threads;
    private CancellationTokenSource cancellationToken;
    private BlockingCollection<Action> actions;
    private AutoResetEvent autoReset = new AutoResetEvent(false);
    private object lockObject = new();
    public MyThreadPool(int countOfThreads)
    {
        if (countOfThreads <= 0)
        {
            throw new ArgumentOutOfRangeException("Count must be positive");
        }
        cancellationToken = new CancellationTokenSource();
        actions = new BlockingCollection<Action>();
        threads = new Thread[countOfThreads];
        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = CreateThread();
            threads[i].Start();
        }
    }

    public IMyTask<TResult> Submit<TResult>(Func<TResult> function)
    {
        if (!cancellationToken.IsCancellationRequested)
        {
            lock (lockObject)
            {
                var task = new MyTask<TResult>(function, this);
                actions.Add(task.Run);
                return task;
            }
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public void ShutDown()
    {
        lock (lockObject)
        {
            cancellationToken.Cancel();
            actions.CompleteAdding();
        }
        foreach (var thread in threads)
        {
            thread.Join();
        }
    }

    private Thread CreateThread()
    {
        var thread = new Thread(() =>
        {
            foreach (var action in actions.GetConsumingEnumerable())
            {
                action();
            }
        })
        {
            IsBackground = true
        };
        return thread;
    }

    private class MyTask<TResult> : IMyTask<TResult>
    {
        private Exception caughtException;
        private Func<TResult> function;
        private MyThreadPool threadPool;
        private object lockObj = new object();
        private TResult result;
        private ManualResetEvent isCalculated = new ManualResetEvent(false);
        private ConcurrentQueue<Action> continuationOfTask = new ConcurrentQueue<Action>();

        public bool IsCompleted { get; private set; }

        public MyTask(Func<TResult> function, MyThreadPool threadPool)
        {
            this.function = function;
            this.threadPool = threadPool;
        }

        public TResult Result
        {
            get
            {
                if (!IsCompleted)
                {
                    isCalculated.WaitOne();
                }
                if (caughtException != null)
                {
                    throw new AggregateException(caughtException);
                }
                return result;
            }
        }

        public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> continuationFunction)
        {
            if (threadPool.cancellationToken.IsCancellationRequested)
            {
                throw new InvalidOperationException();
            }
            lock (lockObj)
            {
                if (IsCompleted)
                {
                    return threadPool.Submit(() => continuationFunction(result));
                }
                var newContinuationOfTask = new MyTask<TNewResult>(() => continuationFunction(result), threadPool);
                var action = newContinuationOfTask.Run;
                continuationOfTask.Enqueue(action);
                return newContinuationOfTask;
            }
        }

        public void Run()
        {
            lock (lockObj)
            {
                try
                {
                    result = function();
                    IsCompleted = true;
                    isCalculated.Set();
                    while (!continuationOfTask.IsEmpty)
                    {
                        if (continuationOfTask.TryDequeue(out var taskRun))
                        {
                            threadPool.actions.Add(taskRun);
                        }
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

            }
        }
    }
}
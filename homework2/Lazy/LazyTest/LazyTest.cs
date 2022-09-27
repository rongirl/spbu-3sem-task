using NUnit.Framework;
using Task_2;
using System.Threading;

namespace LazyTest;

public class Tests
{
    [Test]
    public void LazyGetTest()
    {
        int value = 0;
        Lazy<int> lazy = new Lazy<int>(() => ++value);
        for (int i = 0; i < 100; i++)
        {
            Assert.AreEqual(1, lazy.Get());
        }
    }

    [Test]
    public void LazyGetReturnNull()
    {
        Lazy<object> lazy = new Lazy<object>(() => null);
        Assert.IsNull(lazy.Get());
    }

    [Test]
    public void LazyWithThreadsGetReturnNull()
    {
        LazyWithThreads<object> lazy = new LazyWithThreads<object>(() => null);
        var threads = new Thread[System.Environment.ProcessorCount];
        for (var i = 0; i < threads.Length; ++i)
        {
            threads[i] = new Thread(() =>
            {
                Assert.IsNull(lazy.Get());

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
    }

    [Test]
    public void LazyWithThreadsGetTest()
    {
        int value = 1;
        var threads = new Thread[System.Environment.ProcessorCount];
        LazyWithThreads<int> lazy = new LazyWithThreads<int>(() => ++value);
        for (var i = 0; i < threads.Length; ++i)
        {
            threads[i] = new Thread(() =>
            {
                Assert.AreEqual(2, lazy.Get());
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
    }
}
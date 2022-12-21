using NUnit.Framework;
using test;
using System.Collections;

namespace ConcurrentPriorityQueueTest;

public class Tests
{
    private ConcurrentPriorityQueue<int, int> queue;

    [SetUp]
    public void Setup()
    {
        queue = new ConcurrentPriorityQueue<int, int>();
    }

    [Test]
    public void TestEnqueueAndDequeue()
    {
        queue.Enqueue(100, 1);
        queue.Enqueue(6, 200);
        queue.Enqueue(29293, 4);
        Assert.AreEqual(6, queue.Dequeue());
        Assert.AreEqual(29293, queue.Dequeue());
        Assert.AreEqual(100, queue.Dequeue());
    }

    [Test]
    public void SizeOfQueue()
    {
        Assert.AreEqual(0, queue.Size);
        queue.Enqueue(100, 1);
        Assert.AreEqual(1, queue.Size);
        queue.Enqueue(100, 1);
        queue.Enqueue(6, 200);
        queue.Enqueue(29293, 4);
        Assert.AreEqual(4, queue.Size);
    }

    [Test]
    public void QueueOfString()
    {
        ConcurrentPriorityQueue<string, int> queueOfStrings = new ConcurrentPriorityQueue<string, int>();
        queueOfStrings.Enqueue("aaaaa", 1);
        queueOfStrings.Enqueue("matan)", 200);
        queueOfStrings.Enqueue(":)", 4);
        Assert.AreEqual("matan)", queueOfStrings.Dequeue());
        Assert.AreEqual(":)", queueOfStrings.Dequeue());
        Assert.AreEqual("aaaaa", queueOfStrings.Dequeue());
    }
}
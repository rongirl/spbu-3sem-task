namespace Task_3.Test;

using NUnit.Framework;
using System;

public class Tests
{

     private MyThreadPool threadPool;
     private IMyTask<int>[] tasks;
     private const int countOfTasks = 10;
     [SetUp]
     public void Setup()
     {
         threadPool = new MyThreadPool(Environment.ProcessorCount);
         tasks = new IMyTask<int>[countOfTasks];
     }

     [Test]
     public void ResultTest()
     {
         for (int i = 0; i < tasks.Length; i++) 
         {
              var localI = i;
              tasks[i] = threadPool.Submit(() => localI);
         }
         for (int i = 0; i < tasks.Length;i++)
         {
             Assert.AreEqual(i, tasks[i].Result);
         }
        
     }

    [Test]
    public void ContinueWithTest()
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            var localI = i;
            tasks[i] = threadPool.Submit(() => localI).ContinueWith(x => x + 1);
        }
        for (int i = 0; i < tasks.Length; i++)
        {
            Assert.AreEqual(i + 1, tasks[i].Result);
        }
    }
    
    [Test]
    public void ResultAfterShutDown()
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            var localI = i;
            tasks[i] = threadPool.Submit(() => localI);
        }
        threadPool.ShutDown();
        for (int i = 0; i < tasks.Length; i++)
        {
            Assert.AreEqual(i, tasks[i].Result);
        }
    }

    [Test]
    public void SubmitAfterShutDown()
    {
        threadPool.ShutDown();
        Assert.Throws<InvalidOperationException>(() => threadPool.Submit(() => 1));
    }

    [Test]
    public void ContinueWithAfterShutDown()
    {
        var task = threadPool.Submit(() => 1);
        threadPool.ShutDown();
        Assert.Throws<InvalidOperationException>(() => task.ContinueWith(x => x + 1));
    }
  
}


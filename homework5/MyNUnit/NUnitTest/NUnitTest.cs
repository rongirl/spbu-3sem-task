using NUnit.Framework;
using System.Collections.Concurrent;
using System.Linq;

namespace MyNUnitSpace.Tests;

public class Tests
{
    private readonly ConcurrentQueue<TestInfo> testsInfo = new MyNUnit("../../../../TestProject").testsInfo;

    [Test]
    public void PassedTest()
    {
        var tests = testsInfo.Where(test => test.Name == "PassingTest");
        var testsException = testsInfo.Where(test => test.Name == "ExceptionTest");
        foreach (var test in tests)
        {
            Assert.AreEqual(test.Result, "Passed");
            Assert.AreEqual(test.ReasonShutdown, null);
        }
        foreach (var test in testsException)
        {
            Assert.AreEqual(test.Result, "Passed");
            Assert.AreEqual(test.ReasonShutdown, null);
        }
    }
    
    [Test]
    public void FailedTest()
    {
        var tests = testsInfo.Where(test => test.Name == "FailingTest");
        foreach (var test in tests)
        {
            Assert.AreEqual(test.Result, "Failed");
            Assert.AreEqual(test.ReasonShutdown, null);
        }
    }

    [Test]
    public void IgnoredTest()
    {
        var tests = testsInfo.Where(test => test.Name == "IgnoreTest");
        foreach (var test in tests)
        {
            Assert.AreEqual(test.Result, "Ignored");
            Assert.AreEqual(test.ReasonShutdown, "Test is ignored");
        }
    }

}

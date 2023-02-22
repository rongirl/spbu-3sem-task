namespace MyNUnitSpace;

using Attributes;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;

/// <summary>
/// NUnit class
/// </summary>
public class MyNUnit
{ 
    /// <summary>
    /// State of test
    /// </summary>
    private enum TestState
    {
        Ignored,
        Errored,
        Failed,
        Passed
    };

    /// <summary>
    /// Concurrent queue that stores information about tests
    /// </summary>
    public readonly ConcurrentQueue<TestInfo> testsInfo;

    public MyNUnit(string path)
    {
        var files = Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories);
        var filesWithoutRepeats = new Dictionary<string, string>();
        foreach (var file in files)
        {
            if (!filesWithoutRepeats.ContainsValue(Path.GetFileName(file)))
            {
                filesWithoutRepeats.Add(file, Path.GetFileName(file));
            }

        }
        var classes = filesWithoutRepeats.Keys
            .AsParallel()
            .Select(Assembly.LoadFrom)
            .SelectMany(a => a.ExportedTypes)
            .Where(t => t.IsClass);
        var classesWithTests =
            classes.Where(t => t.GetMethods().Any(m => m.GetCustomAttributes().Any(a => a is Test)));
        testsInfo = new ConcurrentQueue<TestInfo>();
        Parallel.ForEach(classesWithTests, StartTests);
    }

    /// <summary> 
    /// Starts tests for one class with test
    /// </summary>
    /// <param name="classWithTests">Class with test</param>
    private void StartTests(Type classWithTests)
    {
        RunAfterClassOrBeforeClass(classWithTests, typeof(BeforeClass));
        RunTests(classWithTests);
        RunAfterClassOrBeforeClass(classWithTests, typeof(AfterClass));
    }

    /// <summary>
    /// Run methods before or after all tests
    /// </summary>
    private void RunAfterClassOrBeforeClass(Type classWithTests, Type typeOfAttribute)
    {
        var methods = GetMethodsWithAttribute(classWithTests, typeOfAttribute);
        foreach (var method in methods)
        {
            if (!method.IsStatic)
            {
                throw new InvalidOperationException();
            }
            try
            {
                method.Invoke(null, null);
            }
            catch (TargetInvocationException ex)
            {
                throw new AggregateException(ex);
            }
        }
    }

    /// <summary>
    /// Run tests
    /// </summary>
    private void RunTests(Type classWithTests)
    {
        var methodsTests = GetMethodsWithAttribute(classWithTests, typeof(Test));
        Parallel.ForEach(methodsTests, test => RunTest(test, classWithTests));
    }

    /// <summary>
    /// Run one test
    /// </summary>
    private void RunTest(MethodInfo method, Type classWithTests)
    {
        var attribute = (Test?)Attribute.GetCustomAttribute(method, typeof(Test));
        if (attribute != null && attribute.Ignore != null)
        {
            testsInfo.Enqueue(new TestInfo(method.Name, 0, attribute.Ignore, TestState.Ignored.ToString()));
            return;
        }
        var instance = Activator.CreateInstance(classWithTests);
        try
        {
            RunAfterOrBefore(classWithTests, typeof(Before), instance);
        }
        catch (Exception e)
        {
            testsInfo.Enqueue(new TestInfo(
                method.Name, 0, e.Message, TestState.Errored.ToString()));
            return;
        }

        var stopWatch = new Stopwatch();
        try
        {
            stopWatch.Start();
            method.Invoke(instance, null);
            stopWatch.Stop();
            if (attribute != null && attribute.Expected != null)
            {
                testsInfo.Enqueue(new TestInfo(
                    method.Name, stopWatch.ElapsedMilliseconds, null, TestState.Failed.ToString()));
            }
            else
            {
                testsInfo.Enqueue(new TestInfo(
                    method.Name, stopWatch.ElapsedMilliseconds, null, TestState.Passed.ToString()));
            }
        }
        catch (Exception e)
        {
            stopWatch.Stop();
            if (attribute != null && e.InnerException != null && e.InnerException.GetType() != attribute.Expected)
            {
                testsInfo.Enqueue(new TestInfo(
                    method.Name, stopWatch.ElapsedMilliseconds, null, TestState.Failed.ToString()));
            }
            else
            {
                testsInfo.Enqueue(new TestInfo(
                    method.Name, stopWatch.ElapsedMilliseconds, null, TestState.Passed.ToString()));
            }
        }
        try
        {
            RunAfterOrBefore(classWithTests, typeof(After), instance);
        }
        catch (Exception e)
        {
            testsInfo.Enqueue(new TestInfo(
                method.Name, 0, e.Message, TestState.Errored.ToString()));
        }
    }

    /// <summary>
    /// Get methods with certain attribute
    /// </summary>
    private IEnumerable<MethodInfo> GetMethodsWithAttribute(Type classWithTests, Type typeOfAttribute)
    {
        return classWithTests.GetMethods().Where(m => m.GetCustomAttributes().Any(a => a.GetType() == typeOfAttribute));
    }

    /// <summary>
    /// Run methods before or after every test
    /// </summary>
    private void RunAfterOrBefore(Type classWithTests, Type typeOfAttribute, object? instance)
    {
        var methods = GetMethodsWithAttribute(classWithTests, typeOfAttribute);
        foreach (var method in methods)
        {
            method.Invoke(instance, null);
        }
    }
}
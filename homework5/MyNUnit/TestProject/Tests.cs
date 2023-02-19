using Attributes;

namespace TestProject;

public class Tests
{
    [BeforeClass]
    public static void BeforeClass()
    {

    }

    [Before]
    public static void Before()
    {

    }

    [Test]
    public static void PassingTest()
    {
        int a = 0;
        int b = 1;
        a += b;
    }

    [Test]
    public static void FailingTest()
    {
        throw new Exception();
    }

    [Test(Expected = typeof(InvalidOperationException))]
    public static void ExceptionTest()
    {
        throw new InvalidOperationException();
    }

    [Test(Ignore = "Test is ignored")]
    public static void IgnoreTest()
    {

    }

    [After]
    public static void AfterTest()
    {

    }

    [AfterClass]
    public static void AfterClassTest()
    {

    }
}
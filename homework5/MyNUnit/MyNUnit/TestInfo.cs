namespace MyNUnitSpace;

/// <summary>
/// Information of test
/// </summary>
public class TestInfo
{
    /// <summary>
    /// Name of test
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Test running time
    /// </summary>
    public long Time { get; } 

    /// <summary>
    /// Reason for shutdown the test
    /// </summary>
    public string? ReasonShutdown { get; }   

    /// <summary>
    /// Result of test
    /// </summary>
    public string? Result { get; }

    public TestInfo(string name, long time, string? reasonShutdown, string? result)
    {
        Name = name;
        Time = time;
        ReasonShutdown = reasonShutdown;
        Result = result;
    }
}
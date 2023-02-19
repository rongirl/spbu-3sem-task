using MyNUnitSpace;

if (args.Length != 1)
{
    Console.WriteLine("Enter the path");
    return;
}
if (!Directory.Exists(args[0]))
{
    Console.WriteLine("Directory doesn't exist");
    return;
}
var tests = new MyNUnit(args[0]);
foreach (var test in tests.testsInfo)
{
    Console.WriteLine($"{test.Name} {test.Time}ms {test.Result} {test.ReasonShutdown}");
}
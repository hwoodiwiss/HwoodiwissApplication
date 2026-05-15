using BenchmarkDotNet.Running;

namespace HwoodiwissApplication.Benchmarks;

public static class BenchmarkProgram
{
    public static void Main(string[] args)
    {
        BenchmarkSwitcher.FromAssembly(typeof(HwoodiwissApplicationBenchmarks).Assembly).Run(args);
    }
}

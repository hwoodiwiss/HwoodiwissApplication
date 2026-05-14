using BenchmarkDotNet.Running;

BenchmarkSwitcher.FromAssembly(typeof(HwoodiwissApplication.Benchmarks.HwoodiwissApplicationBenchmarks).Assembly).Run(args);

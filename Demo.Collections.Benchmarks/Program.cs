using System;
using BenchmarkDotNet.Running;

namespace Demo.Collections.Benchmarks
{
    public class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}

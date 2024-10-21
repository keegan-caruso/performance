// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using MicroBenchmarks;

namespace System.Linq.Tests
{
    public partial class Perf_Enumerable
    {
        [Benchmark]
        [BenchmarkCategory(Categories.ChainedSelects)]
        [ArgumentsSource(nameof(SelectArguments))]
        public void WhereSelectx1(LinqTestData input) => input.Collection.Where(i => i % 2 == 0).Select(i => i + 1).Consume(_consumer);

        [Benchmark]
        [BenchmarkCategory(Categories.ChainedSelects)]
        [ArgumentsSource(nameof(SelectArguments))]
        public void WhereSelectx2(LinqTestData input) => input.Collection.Where(i => i % 2 == 0).Select(i => i + 1).Select(i => i + 2).Consume(_consumer);

        [Benchmark]
        [BenchmarkCategory(Categories.ChainedSelects)]
        [ArgumentsSource(nameof(SelectArguments))]
        public void WhereSelectx3(LinqTestData input) => input.Collection.Where(i => i % 2 == 0).Select(i => i + 1).Select(i => i + 2).Select(i => i + 3).Consume(_consumer);

        [Benchmark]
        [BenchmarkCategory(Categories.ChainedSelects)]
        [ArgumentsSource(nameof(SelectArguments))]
        public void WhereSelectx4(LinqTestData input) => input.Collection.Where(i => i % 2 == 0).Select(i => i + 1).Select(i => i + 2).Select(i => i + 3).Select(i => i + 4).Consume(_consumer);

        [Benchmark]
        [BenchmarkCategory(Categories.ChainedSelects)]
        [ArgumentsSource(nameof(SelectArguments))]
        public void WhereSelectx5(LinqTestData input) => input.Collection.Where(i => i % 2 == 0).Select(i => i + 1).Select(i => i + 2).Select(i => i + 3).Select(i => i + 4).Select(i => i + 5).Consume(_consumer);
    }
}

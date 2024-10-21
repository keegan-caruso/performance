// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using MicroBenchmarks;

namespace System.Linq.Tests
{
    public class Perf_Enumerable_GVM
    {
        private readonly Consumer _consumer = new Consumer();

        private readonly List<string> _chainedLinqInput = Enumerable.Repeat("foo", 50).Concat(Enumerable.Repeat("bar", 50)).ToList();

        private enum FooBar
        {
            Foo,
            Bar
        }

        public IEnumerable<object> SelectArguments()
        {
            // Select() has 4 code paths: SelectEnumerableIterator, SelectArrayIterator, SelectListIterator, SelectIListIterator
            // https://github.com/dotnet/corefx/blob/dcf1c8f51bcdbd79e08cc672e327d50612690a25/src/System.Linq/src/System/Linq/Select.cs

            yield return LinqTestData.IEnumerable;
            yield return LinqTestData.Array;
            yield return LinqTestData.List;
            yield return LinqTestData.IList;
        }

        public IEnumerable<object> SelectRefArguments()
        {
            yield return LinqTestDataRefTypes.IEnumerable;
            yield return LinqTestDataRefTypes.Array;
            yield return LinqTestDataRefTypes.List;
            yield return LinqTestDataRefTypes.IList;
        }

        [Benchmark]
        [BenchmarkCategory(Categories.Libraries, Categories.ChainedSelects)]
        [ArgumentsSource(nameof(SelectArguments))]
        public void SelectValueType(LinqTestData input) => input.Collection.Select(i => i + 1).Consume(_consumer);


        [Benchmark]
        [BenchmarkCategory(Categories.Libraries, Categories.ChainedSelects)]
        [ArgumentsSource(nameof(SelectArguments))]
        public void WhereSelectx1(LinqTestData input) => input.Collection.Where(i => i % 2 == 0).Select(i => i + 1).Consume(_consumer);

        [Benchmark]
        [BenchmarkCategory(Categories.Libraries, Categories.ChainedSelects)]
        [ArgumentsSource(nameof(SelectArguments))]
        public void WhereSelectx2(LinqTestData input) => input.Collection.Where(i => i % 2 == 0).Select(i => i + 1).Select(i => i + 2).Consume(_consumer);

        [Benchmark]
        [BenchmarkCategory(Categories.Libraries, Categories.ChainedSelects)]
        [ArgumentsSource(nameof(SelectArguments))]
        public void WhereSelectx3(LinqTestData input) => input.Collection.Where(i => i % 2 == 0).Select(i => i + 1).Select(i => i + 2).Select(i => i + 3).Consume(_consumer);

        [Benchmark]
        [BenchmarkCategory(Categories.Libraries, Categories.ChainedSelects)]
        [ArgumentsSource(nameof(SelectArguments))]
        public void WhereSelectx4(LinqTestData input) => input.Collection.Where(i => i % 2 == 0).Select(i => i + 1).Select(i => i + 2).Select(i => i + 3).Select(i => i + 4).Consume(_consumer);

        [Benchmark]
        [BenchmarkCategory(Categories.Libraries, Categories.ChainedSelects)]
        [ArgumentsSource(nameof(SelectArguments))]
        public void WhereSelectx5(LinqTestData input) => input.Collection.Where(i => i % 2 == 0).Select(i => i + 1).Select(i => i + 2).Select(i => i + 3).Select(i => i + 4).Select(i => i + 5).Consume(_consumer);


        [Benchmark]
        [BenchmarkCategory(Categories.Libraries, Categories.ChainedSelects)]
        [ArgumentsSource(nameof(SelectRefArguments))]
        public void SelectRefTypes(LinqTestDataRefTypes input) => input.Collection
            .Select(i =>
            {
                i.Value += 1;
                return i;
            }).Consume(_consumer);

        [Benchmark]
        [BenchmarkCategory(Categories.Libraries, Categories.ChainedSelects)]
        [ArgumentsSource(nameof(SelectRefArguments))]
        public void WhereSelectx1RefTypes(LinqTestDataRefTypes input) => input.Collection
            .Where(i => i.Value % 2 == 0)
            .Select(i =>
            {
                i.Value += 1;
                return i;
            }).Consume(_consumer);

        [Benchmark]
        [BenchmarkCategory(Categories.Libraries, Categories.ChainedSelects)]
        [ArgumentsSource(nameof(SelectRefArguments))]
        public void MultipleSelectRefTypes(LinqTestDataRefTypes input) => input.Collection.
            Select(x =>
            {
                x.Value++;
                return x;
            }).Select(x => {
                x.Value++;
                return x;
            }).Select(x => {
                x.Value++;
                return x;
            }).Consume(_consumer);

        [Benchmark]
        [BenchmarkCategory(Categories.Libraries, Categories.ChainedSelects)]
        public int ChainedLinq()
        {
            return _chainedLinqInput.
                Select(x => x == "foo" ? FooBar.Foo : FooBar.Bar).
                Where(x => x == FooBar.Foo).
                Select(x => (int)x).
                Sum();
        }
    }
}

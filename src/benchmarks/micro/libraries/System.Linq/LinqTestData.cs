// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace System.Linq.Tests
{
    public class LinqTestDataRefTypes
    {
        private static class DataContainer
        {
            internal static TestObject[] ArrayOfObjects = new TestObject[100];

            static DataContainer()
            {
                for (int i = 0; i < 100; i++)
                {
                    ArrayOfObjects[i] = new TestObject { Value = i };
                }
            }
        }

        internal static readonly LinqTestDataRefTypes IEnumerable = new(new LinqTestData.IEnumerableWrapper<TestObject>(DataContainer.ArrayOfObjects));
        internal static readonly LinqTestDataRefTypes IList = new(new LinqTestData.IListWrapper<TestObject>(DataContainer.ArrayOfObjects));
        internal static readonly LinqTestDataRefTypes List = new (new List<TestObject>(DataContainer.ArrayOfObjects));
        internal static readonly LinqTestDataRefTypes Array = new (DataContainer.ArrayOfObjects);

        private LinqTestDataRefTypes(IEnumerable<TestObject> collection) => Collection = collection;

        internal IEnumerable<TestObject> Collection { get; }

        public override string ToString()
        {
            switch (Collection)
            {
                case TestObject[] _:
                    return "Array";
                case List<TestObject> _:
                    return "List";
                case IList<TestObject> _:
                    return "IList";
                case ICollection<TestObject> _:
                    return "ICollection";
                default:
                    return "IEnumerable";
            }
        }
    }

    internal class TestObject
    {
        public int Value { get; set; }
    }

    public class LinqTestData
    {
        // this field is a const (not instance field) to avoid creating closures in tested LINQ
        internal const int Size = 100;

        private static readonly int[] _arrayOf100Integers = Enumerable.Range(0, Size).ToArray();

        internal static readonly LinqTestData Array = new LinqTestData(_arrayOf100Integers);
        internal static readonly LinqTestData List = new LinqTestData(new List<int>(_arrayOf100Integers));
        internal static readonly LinqTestData Range = new LinqTestData(Enumerable.Range(0, Size));
        internal static readonly LinqTestData IEnumerable = new LinqTestData(new IEnumerableWrapper<int>(_arrayOf100Integers));
        internal static readonly LinqTestData IList = new LinqTestData(new IListWrapper<int>(_arrayOf100Integers));
        internal static readonly LinqTestData ICollection = new LinqTestData(new ICollectionWrapper<int>(_arrayOf100Integers));
        internal static readonly LinqTestData IOrderedEnumerable = new LinqTestData(_arrayOf100Integers.OrderBy(i => i)); // OrderBy() returns IOrderedEnumerable (OrderedEnumerable is internal)

        private LinqTestData(IEnumerable<int> collection) => Collection = collection;

        internal IEnumerable<int> Collection { get; }

        public override string ToString()
        {
            if (object.ReferenceEquals(this, Range)) // RangeIterator is a private type
                return "Range";

            switch (Collection)
            {
                case int[] _:
                    return "Array";
                case List<int> _:
                    return "List";
                case IList<int> _:
                    return "IList";
                case ICollection<int> _:
                    return "ICollection";
                case IOrderedEnumerable<int> _:
                    return "IOrderedEnumerable";
                default:
                    return "IEnumerable";
            }
        }

        internal class IEnumerableWrapper<T> : IEnumerable<T>
        {
            private readonly T[] _array;
            public IEnumerableWrapper(T[] array) => _array = array;

            public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_array).GetEnumerator();
            Collections.IEnumerator Collections.IEnumerable.GetEnumerator() => ((IEnumerable<T>)_array).GetEnumerator();
        }

        private class ICollectionWrapper<T> : ICollection<T>
        {
            private readonly T[] _array;
            public ICollectionWrapper(T[] array) => _array = array;

            public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_array).GetEnumerator();
            Collections.IEnumerator Collections.IEnumerable.GetEnumerator() => ((IEnumerable<T>)_array).GetEnumerator();

            public int Count => _array.Length;
            public bool IsReadOnly => true;
            public bool Contains(T item) => System.Array.IndexOf(_array, item) >= 0;
            public void CopyTo(T[] array, int arrayIndex) => _array.CopyTo(array, arrayIndex);

            public void Add(T item) => throw new NotImplementedException();
            public void Clear() => throw new NotImplementedException();
            public bool Remove(T item) => throw new NotImplementedException();
        }

        internal class IListWrapper<T> : IList<T>
        {
            private readonly T[] _array;
            public IListWrapper(T[] array) => _array = array;

            public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_array).GetEnumerator();
            Collections.IEnumerator Collections.IEnumerable.GetEnumerator() => ((IEnumerable<T>)_array).GetEnumerator();

            public int Count => _array.Length;
            public bool IsReadOnly => true;
            public T this[int index]
            {
                get { return _array[index]; }
                set { throw new NotImplementedException(); }
            }
            public bool Contains(T item) => System.Array.IndexOf(_array, item) >= 0;
            public void CopyTo(T[] array, int arrayIndex) => _array.CopyTo(array, arrayIndex);
            public int IndexOf(T item) => System.Array.IndexOf(_array, item);

            public void Add(T item) => throw new NotImplementedException();
            public void Clear() => throw new NotImplementedException();
            public bool Remove(T item) => throw new NotImplementedException();
            public void Insert(int index, T item) => throw new NotImplementedException();
            public void RemoveAt(int index) => throw new NotImplementedException();
        }
    }
}

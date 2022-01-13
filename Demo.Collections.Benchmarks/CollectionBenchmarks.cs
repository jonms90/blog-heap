using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable SwapViaDeconstruction

namespace Demo.Collections.Benchmarks
{
    public class CollectionBenchmarks
    {
        [Params(1000, 10000, 100000)]
        public int N;

        [Benchmark]
        public int ListExtractMinimumValue()
        {
            var random = new Random(1);
            var list = new List<int>();
            for (int i = 0; i < N; i++)
            {
                list.Add(random.Next(0, 50001));
            }

            int minValue = list.OrderBy(x => x).ToList().First();
            list.RemoveAt(0);
            return minValue;
        }

        [Benchmark]
        public int ListHeapExtractMinimumValue()
        {
            var random = new Random(1);
            var listHeap = new ListHeap<int>();
            for (int i = 0; i < N; i++)
            {
                listHeap.Insert(random.Next(0, 50001));
            }

            int minValue = listHeap.ExtractHead();
            return minValue;
        }

        [Benchmark]
        public int ArrayHeapExtractMinimumValue()
        {
            var random = new Random(1);
            var arrayHeap = new ArrayHeap<int>(N);
            for (int i = 0; i < N; i++)
            {
                arrayHeap.Insert(random.Next(0, 50001));
            }

            int minValue = arrayHeap.ExtractHead();
            return minValue;
        }

        public class ListHeap<T> where T : IComparable<T>
        {
            private readonly List<T> _data;

            public ListHeap()
            {
                _data = new List<T>();
            }

            public void Insert(T value)
            {
                _data.Add(value);
                int newIndex = _data.Count - 1;
                while (newIndex > 0 && _data[newIndex].CompareTo(_data[ParentIndex(newIndex)]) < 0)
                {
                    // Swap values
                    T temp = _data[newIndex];
                    _data[newIndex] = _data[ParentIndex(newIndex)];
                    _data[ParentIndex(newIndex)] = temp;

                    // Update current index
                    newIndex = ParentIndex(newIndex);
                }
            }

            public T ExtractHead()
            {
                if (_data.Count == 0)
                {
                    return default;
                }

                T extractedValue = _data[0];
                _data[0] = _data[_data.Count - 1];
                _data.RemoveAt(_data.Count - 1);
                int currentIndex = 0;
                while (true)
                {
                    int leftChildIndex = LeftChildIndex(currentIndex);
                    if (_data.Count - 1 < leftChildIndex)
                    {
                        break;
                    }

                    if (_data.Count - 1 == leftChildIndex) // only left child
                    {
                        if (_data[currentIndex].CompareTo(_data[leftChildIndex]) > 0)
                        {
                            T temp = _data[currentIndex];
                            _data[currentIndex] = _data[leftChildIndex];
                            _data[leftChildIndex] = temp;
                        }

                        break;
                    }

                    int rightChildIndex = RightChildIndex(currentIndex);
                    int smallestChild;
                    if (_data[leftChildIndex].CompareTo(_data[rightChildIndex]) < 0)
                    {
                        smallestChild = leftChildIndex;
                    }
                    else
                    {
                        smallestChild = rightChildIndex;
                    }

                    if (_data[currentIndex].CompareTo(_data[smallestChild]) > 0)
                    {
                        T temp = _data[currentIndex];
                        _data[currentIndex] = _data[smallestChild];
                        _data[smallestChild] = temp;
                    }

                    currentIndex = smallestChild;
                }
                return extractedValue;
            }

            private int ParentIndex(int index)
            {
                return (index - 1) / 2;
            }

            private int LeftChildIndex(int index)
            {
                return (index * 2) + 1;
            }

            private int RightChildIndex(int index)
            {
                return (index * 2) + 2;
            }
        }

        public class ArrayHeap<T> where T : IComparable<T>
        {
            private readonly T[] _data;
            private int _size;

            public ArrayHeap(int size)
            {
                _data = new T[size];
                _size = 0;
            }

            public void Insert(T value)
            {
                _data[_size] = value;
                _size++;
                int newIndex = _size - 1;
                while (newIndex > 0 && _data[newIndex].CompareTo(_data[ParentIndex(newIndex)]) < 0)
                {
                    // Swap values
                    T temp = _data[newIndex];
                    _data[newIndex] = _data[ParentIndex(newIndex)];
                    _data[ParentIndex(newIndex)] = temp;

                    // Update current index
                    newIndex = ParentIndex(newIndex);
                }
            }

            public T ExtractHead()
            {
                if (_size == 0)
                {
                    return default;
                }

                T extractedValue = _data[0];
                _data[0] = _data[_size - 1];
                _size--;
                int currentIndex = 0;
                while (true)
                {
                    int leftChildIndex = LeftChildIndex(currentIndex);
                    if (_size - 1 < leftChildIndex) // no children
                    {
                        break;
                    }

                    if (_size - 1 == leftChildIndex) // only left child
                    {
                        if (_data[currentIndex].CompareTo(_data[leftChildIndex]) > 0)
                        {
                            T temp = _data[currentIndex];
                            _data[currentIndex] = _data[leftChildIndex];
                            _data[leftChildIndex] = temp;
                        }

                        break;
                    }

                    int rightChildIndex = RightChildIndex(currentIndex);
                    int smallestChild;
                    if (_data[leftChildIndex].CompareTo(_data[rightChildIndex]) < 0)
                    {
                        smallestChild = leftChildIndex;
                    }
                    else
                    {
                        smallestChild = rightChildIndex;
                    }

                    if (_data[currentIndex].CompareTo(_data[smallestChild]) > 0)
                    {
                        T temp = _data[currentIndex];
                        _data[currentIndex] = _data[smallestChild];
                        _data[smallestChild] = temp;
                    }

                    currentIndex = smallestChild;
                }
                return extractedValue;
            }

            private static int ParentIndex(int index)
            {
                return (index - 1) / 2;
            }

            private static int LeftChildIndex(int index)
            {
                return (index * 2) + 1;
            }

            private static int RightChildIndex(int index)
            {
                return (index * 2) + 2;
            }
        }
    }
}

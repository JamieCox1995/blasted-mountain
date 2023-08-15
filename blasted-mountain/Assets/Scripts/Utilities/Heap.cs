using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Heap<T> where T : IHeapItem<T>
    {
        private T[] Items;
        public int CurrentCount;

        public Heap(int _MaximumnSize)
        {
            Items = new T[_MaximumnSize];
        }

        public void Add(T _Item)
        {
            _Item.HeapIndex = CurrentCount;
            Items[CurrentCount] = _Item;
            SortUp(_Item);
            CurrentCount++;
        }

        public T RemoveFirst()
        {
            T item = Items[0];
            CurrentCount--;

            Items[0] = Items[CurrentCount];
            Items[0].HeapIndex = 0;

            SortDown(Items[0]);

            return item;
        }

        public void UpdateItem(T _Item)
        {
            SortUp(_Item);
        }

        public int Count { get { return CurrentCount; } }

        public bool Contains(T _Item)
        {
            return Equals(Items[_Item.HeapIndex], _Item);
        }

        private void SortDown(T _Item)
        {
            int leftChildIndex;
            int rightChildIndex;
            int swapIndex;

            while (true)
            {
                leftChildIndex = _Item.HeapIndex * 2 + 1;
                rightChildIndex = _Item.HeapIndex * 2 + 2;
                swapIndex = 0;

                if (leftChildIndex < CurrentCount)
                {
                    swapIndex = leftChildIndex;

                    if (rightChildIndex < CurrentCount)
                    {
                        if (Items[leftChildIndex].CompareTo(Items[rightChildIndex]) < 0)
                        {
                            swapIndex = rightChildIndex;
                        }
                    }

                    if (_Item.CompareTo(Items[swapIndex]) < 0)
                    {
                        Swap(_Item, Items[swapIndex]);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private void SortUp(T _Item)
        {
            int parentIndex = (_Item.HeapIndex - 1) / 2;

            while (true)
            {
                T parent = Items[parentIndex];

                if (_Item.CompareTo(parent) > 0)
                {
                    Swap(_Item, parent);
                }
                else
                {
                    break;
                }

                parentIndex = (_Item.HeapIndex - 1) / 2;
            }
        }

        private void Swap(T _ItemA, T _ItemB)
        {
            Items[_ItemA.HeapIndex] = _ItemB;
            Items[_ItemB.HeapIndex] = _ItemA;

            int indexA = _ItemA.HeapIndex;

            _ItemA.HeapIndex = _ItemB.HeapIndex;
            _ItemB.HeapIndex = indexA;

        }
    }

    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex { get; set; }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{
    private T[] items;
    
    public int Count { get; private set; }

    public Heap(int maxSize)
    {
        items = new T[maxSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = Count;
        items[Count] = item;
        SortUp(item);
        Count++;
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        Count--;
        items[0] = items[Count];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(T item)
    {
        // assumption: item priority is only ever updated to be higher than before
        SortUp(item);
    }

    public bool Contains(T item)
    {
        return Equals(item, items[item.HeapIndex]);
    }

    private void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        T parentItem;
        while (true)
        {
            parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                // item has higher priority than parent item
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    private void SortDown(T item)
    {
        int childIndexLeft, childIndexRight, swapIndex;
        while (true)
        {
            childIndexLeft = item.HeapIndex * 2 + 1;
            childIndexRight = item.HeapIndex * 2 + 2;

            if (childIndexLeft >= Count)
            {
                // item has no children
                return;
            }

            // item has left child
            swapIndex = childIndexLeft;

            // check if item has right child with higher priority than left child
            if (childIndexRight < Count
                && items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
            {
                // item has right child with a higher priority than left child
                swapIndex = childIndexRight;
            }

            if (item.CompareTo(items[swapIndex]) < 0)
            {
                // item at swap index has higher priority than item being sorted
                Swap(item, items[swapIndex]);
            }
            else
            {
                // parent (item being sorted) has higher priority than both children
                return;
            }
        }
    }

    private void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;

        int oldItemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = oldItemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    public int HeapIndex { get; set; }
}

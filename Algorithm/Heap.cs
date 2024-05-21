using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.heapIndex = currItemCount;
        items[currItemCount] = item;
        SortUp(item);
        currItemCount++;
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        currItemCount--;
        items[0] = items[currItemCount];
        items[0].heapIndex = 0;
        SortDown(items[0]);

        return firstItem;
    }

    public int Count
    {
        get
        {
            return currItemCount;
        }
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public bool Contains(T item)
    {
        return Equals(items[item.heapIndex], item);
    }

    void SortDown(T item)
    {
        while (true)
        {
            int leftItem = 2 * item.heapIndex + 1;
            int rightItem = 2 * item.heapIndex + 2;
            int swapIndex = 0;

            if (leftItem < currItemCount)
            {
                swapIndex = leftItem;

                if (rightItem < currItemCount)
                {
                    if (items[leftItem].CompareTo(items[rightItem]) < 0)
                    {
                        swapIndex = rightItem;
                    }
                }

                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
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

    void SortUp(T item)
    {
        int parentIndex = (item.heapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.heapIndex - 1) / 2;
            if (parentIndex == item.heapIndex)
            {
                break;
            }
        }
    }

    void Swap(T itemA, T itemB)
    {
        items[itemA.heapIndex] = itemB;
        items[itemB.heapIndex] = itemA;
        int temp = itemA.heapIndex;
        itemA.heapIndex = itemB.heapIndex;
        itemB.heapIndex = temp;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int heapIndex
    {
        get;
        set;
    }
}
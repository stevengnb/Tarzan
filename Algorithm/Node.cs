    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Quadrant { 
    North,
    West,
    East,
    South,
    Mid,
    Line
}

public class Node : IHeapItem<Node>
{
    // A Star
    public bool pass;
    public bool marked;
    public Vector3 worldPos;
    public int gridX;
    public int gridY;
    public int gCost;
    public int hCost;
    public Node parent;
    int heapIndexx;
    public Quadrant area;
    public bool randomPoint = false;
    public bool isPath = false;
    public bool isPathOptimized = false;

    // WFC
    public List<int> notRestricted;
    public bool isVisitedWFC = false;

    public Node(bool passs, Vector3 worldPostition, int x, int y)
    {
        pass = passs;
        worldPos = worldPostition;
        gridX = x;
        gridY = y;
        notRestricted = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
    }

    public void removePossibility(List<int> toRemove)
    {
        foreach(int a in toRemove)
        {
            notRestricted.Remove(a);
        }
    }

    public int fCost
    {
        get { return gCost + hCost; }
    }

    public int heapIndex
    {
        get
        {
            return heapIndexx;
        }
        set
        {
            heapIndexx = value;
        }
    }

    public int CompareTo(Node node)
    {
        int compare = fCost.CompareTo(node.fCost);

        if (compare == 0)
        {
            compare = hCost.CompareTo(node.hCost);
        }

        return -compare;
    }
}

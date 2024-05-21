using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour
{
    private Grid grid;
    private PathCreator pc;
    private PathfindingOptimized opt;

    public void Start()
    {
        grid = GetComponent<Grid>();
        pc = GetComponent<PathCreator>();
        opt = GetComponent<PathfindingOptimized>();

        // cari path sebelum optimized
        WestPath();
        EastPath();
        SouthPath();
        NorthPath();

        // create optimize path
        opt.Started(grid.startingPoint[0], grid.middlePoint, grid.westPath, grid.simplifiedWest, 1);
        opt.Started(grid.startingPoint[1], grid.middlePoint, grid.northPath, grid.simplifiedNorth, 3);
        opt.Started(grid.startingPoint[2], grid.middlePoint, grid.eastPath, grid.simplifiedEast, 0);
        opt.Started(grid.startingPoint[3], grid.middlePoint, grid.southPath, grid.simplifiedSouth, 2);

        // create path fisik
        pc.CreatePath(grid.westPath);
        pc.CreatePath(grid.northPath);
        pc.CreatePath(grid.eastPath);
        pc.CreatePath(grid.southPath);    
    }

    public void WestPath()
    {
        Node start = grid.startingPoint[0];
        List<Node> west = grid.westRp;

        while (west.Count > 0)
        {
            int minDistance = int.MaxValue;
            Node closest = null;

            for (int i = 0; i < west.Count; i++)
            {
                int distance = getDistance(start, west[i]);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = west[i];
                }
            }

            findPath(start.worldPos, closest.worldPos);
            west.Remove(start);
            start = closest;
        }

        findPath(start.worldPos, grid.middlePoint.worldPos);
    }

    public void EastPath()
    {
        Node start = grid.startingPoint[2];
        List<Node> east = grid.eastRp;

        while (east.Count > 0)
        {
            int minDistance = int.MaxValue;
            Node closest = null;

            for (int i = 0; i < east.Count; i++)
            {
                int distance = getDistance(start, east[i]);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = east[i];
                }
            }

            findPath(start.worldPos, closest.worldPos);
            east.Remove(start);
            start = closest;
        }

        findPath(start.worldPos, grid.middlePoint.worldPos);
    }

    public void SouthPath()
    {
        Node start = grid.startingPoint[3];
        List<Node> south = grid.southRp;

        while (south.Count > 0)
        {
            int minDistance = int.MaxValue;
            Node closest = null;

            for (int i = 0; i < south.Count; i++)
            {
                int distance = getDistance(start, south[i]);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = south[i];
                }
            }

            findPath(start.worldPos, closest.worldPos);
            south.Remove(start);
            start = closest;
        }

        findPath(start.worldPos, grid.middlePoint.worldPos);
    }

    public void NorthPath()
    {
        Node start = grid.startingPoint[1];
        List<Node> north = grid.northRp;

        while (north.Count > 0)
        {
            int minDistance = int.MaxValue;
            Node closest = null;

            for (int i = 0; i < north.Count; i++)
            {
                int distance = getDistance(start, north[i]);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = north[i];
                }
            }

            findPath(start.worldPos, closest.worldPos);
            north.Remove(start);
            start = closest;
        }

        findPath(start.worldPos, grid.middlePoint.worldPos);
    }

    public void findPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch st = new Stopwatch();
        st.Start();
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if (targetNode.pass)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closeSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currNode = openSet.RemoveFirst();
                closeSet.Add(currNode);

                if (currNode == targetNode)
                {
                    st.Stop();
                    pathSuccess = true;
                    break;
                }

                foreach (Node n in grid.GetNeighbours(currNode))
                {
                    if (!n.pass || closeSet.Contains(n))
                    {
                        continue;
                    }

                    int movementCostToNeighbour = currNode.gCost + getDistance(currNode, n);
                    if (movementCostToNeighbour < n.gCost || !openSet.Contains(n))
                    {
                        n.gCost = movementCostToNeighbour;
                        n.hCost = getDistance(n, targetNode);
                        n.parent = currNode;

                        if (!openSet.Contains(n))
                        {
                            openSet.Add(n);
                        }
                        else
                        {
                            openSet.UpdateItem(n);
                        }
                    }
                }
            }
        }

        if (pathSuccess)
        {
            waypoints = retracePath(startNode, targetNode);
        }
    }

    Vector3[] retracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node currNode = end;

        while (currNode != start)
        {
            path.Add(currNode);
            currNode.isPath = true;
            currNode = currNode.parent;
        }

        Vector3[] waypoints = simplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] simplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPos);
            }
            directionOld = directionNew;
        }

        return waypoints.ToArray();
    }

    public int getDistance(Node a, Node b)
    {
        int distanceX = Mathf.Abs(a.gridX - b.gridX);
        int distanceY = Mathf.Abs(b.gridY - a.gridY);

        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        else
        {
            return 14 * distanceX + 10 * (distanceY - distanceX);
        }
    }
}

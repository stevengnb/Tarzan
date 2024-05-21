using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class PathfindingOptimized : MonoBehaviour
{
    private Grid grid;

    public void Started(Node start, Node end, List<Node> quad, Vector3[] sPath, int index)
    {
        grid = GetComponent<Grid>();
        findPath(start.worldPos, end.worldPos, quad, index);
    }

    public void findPath(Vector3 startPos, Vector3 targetPos, List<Node> quad, int index)
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
                    if (!n.pass || closeSet.Contains(n) || !n.isPath)
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
            waypoints = retracePath(startNode, targetNode, quad);

            if(index == 0)
            {
                grid.simplifiedEast = waypoints;
            } else if(index == 1)
            {
                grid.simplifiedWest = waypoints;
            } else if(index == 2)
            {
                grid.simplifiedSouth = waypoints;

            } else if(index == 3)
            {
                grid.simplifiedNorth = waypoints;
            }
        }
    }

    Vector3[] retracePath(Node start, Node end, List<Node> quad)
    {
        List<Node> path = new List<Node>();
        Node currNode = end;

        while (currNode != start)
        {
            path.Add(currNode);
            quad.Add(currNode);
            currNode.isPathOptimized = true;
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

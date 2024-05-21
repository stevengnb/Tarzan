using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public static Grid instance { get; private set; }
    public bool displayGridGizmos;
    public LayerMask unpassableMask;
    public Vector2 gridSize;
    public float nodeRadius;
    public Node[,] grid;
    float diameter;
    public int gridSizeX, gridSizeY;
    public List<Node> startingPoint = new List<Node>();
    public List<Node> eastRp = new List<Node>();
    public List<Node> westRp = new List<Node>();
    public List<Node> southRp = new List<Node>();
    public List<Node> northRp = new List<Node>();

    public List<Node> eastPath = new List<Node>();
    public List<Node> westPath = new List<Node>();
    public List<Node> southPath = new List<Node>();
    public List<Node> northPath = new List<Node>();

    public Vector3[] simplifiedEast;
    public Vector3[] simplifiedWest;
    public Vector3[] simplifiedNorth;
    public Vector3[] simplifiedSouth;

    public Node middlePoint;

    private void Awake()
    {
        if (instance == null)
        {
            diameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridSize.x / diameter);
            gridSizeY = Mathf.RoundToInt(gridSize.y / diameter);
            createGrid();
            middlePoint = grid[gridSizeX / 2, gridSizeY / 2];
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    public void createGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBotomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.forward * gridSize.y / 2;

        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                Vector3 worldPoint = worldBotomLeft + Vector3.right * (i * diameter + nodeRadius) + Vector3.forward * (j * diameter + nodeRadius);
                worldPoint.y = Terrain.activeTerrain.SampleHeight(worldPoint);
                bool pass = !(Physics.CheckSphere(worldPoint, nodeRadius, unpassableMask));
                Node newNode = new Node(pass, worldPoint, i, j);

                if (i >= gridSizeX / 2 - 7 && i <= gridSizeX / 2 + 7 && j >= gridSizeY / 2 - 7 && j <= gridSizeY / 2 + 7)
                {
                    newNode.area = Quadrant.Mid;
                } else if (i < j && (i + j > gridSizeX))
                {
                    newNode.area = Quadrant.South;
                }
                else if (i < j && (i + j < gridSizeY))
                {
                    newNode.area = Quadrant.West;
                }
                else if (i > j && (i + j > gridSizeY))
                {
                    newNode.area = Quadrant.East;
                }
                else if (i > j && (i + j < gridSizeX))
                {
                    newNode.area = Quadrant.North;
                } else
                {
                    newNode.area = Quadrant.Line;
                    newNode.pass = false;
                }

                if (Mathf.Abs(i - j) <= 2 || (gridSizeX - 4 <= i + j && i + j <= gridSizeX - 1))
                {
                    if (i >= gridSizeX / 2 - 7 && i <= gridSizeX / 2 + 7 && j >= gridSizeY / 2 - 7 && j <= gridSizeY / 2 + 7)
                    {
                        newNode.area = Quadrant.Mid;
                    }
                    else
                    {
                        newNode.pass = false;
                    }
                }
                grid[i, j] = newNode;
            }
        }

        EastPoint();
        WestPoint();
        NorthPoint();
        SouthPoint();
        StartPoint();
    }

    public void StartPoint()
    {
        // west
        startingPoint.Add(grid[0, gridSizeX / 2]);

        // north
        startingPoint.Add(grid[gridSizeY / 2, 0]);

        // east
        startingPoint.Add(grid[gridSizeX - 1, gridSizeY / 2]);

        // south
        startingPoint.Add(grid[gridSizeX / 2, gridSizeY - 1]);
    }

    public void EastPoint()
    {
        Node markedNode;
        int x;
        int y;

        for(int i = 0; i < 4; i++)
        {
            while(true)
            {
                x = Random.Range(0, gridSizeX);
                y = Random.Range(0, gridSizeY);

                if(grid[x, y].randomPoint == false && grid[x, y].area == Quadrant.East && grid[x, y].pass == true)
                {
                    break;
                }
            }

            markedNode = grid[x, y];
            markedNode.randomPoint = true;
            eastRp.Add(markedNode);

        }
    }

    public void WestPoint()
    {
        Node markedNode;
        int x;
        int y;

        for (int i = 0; i < 4; i++)
        {
            while (true)
            {
                x = Random.Range(0, gridSizeX);
                y = Random.Range(0, gridSizeY);

                if (grid[x, y].randomPoint == false && grid[x, y].area == Quadrant.West && grid[x, y].pass == true)
                {
                    break;
                }
            }

            markedNode = grid[x, y];
            markedNode.randomPoint = true;
            westRp.Add(markedNode);
        }
    }

    public void NorthPoint()
    {
        Node markedNode;
        int x;
        int y;

        for (int i = 0; i < 4; i++)
        {
            while (true)
            {
                x = Random.Range(0, gridSizeX);
                y = Random.Range(0, gridSizeY);

                if (grid[x, y].randomPoint == false && grid[x, y].area == Quadrant.North && grid[x, y].pass == true)
                {
                    break;
                }
            }

            markedNode = grid[x, y];
            markedNode.randomPoint = true;
            northRp.Add(markedNode);
        }
    }

    public void SouthPoint()
    {
        Node markedNode;
        int x;
        int y;

        for (int i = 0; i < 4; i++)
        {
            while (true)
            {
                x = Random.Range(0, gridSizeX);
                y = Random.Range(0, gridSizeY);

                if (grid[x, y].randomPoint == false && grid[x, y].area == Quadrant.South && grid[x, y].pass == true)
                {
                    break;
                }
            }

            markedNode = grid[x, y];
            markedNode.randomPoint = true;
            southRp.Add(markedNode);
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }
                int checkX = node.gridX + i;
                int checkY = node.gridY + j;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        Vector3 localPosition = worldPosition - transform.position;
        float percentX = Mathf.Clamp01(localPosition.x / gridSize.x + 0.5f);
        float percentY = Mathf.Clamp01(localPosition.z / gridSize.y + 0.5f);

        int x = Mathf.FloorToInt(Mathf.Clamp01(percentX) * (gridSizeX - 1));
        int y = Mathf.FloorToInt(Mathf.Clamp01(percentY) * (gridSizeY - 1));
        return grid[x, y];
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));

        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                if (n.randomPoint || n == middlePoint)
                {
                    Gizmos.color = Color.black;
                } else
                {
                    if (n.pass)
                    {
                        //if(n.area == Quadrant.North)
                        //{
                        //    Gizmos.color = Color.cyan;
                        //} else if(n.area == Quadrant.West)
                        //{
                        //    Gizmos.color = Color.grey;
                        //} else if(n.area == Quadrant.East)
                        //{
                        //    Gizmos.color = Color.white;
                        //} else if(n.area == Quadrant.South)
                        //{
                        //    Gizmos.color = Color.gray;
                        //} else if(n.area == Quadrant.Mid)
                        //{
                        //    Gizmos.color = Color.green;
                        //}
                        Gizmos.color = Color.white;
                        if(n.area == Quadrant.Mid)
                        {
                            Gizmos.color = Color.green;
                        }
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                    }
                }

                if(n.isPath && !n.randomPoint)
                {
                    Gizmos.color = Color.yellow;
                }

                if(n.isPathOptimized)
                {
                    Gizmos.color = Color.magenta;
                }
                Gizmos.DrawCube(n.worldPos, Vector3.one * (diameter - 0.1f));
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFunctionCollapse : MonoBehaviour
{
    Grid gridBox;
    Node[,] nodes;
    [SerializeField] private GameObject wfcPrefabParent;
    [SerializeField] private List<GameObject> wfcPrefab = new List<GameObject>();

    private void Start()
    {
        gridBox = Grid.instance;
        nodes = gridBox.grid;

        int startX = 0;
        int startY = 0;

        DFS(startX, startY);
        DFS(gridBox.gridSizeX - 1, gridBox.gridSizeY - 1);
    }



    private void DFS(int startX, int startY)
    {
        // ambil panjang untuk baris dan kolom serta inisialisasi
        int rows = nodes.GetLength(0);
        //rows = 5;
        int cols = nodes.GetLength(1);
        //cols = 5;

        int index;
        float randomRotateY;
        Quaternion randomRotation;
        GameObject newWfcPrefab;

        // create sebuah stack untuk tampung node-node yang perlu divisit nantinya
        Stack<Node> stack = new Stack<Node>();

        // masukin titik pertama ke dalam stack
        stack.Push(nodes[startX, startY]);

        // Create a 2D array to keep track of visited nodes
        //bool[,] visited = new bool[rows, cols];

        while (stack.Count > 0)
        {
            // remove node paling atas dari stack
            Node curr = stack.Pop();
            int x = curr.gridX;
            int y = curr.gridY;

            // mengecek apakah diluar size grid, telah dikunjungi, merupakan path,
            // merupakan random point, ataupun merupakan node yang tidak walkable
            //Debug.Log("x = " + x + ", y = " + y);
            //Debug.Log("udah visited wfc = " + curr.isVisitedWFC);
            //Debug.Log("bisa lewat ga = " + curr.pass);
            //Debug.Log("random point bukan = " + curr.randomPoint);
            //Debug.Log("path yang optimized bukan = " + curr.isPathOptimized);
            // || !curr.pass
            //|| (!curr.pass && (x != (gridBox.gridSizeX - 1)) && (y != (gridBox.gridSizeY - 1)))
            // || (!curr.pass && x != 0 && y != 0)
            if (x < 0 || x >= rows || y < 0 || y >= cols || curr.isVisitedWFC || curr.isPathOptimized || curr.randomPoint || curr.area == Quadrant.Mid)
            {
                continue;
            }

            // menandakan bahwa node telah dikunjungi
            curr.isVisitedWFC = true;
            randomRotateY = Random.Range(0f, 360f);
            randomRotation = Quaternion.Euler(0f, randomRotateY, 0f);
            //GameObject newPathObject = Instantiate(prefabPath, path[i].worldPos, randomRotation, parentPath.transform);
            index = Random.Range(0, curr.notRestricted.Count - 1);
            //Debug.Log("random dari 0 sampai " + (curr.notRestricted.Count - 1) + " hasil nya = " + index);

            if(curr.notRestricted.Count != 0) {
                //Debug.Log("index asli = " + curr.notRestricted[index]);
                newWfcPrefab = Instantiate(wfcPrefab[curr.notRestricted[index]], curr.worldPos, randomRotation, wfcPrefabParent.transform);
            } else
            {
                newWfcPrefab = Instantiate(wfcPrefab[0], curr.worldPos, randomRotation, wfcPrefabParent.transform);
                //Debug.Log("countnya 0");
            }
            //Debug.Log(newWfcPrefab.name);

            //Debug.Log("Visiting node (" + x + ", " + y + ")");

            // push setiap neighbour ke stack dan kasih limit restriction untuk setiap neighbour
            if (x > 0)
            {
                nodes[x - 1, y].removePossibility(newWfcPrefab.GetComponent<ObjectRestrictions>().siblingsRestrictions);
                stack.Push(nodes[x - 1, y]); // Left
            }

            if (x < rows - 1)
            {
                nodes[x + 1, y].removePossibility(newWfcPrefab.GetComponent<ObjectRestrictions>().siblingsRestrictions);
                stack.Push(nodes[x + 1, y]); // Right
            }

            if (y > 0)
            {
                nodes[x, y - 1].removePossibility(newWfcPrefab.GetComponent<ObjectRestrictions>().siblingsRestrictions);
                stack.Push(nodes[x, y - 1]); // Down
            }

            if (y < cols - 1)
            {
                nodes[x, y + 1].removePossibility(newWfcPrefab.GetComponent<ObjectRestrictions>().siblingsRestrictions);
                stack.Push(nodes[x, y + 1]); // Up
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    [SerializeField] private GameObject prefabPath;
    [SerializeField] private GameObject parentPath;
    
    public void CreatePath(List<Node> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            float randomRotateY = Random.Range(0f, 360f);
            Quaternion randomRotation = Quaternion.Euler(0f, randomRotateY, 0f);
            GameObject newPathObject = Instantiate(prefabPath, path[i].worldPos, randomRotation, parentPath.transform);
        }
    }
}


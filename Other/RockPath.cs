using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockRoad : MonoBehaviour
{
    private void Start()
    {
        float positionY = Terrain.activeTerrain.SampleHeight(new Vector3(transform.position.x, transform.position.y, transform.position.z));
        transform.position = new Vector3(transform.position.x, positionY, transform.position.z);
    }

}

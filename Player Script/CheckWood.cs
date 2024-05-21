using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWood : MonoBehaviour
{
    [SerializeField] private GameObject cr;
    private PlayerSound ps;

    private void Start()
    {
        ps = cr.GetComponent<PlayerSound>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == cr)
        {
            ps.isWood = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        ps.isWood = false;
    }
}

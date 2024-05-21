using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckStone : MonoBehaviour
{
    [SerializeField] private GameObject cr;
    private PlayerSound ps;

    private void Start()
    {
        ps = cr.GetComponent<PlayerSound>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject == cr)
        {
            ps.isStone = true;
            AudioManagerGame.instance.ChangeCave();
        }
    }

    private void OnTriggerExit(Collider col)
    {
        ps.isStone = false;
        AudioManagerGame.instance.ChangeNormal();
    }
}

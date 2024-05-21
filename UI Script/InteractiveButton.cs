using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveButton : MonoBehaviour
{
    [SerializeField] private EnemyController ec;
    private NpcInteract npci;

    private void Start()
    {
        npci = GameObject.Find("NPC").GetComponent<NpcInteract>();
    }

    public void Yes()
    {
        TowerDefense.instance.SwitchDefense();
        TowerDefense.instance.isChanging = false;
        ec.SpawnEnemyTD();
        npci.StopInteract();
        
    }

    public void No()
    {
        npci.StopInteract();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonHead : MonoBehaviour
{
    private bool isPunchingg = true;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (isPunchingg)
            {
                if(PlayerStats.instance.Level == 1)
                {
                    PlayerStats.instance.PlayerHit(PlayerStats.instance.getHealth() / 2);
                } else
                {
                    PlayerStats.instance.PlayerHit(20);
                }
                isPunchingg = false;
                StartCoroutine(isPunchingTrue(0.5f));
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (isPunchingg)
            {
                if (PlayerStats.instance.Level == 1)
                {
                    PlayerStats.instance.PlayerHit(PlayerStats.instance.getHealth() / 2);
                }
                else
                {
                    PlayerStats.instance.PlayerHit(20);
                }
                isPunchingg = false;
                StartCoroutine(isPunchingTrue(0.5f));
            }
        }
    }

    private IEnumerator isPunchingTrue(float duration)
    {
        yield return new WaitForSeconds(duration);
        isPunchingg = true;
    }
}

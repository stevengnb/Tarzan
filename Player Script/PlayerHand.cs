using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    private bool isPunching = true;

    private IEnumerator PunchTrue(float duration)
    {
        yield return new WaitForSeconds(duration);
        isPunching = true;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Animal")
        {
            Animal enemy = col.gameObject.GetComponent<Animal>();
            if (enemy == null)
            {
                enemy = col.gameObject.GetComponentInParent<Animal>();
            }

            if (isPunching)
            {
                Debug.Log("dia punching");
                if (!enemy.isPet)
                {
                    enemy.TakeDamage(PlayerStats.instance.PunchAttack);
                    isPunching = false;
                    StartCoroutine(PunchTrue(0.5f));
                }
                    
            }
        } else if(col.gameObject.tag == "Dragon")
        {
            Dragon dragon = col.gameObject.GetComponent<Dragon>();
            if (dragon == null)
            {
                dragon = col.gameObject.GetComponentInParent<Dragon>();
            }

            if (isPunching)
            {
                dragon.TakeDamage(PlayerStats.instance.PunchAttack);
                isPunching = false;
                StartCoroutine(PunchTrue(0.5f));
            }
        } else if(col.gameObject.tag == "EnemyTD")
        {
            TowerAttacker ta = col.gameObject.GetComponent<TowerAttacker>();
            if (ta == null)
            {
                ta = col.gameObject.GetComponentInParent<TowerAttacker>();
            }

            if (isPunching)
            {
                ta.TakeDamage(PlayerStats.instance.PunchAttack);
                isPunching = false;
                StartCoroutine(PunchTrue(0.5f));
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Animal")
        {
            Animal enemy = col.gameObject.GetComponent<Animal>();
            if (enemy == null)
            {
                enemy = col.gameObject.GetComponentInParent<Animal>();
            }

            if (isPunching)
            {
                Debug.Log("enemynya pet bukan = " + enemy.isPet);
                if (!enemy.isPet)
                {
                    enemy.TakeDamage(PlayerStats.instance.PunchAttack);
                    isPunching = false;
                    StartCoroutine(PunchTrue(0.5f));
                }

            }
        } else if (col.gameObject.tag == "Dragon")
        {
            Dragon dragon = col.gameObject.GetComponent<Dragon>();
            if(dragon == null)
            {
                dragon = col.gameObject.GetComponentInParent<Dragon>();
            }

            if (isPunching)
            {
                dragon.TakeDamage(PlayerStats.instance.PunchAttack);
                isPunching = false;
                StartCoroutine(PunchTrue(0.5f));
            }
        } else if (col.gameObject.tag == "EnemyTD")
        {
            TowerAttacker ta = col.gameObject.GetComponent<TowerAttacker>();
            if (ta == null)
            {
                ta = col.gameObject.GetComponentInParent<TowerAttacker>();
            }

            if (isPunching)
            {
                ta.TakeDamage(PlayerStats.instance.PunchAttack);
                isPunching = false;
                StartCoroutine(PunchTrue(0.5f));
            }
        }
    }
}

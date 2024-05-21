using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalHand : MonoBehaviour
{
    private bool isPunchingg = true;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (isPunchingg)
            {
                PlayerStats.instance.PlayerHit(10);
                isPunchingg = false;
                StartCoroutine(isPunchingTrue(0.5f));
            }
        }
        
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(col.gameObject.tag == "Dragon")
            {
                Dragon enemy = col.gameObject.GetComponent<Dragon>();
                if (enemy == null)
                {
                    enemy = col.gameObject.GetComponentInParent<Dragon>();
                }

                if (isPunchingg)
                {
                    enemy.TakeDamage(10);
                    isPunchingg = false;
                    StartCoroutine(isPunchingTrue(0.5f));
                }
            } else if(col.gameObject.tag =="Animal")
            {
                Animal enemy = col.gameObject.GetComponent<Animal>();
                if (enemy == null)
                {
                    enemy = col.gameObject.GetComponentInParent<Animal>();
                }

                if (isPunchingg)
                {
                    if (!enemy.isPet)
                    {
                        enemy.TakeDamage(10);
                        isPunchingg = false;
                        StartCoroutine(isPunchingTrue(0.5f));
                    }
                }
            } else if(col.gameObject.tag == "EnemyTD")
            {
                TowerAttacker enemy = col.gameObject.GetComponent<TowerAttacker>();
                if (enemy == null)
                {
                    enemy = col.gameObject.GetComponentInParent<TowerAttacker>();
                }

                if (isPunchingg)
                {
                    enemy.TakeDamage(10);
                    isPunchingg = false;
                    StartCoroutine(isPunchingTrue(0.5f));
                }
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (isPunchingg)
            {
                PlayerStats.instance.PlayerHit(10);
                isPunchingg = false;
                StartCoroutine(isPunchingTrue(0.5f));
            }
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (col.gameObject.tag == "Dragon")
            {
                Dragon enemy = col.gameObject.GetComponent<Dragon>();
                if (enemy == null)
                {
                    enemy = col.gameObject.GetComponentInParent<Dragon>();
                }

                if (isPunchingg)
                {
                    enemy.TakeDamage(10);
                    isPunchingg = false;
                    StartCoroutine(isPunchingTrue(0.5f));
                }
            }
            else if (col.gameObject.tag == "Animal")
            {
                Animal enemy = col.gameObject.GetComponent<Animal>();
                if (enemy == null)
                {
                    enemy = col.gameObject.GetComponentInParent<Animal>();
                }

                if (isPunchingg)
                {
                    if (!enemy.isPet)
                    {
                        enemy.TakeDamage(10);
                        isPunchingg = false;
                        StartCoroutine(isPunchingTrue(0.5f));
                    }
                }
            }
            else if (col.gameObject.tag == "EnemyTD")
            {
                TowerAttacker enemy = col.gameObject.GetComponent<TowerAttacker>();
                if (enemy == null)
                {
                    enemy = col.gameObject.GetComponentInParent<TowerAttacker>();
                }

                if (isPunchingg)
                {
                    enemy.TakeDamage(10);
                    isPunchingg = false;
                    StartCoroutine(isPunchingTrue(0.5f));
                }
            }
        }
    }

    private IEnumerator isPunchingTrue(float duration)
    {
        yield return new WaitForSeconds(duration);
        isPunchingg = true;
    }
}

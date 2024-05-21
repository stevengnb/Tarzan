using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private float duration = 4f;
    private bool hit = false;
    private void Awake()
    {
        Destroy(gameObject, duration);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Animal")
        {
            if(!hit)
            {
                AudioManagerGame.instance.RockThrownSound();
                Animal enemy = col.gameObject.GetComponent<Animal>();
                if(enemy == null)
                {
                    enemy = col.gameObject.GetComponentInParent<Animal>();
                }

                if(!enemy.isPet)
                {
                    enemy.TakeDamage(PlayerStats.instance.RockAttack);
                    hit = true;
                }
            }
        } else if (col.gameObject.tag == "Dragon")
        {
            if (!hit)
            {
                AudioManagerGame.instance.RockThrownSound();
                Dragon dragon = col.gameObject.GetComponent<Dragon>();
                if (dragon == null)
                {
                    dragon = col.gameObject.GetComponentInParent<Dragon>();
                }

                dragon.TakeDamage(PlayerStats.instance.RockAttack);
                hit = true;
            }
        } else if(col.gameObject.tag == "EnemyTD")
        {
            if (!hit)
            {
                AudioManagerGame.instance.RockThrownSound();
                TowerAttacker ta = col.gameObject.GetComponent<TowerAttacker>();
                if (ta == null)
                {
                    ta = col.gameObject.GetComponentInParent<TowerAttacker>();
                }

                ta.TakeDamage(PlayerStats.instance.RockAttack);
                hit = true;
            }
        }
    }
}

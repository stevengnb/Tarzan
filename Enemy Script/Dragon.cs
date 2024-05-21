using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    [SerializeField] protected DragonHealthBar hBar;
    [SerializeField] protected int dragonHealth;
    [SerializeField] protected int dragonAttack;
    private int currHealth;

    private void Start()
    {
        hBar.SetMaxHealth(dragonHealth);
        currHealth = dragonHealth;

    }

    public void TakeDamage(int amount)
    {
        currHealth -= amount;
        hBar.SetHealth(currHealth);

        if(currHealth <= 0)
        {
            GameManager.instance.EndGame(2);
        }
    }
}

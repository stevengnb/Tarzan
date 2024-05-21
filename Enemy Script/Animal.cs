using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] protected AnimalHealthBar hBar;
    [SerializeField] protected int enemyHealth;
    [SerializeField] private Animator animator;
    private int currHealth;
    private EnemyController ec;
    public bool isPet;
    private int isHitHash;

    public void Awake()
    {
        ec = FindObjectOfType<EnemyController>();
        isPet = false;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        isHitHash = Animator.StringToHash("isHit");

        hBar.SetMaxHealth(enemyHealth);
        currHealth = enemyHealth;
    }

    public void TakeDamage(int amount)
    {
        currHealth -= amount;
        hBar.SetHealth(currHealth);
        animator.SetBool(isHitHash, true);
        StartCoroutine(isHitAnimationFalse(0.4f));

        if(currHealth <= 0)
        {
            if(!isPet)
            {
                EnemyDie();
            }
        }
    }

    private IEnumerator isHitAnimationFalse(float duration)
    {
        yield return new WaitForSeconds(duration);
        animator.SetBool(isHitHash, false);
    }

    private void EnemyDie()
    {
        isPet = true;
        gameObject.layer = LayerMask.NameToLayer("Pet");
        ChangeAllLayer(transform, "Pet");
        ec.EnemyDied(gameObject);
        currHealth = enemyHealth;
        hBar.SetHealth(currHealth);
        PlayerStats.instance.PlusExperience(enemyHealth);
        this.GetComponent<AnimalPathfinding>().ResetAnimation();
    }

    private void ChangeAllLayer(Transform trans, string layerName)
    {
        trans.gameObject.layer = LayerMask.NameToLayer(layerName);

        foreach (Transform child in trans)
        {
            ChangeAllLayer(child, layerName);
        }
    }
}

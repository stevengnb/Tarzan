using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance { get; private set; }
    [SerializeField] protected int playerHealth;
    [SerializeField] protected int playerExp;
    [SerializeField] protected int punchAttack;
    [SerializeField] protected int rockAttack;
    [SerializeField] protected List<GameObject> pets = new List<GameObject>();
    protected int maxDefense;
    protected int defense;
    protected int mainHealth;
    protected int mainExp;
    protected int mainPunch;
    protected int mainRock;
    protected int currHealth;
    protected int currExp;
    protected int level;

    [SerializeField] protected HealthBar hBar;
    [SerializeField] protected ExpBar eBar;

    private void Awake()
    {
        if (instance == null)
        {
            level = 1;
            maxDefense = defense = 10;
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        mainHealth = playerHealth;
        mainExp = playerExp;
        mainPunch = punchAttack;
        mainRock = rockAttack;

        SetStats();

        currHealth = playerHealth;
        currExp = 0;
        hBar.SetMaxHealth(playerHealth);
        eBar.SetMaxExp(playerExp);
    }

    private void Update()
    {
        hBar.SetHealth(currHealth);
        eBar.SetExp(currExp);
    }

    private void LevelUp()
    {
        level++;
        SetStats();
        currExp = 0;
        currHealth = playerHealth;
    }

    private void SetStats()
    {
        playerHealth = mainHealth + (level * 50);
        playerExp = mainExp + (level * 50);
        rockAttack = mainRock + (level * 5);
        punchAttack = mainPunch + (level * 5);

        hBar.SetMaxHealth(playerHealth);
        eBar.SetMaxExp(playerExp);
    }

    public void PlusExperience(int exp)
    {
        currExp += exp;

        if(currExp >= playerExp)
        {
            LevelUp();
        }
    }

    public void PlayerHit(int damage)
    {
        currHealth -= damage;

        if(currHealth <= 0)
        {
            GameManager.instance.EndGame(1);
        }
    }

    public int TotalPet()
    {
        return pets.Count;
    }

    public void AddPet(GameObject pet)
    {
        pets.Add(pet);
    }

    public int PunchAttack
    {
        get { return punchAttack; }
    }

    public int RockAttack
    {
        get { return rockAttack; }
    }

    public int Level
    {
        get { return level; }
    }

    public int getHealth()
    {
        return playerHealth;
    }

    public void EnemyTDArrive()
    {
        defense--;
    }

    public int GetDefense()
    {
        return defense;
    }

    public int GetMaxDefense()
    {
        return maxDefense;
    }
}

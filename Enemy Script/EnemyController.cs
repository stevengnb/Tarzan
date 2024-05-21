using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnPoints = new List<GameObject>();
    [SerializeField] private List<GameObject> spawnPointsTD = new List<GameObject>();
    [SerializeField] private List<GameObject> enemyLists = new List<GameObject>();
    [SerializeField] public List<GameObject> enemyTDLists = new List<GameObject>();
    [SerializeField] private List<GameObject> enemyTD = new List<GameObject>();
    [SerializeField] private GameObject enemy;
    [SerializeField] private int maxEnemy;
    [SerializeField] private GameObject enemyParent;
    [SerializeField] private GameObject petParent;
    [SerializeField] private EnemyRemaining er;
    [SerializeField] private TowerHealth th;

    private int activeEnemy;
    private GameObject newEnemy;

    private void Start()
    {
        activeEnemy = 0;
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        for(int i = 0; i < spawnPoints.Count; i++)
        {
            if(activeEnemy < maxEnemy)
            {
                newEnemy = Instantiate(enemy, spawnPoints[i].transform.position, spawnPoints[i].transform.rotation, enemyParent.transform);
                newEnemy.GetComponent<AnimalPathfinding>().houseArea = spawnPoints[i];
                enemyLists.Add(newEnemy);
                activeEnemy++;
            }
        }
    }

    public void EnemyDied(GameObject enemy)
    {
        activeEnemy--;
        enemy.transform.SetParent(petParent.transform);
        PlayerStats.instance.AddPet(enemy);
        enemyLists.Remove(enemy);
    }

    public void SpawnEnemyTD()
    {
        int quadrant = Random.Range(0, spawnPointsTD.Count - 1);
        int randomEnemy;

        for(int i = 0; i < TowerDefense.instance.GetWave(); i++)
        {
            randomEnemy = Random.Range(0, enemyTD.Count - 1);
            newEnemy = Instantiate(enemyTD[randomEnemy], spawnPointsTD[quadrant].transform.position, spawnPointsTD[quadrant].transform.rotation, enemyParent.transform);
            if(quadrant == 0)
            {
                newEnemy.GetComponent<TowerAttacker>().SetPath(Grid.instance.southPath);
            } else if(quadrant == 1)
            {
                newEnemy.GetComponent<TowerAttacker>().SetPath(Grid.instance.eastPath);
            } else if(quadrant == 2)
            {
                newEnemy.GetComponent<TowerAttacker>().SetPath(Grid.instance.northPath);
            } else if(quadrant == 3)
            {
                newEnemy.GetComponent<TowerAttacker>().SetPath(Grid.instance.westPath);
            }

            enemyTDLists.Add(newEnemy);
            quadrant = Random.Range(0, spawnPointsTD.Count - 1);
        }
            
        er.SetMaxHealth(TowerDefense.instance.GetWave(), TowerDefense.instance.GetWave());
        th.SetMaxHealth(PlayerStats.instance.GetDefense(), PlayerStats.instance.GetMaxDefense());
    }

    public void EnemyTDDied(GameObject enemy)
    {
        enemyTDLists.Remove(enemy);
        er.SetHealth(enemyTDLists.Count, TowerDefense.instance.GetWave());
        Destroy(enemy.gameObject);

        if(enemyTDLists.Count <= 0)
        {
            TowerDefense.instance.SwitchNormal();
            TowerDefense.instance.isChanging = false;
        }
    }

    public void EnemyTDWin(GameObject enemy)
    {
        enemyTDLists.Remove(enemy);
        PlayerStats.instance.EnemyTDArrive();
        th.SetHealth(PlayerStats.instance.GetDefense());
        er.SetHealth(enemyTDLists.Count, TowerDefense.instance.GetWave());
        Destroy(enemy);

        if (PlayerStats.instance.GetDefense() <= 0)
        {
            GameManager.instance.EndGame(1);
        }

        if (enemyTDLists.Count <= 0)
        {
            TowerDefense.instance.SwitchNormal();
            TowerDefense.instance.isChanging = false;
        }
    }
}

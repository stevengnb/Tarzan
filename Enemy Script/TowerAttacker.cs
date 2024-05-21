using UnityEngine;
using System.Collections.Generic;

public class TowerAttacker : MonoBehaviour
{

	[SerializeField] protected AnimalHealthBar hBar;
	[SerializeField] protected int enemyHealth;
	private List<Vector3> path = new List<Vector3>();
	private bool isActive;
    private int currHealth;
	private EnemyController ec;
	float speed = 2f;
	float speedRotate = 8f;
	int targetIndex = 0;

    private void Start()
    {
		enemyHealth = enemyHealth + TowerDefense.instance.GetWave() * 50;
		speed = speed + TowerDefense.instance.GetWave() * 0.5f;
		ec = FindObjectOfType<EnemyController>();
		hBar.SetMaxHealth(enemyHealth);
		currHealth = enemyHealth;
		isActive = true;
    }

	public void SetPath(List<Node> pathNode)
    {
		path = GetVectorList(pathNode);
    }

    private List<Vector3> GetVectorList(List<Node> pathNode)
    {
        List<Vector3> tempPath = new List<Vector3>();

        foreach (Node n in pathNode)
        {
            tempPath.Add(n.worldPos);
        }

		tempPath.Reverse();
        return tempPath;
    }

    private void Update()
    {
		if(isActive)
        {
			Move();
		}
	}

	private void Move()
	{

		if (targetIndex >= path.Count)
		{
			isActive = false;
			EnemyWin();
			return;
		}

		// mengambil waypoints serta menggerakkan tower attacker
		Vector3 waypoints = path[targetIndex];
		transform.position = Vector3.MoveTowards(transform.position, waypoints, speed * Time.deltaTime);
		
		// mengecek seberapa drastis perubahan rotasi, apabila tidak terlalu drastis maka rotasinya akan sama dengan yang sebelumnya
		Vector3 dir = waypoints - transform.position;
		if (dir.magnitude < 0.2f)
		{
			return;
		}

		// rotasi tower attacker mengikuti waypoints
		Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);
		transform.rotation = Quaternion.Lerp(transform.rotation, rotation, speedRotate * Time.deltaTime);

		// mengecek apakah jarak antar tower attacker dengan waypoints sudah dekat
		// apabila iya maka akan langsung lanjut ke waypoints selanjutnyaa
		if (Vector3.Distance(transform.position, waypoints) < 0.5f)
		{
			targetIndex++;
		}
	}

	public void TakeDamage(int amount)
	{
		currHealth -= amount;
		hBar.SetHealth(currHealth);

		if (currHealth <= 0)
		{
			EnemyDie();
		}
	}

	private void EnemyDie()
	{
		if(gameObject != null)
        {
			ec.EnemyTDDied(gameObject);
			PlayerStats.instance.PlusExperience(enemyHealth);
        }
	}

	private void EnemyWin()
    {
		if(gameObject != null)
        {
			ec.EnemyTDWin(gameObject);
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Count; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
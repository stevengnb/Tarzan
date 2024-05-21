using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalPathfinding : MonoBehaviour
{
    [Header("Navmesh Requirements")]
    [SerializeField] public GameObject houseArea;
    [SerializeField] private GameObject player;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask animalLayer;
    [SerializeField] private Animator animator;
    private NavMeshAgent animalAgent;
    private Vector3 randomPoint;

    [Header("Attack and Chase")]
    [SerializeField] private float attackGap;
    private bool isAttack;
    private bool chasePointReady;
    [SerializeField] private float chaseRange;
    [SerializeField] private float attackRange;
    [SerializeField] private bool canBeChased;
    [SerializeField] private bool attackAble;

    private int isChaseHash;
    private int isAttackHash;
    private int isHitHash;
    private int isIdleHash;

    private float beforeTime = 0f;
    private float beforeXPos;
    private float beforeZPos;

    private float walkRange = 5f;
    private float playerRange = 4f;

    private EnemyController ec;

    private void Awake()
    {
        animalAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Tarzan");
        chasePointReady = false;
        ec = GameObject.Find("EnemyController").GetComponent<EnemyController>();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        isChaseHash = Animator.StringToHash("isChase");
        isAttackHash = Animator.StringToHash("isAttack");
        isHitHash = Animator.StringToHash("isHit");
        isIdleHash = Animator.StringToHash("isIdle");
    }

    private void Update()
    {
        if (!(this.GetComponent<Animal>().isPet))
        {
            if(!attackAble)
            {
                if (Mathf.Approximately(transform.position.x, beforeXPos) || Mathf.Approximately(transform.position.z, beforeZPos))
                {
                    animator.SetBool(isIdleHash, true);
                }
                else
                {
                    animator.SetBool(isIdleHash, false);
                }
            } else
            {
                animator.SetBool(isIdleHash, false);
            }

            beforeXPos = transform.position.x;
            beforeZPos = transform.position.z;

            if ((Time.time - beforeTime) > 0.6f)
            {
                canBeChased = Physics.CheckSphere(transform.position, chaseRange, playerLayer);
                attackAble = Physics.CheckSphere(transform.position, attackRange, playerLayer);
                beforeTime = Time.time + 0.6f;

                if((!canBeChased && !attackAble) || !PlayerInArea())
                {
                    Patrol();
                }

                if((canBeChased && !attackAble) && PlayerInArea()) {
                    Chase();
                }

                if(canBeChased && attackAble)
                {
                    Attack();
                }
            }
        } else
        {
            float closestDistance = Mathf.Infinity;
            if(TowerDefense.instance.CurrState == GameState.NormalMode)
            {
                bool isWildBearClose = false;
                Transform closestWild = null;

                Collider[] colliders = Physics.OverlapSphere(transform.position, 10f, animalLayer);

                if (colliders.Length != 0)
                {
                    isWildBearClose = true;

                    foreach (Collider animal in colliders)
                    {
                        float distance = Vector3.Distance(transform.position, animal.transform.position);

                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestWild = animal.transform;
                        }
                        }
                }

                if (isWildBearClose)
                {
                    attackAble = Physics.CheckSphere(transform.position, attackRange, animalLayer);

                    if(!attackAble)
                    {
                        PetChase(closestWild);
                    } else
                    {
                        PetAttack(closestWild);
                    }
                }
                else if (IsPlayerClose())
                {
                    PetIdle();
                }
                else
                {
                    animator.SetBool(isIdleHash, false);
                    animator.SetBool(isChaseHash, true);
                    animalAgent.SetDestination(player.transform.position);
                }
            } else
            {
                if((Time.time - beforeTime) > 0.6f)
                {
                    beforeTime = Time.time + 0.6f;
                    GameObject closest = null;
                    foreach(GameObject enemyTD in ec.enemyTDLists) {
                        float distance = Vector3.Distance(enemyTD.transform.position, gameObject.transform.position);
                        if (distance < closestDistance)
                        {
                            closest = enemyTD;
                            closestDistance = distance;
                        }
                    }

                    float playerDistance = Vector3.Distance(gameObject.transform.position, player.transform.position);
                    
                    if(closest != null)
                    {
                        attackAble = Physics.CheckSphere(transform.position, (attackRange + 0.5f), animalLayer);

                        if (!attackAble)
                        {
                            PetChase(closest.transform);
                        } else
                        {
                            PetAttack(closest.transform);
                        }
                    }
                }


            }
        }
    }

    private bool IsPlayerClose()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, playerRange);

        foreach(Collider col in colliders)
        {
            if(col.gameObject == player)
            {
                return true;
            }
        }

        return false;
    }

    private bool PlayerInArea()
    {
        return player.transform.position.x >= houseArea.GetComponent<Collider>().bounds.min.x
            && player.transform.position.x <= houseArea.GetComponent<Collider>().bounds.max.x
            && player.transform.position.z >= houseArea.GetComponent<Collider>().bounds.min.z
            && player.transform.position.z <= houseArea.GetComponent<Collider>().bounds.max.z;
    }

    private void PetChase(Transform cls)
    {
        animator.SetBool(isAttackHash, false);
        animator.SetBool(isIdleHash, false);
        animator.SetBool(isChaseHash, true);
        animalAgent.SetDestination(cls.position);
    }

    private void PetIdle()
    {
        animator.SetBool(isIdleHash, true);
        animator.SetBool(isChaseHash, false);
        animator.SetBool(isAttackHash, false);
        animalAgent.SetDestination(transform.position);
        transform.LookAt(player.transform);
    }

    private void PetAttack(Transform enm)
    {
        animator.SetBool(isChaseHash, false);
        animator.SetBool(isAttackHash, true);
        animalAgent.SetDestination(transform.position);
        transform.LookAt(enm.transform);
        StartCoroutine(isAttackFalse(attackGap));
    }

    private void Patrol()
    {
        animator.SetBool(isIdleHash, false);
        animator.SetBool(isAttackHash, false);
        animator.SetBool(isChaseHash, false);

        if (!chasePointReady)
        {
            GetRandomPoint();
        }
        if (chasePointReady)
        {
            animalAgent.SetDestination(randomPoint);
        }

        Vector3 randomPointDistance = transform.position - randomPoint;
        if (randomPointDistance.magnitude < 1.35f)
        {
            chasePointReady = false;
        }
    }

    private void Chase()
    {
        animator.SetBool(isIdleHash, false);
        animator.SetBool(isChaseHash, true);
        animator.SetBool(isAttackHash, false);
        animalAgent.SetDestination(player.transform.position);
    }

    private void Attack()
    {
        animator.SetBool(isIdleHash, true);
        animator.SetBool(isChaseHash, false);
        animalAgent.SetDestination(transform.position);
        transform.LookAt(player.transform);

        if(!isAttack)
        {
            animator.SetBool(isAttackHash, true); 
            isAttack = true;
            StartCoroutine(isAttackFalse(attackGap));
        }
    }

    private IEnumerator isAttackFalse(float duration)
    {
        yield return new WaitForSeconds(duration);
        isAttack = false;
        animator.SetBool(isAttackHash, false);
    }
        
    private void GetRandomPoint()
    {
        float randomPointX = Random.Range(-walkRange, walkRange);
        float randomPointZ = Random.Range(-walkRange, walkRange);

        randomPoint = new Vector3(transform.position.x + randomPointX, transform.position.y, transform.position.z + randomPointZ);

        if (Physics.Raycast(randomPoint, -transform.up, 2f, groundLayer))
        {
            chasePointReady = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

    public void ResetAnimation()
    {
        animator.SetBool(isChaseHash, false);
        animator.SetBool(isAttackHash, false);
        animator.SetBool(isHitHash, false);
        animator.SetBool(isIdleHash, true);
    }
}
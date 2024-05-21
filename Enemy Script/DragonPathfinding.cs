using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DragonPathfinding : MonoBehaviour
{
    [Header("Navmesh Requirements")]
    [SerializeField] public GameObject houseArea;
    [SerializeField] private GameObject player;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animator;
    private NavMeshAgent dragonAgent;
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
    private int isIdleHash;

    private float beforeTime = 0f;
    private float beforeXPos;
    private float beforeZPos;

    private float walkRange = 5f;

    private void Awake()
    {
        dragonAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Tarzan");
        houseArea = GameObject.Find("DragonArea");
        chasePointReady = false;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        isChaseHash = Animator.StringToHash("isChase");
        isAttackHash = Animator.StringToHash("isAttack");
        isIdleHash = Animator.StringToHash("isIdle");
    }

    private void Update()
    {
        if (!attackAble)
        {
            if (Mathf.Approximately(transform.position.x, beforeXPos) || Mathf.Approximately(transform.position.z, beforeZPos))
            {
                animator.SetBool(isIdleHash, true);
            }
            else
            {
                animator.SetBool(isIdleHash, false);
            }
        }
        else
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

            if ((!canBeChased && !attackAble) || !PlayerInArea())
            {
                Patrol();
            }

            if ((canBeChased && !attackAble) && PlayerInArea())
            {
                Chase();
            }

            if (canBeChased && attackAble)
            {
                Debug.Log("sekarang bisa attack");
                Attack();
            }

        }
    }

    private bool PlayerInArea()
    {
        return player.transform.position.x >= houseArea.GetComponent<Collider>().bounds.min.x
            && player.transform.position.x <= houseArea.GetComponent<Collider>().bounds.max.x
            && player.transform.position.z >= houseArea.GetComponent<Collider>().bounds.min.z
            && player.transform.position.z <= houseArea.GetComponent<Collider>().bounds.max.z;
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
            dragonAgent.SetDestination(randomPoint);
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
        dragonAgent.SetDestination(player.transform.position);
    }

    private void Attack()
    {
        animator.SetBool(isIdleHash, true);
        animator.SetBool(isChaseHash, false);
        dragonAgent.SetDestination(transform.position);
        transform.LookAt(player.transform);

        if (!isAttack)
        {
            animator.SetBool(isAttackHash, true);
            isAttack = true;
            if (PlayerStats.instance.Level == 1)
            {
                PlayerStats.instance.PlayerHit(PlayerStats.instance.getHealth() / 2);
            }
            else
            {
                PlayerStats.instance.PlayerHit(20);
            }
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
}
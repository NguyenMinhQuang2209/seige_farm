using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class WildAnimal : Interactible
{
    private NavMeshAgent agent;
    private Animator animator;

    [SerializeField] private float radious = 1f;
    [SerializeField] private float waitTime = 1f;
    float currentWaiTime = 0f;

    private Vector3 rootPosition;

    [Header("Movement Speed")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float runSpeed = 5f;
    float currentSpeed = 0f;

    [Header("Growing")]
    [SerializeField] private float growingTime = 1f;
    [SerializeField] private float growingRate = 1f;
    [SerializeField] private float smallRate = 0.5f;
    [SerializeField] private float adultRate = 1f;
    float currentGrowingTime = 0f;

    private GameObject player;
    [Header("Chase Player")]
    [SerializeField] private float sawAngle = 80f;
    [SerializeField] private float sawDistance = 5f;
    [SerializeField] private float chaseDistance = 10f;
    [SerializeField] private float stopDistance = 1f;
    [SerializeField] private float rotateSpeed = 5f;

    bool chasePlayer = false;

    [Header("Attacking")]
    [SerializeField] private float timeBwtAttack = 1f;
    [Tooltip("Only for shot attacking")]
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRadious = 1f;
    [SerializeField] private LayerMask attackMask;
    [SerializeField] private float damage = 1f;
    float currentTimeBwtAttack = 0f;

    [Header("Far Attacking Config")]
    [SerializeField] private bool farAttacking = false;
    [Tooltip("Only for far attacking")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed = 1f;
    [SerializeField] private float destroyBulletTime = 1f;

    [Header("health Config")]
    [SerializeField] private float startHealth = 100f;
    [SerializeField] private float maxHealth = 200f;
    float currentMaxHealth = 0f;
    float currentHealth = 0f;

    [Header("Recover Health Speed")]
    [SerializeField] private float waitRecover = 1f;
    [SerializeField] private float recoverHealthRate = 1f;
    float currentWaitRecoverTime = 0f;

    [Header("destroy time")]
    [SerializeField] private float destroyDelayTime = 2f;
    bool dealth = false;

    GameObject target = null;

    [SerializeField] private bool useDrawCircle = false;
    [Header("Collect Config")]
    [SerializeField] private List<CollectItem> collectItems = new();
    [SerializeField] private Vector2Int getCoinRange = Vector2Int.zero;
    private List<CollectItem> collectItemsFinal = new();

    [Header("Animal Type")]
    [SerializeField] private bool sawAndAttackPlayer = true;



    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = moveSpeed;
        }
        rootPosition = transform.position;
        animator = GetComponent<Animator>();
        currentSpeed = moveSpeed;
        transform.localScale = new Vector3(smallRate, smallRate, smallRate);
        if (PreferenceController.instance != null)
        {
            player = PreferenceController.instance.Player;
        }
        currentMaxHealth = startHealth;
        currentHealth = currentMaxHealth;
        collectItemsFinal = GetRandomList();
    }

    private List<CollectItem> GetRandomList()
    {
        List<CollectItem> newTemp = new();
        for (int i = 0; i < collectItems.Count; i++)
        {
            CollectItem item = collectItems[i].Clone();
            bool canGet = true;
            if (item.useRandomGet)
            {
                float canGetInt = Random.Range(0f, 100f);
                if (canGetInt > item.getRate)
                {
                    canGet = false;
                }
            }
            if (!canGet)
            {
                continue;
            }

            if (item.useRandomRange)
            {
                item.quantity = Random.Range(0, item.quantity + 1);
            }
            if (item.quantity > 0)
            {
                newTemp.Add(item);
            }
        }
        return newTemp;
    }
    private void Update()
    {
        if (player == null)
        {
            player = PreferenceController.instance.Player;
        }
        promptMessage = "HP:" + Mathf.Round(currentHealth) + "/" + Mathf.Round(currentMaxHealth);
        agent.speed = currentSpeed;
        if (dealth)
        {
            return;
        }
        currentTimeBwtAttack += Time.deltaTime;
        if (target != null)
        {
            animator.SetFloat("Speed", currentSpeed);
            float checkistance = Vector3.Distance(target.transform.position, transform.position);
            if (checkistance <= stopDistance)
            {
                agent.SetDestination(transform.position);
                Vector3 targetDir = target.transform.position - transform.position;
                Quaternion desiredRotation = Quaternion.LookRotation(targetDir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotateSpeed * Time.deltaTime);
                if (currentTimeBwtAttack >= timeBwtAttack)
                {
                    Attack();
                    currentTimeBwtAttack = 0f;
                }
                currentSpeed = 0f;
            }
            else
            {
                if (checkistance >= chaseDistance)
                {
                    target = null;
                    agent.ResetPath();
                    agent.SetDestination(rootPosition);
                }
                else
                {
                    currentSpeed = runSpeed;
                    agent.SetDestination(target.transform.position);
                }
            }
            return;
        }
        CheckDistance();
    }
    private void RandomPosition()
    {
        Vector3 newPosition = GetRandomPosition();
        if (agent != null)
        {
            agent.SetDestination(newPosition);
        }
    }
    public Vector3 GetRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * Mathf.Max(0f, radious);
        Vector3 newPosition = new(randomDirection.x + rootPosition.x, rootPosition.y, randomDirection.z + rootPosition.z);

        if (NavMesh.SamplePosition(newPosition, out NavMeshHit navMeshHit, radious, agent.areaMask))
        {
            return navMeshHit.position;
        };
        return rootPosition;
    }
    private void OnDrawGizmos()
    {
        if (!useDrawCircle)
        {
            return;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radious);
        if (!farAttacking)
        {
            Gizmos.DrawWireSphere(attackPos.position, attackRadious);
        }
    }

    private void CheckDistance()
    {
        currentWaitRecoverTime += Time.deltaTime;
        if (currentWaitRecoverTime >= waitRecover)
        {
            currentHealth = Mathf.Min(currentHealth + Time.deltaTime * recoverHealthRate, currentMaxHealth);
        }
        if (currentGrowingTime <= growingTime)
        {
            currentGrowingTime += Time.deltaTime * growingRate;
            float newRate = Mathf.Lerp(smallRate, adultRate, currentGrowingTime / growingTime);
            transform.localScale = new Vector3(newRate, newRate, newRate);
            currentMaxHealth = Mathf.Lerp(startHealth, maxHealth, currentGrowingTime / growingTime);
        }
        if (sawAndAttackPlayer)
        {
            float newStop = chasePlayer ? stopDistance : 0.1f;
            animator.SetFloat("Speed", agent.remainingDistance <= newStop ? 0f : currentSpeed);
            if (!chasePlayer)
            {
                chasePlayer = CanSeePlayer();
                PatrolState();
            }
            else
            {
                chasePlayer = ChaseSeePlayer();
                AttackState();
                currentWaiTime = waitTime;
            }
        }
        else
        {
            float newStop = chasePlayer ? stopDistance : 0.1f;
            animator.SetFloat("Speed", agent.remainingDistance <= newStop ? 0f : currentSpeed);
            PatrolState();
        }

    }
    private void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }
    public void Attacking()
    {
        if (!farAttacking)
        {
            ShortAttacking();
        }
        else
        {
            FarAttacking();
        }
    }
    private void FarAttacking()
    {
        if (bullet != null)
        {
            GameObject spawnBullet = Instantiate(bullet, attackPos.position, Quaternion.identity);
            if (spawnBullet.TryGetComponent<EnemyBullet>(out EnemyBullet newBullet))
            {
                Vector3 newDir;
                if (target == null)
                {
                    newDir = player.transform.position - transform.position;
                }
                else
                {
                    newDir = target.transform.position - transform.position;
                }
                newBullet.Shoot(gameObject, newDir, bulletSpeed, destroyBulletTime, damage);
            }
        }
    }
    private void ShortAttacking()
    {
        Collider[] hits = Physics.OverlapSphere(attackPos.position, attackRadious, attackMask);
        bool targetDie = false;
        foreach (Collider hit in hits)
        {
            if (hit.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth health))
            {
                targetDie = health.GetDamage(damage);
            }
            else if (hit.gameObject.TryGetComponent<Solider>(out Solider solider))
            {
                targetDie = solider.GetDamage(damage);
            }
        }
        if (targetDie)
        {
            target = null;
            agent.ResetPath();
            agent.SetDestination(rootPosition);
        }
    }
    private void PatrolState()
    {
        if (agent.remainingDistance <= 0.1f)
        {
            currentSpeed = 0f;
            currentWaiTime += Time.deltaTime;
            if (currentWaiTime >= waitTime)
            {
                currentWaiTime = 0f;
                RandomPosition();
            }
        }
        else
        {
            currentSpeed = moveSpeed;
        }
    }
    private bool CanSeePlayer()
    {
        Vector3 dir = player.transform.position - transform.position;
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance <= sawDistance)
        {
            float angle = Vector3.Angle(transform.forward, dir);
            if (angle >= -sawAngle && angle <= sawAngle)
            {
                return true;
            }
        }
        return false;
    }

    private void AttackState()
    {
        float checkistance = Vector3.Distance(player.transform.position, transform.position);
        if (checkistance <= stopDistance)
        {
            agent.SetDestination(transform.position);
            Vector3 targetDir = player.transform.position - transform.position;
            Quaternion desiredRotation = Quaternion.LookRotation(targetDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotateSpeed * Time.deltaTime);
            if (currentTimeBwtAttack >= timeBwtAttack)
            {
                Attack();
                currentTimeBwtAttack = 0f;
            }
            currentSpeed = 0f;
        }
        else
        {
            currentSpeed = runSpeed;
            agent.SetDestination(player.transform.position);
        }
    }
    private bool ChaseSeePlayer()
    {
        float checkistance = Vector3.Distance(player.transform.position, transform.position);
        if (checkistance <= chaseDistance)
        {
            return true;
        }
        return false;
    }
    public void GetDamage(float damage, GameObject targetObject)
    {
        if (dealth)
        {
            return;
        }
        if (target == null)
        {
            target = targetObject;
        }
        currentHealth = Mathf.Max(0f, currentHealth - damage);
        PreferenceController.instance.AddText(transform.transform.position + Vector3.up * 2f, "-" + damage.ToString(), Color.red);
        if (currentHealth == 0)
        {
            dealth = true;
            animator.SetTrigger("Die");
            Destroy(gameObject, destroyDelayTime);
            Invoke(nameof(GetCollect), destroyDelayTime - 0.1f);
        }
    }
    private void GetCollect()
    {
        int coins = Random.Range(Mathf.Min(getCoinRange.x, getCoinRange.y), Mathf.Max(getCoinRange.x, getCoinRange.y));
        GameObject bag = Instantiate(PreferenceController.instance.present, transform.position, Quaternion.identity);
        if (bag.TryGetComponent<BagCollect>(out BagCollect bagCollect))
        {
            bagCollect.SpawnObject(collectItemsFinal, coins);
        }
    }
}

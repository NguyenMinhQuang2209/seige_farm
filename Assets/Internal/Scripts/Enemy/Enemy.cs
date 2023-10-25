using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private GameObject target;

    [Header("Comfig")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float stopDistance = 0.1f;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private float timeBwtAttack = 1f;
    float currentTimeBwtAttack = 0f;
    [SerializeField] private float damage = 10f;


    private EnemyHealth enemyHealth;

    [SerializeField] private float rotateSpeed = 10f;


    [Header("Attacking")]
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRadious = 1f;
    bool changeDir = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        agent.speed = speed;
        if (PreferenceController.instance != null)
        {
            target = PreferenceController.instance.enemyTarget;
            agent.SetDestination(target.transform.position);
        }
        agent.stoppingDistance = stopDistance;
    }
    private void Update()
    {
        currentTimeBwtAttack += Time.deltaTime;
        if (enemyHealth != null && enemyHealth.ObjectDie())
        {
            agent.isStopped = true;
            animator.SetFloat("Speed", 0);
            return;
        }
        animator.SetFloat("Speed", agent.remainingDistance >= stopDistance ? 1 : 0);
        if (enemyHealth != null)
        {
            GameObject newTarget = enemyHealth.NewAttackTarget();
            if (newTarget != null)
            {
                if (!changeDir)
                {
                    agent.SetDestination(newTarget.transform.position);
                }
                changeDir = true;
                if (agent.remainingDistance <= stopDistance)
                {
                    agent.SetDestination(transform.position);
                    Vector3 targetDir = newTarget.transform.position - transform.position;
                    Quaternion desiredRotation = Quaternion.LookRotation(targetDir, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotateSpeed * Time.deltaTime);
                    if (currentTimeBwtAttack >= timeBwtAttack)
                    {
                        animator.SetTrigger("Attack");
                        currentTimeBwtAttack = 0f;
                    }
                }
                else
                {
                    agent.SetDestination(newTarget.transform.position);
                }
                return;
            }
        }
        changeDir = false;
        if (agent.remainingDistance <= stopDistance)
        {
            Vector3 targetDir = target.transform.position - transform.position;
            Quaternion desiredRotation = Quaternion.LookRotation(targetDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotateSpeed * Time.deltaTime);
            Collider[] hits = Physics.OverlapSphere(attackPos.position, attackRadious, wallMask);
            if (hits.Length > 0)
            {
                if (currentTimeBwtAttack >= timeBwtAttack)
                {
                    animator.SetTrigger("Attack");
                    currentTimeBwtAttack = 0f;
                }
            }
            else
            {
                agent.SetDestination(target.transform.position);
            }
        }
        else
        {
            currentTimeBwtAttack = 0f;
        }
    }
    public void Attacking()
    {
        bool targetDie = false;
        Collider[] hits = Physics.OverlapSphere(attackPos.position, attackRadious, wallMask);
        foreach (Collider hit in hits)
        {
            if (hit.gameObject.TryGetComponent<ItemOffset>(out ItemOffset target))
            {
                targetDie = target.GetDamage(damage);
            }

            else if (hit.gameObject.TryGetComponent<Solider>(out Solider solider))
            {
                targetDie = solider.GetDamage(damage);
            }
            else if (hit.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth player))
            {
                targetDie = player.GetDamage(damage);
            }
        }
        if (targetDie)
        {
            enemyHealth.ReleaseAttackTarget();
        }
    }
    private void OnDrawGizmos()
    {
        if (attackPos != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackPos.position, attackRadious);
        }
    }
}

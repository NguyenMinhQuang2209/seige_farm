using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShyAnimal : Interactible
{
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Movement Speed")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float radious = 1f;
    [SerializeField] private float waitTime = 1f;
    [SerializeField] private float waitEating = 1f;
    float currentWaitEating = 0f;
    float currentWaitTime = 0f;
    float currentSpeed = 0f;

    [Header("health Config")]
    [SerializeField] private float maxHealth = 50f;
    float currentHealth = 0f;

    [Header("Recover Health Speed")]
    [SerializeField] private float waitRecover = 1f;
    [SerializeField] private float recoverHealthRate = 1f;
    float currentWaitRecoverTime = 0f;

    [Header("Run away")]
    [SerializeField] private float runAwayTime = 1f;
    float currentRunAwayTime = 0f;

    [Header("destroy time")]
    [SerializeField] private float destroyDelayTime = 2f;
    bool dealth = false;

    [Header("Collect Config")]
    [SerializeField] private List<CollectItem> collectItems = new();
    [SerializeField] private Vector2Int getCoinRange = Vector2Int.zero;

    private List<CollectItem> collectItemsFinal = new();

    private void Start()
    {
        currentSpeed = moveSpeed;
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = currentSpeed;
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
        promptMessage = "HP:" + Mathf.Round(currentHealth) + "/" + Mathf.Round(maxHealth);
        agent.speed = currentSpeed;
        currentRunAwayTime -= Time.deltaTime;
        currentWaitTime += Time.deltaTime;
        currentWaitEating += Time.deltaTime;
        animator.SetFloat("Speed", agent.remainingDistance <= 0.1f ? 0f : 1f);
        if (currentRunAwayTime > 0f)
        {
            currentRunAwayTime = 0f;
            currentWaitTime = waitTime;
        }
        if (currentWaitTime >= waitTime)
        {
            currentWaitTime = 0f;
            RandomPosition();
        }
        else
        {
            if (currentWaitEating >= waitEating)
            {
                int ran = Random.Range(0, 1);
                if (ran == 1) animator.SetTrigger("Eat");
            }
        }

        if (currentHealth < maxHealth)
        {
            currentWaitRecoverTime += Time.deltaTime;
            if (currentWaitRecoverTime >= waitRecover)
            {
                currentSpeed = moveSpeed;
                currentWaitRecoverTime = 0f;
                currentHealth = Mathf.Min(currentHealth + Time.deltaTime * recoverHealthRate, maxHealth);
            }
        }
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
        Vector3 newPosition = new(randomDirection.x + transform.position.x, transform.position.y, randomDirection.z + transform.position.z);

        if (NavMesh.SamplePosition(newPosition, out NavMeshHit navMeshHit, radious, agent.areaMask))
        {
            return navMeshHit.position;
        };
        return transform.position;
    }
    public void GetDamage(float damage)
    {
        if (dealth)
        {
            return;
        }
        currentHealth = Mathf.Max(0f, currentHealth - damage);
        currentWaitRecoverTime = 0f;
        currentSpeed = runSpeed;
        currentRunAwayTime = runAwayTime;
        PreferenceController.instance.AddText(transform.transform.position + Vector3.up * 2f, "-" + damage.ToString(), Color.red);
        if (currentHealth == 0f)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Interactible
{
    [SerializeField] private float maxHealth = 100f;
    float currentHealth = 0;
    private Animator animator;
    [SerializeField] private float dieDelayTime = 2f;
    [SerializeField] private float checkDistance = 3f;

    [SerializeField] private Vector2Int getCoinRange = Vector2Int.zero;
    [SerializeField] private List<CollectItem> collectItems = new();

    GameObject hitted = null;
    private List<CollectItem> collectItemsFinal = new();
    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
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
            newTemp.Add(item);
        }
        return newTemp;
    }
    public void GetDamage(float damage, GameObject hitBy)
    {
        if (ObjectDie())
        {
            return;
        }
        if (hitted == null)
        {
            hitted = hitBy;
        }
        PreferenceController.instance.AddText(transform.transform.position + Vector3.up * 2f, "-" + damage.ToString(), Color.red);
        currentHealth = Mathf.Max(0f, currentHealth - damage);
        if (currentHealth == 0f)
        {
            animator.SetTrigger("Die");
            Destroy(gameObject, dieDelayTime);
            Invoke(nameof(GetCollect), dieDelayTime - 0.1f);

        }
    }
    private void Update()
    {
        promptMessage = "HP:" + Mathf.Round(currentHealth) + "/" + Mathf.Round(maxHealth);
        if (hitted != null)
        {
            if (Vector3.Distance(hitted.transform.position, transform.position) > checkDistance)
            {
                hitted = null;
            }
        }
    }
    public bool ObjectDie()
    {
        return currentHealth == 0;
    }
    public GameObject NewAttackTarget()
    {
        return hitted;
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
    public void ReleaseAttackTarget()
    {
        hitted = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Seller : Interactible
{
    private List<SellItem> sellItems = new List<SellItem>();
    [SerializeField] private bool useMaxSellItem = false;
    [SerializeField] private int maxSellItem = 1;
    private GameObject shop;
    [SerializeField] private float reloadTime = 10f;
    float currentReloadTime = 0f;

    [Header("Random Movement")]
    [SerializeField] private float radious = 10f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxDelayTime = 5f;
    float currentDelayTime = 0f;
    float delayTime = 0f;
    private NavMeshAgent agent;
    private Animator animator;

    private void Start()
    {
        shop = PreferenceController.instance.shop;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        if(ShopController.instance != null)
        {
            sellItems = ShopController.instance.GetSellerItem(useMaxSellItem,maxSellItem);
        }
        delayTime = Random.Range(0f,maxDelayTime);
    }
    private void Update()
    {
        currentReloadTime = Mathf.Min(currentReloadTime + Time.deltaTime,reloadTime);
        if (shop == null)
        {
            shop = PreferenceController.instance.shop;
            if (ShopController.instance != null)
            {
                sellItems = ShopController.instance.GetSellerItem(useMaxSellItem, maxSellItem);
            }
        }
        if(currentReloadTime ==  reloadTime)
        {
            currentReloadTime = 0f;
            ReloadShop();
        }
        animator.SetBool("Movement", agent.remainingDistance > 0.1f);
        if (CursorController.instance.CurrentCursorName().Equals(gameObject.name))
        {
            if(agent.remainingDistance > 0.1f)
            {
                agent.SetDestination(transform.position);
            }
            return;
        }

        if (agent.remainingDistance <= 0.1f)
        {
            currentDelayTime += Time.deltaTime;
            if(currentDelayTime >= delayTime)
            {
                agent.SetDestination(RandomNewPos());
                currentDelayTime = 0f;
                delayTime = Random.Range(0f, maxDelayTime);
            }
        }
    }
    protected override void Interact()
    {
        if(shop != null)
        {
            ShopController.instance.UpdateShopItem(sellItems);
            CursorController.instance.TriggerCursor(gameObject.name, new List<GameObject>() { shop });
        }
    }
    private void ReloadShop()
    {
        sellItems = ShopController.instance.GetSellerItem(useMaxSellItem, maxSellItem);
    }
    private Vector3 RandomNewPos()
    {
        Vector3 rdPosition = Random.insideUnitSphere * Mathf.Max(0f, radious);
        Vector3 targetPos = transform.position + rdPosition;

        if(NavMesh.SamplePosition(targetPos,out NavMeshHit hit, radious, agent.areaMask))
        {
            return hit.position;
        }
        return transform.position;
    }
}
[System.Serializable]
public class SellItem
{
    public SellerItem item;
    public float rating = 1;
}
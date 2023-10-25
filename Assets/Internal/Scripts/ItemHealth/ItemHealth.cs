using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    float currentHealth;

    [Header("Collecting Item")]
    [SerializeField] private ItemType itemType;
    [SerializeField] private ItemName itemName;
    [SerializeField] private int colllectItemQuantity = 1;

    [Header("Animation")]
    private Animator animator;
    [SerializeField] private float dealthDelayTime = 1f;
    [SerializeField] private Collider itemCollider;
    bool dealth = false;
    int currentQuantity = 0;
    float getFloatQuantity = 0;

    [SerializeField] private WorldType worldType;
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        currentQuantity = colllectItemQuantity;
    }

    public void GetDamage(float enemyDamage, float treeDamage, float rockDamage)
    {
        float damage;
        switch (worldType)
        {
            case WorldType.Tree:
                damage = treeDamage;
                break;
            case WorldType.Rock:
                damage = rockDamage;
                break;
            default:
                damage = enemyDamage;
                break;
        }
        if (dealth)
        {
            return;
        }
        PreferenceController.instance.AddText(transform.transform.position + Vector3.up * 2f, "-" + damage.ToString(), Color.red);
        currentHealth = Mathf.Max(0, currentHealth - damage);
        getFloatQuantity += ((damage / maxHealth) * colllectItemQuantity) - Mathf.Floor((damage / maxHealth) * colllectItemQuantity);
        int getQuantity = (int)Mathf.Floor((damage / maxHealth) * colllectItemQuantity);
        if (getFloatQuantity >= 1)
        {
            getFloatQuantity = 0;
            getQuantity += 1;
        }
        if (currentHealth <= 0f)
        {
            dealth = true;
            if (animator != null)
            {
                animator.SetTrigger("Die");
            }
            Destroy(gameObject, dealthDelayTime);
            itemCollider.enabled = false;
            if (currentQuantity > 0)
            {
                InventoryItems item = ObjectPreferenceController.instance.GetPreferenceInventoryItem(itemType, itemName);
                bool canAdd = InventoryController.instance.AddItem(item, currentQuantity);
                if (!canAdd)
                {
                    GameObject bag = Instantiate(PreferenceController.instance.present, transform.position, Quaternion.identity);
                    CollectItem collectItemTemp = new(itemType, itemName, currentQuantity, true, false, false);
                    List<CollectItem> list = new() { collectItemTemp };
                    if (bag.TryGetComponent<BagCollect>(out BagCollect bagCollect))
                    {
                        bagCollect.SpawnObject(list, 0);
                    }
                    ErrorController.instance.ChangeTxt("Túi đồ đầy\n (Inventory is full!)", Color.red);
                }
            }
        }
        else
        {
            InventoryItems item = ObjectPreferenceController.instance.GetPreferenceInventoryItem(itemType, itemName);
            int getAmount = currentQuantity >= getQuantity ? getQuantity : currentQuantity;
            if (getAmount > 0)
            {
                bool canAdd = InventoryController.instance.AddItem(item, getAmount);
                if (!canAdd)
                {
                    GameObject bag = Instantiate(PreferenceController.instance.present, transform.position, Quaternion.identity);
                    CollectItem collectItemTemp = new(itemType, itemName, currentQuantity, true, false, false);
                    List<CollectItem> list = new() { collectItemTemp };
                    if (bag.TryGetComponent<BagCollect>(out BagCollect bagCollect))
                    {
                        bagCollect.SpawnObject(list, 0);
                    }
                    ErrorController.instance.ChangeTxt("Túi đồ đầy\n (Inventory is full!)", Color.red);
                }
            }
            currentQuantity = Mathf.Max(0, currentQuantity - getQuantity);
        }
    }
}

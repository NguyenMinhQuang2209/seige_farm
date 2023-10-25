using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagCollect : Interactible
{
    private List<CollectItem> collectItems = new List<CollectItem>();
    private float coin = 0;
    private void Start()
    {
        promptMessage = "Túi đồ (Bag)";
    }

    public void SpawnObject(List<CollectItem> collectItems, float coin)
    {
        this.collectItems = collectItems;
        this.coin = coin;
    }
    protected override void Interact()
    {
        PickupItem();
    }
    private void PickupItem()
    {
        CoinController.instance.AddCoin(coin);
        MusicController.instance.collectMusic.Play();
        if (collectItems.Count == 0)
        {
            Destroy(gameObject);
            return;
        }
        coin = 0;
        bool addFully = true;
        List<CollectItem> newListCollect = new();
        for (int i = 0; i < collectItems.Count; i++)
        {
            CollectItem item = collectItems[i];
            InventoryItems inventoryItem = ObjectPreferenceController.instance.GetPreferenceInventoryItem(item.itemType, item.itemName);
            bool addingStatus = InventoryController.instance.AddItem(inventoryItem, collectItems[i].quantity);
            if (!addingStatus)
            {
                newListCollect.Add(collectItems[i]);
                addFully = false;
            }
        }
        if (addFully)
        {
            Destroy(gameObject);
        }
        else
        {
            collectItems.Clear();
            collectItems = newListCollect;
            ErrorController.instance.ChangeTxt("Túi đồ đầy\n (Inventory is full!)", Color.red);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagController.playerTag))
        {
            PickupItem();
        }
    }
}
[System.Serializable]
public class CollectItem
{
    public ItemType itemType;
    public ItemName itemName;

    public bool getAll = true;
    public bool useRandomRange = false;

    [Tooltip("Use only for use random rate")]
    public bool useRandomGet = false;

    [Tooltip("Use only for use random get")]
    public float getRate = 1f;
    public int quantity = 1;
    public CollectItem(ItemType type, ItemName itemName, int quantity)
    {
        this.itemType = type;
        this.itemName = itemName;
        this.quantity = quantity;
    }
    public CollectItem(ItemType itemType, ItemName itemName, int quantity, bool getAll, bool useRandomRange, bool useRandomGet, float getRate = 100f)
    {
        this.itemType = itemType;
        this.quantity = quantity;
        this.getAll = getAll;
        this.itemName = itemName;
        this.useRandomRange = useRandomRange;
        this.useRandomGet = useRandomGet;
        this.getRate = getRate;
    }
    public CollectItem Clone()
    {
        return new CollectItem(itemType, itemName, quantity, getAll, useRandomRange, useRandomRange, getRate);
    }
}

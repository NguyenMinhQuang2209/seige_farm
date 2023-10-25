using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private Transform canvasRoot;
    public List<Container> containers = new List<Container>();
    private Dictionary<string, int> stock = new Dictionary<string, int>();
    public static InventoryController instance;
    private GameObject inventoryContainer;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Update()
    {
        if (inventoryContainer == null)
        {
            inventoryContainer = PreferenceController.instance.inventoryContainer;
        }
    }
    private void Start()
    {
        foreach (Container contain in containers)
        {
            for (int i = 0; i < contain.amount; i++)
            {
                Instantiate(contain.item, contain.container.transform);
            }
        }
        if (PreferenceController.instance != null)
        {
            inventoryContainer = PreferenceController.instance.inventoryContainer;
        }
        UpdateStock();
    }
    public bool AddItem(InventoryItems item, int quantity = 1)
    {
        bool addInventory = AddItemChild(inventoryContainer.transform, item, quantity);
        UpdateStock();
        if (addInventory)
        {
            return true;
        }
        return addInventory;
    }
    private bool AddItemChild(Transform container, InventoryItems item, int quantity = 1)
    {
        foreach (Transform child in container)
        {
            if (child.TryGetComponent<InventorySlots>(out InventorySlots slotItem))
            {
                if (slotItem.IsEmpty(item, quantity))
                {
                    slotItem.AddingItem(item, quantity);
                    UpdateStock();
                    return true;
                }
            }
        }
        return false;
    }
    public Transform GetRootSpawn()
    {
        return canvasRoot;
    }
    public void UpdateStock()
    {
        stock.Clear();
        UpdateStock(inventoryContainer.transform);
    }

    private void UpdateStock(Transform container)
    {
        foreach (Transform child in container)
        {
            if (child.TryGetComponent<InventorySlots>(out InventorySlots slotItem))
            {
                InventoryItems item = slotItem.GetItem();
                if (item != null)
                {
                    string key = item.GetItemName().ToString();
                    stock[key] = stock.ContainsKey(key) ? stock[key] + item.GetCurrentQuantity() : item.GetCurrentQuantity();
                }
            }
        }
    }
    public void ClearAllStock()
    {
        ClearAllStock(inventoryContainer.transform);
        stock.Clear();
    }
    private void ClearAllStock(Transform container)
    {
        foreach (Transform child in container)
        {
            if (child.TryGetComponent<InventorySlots>(out InventorySlots slotItem))
            {
                InventoryItems item = slotItem.GetItem();
                if (item != null)
                {
                    Destroy(item.gameObject);
                }
            }
        }
    }
    public bool RemoveItem(ItemName? itemName, ItemType? itemType, int quantity)
    {
        int remainInventory = RemoveItem(inventoryContainer.transform, itemName, itemType, quantity);
        UpdateStock();
        if (remainInventory == 0)
        {
            return true;
        }
        return remainInventory == 0;
    }
    private int RemoveItem(Transform container, ItemName? itemName, ItemType? itemType, int quantity)
    {
        foreach (Transform child in container.transform)
        {
            if (child.TryGetComponent<InventorySlots>(out InventorySlots slotItem))
            {
                InventoryItems childItem = slotItem.GetItem();
                if (childItem != null)
                {
                    if (childItem.CheckEqual(itemName, itemType))
                    {
                        int remain = childItem.ChangeQuantity(-quantity);
                        childItem.CheckQuantity();
                        quantity = remain * -1;
                        if (quantity == 0)
                        {
                            break;
                        }
                    }
                }
            }
        }
        return quantity;
    }
    public bool RemoveItem(ItemName? itemName, int quantity)
    {
        int remainInventory = RemoveItem(inventoryContainer.transform, itemName, quantity);
        UpdateStock();
        if (remainInventory == 0)
        {
            return true;
        }
        return remainInventory == 0;
    }
    private int RemoveItem(Transform container, ItemName? itemName, int quantity)
    {
        foreach (Transform child in container.transform)
        {
            if (child.TryGetComponent<InventorySlots>(out InventorySlots slotItem))
            {
                InventoryItems childItem = slotItem.GetItem();
                if (childItem != null)
                {
                    if (childItem.CheckEqual(itemName))
                    {
                        int remain = childItem.ChangeQuantity(-quantity);
                        childItem.CheckQuantity();
                        quantity = remain * -1;
                        if (quantity == 0)
                        {
                            break;
                        }
                    }
                }
            }
        }
        return quantity;
    }
    public int GetQuantityInStock(ItemName? item)
    {
        string key = item.ToString();
        return stock.ContainsKey(key) ? stock[key] : 0;
    }

}
[System.Serializable]
public class Container
{
    public GameObject container;
    public GameObject item;
    public int amount;
}

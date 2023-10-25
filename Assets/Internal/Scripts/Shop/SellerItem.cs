using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellerItem : MonoBehaviour
{
    [SerializeField] private Texture2D itemImg;
    [SerializeField] private int price = 1;
    [SerializeField] private string itemName;
    [SerializeField] private InventoryItems inventoryItem;
    [SerializeField] private int quantity = 1;
    public Texture2D GetItemImage()
    {
        return itemImg;
    }

    public int GetPrice()
    {
        return price;
    }
    public InventoryItems GetInventoryItem()
    {
        return inventoryItem;
    }
    public string GetItemName()
    {
        return itemName;
    }
    public int GetQuantity()
    {
        return quantity;
    }
}

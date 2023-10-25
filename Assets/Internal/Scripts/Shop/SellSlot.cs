using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SellSlot : MonoBehaviour
{
    #pragma warning disable IDE0052 
    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI txtPrice;
    [SerializeField] private TextMeshProUGUI txtName;
    private InventoryItems inventoryItem;
    private int quantity = 1;
    private int price = 0;
    public void UpdateSellSlot(Texture2D myTexture, int price, string name,InventoryItems inventoryItem,int quantity)
    {
        Sprite sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));
        img.sprite = sprite;
        txtPrice.text = price.ToString();
        this.price = price;
        txtName.text = name + " x " + quantity;
        this.inventoryItem = inventoryItem;
        this.quantity = quantity;
    }
    public void BuyItem()
    {
        if (CoinController.instance.CheckMinusCoin(price,true))
        {
            bool canAdd = InventoryController.instance.AddItem(inventoryItem, quantity);
            if (canAdd)
            {
                Debug.Log("Add successfully!");
            }
            else
            {
                Debug.Log("Inventory is full");
            }
        }
        else
        {
            Debug.Log("Not Engouh money");
        }
    }
}

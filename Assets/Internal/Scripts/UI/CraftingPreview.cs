using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftingPreview : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private List<TextMeshProUGUI> itemRequires;
    [SerializeField] private GameObject button;
    private CraftingPreviewItem item = null;
    private void Start()
    {
        ChangeItemTxt();
    }
    public void SwitchPreviewItem(CraftingPreviewItem newItem)
    {
        item = newItem;
        ChangeItemTxt();
    }
    private void ChangeItemTxt()
    {
        ReloadTxt();
        if (item != null)
        {
            itemName.text = item.GetItemName();
            itemDescription.text = item.GetItemDescription();
            List<CraftingRequire> requires = item.GetCraftingRequires();
            int amount = requires.Count > itemRequires.Count ? itemRequires.Count : requires.Count;
            bool enough = true;
            for (int i = 0; i < amount; i++)
            {
                int inventoryAmount = InventoryController.instance.GetQuantityInStock(requires[i].itemName);
                itemRequires[i].text = requires[i].previewName + "(" + requires[i].itemName + ")" + " x " + requires[i].quantity + " [" + inventoryAmount + "]";
                itemRequires[i].color = inventoryAmount < requires[i].quantity ? Color.red : Color.green;
                if (inventoryAmount < requires[i].quantity)
                {
                    enough = false;
                }
            }
            button.SetActive(enough);
        }
    }
    private void ReloadTxt()
    {
        itemName.text = string.Empty;
        itemDescription.text = string.Empty;
        for (int i = 0; i < itemRequires.Count; i++)
        {
            itemRequires[i].text = string.Empty;
        }
        button.SetActive(false);
    }
    public void CraftingItem()
    {
        if (item != null)
        {
            InventoryItems inventoryitem = item.GetInventoryItem();
            if (inventoryitem != null)
            {
                bool addItem = InventoryController.instance.AddItem(inventoryitem);
                if (addItem)
                {
                    List<CraftingRequire> requires = item.GetCraftingRequires();
                    int amount = requires.Count > itemRequires.Count ? itemRequires.Count : requires.Count;
                    for (int i = 0; i < amount; i++)
                    {
                        int removeQuantity = requires[i].quantity;
                        InventoryController.instance.RemoveItem(requires[i].itemName, removeQuantity);
                    }
                    ChangeItemTxt();
                }
                else
                {
                    ErrorController.instance.ChangeTxt("Túi đồ đầy\n (Inventory is full!)", Color.red);
                }
            }
        }
    }
}

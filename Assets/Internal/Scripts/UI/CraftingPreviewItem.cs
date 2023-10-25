using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingPreviewItem : MonoBehaviour
{
    private string itemName;
    [SerializeField] private string itemDescription;
    [SerializeField] private ItemType itemType;
    [SerializeField] private ItemName itemNamePre;
    [SerializeField] private List<CraftingRequire> craftingRequires = new List<CraftingRequire>();
    private InventoryItems inventoryItems;
    private void Start()
    {
        inventoryItems = ObjectPreferenceController.instance.GetPreferenceInventoryItem(itemType, itemNamePre);
        if (inventoryItems != null)
        {
            itemName = inventoryItems.GetPreviewName();
        }
    }
    private void Update()
    {
        if (inventoryItems == null)
        {
            inventoryItems = ObjectPreferenceController.instance.GetPreferenceInventoryItem(itemType, itemNamePre);
            if (inventoryItems != null)
            {
                itemName = inventoryItems.GetPreviewName();
            }
        }
    }
    public void PreviewItem()
    {
        CraftingPreviewController.instance.SwitchCraftingPreviewItem(this);
    }
    public string GetItemName()
    {
        return itemName;
    }
    public InventoryItems GetInventoryItem()
    {
        return inventoryItems;
    }
    public List<CraftingRequire> GetCraftingRequires()
    {
        return craftingRequires;
    }
    public string GetItemDescription()
    {
        return itemDescription;
    }

}
[System.Serializable]
public class CraftingRequire
{
    public ItemName itemName;
    public string previewName;
    public int quantity;
}
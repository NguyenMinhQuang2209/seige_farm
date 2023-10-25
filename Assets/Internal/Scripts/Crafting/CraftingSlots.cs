using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSlots : MonoBehaviour
{
    [SerializeField] private List<CraftingPreviewItem> craftingItems = new();
    public void SwitchCraftingSlots()
    {
        CraftingController.instance.SwitchShowType(craftingItems);
    }
}

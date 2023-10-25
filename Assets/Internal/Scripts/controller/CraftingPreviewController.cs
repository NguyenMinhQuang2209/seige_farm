using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingPreviewController : MonoBehaviour
{
    public static CraftingPreviewController instance;
    [SerializeField] private CraftingPreview craftingPreview;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void SwitchCraftingPreviewItem(CraftingPreviewItem item)
    {
        if (craftingPreview != null)
        {
            craftingPreview.SwitchPreviewItem(item);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftingController : MonoBehaviour
{
    public static CraftingController instance;

    [SerializeField] private GameObject craftingSlotsContainer;
    [SerializeField] private GameObject craftingItemsContainer;
    [SerializeField] private GameObject craftingItemShow;

    private List<CraftingPreviewItem> craftingItems = new();
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void SwitchShowType(List<CraftingPreviewItem> itemType)
    {
        craftingItems = itemType;
        SpawnObject();
        if (craftingSlotsContainer != null)
            craftingSlotsContainer.SetActive(false);
        if (craftingItemsContainer != null)
            craftingItemsContainer.SetActive(true);
    }
    public void OpenCrafting()
    {
        if (craftingSlotsContainer != null)
            craftingSlotsContainer.SetActive(true);
        if (craftingItemsContainer != null)
            craftingItemsContainer.SetActive(false);
    }
    private void SpawnObject()
    {
        for (int i = 0; i < craftingItemShow.transform.childCount; i++)
        {
            Destroy(craftingItemShow.transform.GetChild(i).gameObject);
        }
        if (craftingItems == null) return;
        for (int i = 0; i < craftingItems.Count; i++)
        {
            Instantiate(craftingItems[i], craftingItemShow.transform);
        }
    }
}

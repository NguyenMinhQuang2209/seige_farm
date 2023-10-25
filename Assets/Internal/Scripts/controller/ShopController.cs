using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public static ShopController instance;
    [SerializeField] private List<SellItem> sellItems = new List<SellItem>();
    [SerializeField] private int maxSellSlots = 15;
    private GameObject shop;
    private class RateComparer : IComparer<SellItem>
    {
        public int Compare(SellItem a, SellItem b)
        {
            return a.rating.CompareTo(b.rating);
        }
    }
    private void Start()
    {
        sellItems.Sort(new RateComparer());
        shop = PreferenceController.instance.shop;
    }
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public List<SellItem> GetSellerItem()
    {
        return GetListItem(true);
    }
    public List<SellItem> GetSellerItem(bool useMaximum,int amount)
    {
        return GetListItem(useMaximum, amount);
    }
    private List<SellItem> GetListItem(bool useMaximum,int amount = 1)
    {
        if (useMaximum)
        {
            amount = maxSellSlots;
        }
        List<SellItem> tempItem = new();
        int currentLength = 0;
        while(currentLength < amount)
        {
            foreach (SellItem item in sellItems)
            {
                if (currentLength == amount)
                {
                    break;
                }
                float randomRate = Random.Range(0f, 100f);
                if (randomRate <= item.rating)
                {
                    tempItem.Add(item);
                    currentLength++;
                }
            }
        }
        return tempItem;
    }
    public void UpdateShopItem(List<SellItem> items)
    {
        if(shop == null)
        {
            shop = PreferenceController.instance.shop;
        }
        if(shop.TryGetComponent<ItemPreference>(out ItemPreference shopItem))
        {
            List<GameObject> shopContainer = shopItem.GetContainChild();
            if(shopContainer.Count > 0)
            {
                GameObject container = shopContainer[0];
                for(int i = 0;i < items.Count; i++)
                {
                    if(container.transform.childCount > i)
                    {
                        GameObject sellSlot = container.transform.GetChild(i).gameObject;
                        if(sellSlot.TryGetComponent<SellSlot>(out SellSlot slot))
                        {
                            SellerItem sellerItem = items[i].item;
                            slot.UpdateSellSlot(sellerItem.GetItemImage(), 
                                sellerItem.GetPrice(), sellerItem.GetItemName(), 
                                sellerItem.GetInventoryItem(), sellerItem.GetQuantity());
                        }
                    }
                }
            }
        }
    }
}


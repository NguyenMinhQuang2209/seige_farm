using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public List<ChestCollect> chestCollects = new List<ChestCollect>();
    public static ChestController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {

    }
    public List<ChestCollectItem> GetCollections(int price, bool getLess = true)
    {
        List<ChestCollectItem> temp = new();
        if (getLess)
        {
            foreach (ChestCollect item in chestCollects)
            {
                if (item.price <= price)
                {
                    temp.AddRange(item.collections);
                }
            }
        }
        else
        {
            foreach (ChestCollect item in chestCollects)
            {
                if (item.price == price)
                {
                    temp.AddRange(item.collections);
                }
            }
        }
        return temp;
    }
}
[System.Serializable]
public class ChestCollect
{
    public int price;
    public List<ChestCollectItem> collections;
}

[System.Serializable]
public class ChestCollectItem
{
    public ItemName ItemName;
    public ItemType itemType;
    public int quantity = 0;
}

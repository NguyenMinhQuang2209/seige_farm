using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolliderBuyController : MonoBehaviour
{
    [SerializeField] private GameObject solliderContainer;
    [SerializeField] private List<BuySolliderItem> solliderItems = new List<BuySolliderItem>();
    [SerializeField] private SolliderBuyItem buySolliderItem;
    [SerializeField] private SolliderItem solliderUIItem;

    public static SolliderBuyController instance;
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
        ResetItem();
    }
    public void SolliderBuy(int location)
    {
        BuySolliderItem item = solliderItems[location].Clone();
        float remainCoin = CoinController.instance.GetCurrentCoint();
        if (remainCoin < item.price)
        {
            ErrorController.instance.ChangeTxt("Thiếu tiền \n(coin is not enough!)", Color.red);
            return;
        }
        CoinController.instance.CheckMinusCoin(item.price, true);
        Vector3 randomPos = new(Random.Range(0f, 1f), 0f, Random.Range(0f, 1f));
        Solider sollider = Instantiate(item.solliderObject, PreferenceController.instance.Player.transform.position + randomPos, Quaternion.identity);
        item.worldObject = sollider;
        solliderItems[location] = item;
        Invoke(nameof(ResetItem), 0.1f);
    }
    private void ResetItem()
    {
        foreach (Transform item in solliderContainer.transform)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < solliderItems.Count; i++)
        {
            BuySolliderItem item = solliderItems[i];
            if (item.worldObject == null)
            {
                SolliderBuyItem solliderBuyUI = Instantiate(buySolliderItem, solliderContainer.transform);
                solliderBuyUI.priceTxt.text = item.price + " coins";
                solliderBuyUI.location = i;
            }
            else
            {
                SolliderItem solliderUI = Instantiate(solliderUIItem, solliderContainer.transform);
                solliderUI.SwitchItem(item.worldObject);
            }
        }
    }
}
[System.Serializable]
public class BuySolliderItem
{
    public Solider solliderObject;
    public int price = 1;
    public Solider worldObject = null;

    public BuySolliderItem(Solider solliderObject, int price, Solider worldObject)
    {
        this.solliderObject = solliderObject;
        this.price = price;
        this.worldObject = worldObject;
    }

    public BuySolliderItem Clone()
    {
        return new BuySolliderItem(solliderObject, price, worldObject);
    }
}

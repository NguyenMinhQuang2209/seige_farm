using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ItemUpdating : MonoBehaviour
{
    [SerializeField] private List<UpdatingRequire> updatingRequires = new List<UpdatingRequire>();
    public bool showInWorld = false;

    int currentUpdate = 0;
    private float plusHealth = 0f;
    private float plusMana = 0f;
    private float plusDamage = 0f;
    private float plusRadious = 0f;
    private float plusSpeed = 0f;
    public string GetLevel()
    {
        string results = currentUpdate == updatingRequires.Count ? "Level: Max" : "Level: " + currentUpdate.ToString();
        return results;
    }
    public string GetLevelShow()
    {
        StringBuilder sb = new();
        if (currentUpdate < updatingRequires.Count)
        {
            sb.Append("U to update (");
            sb.Append(GetCoin());
            sb.Append("coins)");
        }
        else
        {
            sb.Append("Level: Max");
        }
        return sb.ToString();
    }
    public int GetCoin()
    {
        if (currentUpdate < updatingRequires.Count)
        {
            return updatingRequires[currentUpdate].price;
        }
        return 0;
    }
    public void UpdatingItem()
    {
        if (currentUpdate < updatingRequires.Count)
        {
            bool canUpdate = CoinController.instance.CheckMinusCoin(GetCoin(), true);
            if (canUpdate)
            {
                UpdatingRequire currentUpdateRequire = updatingRequires[currentUpdate];
                for (int i = 0; i < currentUpdateRequire.updateRequireItems.Count; i++)
                {
                    UpdateRequireItem require = currentUpdateRequire.updateRequireItems[i];
                    switch (require.type)
                    {
                        case UpdateItemType.Health:
                            plusHealth = require.value;
                            break;
                        case UpdateItemType.Mana:
                            plusMana = require.value;
                            break;
                        case UpdateItemType.Damage:
                            plusDamage = require.value;
                            break;
                        case UpdateItemType.Radious:
                            plusRadious = require.value;
                            break;
                        case UpdateItemType.Speed:
                            plusSpeed = require.value;
                            break;
                    }
                }
                currentUpdate += 1;
            }
            else
            {
                ErrorController.instance.ChangeTxt("Thiếu tiền \n(coin is not enough!)", Color.red);
            }
        }
    }
    public float GetPlusValue(UpdateItemType type)
    {
        switch (type)
        {
            case UpdateItemType.Health:
                return plusHealth;
            case UpdateItemType.Mana:
                return plusMana;
            case UpdateItemType.Damage:
                return plusDamage;
            case UpdateItemType.Radious:
                return plusRadious;
            default:
                break;
        }
        return 0f;
    }
}
[System.Serializable]
public class UpdatingRequire
{
    public List<UpdateRequireItem> updateRequireItems = new List<UpdateRequireItem>();
    public int price;
}
[System.Serializable]
public class UpdateRequireItem
{
    public UpdateItemType type;
    public float value;
}

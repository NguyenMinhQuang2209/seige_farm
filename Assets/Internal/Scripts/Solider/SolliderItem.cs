using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SolliderItem : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI hpTxt;
    public TextMeshProUGUI damageTxt;
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI priceTxt;
    public TextMeshProUGUI recoverTxt;
    public TextMeshProUGUI radiousTxt;
    private Solider sollider;
    public void SwitchItem(Solider newSolider)
    {
        sollider = newSolider;
        UpdateItem();
    }
    public void UpdateSollider()
    {
        if (sollider != null)
        {
            sollider.UpdateItem();
            Invoke(nameof(UpdateItem), 0.1f);
        }
    }
    public void RecoverHealthSollider()
    {
        if (sollider != null)
        {
            sollider.RecoverHealthItem();
            Invoke(nameof(UpdateItem), 0.1f);
        }
    }
    public void UpdateItem()
    {
        if (sollider != null)
        {
            Texture2D texture = sollider.GetItemImage();
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            itemImage.sprite = sprite;
            hpTxt.text = "HP: " + sollider.GetItemHP();
            damageTxt.text = "Damage: " + sollider.GetCurrentDamage() + "";
            levelTxt.text = sollider.GetNextUpdate();
            priceTxt.text = sollider.GetCoin() + "";
            recoverTxt.text = sollider.GetRecoverHealth() + "";
            radiousTxt.text = "Radious: " + sollider.GetRadious();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class SolliderBuyItem : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI priceTxt;
    [HideInInspector] public int location = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        SolliderBuyController.instance.SolliderBuy(location);
    }
}

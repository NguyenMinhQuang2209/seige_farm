using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foods : MonoBehaviour
{
    [SerializeField] private float reHealth = 1f;
    [SerializeField] private float reMana = 1f;
    [SerializeField] private float reFood = 1f;

    private InventoryItems inventoryItem;
    private void Start()
    {
        inventoryItem = GetComponent<InventoryItems>();
    }

    public float GetReHealth()
    {
        return reHealth;
    }
    public float GetReMana()
    {
        return reMana;
    }
    public float GetReFood()
    {
        return reFood;
    }
    public void UseFood()
    {
        if (PreferenceController.instance.Player.TryGetComponent<PlayerHealth>(out PlayerHealth player))
        {
            player.Recover(reHealth,reMana,reFood);
            if (inventoryItem != null)
            {
                inventoryItem.ChangeQuantity(-1);
                inventoryItem.CheckQuantity();
                InventoryController.instance.UpdateStock();
            }
        }
    }
}

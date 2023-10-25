using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : Interactible
{
    [SerializeField] private InventoryItems inventoryItems;
    [SerializeField] private int quantity = 1;
    private void Start()
    {
        promptMessage = string.Empty;
    }
    protected override void Interact()
    {
        PickupItem();
    }
    private void PickupItem()
    {
        bool addingStatus = InventoryController.instance.AddItem(inventoryItems, quantity);
        if (addingStatus) Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagController.playerTag))
        {
            PickupItem();
        }
    }
}

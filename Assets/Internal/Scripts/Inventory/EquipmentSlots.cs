using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlots : MonoBehaviour, IDropHandler
{
    [SerializeField] private Transform container;
    [SerializeField] private GameObject defaultItems;
    [SerializeField] private List<ItemType> itemTypeList = new();
    [SerializeField] private EquipmentType type;
    ItemName? itemName = null;
    ItemType? itemType = null;
    private void Update()
    {
        defaultItems.SetActive(container.childCount == 0);
        if (container.childCount > 0)
        {
            GameObject child = container.GetChild(0).gameObject;
            if (child.TryGetComponent<InventoryItems>(out InventoryItems item))
            {
                if (!item.GetItemName().Equals(itemName))
                {
                    itemName = item.GetItemName();
                    itemType = item.GetItemType();
                    EquipmentController.instance.EquipmentObject(type, item.GetEquipmentMaterial(), item.GetEquipmentObject());
                    if (type.Equals(EquipmentType.Hand))
                    {
                        EquipmentController.instance.UpdateHandHoldingItem(itemType);
                    }
                }
            }
        }
        else
        {
            if (itemName != null)
            {
                EquipmentController.instance.EquipmentObject(type, null, null);
                itemName = null;
                itemType = null;
                if (type.Equals(EquipmentType.Hand))
                {
                    EquipmentController.instance.UpdateHandHoldingItem(itemType);
                }
            }
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject item = eventData.pointerDrag;
        if (item.TryGetComponent<InventoryItems>(out InventoryItems itemObject))
        {
            if (itemTypeList.Contains(itemObject.GetItemType()))
            {
                if (container.childCount == 0)
                {
                    itemObject.rootParent = container;
                    return;
                }
                GameObject child = container.GetChild(0).gameObject;
                Transform itemParent = itemObject.rootParent;
                itemObject.rootParent = child.transform.parent;
                child.transform.SetParent(itemParent, false);

            }
        }
    }
    public GameObject GetEquipmentItem()
    {
        if (container.childCount > 0)
        {
            GameObject child = container.GetChild(0).gameObject;
            if (child.GetComponent<InventoryItems>() != null)
            {
                return child;
            }
        }
        return null;
    }
}

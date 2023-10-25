using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class InventorySlots : MonoBehaviour, IDropHandler
{
    [SerializeField] private Transform container;
    public void OnDrop(PointerEventData eventData)
    {
        GameObject item = eventData.pointerDrag;
        if(item.TryGetComponent<InventoryItems>(out InventoryItems itemObject))
        {
            if (container.childCount == 0)
            {
                itemObject.rootParent = container;
                return;
            }
            GameObject child = container.GetChild(0).gameObject;
            if (child.TryGetComponent<InventoryItems>(out InventoryItems childObject))
            {
                if (childObject.CanAdding() && childObject.CheckEqual(itemObject.GetItemName(), itemObject.GetItemType()))
                {
                    int remain = childObject.ChangeQuantity(itemObject.GetCurrentQuantity());
                    itemObject.UpdateQuantity(remain);
                    itemObject.CheckQuantity();
                }
            }
        }
    }
    public Transform GetContainer()
    {
        return container;
    }
    public bool IsEmpty(InventoryItems i,int quantity)
    {
        if(container.childCount == 0)
        {
            return true;
        }
        GameObject item = container.GetChild(0).gameObject;
        if(item.TryGetComponent<InventoryItems>(out InventoryItems target))
        {
            if (target.CheckEqual(i))
            {
                return target.CanAdding(quantity);
            }
        }
        return false;
    }
    public void AddingItem(InventoryItems item,int quantity)
    {
        if (container.childCount == 0)
        {
            InventoryItems i = Instantiate(item, container.transform);
            i.UpdateQuantity(quantity);
            return;
        }
        GameObject slot = container.GetChild(0).gameObject;
        if (slot.TryGetComponent<InventoryItems>(out InventoryItems target))
        {
            target.ChangeQuantity(quantity);
        }
    }
    public InventoryItems GetItem()
    {
        if (container.childCount == 0)
        {
            return null;
        }
        return container.GetChild(0).gameObject.GetComponent<InventoryItems>();
    }
}

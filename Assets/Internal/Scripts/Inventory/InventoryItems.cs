using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class InventoryItems : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public Transform rootParent;
    [SerializeField] private int maxQuantity = 1;
    [SerializeField] private bool useStack;
    [SerializeField] private ItemName itemName;
    [SerializeField] private ItemType itemType;
    [SerializeField] private TextMeshProUGUI quantityString;
    [SerializeField] private int currentQuantity = 1;
    [SerializeField] private Image image;
    Transform canvasRoot;

    [Header("Building Object")]
    [SerializeField] private BuildingItem buildingItem;

    [Header("Equipment Object")]
    [SerializeField] private GameObject equipmentObject;
    [SerializeField] private Material material;


    [Header("Preview")]
    [SerializeField] private string previewContent = "";
    private void Start()
    {
        rootParent = transform.parent;
        ChangeQuantityString(useStack ? currentQuantity.ToString() : string.Empty);
        canvasRoot = InventoryController.instance.GetRootSpawn();
    }
    public Material GetEquipmentMaterial()
    {
        return material;
    }
    public GameObject GetEquipmentObject()
    {
        return equipmentObject;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        rootParent = transform.parent;
        transform.SetParent(canvasRoot, false);
        image.raycastTarget = false;
        transform.SetAsLastSibling();
        ChangeQuantityString(string.Empty);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(rootParent, false);
        transform.SetAsLastSibling();
        image.raycastTarget = true;
        ChangeQuantityString(useStack ? currentQuantity.ToString() : string.Empty);
    }
    private void ChangeQuantityString(string newQuantityString)
    {
        quantityString.text = newQuantityString;
    }
    public bool CanAdding(int quantity = 1)
    {
        return useStack && currentQuantity + quantity <= maxQuantity;
    }
    public int ChangeQuantity(int quantity)
    {
        currentQuantity += quantity;
        if (currentQuantity > maxQuantity)
        {
            int temp = currentQuantity;
            currentQuantity = maxQuantity;
            ChangeQuantityString(useStack ? currentQuantity.ToString() : string.Empty);
            return temp - maxQuantity;
        }
        if (currentQuantity <= 0)
        {
            int temp = currentQuantity;
            currentQuantity = 0;
            ChangeQuantityString(useStack ? currentQuantity.ToString() : string.Empty);
            return temp;
        }
        ChangeQuantityString(useStack ? currentQuantity.ToString() : string.Empty);
        return 0;
    }
    public void CheckQuantity()
    {
        if (currentQuantity <= 0)
        {
            Destroy(gameObject);
        }
    }
    public int GetCurrentQuantity()
    {
        return currentQuantity;
    }
    public void UpdateQuantity(int newQuantity)
    {
        currentQuantity = newQuantity;
        ChangeQuantityString(useStack ? currentQuantity.ToString() : string.Empty);
    }
    public ItemName GetItemName()
    {
        return itemName;
    }
    public ItemType GetItemType()
    {
        return itemType;
    }
    public bool CheckEqual(ItemName? name, ItemType? type)
    {
        return itemName.Equals(name) && itemType.Equals(type);
    }
    public bool CheckEqual(ItemName? name)
    {
        return itemName.Equals(name);
    }
    public bool CheckEqual(InventoryItems item)
    {
        return itemName.Equals(item.GetItemName()) && itemType.Equals(item.GetItemType());
    }
    public void ItemUse()
    {
        switch (itemType)
        {
            case ItemType.Building:
                if (PreferenceController.instance.Player.TryGetComponent<PlayerBuilding>(out PlayerBuilding playerBuilding))
                {
                    playerBuilding.SwitchBuildingObject(buildingItem, itemName, itemType);
                    CursorController.instance.CloseAll();
                }
                break;
        }
    }
    public string GetPreviewName()
    {
        return previewContent;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PreviewController.instance.ChangePreviewTxt(previewContent, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PreviewController.instance.ChangePreviewTxt(string.Empty, transform.position);
    }
}

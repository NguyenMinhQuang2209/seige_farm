using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilding : MonoBehaviour
{
    private BuildingItem currentBuildingItem;
    private Transform currentPreview = null;
    PreviewBuildingItem currentPriviewBuildingItem;
    Vector3 currentPos = Vector3.zero;
    Vector3 currentRot;
    private PlayerInput input;

    bool isColliding = false;
    private PlayerMovement playerMovement;
    ItemName? itemName;
    ItemType? itemType;
    int currentQuantity = 0;

    [SerializeField] private float waitAttackTime = 2f;
    bool isBuzy = false;

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        currentPriviewBuildingItem = null;
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        ShowPreview();
        if (input.onFoot.Spawn.triggered)
        {
            SpawnObject();
        }
        if (input.onFoot.Cancel.triggered)
        {
            SwitchBuildingObject(null, null, null);
        }
    }
    private void LateUpdate()
    {
        playerMovement.IsBuilding(isBuzy);
    }
    public void SwitchBuildingObject(BuildingItem newBuildingObject, ItemName? itemName, ItemType? itemType)
    {
        if (newBuildingObject != null)
        {
            isBuzy = true;
        }
        else
        {
            Invoke(nameof(ChangeBuzy), waitAttackTime);
        }
        this.itemName = itemName;
        this.itemType = itemType;
        if (currentPreview != null)
        {
            Destroy(currentPreview.gameObject);
        }
        currentBuildingItem = newBuildingObject;
        if (currentBuildingItem == null)
        {
            currentPriviewBuildingItem = null;
            return;
        }
        currentQuantity = this.itemName == null ? 0 : InventoryController.instance.GetQuantityInStock(this.itemName);
        GameObject newItem = Instantiate(newBuildingObject.previewObject, currentPos, Quaternion.Euler(currentRot));
        if (newItem.TryGetComponent<PreviewBuildingItem>(out PreviewBuildingItem item))
        {
            currentPriviewBuildingItem = item;
        }
        currentPreview = newItem.transform;
    }
    private void ChangeBuzy()
    {
        isBuzy = false;
    }
    public void ShowPreview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (isColliding)
            {
                float distance = Vector3.Distance(hit.point, currentPos);
                if (distance >= 0.5f)
                {
                    currentPos = hit.point;
                    if (currentPreview != null)
                    {
                        currentPreview.position = hit.point;
                    }
                    isColliding = false;
                }
                return;
            }
            if (currentPriviewBuildingItem != null)
            {
                if (currentPriviewBuildingItem.IsColliding())
                {
                    if (currentPreview != null)
                    {
                        currentPreview.position = currentPriviewBuildingItem.TargetPos();
                    }
                    currentPos = currentPriviewBuildingItem.TargetPos();
                    isColliding = true;
                }
                else
                {
                    currentPos = hit.point;
                    if (currentPreview != null)
                    {
                        currentPreview.position = hit.point;
                    }
                }
            }
            else
            {
                currentPos = hit.point;
                if (currentPreview != null)
                {
                    currentPreview.position = hit.point;
                }
            }
        }
    }
    public void SpawnObject()
    {
        if (currentPreview != null && itemName != null && itemType != null)
        {
            Instantiate(currentBuildingItem.worldObject, currentPos, Quaternion.Euler(currentRot));
            InventoryController.instance.RemoveItem(itemName, itemType, 1);
            currentQuantity = InventoryController.instance.GetQuantityInStock(itemName);
            if (currentQuantity == 0)
            {
                SwitchBuildingObject(null, null, null);
            }
        }
    }
}
[System.Serializable]
public class BuildingItem
{
    public GameObject worldObject;
    public GameObject previewObject;
}
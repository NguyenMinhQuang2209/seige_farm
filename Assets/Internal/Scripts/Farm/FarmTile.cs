using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTile : Interactible
{
    public static string defaultMessage = "Farm Tile";
    private PlanetProcessItem planetProcessItem;
    bool havingItem = false;
    int currentIndex = 0;
    float currentTime = 0;
    float targetTime = 0;
    GameObject currentObject = null;
    bool canCollect = false;
    private void Start()
    {
        promptMessage = defaultMessage;
    }
    protected override void Interact()
    {
        if (canCollect && planetProcessItem != null)
        {
            int randomCollect = (int)Random.Range(Mathf.Min(planetProcessItem.planetCollectRange.x, planetProcessItem.planetCollectRange.y), Mathf.Max(planetProcessItem.planetCollectRange.x, planetProcessItem.planetCollectRange.y));
            bool canAdd = InventoryController.instance.AddItem(planetProcessItem.planetCollect, randomCollect);
            if (!canAdd)
            {
                ErrorController.instance.ChangeTxt("Túi đồ đầy\n (Inventory is full!)", Color.red);
            }
            else
            {
                canCollect = false;
                havingItem = false;
                currentIndex = 0;
                currentTime = 0;
                planetProcessItem = null;
                promptMessage = defaultMessage;
                Destroy(currentObject);
            }
            return;
        }
        ItemType? type = EquipmentController.instance.GetHandEquipmentType();
        GameObject handHolding = EquipmentController.instance.GetHandEquipment();
        if (handHolding == null || havingItem)
        {
            return;
        }
        switch (type)
        {
            case ItemType.Seed:
                if (handHolding.TryGetComponent<PlanetTree>(out PlanetTree item))
                {
                    GameObject handHold = EquipmentController.instance.GetHandEquipmentUI();
                    if (handHold.TryGetComponent<InventoryItems>(out InventoryItems invenItem))
                    {
                        invenItem.ChangeQuantity(-1);
                        if (invenItem.GetCurrentQuantity() <= 0)
                        {
                            EquipmentController.instance.EquipmentObject(EquipmentType.Hand, null, null);
                        }
                        invenItem.CheckQuantity();
                    }
                    planetProcessItem = item.GetPlanetProcess();
                    havingItem = true;
                    canCollect = false;
                    currentIndex = 0;
                    currentTime = 0f;
                    if (planetProcessItem.planetProcesses.Count > 0)
                    {
                        targetTime = planetProcessItem.planetProcesses[currentIndex].growTime;
                        currentObject = Instantiate(planetProcessItem.planetProcesses[currentIndex].planetObject, transform);
                    }
                }
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        if (havingItem)
        {
            if (currentIndex <= planetProcessItem.planetProcesses.Count - 1)
            {
                int minutes = Mathf.FloorToInt(((targetTime - currentTime) % 3600f) / 60f);
                int seconds = Mathf.FloorToInt((targetTime - currentTime) % 60f);
                promptMessage = "Period: " + (currentIndex + 1) + " / " + planetProcessItem.planetProcesses.Count + " \n" + minutes.ToString("D2") + ":" + seconds.ToString("D2");
                currentTime += Time.deltaTime;
                canCollect = false;
            }
            else
            {
                promptMessage = "Collecting";
                canCollect = true;
            }
            if (currentTime >= targetTime)
            {
                SwitchProcess();
            }
        }
    }
    public void SwitchProcess()
    {
        currentIndex += 1;
        if (currentIndex <= planetProcessItem.planetProcesses.Count - 1)
        {
            currentTime = 0f;
            Destroy(currentObject);
            targetTime = planetProcessItem.planetProcesses[currentIndex].growTime;
            currentObject = Instantiate(planetProcessItem.planetProcesses[currentIndex].planetObject, transform);
            return;
        }
    }
}

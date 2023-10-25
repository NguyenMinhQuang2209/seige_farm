using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public static BoxController instance;
    private Box currentBox = null;
    private GameObject boxUI;
    private GameObject rabishUI;

    private List<GameObject> mutipleBox = new List<GameObject>();
    bool useMultipleBox = false;
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        boxUI = PreferenceController.instance.box;
        rabishUI = PreferenceController.instance.rabish;
        for(int i = 0;i < PreferenceController.instance.inventorySlots; i++)
        {
            mutipleBox.Add(null);
        }
    }
    public void SwitchBox(Box newBox,bool isMutipleBox = false)
    {
        useMultipleBox = isMutipleBox;
        if (currentBox == newBox)
        {
            return;
        }
        currentBox = newBox;
        if(currentBox != null)
        {
            SpawnObject();
        }
    }
    private void SpawnObject()
    {
        List<GameObject> boxList = !useMultipleBox ? currentBox.GetBoxItemContain() : mutipleBox;
        if (boxUI != null && boxUI.TryGetComponent<ItemPreference>(out ItemPreference mainChild))
        {
            List<GameObject> list = mainChild.GetContainChild();
            if (list.Count > 0)
            {
                GameObject item = list[0];
                for(int  i = 0;i < item.transform.childCount;i++)
                {
                    GameObject slot = item.transform.GetChild(i).gameObject;
                    if(slot.TryGetComponent<InventorySlots>(out InventorySlots slotItem))
                    {
                        foreach (Transform slotChild in slotItem.GetContainer().transform)
                        {
                            Destroy(slotChild.gameObject);
                        }
                    }
                    slot.SetActive(false);
                    if (i < currentBox.GetSlots())
                    {
                        slot.SetActive(true);
                        GameObject box = boxList[i];
                        if (box != null)
                        {
                            Instantiate(box, slotItem.GetContainer().transform, false);
                        }
                    }
                }
            }
        }
    }
    public void UpdateBoxItems()
    {
        if(currentBox != null)
        {
            List<GameObject> boxList = !useMultipleBox ? currentBox.GetBoxItemContain() : mutipleBox;
            if (boxUI != null && boxUI.TryGetComponent<ItemPreference>(out ItemPreference mainChild)) 
            {
                List<GameObject> list = mainChild.GetContainChild();
                if (list.Count > 0)
                {
                    for(int  i = 0; i < currentBox.GetSlots(); i++)
                    {
                        if (boxList[i] != null)
                        {
                            Destroy(boxList[i]);
                        }
                        GameObject slot = list[0].transform.GetChild(i).gameObject;
                        if (slot.TryGetComponent<InventorySlots>(out InventorySlots slotItem))
                        {
                            InventoryItems item = slotItem.GetItem();
                            if(item != null)
                            {
                                GameObject itemPre = Instantiate(item.gameObject, rabishUI.transform, false);
                                boxList[i] = itemPre;
                            }
                            else
                            {
                                boxList[i] = null;
                            }
                        }
                        else
                        {
                            boxList[i] = null;
                        }
                    }
                }
            }
        }
    }
}

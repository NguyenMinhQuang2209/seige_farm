using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Interactible
{
    private GameObject box;
    private GameObject inventory;
    private List<GameObject> boxContainItems = new List<GameObject>();
    private int numberSlots = 1;
    [SerializeField] private int maxSlots = 1;
    [SerializeField] private bool useMaxInventorySlots = false;
    [SerializeField] private bool multipleBox = false;
    private void Start()
    {
        box = PreferenceController.instance.box;
        inventory = PreferenceController.instance.inventory;
        maxSlots = useMaxInventorySlots ? PreferenceController.instance.inventorySlots : Mathf.Min(maxSlots, PreferenceController.instance.inventorySlots);
        numberSlots = Random.Range(1, maxSlots);
        for (int i = 0; i < numberSlots; i++)
        {
            boxContainItems.Add(null);
        }
    }
    protected override void Interact()
    {
        CursorController.instance.TriggerCursor("box", new List<GameObject>() { box, inventory });
        BoxController.instance.SwitchBox(this, multipleBox);
    }
    public List<GameObject> GetBoxItemContain()
    {
        return boxContainItems;
    }
    public int GetSlots()
    {
        return numberSlots;
    }
}

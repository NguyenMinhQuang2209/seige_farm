using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject uiContainer;
    private GameObject inventory;
    private GameObject switchButton;
    private PlayerInput input;
    bool openInventory = false;
    private void Start()
    {
        input = GetComponent<PlayerInput>();
        if (uiContainer != null)
        {
            foreach (Transform child in uiContainer.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        inventory = PreferenceController.instance.inventory;
        switchButton = PreferenceController.instance.switchButton;
    }
    private void Update()
    {
        if (input.onFoot.Inventory.triggered)
        {
            openInventory = !openInventory;
            if (!openInventory)
            {
                CursorController.instance.TriggerCursor("", null);
            }
            else
            {
                CursorController.instance.TriggerCursor("Inventory", new List<GameObject>() { inventory, switchButton });
            }
        }
        if (input.onFoot.Cancel.triggered)
        {
            CursorController.instance.TriggerCursor("", null);
        }
    }
}

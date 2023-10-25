using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchButton : MonoBehaviour
{
    string currentItem = string.Empty;
    private void Update()
    {
        currentItem = CursorController.instance.CurrentCursorName();
    }
    public void OpenInventory()
    {
        if (!currentItem.Equals("Inventory"))
        {
            currentItem = "Inventory";
            CursorController.instance.TriggerCursor("Inventory", new List<GameObject>() {
                PreferenceController.instance.inventory,
                PreferenceController.instance.switchButton});
        }
    }
    public void OpenCrafting()
    {
        if (!currentItem.Equals("Crafting"))
        {
            currentItem = "Crafting";
            CursorController.instance.TriggerCursor("Crafting", new List<GameObject>() {
                PreferenceController.instance.crafting,
                PreferenceController.instance.switchButton});
            CraftingController.instance.OpenCrafting();
        }
    }
    public void OpenSolliderUI()
    {
        if (!currentItem.Equals("SolliderUI"))
        {
            currentItem = "SolliderUI";
            CursorController.instance.TriggerCursor("SolliderUI", new List<GameObject>() {
                PreferenceController.instance.solliderUI,
                PreferenceController.instance.switchButton});
        }
    }
    public void OpenSettings()
    {
        if (!currentItem.Equals("Settings"))
        {
            currentItem = "Settings";
            CursorController.instance.TriggerCursor("Settings", new List<GameObject>() {
                PreferenceController.instance.setting,
                PreferenceController.instance.switchButton});
        }
    }
    public void OpenHowToPlay()
    {
        if (!currentItem.Equals("HowToPlay"))
        {
            currentItem = "HowToPlay";
            CursorController.instance.TriggerCursor("HowToPlay", new List<GameObject>() {
                PreferenceController.instance.howtoplay,
                PreferenceController.instance.switchButton});
        }
    }
    public void ClearAll()
    {
        currentItem = string.Empty;
    }
}

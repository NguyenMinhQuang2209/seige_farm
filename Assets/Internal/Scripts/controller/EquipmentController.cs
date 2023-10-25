using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
public class EquipmentController : MonoBehaviour
{
    public static EquipmentController instance;
    private GameObject character;
    ItemType? handItemType;

    [SerializeField] private GameObject handEquipment;
    [SerializeField] private GameObject hatEquipment;
    [SerializeField] private GameObject shirtEquipment;
    [SerializeField] private GameObject pantEquipment;
    [SerializeField] private GameObject shoeEquipment;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        if (PreferenceController.instance != null)
        {
            character = PreferenceController.instance.Character;
        }
    }
    public EquipmentItem GetEquipmentInventoryItem(EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.Shoe:
                if (shoeEquipment.TryGetComponent<EquipmentSlots>(out EquipmentSlots shoe))
                {
                    GameObject child = shoe.GetEquipmentItem();
                    if (child != null)
                    {
                        return child.GetComponent<EquipmentItem>();
                    }
                }
                break;
            case EquipmentType.Pant:
                if (pantEquipment.TryGetComponent<EquipmentSlots>(out EquipmentSlots pant))
                {
                    GameObject child = pant.GetEquipmentItem();
                    if (child != null)
                    {
                        return child.GetComponent<EquipmentItem>();
                    }
                }
                break;
            case EquipmentType.Hat:
                if (hatEquipment.TryGetComponent<EquipmentSlots>(out EquipmentSlots hat))
                {
                    GameObject child = hat.GetEquipmentItem();
                    if (child != null)
                    {
                        return child.GetComponent<EquipmentItem>();
                    }
                }
                break;
            case EquipmentType.Shirt:
                if (shirtEquipment.TryGetComponent<EquipmentSlots>(out EquipmentSlots shirt))
                {
                    GameObject child = shirt.GetEquipmentItem();
                    if (child != null)
                    {
                        return child.GetComponent<EquipmentItem>();
                    }
                }
                break;
        }
        return null;
    }
    public void EquipmentObject(EquipmentType type, Material color, GameObject item = null)
    {
        if (character != null)
        {
            if (character.TryGetComponent<CharacterEquipment>(out CharacterEquipment target))
            {
                target.UpdateEquipment(type, item, color);
            }
        }
    }
    public GameObject GetHandEquipment()
    {
        if (character != null)
        {
            if (character.TryGetComponent<CharacterEquipment>(out CharacterEquipment target))
            {
                return target.GetHandWeapon();
            }
        }
        return null;
    }
    public void UpdateHandHoldingItem(ItemType? itemType)
    {
        handItemType = itemType;
    }
    public ItemType? GetHandEquipmentType()
    {
        return handItemType;
    }

    public GameObject GetHandEquipmentUI()
    {
        if (handEquipment.TryGetComponent<EquipmentSlots>(out EquipmentSlots slot))
        {
            return slot.GetEquipmentItem();
        }
        return null;
    }
}
public enum EquipmentType
{
    Hand,
    Hat,
    Shirt,
    Pant,
    Shoe,
    Other
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    [SerializeField] private Transform handHolding;
    [SerializeField] private SkinnedMeshRenderer hat;
    [SerializeField] private SkinnedMeshRenderer shirt;
    [SerializeField] private SkinnedMeshRenderer pant;
    [SerializeField] private SkinnedMeshRenderer shoe;
    Material defaultHat;
    Material defaultShirt;
    Material defaultShoe;
    Material defaultPant;

    private GameObject player;
    private void Start()
    {
        defaultHat = hat.material;
        defaultShirt = shirt.material;
        defaultPant = pant.material;
        defaultShoe = shoe.material;
        if (PreferenceController.instance != null)
        {
            player = PreferenceController.instance.Player;
        }
    }
    private void Update()
    {
        if (player == null)
        {
            if (PreferenceController.instance != null)
            {
                player = PreferenceController.instance.Player;
            }
        }
    }
    public void UpdateEquipment(EquipmentType type, GameObject item, Material color)
    {
        if (!type.Equals(EquipmentType.Hand))
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            EquipmentItem shirtEquipment = EquipmentController.instance.GetEquipmentInventoryItem(type);
            if (shirtEquipment != null)
            {
                playerController.GetEquipmentItem(type, shirtEquipment.GetHealth(), shirtEquipment.GetMana(), shirtEquipment.GetFood(), shirtEquipment.GetSpeed());
            }
            else
            {
                playerController.GetEquipmentItem(type, 0, 0, 0, 0);
            }
        }
        switch (type)
        {
            case EquipmentType.Hand:
                foreach (Transform child in handHolding)
                {
                    Destroy(child.gameObject);
                }
                if (item != null)
                {
                    Instantiate(item, handHolding.transform);
                }
                break;
            case EquipmentType.Shirt:
                shirt.material = color != null ? color : defaultShirt;
                break;
            case EquipmentType.Pant:
                pant.material = color != null ? color : defaultPant;
                break;
            case EquipmentType.Shoe:
                shoe.material = color != null ? color : defaultShoe;
                break;
            case EquipmentType.Hat:
                hat.material = color != null ? color : defaultHat;
                break;
            default:
                break;

        }
    }
    public GameObject GetHandWeapon()
    {
        return handHolding.transform.childCount > 0 ? handHolding.transform.GetChild(0).gameObject : null;
    }
}

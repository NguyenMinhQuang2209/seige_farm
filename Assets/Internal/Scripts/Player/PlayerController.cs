using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;

    EquipmentManagement shirt = new(EquipmentType.Shirt);
    EquipmentManagement pant = new(EquipmentType.Pant);
    EquipmentManagement shoe = new(EquipmentType.Shoe);
    EquipmentManagement hat = new(EquipmentType.Hat);

    private class EquipmentManagement
    {
        public EquipmentType type;
        public float plusHealth = 0f;
        public float plusMana = 0f;
        public float plusFood = 0f;
        public float plusSpeed = 0f;
        public EquipmentManagement(EquipmentType type)
        {
            this.type = type;
        }
        public EquipmentManagement(EquipmentType type, float plusHealth, float plusMana, float plusFood, float plusSpeed)
        {
            this.type = type;
            this.plusHealth = plusHealth;
            this.plusMana = plusMana;
            this.plusFood = plusFood;
            this.plusSpeed = plusSpeed;
        }

    }
    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    public void GetEquipmentItem(EquipmentType type, float plusHealth, float plusMana, float plusFood, float plusSpeed)
    {
        switch (type)
        {
            case EquipmentType.Hat:
                hat.plusHealth = plusHealth;
                hat.plusMana = plusMana;
                hat.plusFood = plusFood;
                hat.plusSpeed = plusSpeed;
                break;
            case EquipmentType.Shirt:
                shirt.plusHealth = plusHealth;
                shirt.plusMana = plusMana;
                shirt.plusFood = plusFood;
                shirt.plusSpeed = plusSpeed;
                break;
            case EquipmentType.Pant:
                pant.plusHealth = plusHealth;
                pant.plusMana = plusMana;
                pant.plusFood = plusFood;
                pant.plusSpeed = plusSpeed;
                break;
            case EquipmentType.Shoe:
                shoe.plusHealth = plusHealth;
                shoe.plusMana = plusMana;
                shoe.plusFood = plusFood;
                shoe.plusSpeed = plusSpeed;
                break;
        }
        float newPlusHealth = shirt.plusHealth + hat.plusHealth + pant.plusHealth + shoe.plusHealth;
        float newplusMana = shirt.plusMana + hat.plusMana + pant.plusMana + shoe.plusMana;
        float newplusFood = shirt.plusFood + hat.plusFood + pant.plusFood + shoe.plusFood;
        float newplusSpeed = shirt.plusSpeed + hat.plusSpeed + pant.plusSpeed + shoe.plusSpeed;

        playerHealth.UpdatePlus(newPlusHealth, newplusMana, newplusFood);
        playerMovement.ChangePlusSpeed(newplusSpeed);
    }
}

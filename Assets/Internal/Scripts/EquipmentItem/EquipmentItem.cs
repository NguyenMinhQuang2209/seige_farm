using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentItem : MonoBehaviour
{
    [SerializeField] private float health = 0f;
    [SerializeField] private float mana = 0f;
    [SerializeField] private float food = 0f;
    [SerializeField] private float speed = 0f;

    public float GetHealth()
    {
        return health;
    }
    public float GetMana()
    {
        return mana;
    }
    public float GetFood()
    {
        return food;
    }
    public float GetSpeed()
    {
        return speed;
    }
}

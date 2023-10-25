using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponConfig : MonoBehaviour
{
    [SerializeField] private float treeDamage = 1f;
    [SerializeField] private float rockDamage = 1f;
    [SerializeField] private float enemyDamage = 1f;
    [SerializeField] private ItemType type;
    [SerializeField] private float timeBwtAttack = 1f;
    [SerializeField] private bool useCustomTimeBwtAttack = false;

    public float GetTreeDamage()
    {
        return treeDamage;
    }
    public float GetRockDamage()
    {
        return rockDamage;
    }
    public float GetEnemyDamage()
    {
        return enemyDamage;
    }
    public ItemType GetItemType()
    {
        return type;
    }
    public bool GetTimeBwtAttackCustom()
    {
        return useCustomTimeBwtAttack;
    }
    public float GetTimeBwtAttack()
    {
        return timeBwtAttack;
    }
}

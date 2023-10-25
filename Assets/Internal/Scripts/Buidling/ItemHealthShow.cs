using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealthShow : Interactible
{
    private ItemOffset itemOffset;
    float currentHealth = 0f;
    float maxHealth = 0f;
    private void Start()
    {
        if (TryGetComponent<ItemOffset>(out itemOffset))
        {
            currentHealth = itemOffset.GetCurrentHealth();
            maxHealth = itemOffset.GetMaxHealth();
        }
    }
    private void Update()
    {
        if (itemOffset != null)
        {
            currentHealth = itemOffset.GetCurrentHealth();
            maxHealth = itemOffset.GetMaxHealth();
        }
        promptMessage = currentHealth.ToString("0") + "/" + maxHealth;
    }
}

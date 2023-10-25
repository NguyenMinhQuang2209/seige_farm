using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private Slider healthSlide;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private TextMeshProUGUI healthTxt;
    float currentHealth;

    [Header("Mana")]
    [SerializeField] private Slider manaSlide;
    [SerializeField] private float maxMana = 100f;
    [SerializeField] private float recoverManaRate = 1f;
    [SerializeField] private float delayTimeRecover = 1f;
    [SerializeField] private TextMeshProUGUI manaTxt;

    [SerializeField] private float delayWaitTime = 2f;
    float currentDelayWaitTime = 0f;
    float currentDelayTime = 0f;
    float currentMana;
    bool useMana = false;

    [Header("Foods")]
    [SerializeField] private Slider foodSlide;
    [SerializeField] private float maxFood = 100f;
    [SerializeField] private float foodRate = 1f;
    [SerializeField] private float foodRateMana = 1f;
    [SerializeField] private TextMeshProUGUI foodTxt;
    float currentFood;

    [Header("Respawn Object")]
    [SerializeField] private Vector3 respawnPosition = Vector3.zero;
    [SerializeField] private float respawnTime = 2f;

    bool objectDie = false;


    float plusHealth = 0f;
    float plusMana = 0f;
    float plusFood = 0f;
    private void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        currentFood = maxFood;

        healthSlide.minValue = 0;
        manaSlide.minValue = 0;
        foodSlide.minValue = 0;

        SliderUpdate();

        healthSlide.value = currentHealth;
        manaSlide.value = currentMana;
        foodSlide.value = currentFood;

        currentDelayWaitTime = delayWaitTime;
    }
    private void SliderUpdate()
    {
        healthSlide.maxValue = maxHealth + plusHealth;
        manaSlide.maxValue = maxMana + plusMana;
        foodSlide.maxValue = maxFood + plusFood;
    }
    private void Update()
    {
        currentDelayWaitTime = Mathf.Min(delayWaitTime, currentDelayWaitTime + Time.deltaTime);
        if (transform.position.y <= -50f)
        {
            transform.position = respawnPosition;
            objectDie = true;
        }
        healthTxt.text = (int)currentHealth + "/" + (int)(maxHealth + plusHealth);
        manaTxt.text = (int)currentMana + "/" + (int)(maxMana + plusMana);
        foodTxt.text = (int)currentFood + "/" + (int)(maxFood + plusFood);
        if (ObjectDie())
        {
            return;
        }

        currentFood = Mathf.Max(0, currentFood - Time.deltaTime * foodRate);
        if (useMana)
        {
            currentDelayTime += Time.deltaTime;
            if (currentDelayTime >= delayTimeRecover)
            {
                currentDelayTime = 0f;
                useMana = false;
            }
        }
        else
        {
            if (currentFood > 0f)
            {
                currentMana = Mathf.Min(maxMana + plusMana, currentMana + Time.deltaTime * recoverManaRate);
                if (currentMana < maxMana)
                {
                    currentFood = Mathf.Max(0, currentFood - Time.deltaTime * foodRateMana);
                    foodSlide.value = currentFood;
                }
            }
        }
    }
    private void LateUpdate()
    {
        foodSlide.value = currentFood;
        healthSlide.value = currentHealth;
        manaSlide.value = currentMana;
    }
    public bool UseMana(float value)
    {
        if (currentDelayWaitTime < delayWaitTime)
        {
            return false;
        }
        currentMana = Mathf.Max(0, currentMana - value);
        useMana = currentMana > 0;
        if (currentMana == 0)
        {
            currentDelayWaitTime = 0f;
        }
        return currentMana > 0;
    }
    public void Recover(RecoverEnum type, float value)
    {
        switch (type)
        {
            case RecoverEnum.Health:
                currentHealth = Mathf.Min(maxHealth + plusHealth, currentHealth + value);
                break;
            case RecoverEnum.Mana:
                currentMana = Mathf.Min(maxMana + plusMana, currentMana + value);
                break;
            case RecoverEnum.Food:
                currentFood = Mathf.Min(maxFood + plusFood, currentFood + value);
                break;
            default:
                break;
        }
    }
    public bool GetDamage(float damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        PreferenceController.instance.AddText(transform.transform.position + Vector3.up * 2f, "-" + damage.ToString(), Color.red);
        if (currentHealth == 0)
        {
            objectDie = true;
            Invoke(nameof(RespawnObject), respawnTime);
        }
        return currentHealth == 0;
    }
    public bool ObjectDie()
    {
        return objectDie;
    }
    public void Recover(float health, float mana, float food)
    {
        currentHealth = Mathf.Min(maxHealth + plusHealth, currentHealth + health);
        currentMana = Mathf.Min(maxMana + plusMana, currentMana + mana);
        currentFood = Mathf.Min(maxFood + plusFood, currentFood + food);
    }
    public void UpdatePlus(float plusHealth, float plusMana, float plusFood)
    {
        this.plusHealth = plusHealth;
        this.plusMana = plusMana;
        this.plusFood = plusFood;
        SliderUpdate();
    }

    public void RespawnObject()
    {
        if (objectDie)
        {
            objectDie = false;
            GetComponent<PlayerMovement>().ChangePosition(respawnPosition);
            currentHealth = Mathf.Round((maxHealth + plusHealth) / 2);
            currentMana = Mathf.Round((maxMana + plusMana) / 2);
            currentFood = Mathf.Round((maxFood + plusFood) / 2);
            CoinController.instance.ChangeCurrentCoin(0);
            InventoryController.instance.ClearAllStock();
        }
    }
}
public enum RecoverEnum
{
    Health,
    Mana,
    Food
}

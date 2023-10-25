using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOffset : MonoBehaviour
{
    [SerializeField] private float radiousY = 0.25f;
    [SerializeField] private float radiousX = 0f;
    [SerializeField] private float radiousZ = 0f;
    [SerializeField] private float offsetY = 0f;

    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private LayerMask mask;
    float currentHealth = 0f;

    ItemOffset topItem = null;
    private ItemUpdating itemUpdating;
    float plusHealth = 0f;

    [SerializeField] private float priceEachHP = 0.1f;

    [Header("Health Recover Config")]
    [SerializeField] private bool useRecoverHealth = false;
    [SerializeField] private float recoverRate = 1f;


    private void Start()
    {
        currentHealth = maxHealth + plusHealth;
        itemUpdating = GetComponent<ItemUpdating>();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + Vector3.up * offsetY, new Vector3(radiousX, radiousY, radiousZ));
    }
    public void RecoverHealthAuto()
    {
        currentHealth = Mathf.Min(currentHealth + Time.deltaTime * recoverRate, maxHealth + plusHealth);
    }
    public Vector3 GetPosition(ItemSide side)
    {
        switch (side)
        {
            case ItemSide.Left:
                return transform.position + -1 * radiousX * Vector3.right;
            case ItemSide.Right:
                return transform.position + Vector3.right * radiousX;
            case ItemSide.Top:
                return transform.position + Vector3.up * radiousY;
            case ItemSide.Bottom:
                return transform.position - Vector3.up * radiousY;
            case ItemSide.Forward:
                return transform.position + Vector3.forward * radiousZ;
            case ItemSide.Backward:
                return transform.position + -1 * radiousZ * Vector3.forward;
            default:
                return transform.position + Vector3.up * radiousY;
        }
    }
    private void Update()
    {
        if (itemUpdating != null)
        {
            plusHealth = itemUpdating.GetPlusValue(UpdateItemType.Health);
        }
        if (useRecoverHealth)
        {
            RecoverHealthAuto();
        }
        Ray ray = new(transform.position, transform.up);
        RaycastHit[] hit = Physics.RaycastAll(ray, Mathf.Infinity, mask);
        if (hit.Length > 0)
        {
            if (hit[^1].collider.gameObject.TryGetComponent<ItemOffset>(out ItemOffset target))
            {
                topItem = target;
            }
        }
    }
    public bool GetDamage(float damage)
    {
        if (topItem != null)
        {
            return topItem.GetDamage(damage);
        }
        else
        {
            PreferenceController.instance.AddText(transform.transform.position + Vector3.up * 2f, "-" + damage.ToString(), Color.red);
            currentHealth = Mathf.Max(0, currentHealth - damage);
            if (currentHealth == 0)
            {
                Destroy(gameObject, 0.1f);
            }
            return currentHealth == 0f;
        }
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    public float GetMaxHealth()
    {
        return maxHealth + plusHealth;
    }
    public void RecoverHealthItem()
    {
        return;
        /*float remainHealth = maxHealth + plusHealth - currentHealth;
        float remainCoin = CoinController.instance.GetCurrentCoint();
        float newCoin = remainCoin - remainHealth * priceEachHP;
        if (newCoin < 0)
        {
            ErrorController.instance.ChangeTxt("Thiếu tiền \n(Coin is not enough!)", Color.red);
            return;
        }
        CoinController.instance.ChangeCurrentCoin(newCoin);
        currentHealth = Mathf.Min(currentHealth + remainHealth, maxHealth + plusHealth);*/
    }
    public string GetRecoverHealth()
    {
        float remainHealth = maxHealth + plusHealth - currentHealth;
        return remainHealth * priceEachHP + "";
    }
}

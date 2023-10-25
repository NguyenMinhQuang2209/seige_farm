using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Solider : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    [SerializeField] private float radious = 5f;
    [SerializeField] private GameObject vfx;
    [SerializeField] private LayerMask enemyMask;

    GameObject targetItem = null;
    private float stopDistance = 0.1f;
    [SerializeField] private float timeBwtAttack = 1f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float attackDistance = 1f;
    float currentTimeBwtAttack = 0f;
    Vector3 ownRootPosition;

    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    float currentHealth = 0f;
    ItemUpdating itemUpdating = null;

    [Header("Attacking")]
    [SerializeField] private GameObject attackPos;
    [SerializeField] private float attackingRadious = 0.5f;
    [SerializeField] private float damage = 1f;
    float plusHealth = 0f;
    float plusDamage = 0f;
    float plusRadious = 0f;
    float plusSpeed = 0f;

    private LongPickupItem longItem;

    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float dieDelayTime = 2f;


    bool objectDie = false;

    bool wasBack = false;

    [SerializeField] private float priceEachHP = 0.1f;


    bool endAttacking = true;

    [SerializeField] private bool enableCircle = false;
    [Header("UI config")]
    [SerializeField] private Texture2D solliderImage;


    private void Start()
    {
        currentHealth = maxHealth + plusHealth;
        itemUpdating = GetComponent<ItemUpdating>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        longItem = GetComponent<LongPickupItem>();
        if (vfx != null)
        {
            foreach (Transform child in vfx.transform)
            {
                if (child.TryGetComponent<ParticleSystem>(out ParticleSystem item))
                {
                    var shaped = item.shape;
                    shaped.radius = radious + plusRadious;
                    item.Stop();
                }
            }
        }
        if (agent != null)
        {
            agent.speed = speed;
            agent.stoppingDistance = stopDistance;
        }
        ownRootPosition = transform.position;
    }
    private void Update()
    {
        agent.speed = speed + plusSpeed;
        if (itemUpdating == null)
        {
            itemUpdating = GetComponent<ItemUpdating>();
        }
        if (longItem.ArePickUp())
        {
            wasBack = false;
            return;
        }
        if (objectDie)
        {
            return;
        }
        if (itemUpdating != null)
        {
            plusHealth = itemUpdating.GetPlusValue(UpdateItemType.Health);
            plusDamage = itemUpdating.GetPlusValue(UpdateItemType.Damage);
            plusRadious = itemUpdating.GetPlusValue(UpdateItemType.Radious);
            plusSpeed = itemUpdating.GetPlusValue(UpdateItemType.Speed);

            if (plusRadious > 0f)
            {
                if (vfx != null)
                {
                    foreach (Transform child in vfx.transform)
                    {
                        if (child.TryGetComponent<ParticleSystem>(out ParticleSystem item))
                        {
                            var shaped = item.shape;
                            shaped.radius = radious + plusRadious;
                        }
                    }
                }
            }
        }

        animator.SetFloat("Speed", agent.remainingDistance >= 0.1f ? 1f : 0f);
        if (endAttacking)
        {
            currentTimeBwtAttack += Time.deltaTime;
        }
        if (targetItem != null)
        {
            wasBack = false;
            if (targetItem.TryGetComponent<AttackOffset>(out AttackOffset item))
            {
                stopDistance = item.offset;
            }
            float toTargetDistance = Vector3.Distance(transform.position, targetItem.transform.position);
            if (toTargetDistance > stopDistance)
            {
                agent.SetDestination(targetItem.transform.position);
            }
            else
            {
                agent.SetDestination(transform.position);
                Vector3 targetDir = targetItem.transform.position - transform.position;
                Quaternion desiredRotation = Quaternion.LookRotation(targetDir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotateSpeed * Time.deltaTime);
            }

            if (toTargetDistance <= attackDistance)
            {
                if (currentTimeBwtAttack >= timeBwtAttack)
                {
                    currentTimeBwtAttack = 0f;
                    Attacking();
                }
            }
            return;
        }
        else
        {
            if (!wasBack)
            {
                agent.SetDestination(longItem.GetRootPosition());
                wasBack = true;
            }
        }
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, radious + plusRadious, Vector3.forward, 0f, enemyMask);
        if (hits.Length > 0)
        {
            targetItem = hits[0].collider.gameObject;
        }

    }

    private void OnDrawGizmos()
    {
        if (!enableCircle)
        {
            return;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radious + plusRadious);
        if (attackPos != null)
        {
            Gizmos.DrawSphere(attackPos.transform.position, attackingRadious);
        }
    }
    private void Attacking()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
            endAttacking = false;
        }
    }
    public void EndAttacking()
    {
        endAttacking = true;
    }
    public void AttackingHit()
    {
        RaycastHit[] hits = Physics.SphereCastAll(attackPos.transform.position, attackingRadious, Vector3.forward, 0f, enemyMask);
        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
                {
                    enemy.GetDamage(damage + plusDamage, gameObject);
                }
                else if (hit.collider.gameObject.TryGetComponent<WildAnimal>(out WildAnimal wildAnimal))
                {
                    wildAnimal.GetDamage(damage, gameObject);
                }
            }
        }
    }
    public bool GetDamage(float damage)
    {
        if (objectDie)
        {
            return true;
        }
        PreferenceController.instance.AddText(transform.transform.position + Vector3.up * 2f, "-" + damage.ToString(), Color.red);
        currentHealth = Mathf.Max(0f, currentHealth - damage);
        if (currentHealth == 0f)
        {
            animator.SetTrigger("Die");
            objectDie = true;
            Destroy(gameObject, dieDelayTime);
        }
        return currentHealth == 0f;
    }
    public void RecoverHealthItem()
    {
        float remainHealth = maxHealth + plusHealth - currentHealth;
        float remainCoin = CoinController.instance.GetCurrentCoint();
        float newCoin = remainCoin - remainHealth * priceEachHP;
        if (newCoin < 0)
        {
            ErrorController.instance.ChangeTxt("Thiếu tiền \n(Coin is not enough!)", Color.red);
            return;
        }
        CoinController.instance.ChangeCurrentCoin(newCoin);
        currentHealth = Mathf.Min(currentHealth + remainHealth, maxHealth + plusHealth);
    }
    public string GetRecoverHealth()
    {
        float remainHealth = maxHealth + plusHealth - currentHealth;
        return remainHealth * priceEachHP + "";
    }
    public Texture2D GetItemImage()
    {
        return solliderImage;
    }
    public string GetItemHP()
    {
        return currentHealth + "/" + (maxHealth + plusHealth);
    }
    public string GetNextUpdate()
    {
        return itemUpdating ? itemUpdating.GetLevel() : "Level: 0";
    }
    public int GetCoin()
    {
        return itemUpdating ? itemUpdating.GetCoin() : 0;
    }
    public void UpdateItem()
    {
        itemUpdating.UpdatingItem();
    }
    public float GetCurrentDamage()
    {
        return damage + plusDamage;
    }

    public string GetRadious()
    {
        return radious + plusRadious + "";
    }

}

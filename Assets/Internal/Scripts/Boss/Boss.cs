using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    bool hasPlayer = false;

    private Animator animator;
    private NavMeshAgent agent;

    [SerializeField] private float runSpeed = 4f;
    [SerializeField] private float tiredAt = 20f;
    [SerializeField] private float restTime = 3f;
    float currentTiredAt = 0f;
    float currentRestTime = 0f;
    private GameObject player;

    [Header("attacking")]
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRadious = 1f;
    [SerializeField] private int attackShortMax = 2;
    [SerializeField] private float stopDistance = 1f;

    [SerializeField] private List<float> damage = new();
    [SerializeField] private LayerMask attackMask;

    [SerializeField] private float timeBwtAttack = 0f;
    float currentTimeBwtAttack = 2f;

    bool isAttacking = false;
    int randomAttack = 0;

    [Header("Attack 2")]
    [SerializeField] private GameObject bulletContainer;
    [SerializeField] private float bulletDamage = 1f;
    [SerializeField] private float bulletContainerDelayDieTime = 1f;
    [SerializeField] private float bulletSpeed = 500f;
    [SerializeField] private float bulletDelayTimeDie = 1f;

    [Header("Enemy health")]
    [SerializeField] private float maxHealth = 1000f;
    float currentHealth = 0f;

    [Header("Attack 3")]
    [SerializeField] private RockAttack rock;
    [SerializeField] private float rockDamage = 1f;
    [SerializeField] private float rockSpeed = 1f;
    [SerializeField] private GameObject rockAttackPos;
    [SerializeField] private float angleAttack = 8f;
    [SerializeField] private float rockDelayDieTime = 1f;

    [Header("Far Attack")]
    [SerializeField] private int farAttackFrom = 3;
    [SerializeField] private int farAttackTo = 5;
    [SerializeField] private float farAttackTime = 2f;
    [SerializeField] private float farAttackDistance = 10f;
    float farAttackCurrentTime = 0f;

    [Header("Slider")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI txtHealth;

    [Header("Win")]
    [SerializeField] private GameObject winUI;
    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = runSpeed;
        if (PreferenceController.instance != null)
        {
            player = PreferenceController.instance.Player;
        }
        currentHealth = maxHealth;
        txtHealth.text = currentHealth + "/" + maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.minValue = 0f;
        healthSlider.value = currentHealth;
        healthSlider.gameObject.SetActive(false);
        winUI.SetActive(false);
    }
    private void Update()
    {
        agent.speed = runSpeed;
        if (player == null)
        {
            if (PreferenceController.instance != null)
            {
                player = PreferenceController.instance.Player;
            }
        }
        healthSlider.gameObject.SetActive(hasPlayer);
        healthSlider.value = currentHealth;
        if (!hasPlayer)
        {
            currentHealth = maxHealth;
            animator.SetFloat("Speed", 0f);
            return;
        }
        float currentSpeed;
        currentTiredAt += Time.deltaTime;
        if (!isAttacking)
        {
            currentTimeBwtAttack += Time.deltaTime;
            farAttackCurrentTime += Time.deltaTime;
        }
        if (currentTiredAt >= tiredAt)
        {
            currentRestTime += Time.deltaTime;
            currentSpeed = 0f;
            if (currentRestTime >= restTime)
            {
                currentRestTime = 0f;
                currentTiredAt = 0f;
            }
        }
        else
        {
            currentRestTime = 0f;
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance <= stopDistance)
            {
                currentSpeed = 0f;
                Vector3 targetDir = player.transform.position - transform.position;
                Quaternion desiredRotation = Quaternion.LookRotation(targetDir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, 10f * Time.deltaTime);
                if (currentTimeBwtAttack >= timeBwtAttack)
                {
                    currentTimeBwtAttack = 0f;
                    isAttacking = true;
                    randomAttack = Random.Range(0, attackShortMax + 1);
                    animator.SetTrigger("Attack");
                    animator.SetFloat("AttackIndex", randomAttack);
                }
                agent.SetDestination(transform.position);
            }
            else
            {
                if (distance <= farAttackDistance)
                {
                    if (farAttackCurrentTime >= farAttackTime)
                    {
                        farAttackCurrentTime = 0f;
                        isAttacking = true;
                        randomAttack = Random.Range(farAttackFrom, farAttackTo + 1);
                        animator.SetTrigger("Attack");
                        animator.SetFloat("AttackIndex", randomAttack);
                    }
                }
                currentSpeed = runSpeed;
                agent.SetDestination(player.transform.position);
            }
        }
        animator.SetFloat("Speed", currentSpeed);
    }
    private void OnDrawGizmos()
    {
        if (attackPos != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackPos.position, attackRadious);
        }
    }
    public void Attack1()
    {
        Collider[] hits = Physics.OverlapSphere(attackPos.position, attackRadious, attackMask);
        bool targetDie = false;
        foreach (Collider hit in hits)
        {
            if (hit.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth health))
            {
                targetDie = health.GetDamage(damage[randomAttack]);
            }
        }
        if (targetDie)
        {
            hasPlayer = false;
        }
    }
    public void Attack2()
    {
        GameObject bulletContain = Instantiate(bulletContainer, transform);
        if (bulletContain.TryGetComponent(out BossBullet bullet))
        {
            bullet.Initialized(bulletDamage, bulletSpeed, bulletContainerDelayDieTime, bulletDelayTimeDie);
        }
    }
    public void Attack3()
    {
        for (float i = 0; i < 360f; i += angleAttack)
        {
            Vector3 newRotation = new(0f, i, 0f);
            RockAttack rockBullet = Instantiate(rock, transform.position, Quaternion.Euler(newRotation));
            rockBullet.Initialized(rockDamage, rockSpeed, rockDelayDieTime);
        }
    }
    public void EndAttacking()
    {
        isAttacking = false;
    }

    public void HavingPlayer()
    {
        Invoke(nameof(TriggerHasPlayer), 0.5f);
    }
    private void TriggerHasPlayer()
    {
        hasPlayer = true;
    }

    public void GetDamage(float damage)
    {
        currentHealth = Mathf.Max(0f, currentHealth - damage);
        PreferenceController.instance.AddText(transform.transform.position + Vector3.up * 2f, "-" + damage.ToString(), Color.red);
        if (currentHealth == 0)
        {
            winUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }

}

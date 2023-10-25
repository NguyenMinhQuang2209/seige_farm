using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Player Hand Attacking")]
    [SerializeField] private int maxPunch = 1;
    [SerializeField] private float timeBwtAttack = 1f;
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private Animator animator;
    [SerializeField] private float handEnemyDamage = 5f;
    [SerializeField] private float handTreeDamage = 5f;
    [SerializeField] private float handRockDamage = 5f;
    bool attacking = false;

    [Header("Player Sword Attacking")]
    [SerializeField] private int maxSwordPunch = 1;
    [SerializeField] private float swordTimeBwtAttack = 1f;

    [Header("Player Gun Attacking")]
    [SerializeField] private float bowTimeBwtAttack = 1f;

    [Header("Player tools Attacking")]
    [SerializeField] private float toolsTimeBwtAttack = 1f;

    float currentEnemyDamage = 0f;
    float currentTreeDamage = 0f;
    float currentRockDamage = 0f;

    private CharacterEquipment characterEquipment;

    GameObject weaponItem = null;

    ItemType? weaponType = null;

    float currentTimeMaxBwtAttack = 0f;
    float currentTimeBwtAttack = 0f;

    private void Start()
    {
        if (PreferenceController.instance != null)
        {
            GameObject player = PreferenceController.instance.Player;
            if (player != null)
            {
                playerInput = player.GetComponent<PlayerInput>();
                playerMovement = player.GetComponent<PlayerMovement>();
            }
        }
        animator = GetComponent<Animator>();
        currentEnemyDamage = handEnemyDamage;
        currentTreeDamage = handTreeDamage;
        currentRockDamage = handRockDamage;
        characterEquipment = GetComponent<CharacterEquipment>();
        currentTimeBwtAttack = 0f;
        currentTimeMaxBwtAttack = timeBwtAttack;
    }
    private void Update()
    {
        if (PreferenceController.instance != null)
        {
            GameObject player = PreferenceController.instance.Player;
            if (player != null)
            {
                playerInput = player.GetComponent<PlayerInput>();
                playerMovement = player.GetComponent<PlayerMovement>();
            }
        }
        Attacking();

        GameObject handHold = characterEquipment.GetHandWeapon();

        if (weaponItem != handHold)
        {
            weaponItem = handHold;
            if (weaponItem != null)
            {
                if (weaponItem.TryGetComponent<WeaponConfig>(out WeaponConfig config))
                {
                    currentEnemyDamage = config.GetEnemyDamage();
                    currentTreeDamage = config.GetTreeDamage();
                    currentRockDamage = config.GetRockDamage();
                    weaponType = config.GetItemType();
                    if (config.GetTimeBwtAttackCustom())
                    {
                        currentTimeMaxBwtAttack = config.GetTimeBwtAttack();
                    }
                    else
                    {
                        switch (weaponType)
                        {
                            case ItemType.Sword:
                                currentTimeMaxBwtAttack = swordTimeBwtAttack;
                                break;
                            case ItemType.Gun:
                                currentTimeMaxBwtAttack = bowTimeBwtAttack;
                                break;
                            case ItemType.Tools:
                                currentTimeMaxBwtAttack = toolsTimeBwtAttack;
                                break;
                            default:
                                currentTimeMaxBwtAttack = timeBwtAttack;
                                break;
                        }
                    }
                }
                else
                {
                    currentEnemyDamage = handEnemyDamage;
                    currentTreeDamage = handTreeDamage;
                    currentRockDamage = handRockDamage;
                    currentTimeMaxBwtAttack = timeBwtAttack;
                    weaponType = null;
                }
            }
            else
            {
                currentEnemyDamage = handEnemyDamage;
                currentTreeDamage = handTreeDamage;
                currentRockDamage = handRockDamage;
                currentTimeMaxBwtAttack = timeBwtAttack;
                weaponType = null;
            }
        }

        if (weaponType.Equals(ItemType.Gun))
        {
            if (weaponItem.TryGetComponent<GunSetup>(out GunSetup setup))
            {
                BulletController.instance.ChangeBulletTxt(setup.GetMagazine());
            }
            else
            {
                BulletController.instance.ChangeBulletTxt(string.Empty);
            }
        }
        else
        {
            BulletController.instance.ChangeBulletTxt(string.Empty);
        }
    }
    public void Attacking()
    {
        //playerMovement.ChangeAttackingStatus(attacking);
        animator.SetBool("Sword", weaponType.Equals(ItemType.Sword));
        animator.SetBool("Gun", weaponType.Equals(ItemType.Gun));
        animator.SetBool("Tools", weaponType.Equals(ItemType.Tools));
        if (!attacking)
        {
            currentTimeBwtAttack += Time.deltaTime;
            if (currentTimeBwtAttack >= currentTimeMaxBwtAttack)
            {
                currentTimeBwtAttack = currentTimeMaxBwtAttack;
                if (playerInput.onFoot.Attack.IsPressed() && playerMovement.CanAttack())
                {
                    currentTimeBwtAttack = 0f;
                    switch (weaponType)
                    {
                        case ItemType.Sword:
                            int swordPunch = Random.Range(0, maxSwordPunch);
                            animator.SetFloat("SwordAttackIndex", swordPunch);
                            animator.SetTrigger("Attack");
                            StartAttacking();
                            break;
                        case ItemType.Gun:
                            GunAttacking();
                            break;
                        case ItemType.Tools:
                            int punch = Random.Range(0, maxPunch);
                            animator.SetFloat("AttackIndex", punch);
                            animator.SetTrigger("Attack");
                            StartAttacking();
                            break;
                        default:
                            int handPunch = Random.Range(0, maxPunch);
                            animator.SetFloat("AttackIndex", handPunch);
                            animator.SetTrigger("Attack");
                            StartAttacking();
                            MusicController.instance.handAttackMusic.Play();
                            break;
                    }
                }
            }
        }

    }
    private void GunAttacking()
    {
        if (weaponItem != null)
        {
            if (weaponItem.TryGetComponent<GunSetup>(out GunSetup gun))
            {
                gun.Shot();
            }
        }
    }
    public void StartAttacking()
    {
        attacking = true;
    }
    public void EndAttacking()
    {
        attacking = false;
    }
    public bool IsAttacking()
    {
        return attacking;
    }
    public float GetEnemyDamage()
    {
        return currentEnemyDamage;
    }
    public float GetTreeDamage()
    {
        return currentTreeDamage;
    }
    public float GetRockDamage()
    {
        return currentRockDamage;
    }
    public void PlayerReload()
    {
        animator.SetTrigger("Reload");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingControl : MonoBehaviour
{
    private GameObject player;
    private PlayerAttack playerAttack;
    [SerializeField] private GameObject attackingPoint;
    [SerializeField] private float radious = 0.1f;
    bool wasHit = false;
    void Start()
    {
        player = PreferenceController.instance.Character;
        if (player != null)
        {
            playerAttack = player.GetComponent<PlayerAttack>();
        }
        wasHit = false;
    }

    void Update()
    {
        if (player == null)
        {
            player = PreferenceController.instance.Character;
            if (player != null)
            {
                playerAttack = player.GetComponent<PlayerAttack>();
            }
        }
        if (playerAttack != null)
        {
            if (playerAttack.IsAttacking())
            {
                if (!wasHit)
                {
                    Attacking();
                }
            }
            else
            {
                wasHit = false;
            }
        }
    }
    private void Attacking()
    {
        Collider[] colliders = Physics.OverlapSphere(attackingPoint.transform.position, radious);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.TryGetComponent<ItemHealth>(out ItemHealth item))
            {
                item.GetDamage(playerAttack.GetEnemyDamage(), playerAttack.GetTreeDamage(), playerAttack.GetRockDamage());
                wasHit = true;
                MusicController.instance.hitMusic.Play();
                return;
            }
            else if (collider.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
            {
                enemy.GetDamage(playerAttack.GetEnemyDamage(), gameObject);
                wasHit = true;
                MusicController.instance.hitMusic.Play();
                return;
            }
            else if (collider.gameObject.TryGetComponent<WildAnimal>(out WildAnimal wildAinamal))
            {
                wildAinamal.GetDamage(playerAttack.GetEnemyDamage(), gameObject);
                wasHit = true;
                MusicController.instance.hitMusic.Play();
                return;
            }
            else if (collider.gameObject.TryGetComponent<ShyAnimal>(out ShyAnimal shyAnimal))
            {
                shyAnimal.GetDamage(playerAttack.GetEnemyDamage());
                wasHit = true;
                MusicController.instance.hitMusic.Play();
                return;
            }
            else if (collider.gameObject.TryGetComponent(out Boss boss))
            {
                boss.GetDamage(playerAttack.GetEnemyDamage());
                wasHit = true;
                MusicController.instance.hitMusic.Play();
                return;
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (attackingPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackingPoint.transform.position, radious);
        }
    }
}

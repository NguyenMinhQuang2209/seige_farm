using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    bool shot = false;
    Vector3 dir;
    float speed = 1f;
    float damage = 1f;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (shot)
        {
            rb.velocity = speed * Time.deltaTime * dir;
        }
    }
    public void Shoot(Vector3 newDir, float newSpeed, float destroyTime, float newDamage)
    {
        speed = newSpeed;
        damage = newDamage;
        dir = newDir;
        shot = true;
        Destroy(gameObject, destroyTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
        {
            enemy.GetDamage(damage, PreferenceController.instance.Player);
        }
        else if (target.TryGetComponent(out Boss boss))
        {
            boss.GetDamage(damage);
        }
        if (!target.CompareTag(TagController.playerTag) && !target.CompareTag(TagController.bulletTag))
        {
            Destroy(gameObject);
        }
    }
}

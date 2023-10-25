using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody rb;
    bool shot = false;
    Vector3 dir;
    float speed = 1f;
    float damage = 1f;
    GameObject parentItem = null;
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
    public void Shoot(GameObject parentItem, Vector3 newDir, float newSpeed, float destroyTime, float newDamage)
    {
        speed = newSpeed;
        damage = newDamage;
        dir = newDir;
        shot = true;
        this.parentItem = parentItem;
        Destroy(gameObject, destroyTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.TryGetComponent<PlayerHealth>(out PlayerHealth player))
        {
            player.GetDamage(damage);
        }
        else if (target.TryGetComponent<Solider>(out Solider sollider))
        {
            sollider.GetDamage(damage);
        }
        if (target.gameObject != parentItem)
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletItem : MonoBehaviour
{
    private Rigidbody rb;
    float damage = 0f;
    float speed = 0f;

    bool attack = false;
    Vector3 dir = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (!attack)
        {
            return;
        }
        rb.velocity = speed * Time.deltaTime * dir;
    }
    public void Initialize(float damage, float speed, Vector3 dir, float delayTime)
    {
        this.damage = damage;
        this.speed = speed;
        this.dir = dir;
        attack = true;
        Destroy(gameObject, delayTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject hit = other.gameObject;
        if (hit.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.GetDamage(damage);
        }
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockAttack : MonoBehaviour
{
    private Rigidbody rb;
    float damage = 0f;
    float speed = 0f;
    bool attacking = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Initialized(float damage, float speed, float delayDieTime)
    {
        this.damage = damage;
        this.speed = speed;
        attacking = true;
        Destroy(gameObject, delayDieTime);
    }
    public void Update()
    {
        if (!attacking)
        {
            return;
        }
        rb.velocity = speed * Time.deltaTime * transform.forward;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Boss") && !other.gameObject.CompareTag("Ground"))
        {
            if (other.gameObject.TryGetComponent(out PlayerHealth playerHealth))
            {
                playerHealth.GetDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}

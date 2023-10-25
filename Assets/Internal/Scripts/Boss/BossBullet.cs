using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    float damage = 0f;
    float speed = 0f;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float startShoot = 2f;
    [SerializeField] private float timeBwtShoot = 2f;
    private float bulletDelayTime = 0f;

    float currentStartShootTime = 0f;
    float currentTimeBwtShoot = 0f;

    private GameObject player;
    private void Start()
    {
        player = PreferenceController.instance.Player;
    }
    public void Initialized(float damage, float bulletSpeed, float bulletContaineDelayTimeDie, float bulletDelayTimeDie)
    {
        this.damage = damage;
        speed = bulletSpeed;
        bulletDelayTime = bulletDelayTimeDie;
        Destroy(gameObject, bulletContaineDelayTimeDie);
    }

    private void Update()
    {
        currentStartShootTime = Mathf.Min(currentStartShootTime + Time.deltaTime, startShoot);
        if (currentStartShootTime < startShoot)
        {
            return;
        }
        currentTimeBwtShoot = Mathf.Min(currentTimeBwtShoot + Time.deltaTime, timeBwtShoot);
        if (currentTimeBwtShoot >= timeBwtShoot)
        {
            currentTimeBwtShoot = 0f;
            Shoot();
        }
    }
    public void Shoot()
    {
        GameObject bulletItem = Instantiate(bullet, transform.position, Quaternion.identity);
        if (bulletItem.TryGetComponent(out BossBulletItem item))
        {
            Vector3 dir = player.transform.position - transform.position;
            item.Initialize(damage, speed, dir, bulletDelayTime);
        }
    }
}

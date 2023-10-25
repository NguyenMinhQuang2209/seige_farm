using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GunSetup : MonoBehaviour
{
    [SerializeField] private GameObject shootPlace;
    [SerializeField] private float bulletSpeed = 1f;
    [SerializeField] private float bulletDelayTime = 1f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private int numberBullet = 1;
    [SerializeField] private float bulletAngle = 5f;
    [SerializeField] private int bulletMagazine = 1;
    [SerializeField] private float reloadTime = 2.5f;
    int currentBulletMagazine = 0;
    private GameObject bullet;
    private int totalBullet = 1;
    bool reload = false;
    int remainBullet = 0;
    private GameObject character;
    private void Start()
    {
        if (PreferenceController.instance != null)
        {
            bullet = PreferenceController.instance.bullet;
            character = PreferenceController.instance.Character;
        }
        totalBullet = InventoryController.instance.GetQuantityInStock(ItemName.Bullet);
        currentBulletMagazine = totalBullet > bulletMagazine ? bulletMagazine : totalBullet;
        remainBullet = totalBullet - currentBulletMagazine;
    }
    private void Update()
    {
        if (bullet == null)
        {
            if (PreferenceController.instance != null)
            {
                bullet = PreferenceController.instance.bullet;
                character = PreferenceController.instance.Character;
            }
        }
        int newTotalBullet = InventoryController.instance.GetQuantityInStock(ItemName.Bullet);
        if (totalBullet != newTotalBullet)
        {
            totalBullet = newTotalBullet;
            remainBullet = totalBullet - currentBulletMagazine;
        }
        if (currentBulletMagazine == 0 && !reload)
        {
            reload = true;
            Invoke(nameof(ReloadItem), reloadTime);
            if (totalBullet > 0)
            {
                if (character.TryGetComponent<PlayerAttack>(out PlayerAttack player))
                {
                    player.PlayerReload();
                }
            }
        }
    }
    public void Shot()
    {
        int remainBullet;
        remainBullet = currentBulletMagazine >= numberBullet ? numberBullet : currentBulletMagazine;
        InventoryController.instance.RemoveItem(ItemName.Bullet, ItemType.Other, remainBullet);
        currentBulletMagazine = Mathf.Max(0, currentBulletMagazine - remainBullet);
        Vector3 playerDir = PreferenceController.instance.Player.transform.forward;
        for (int i = 0; i < remainBullet; i++)
        {
            GameObject newBullet = Instantiate(bullet, shootPlace.transform.position, Quaternion.identity);
            if (newBullet.TryGetComponent<Bullet>(out Bullet bull))
            {
                float bulletAngleDir = (i % 2 == 0) ? i * bulletAngle : -i * bulletAngle;
                Vector3 dir = Quaternion.Euler(0, bulletAngleDir, 0) * playerDir;
                bull.Shoot(dir, bulletSpeed, bulletDelayTime, damage);
            }
        }
    }
    public void ReloadItem()
    {
        currentBulletMagazine = totalBullet > bulletMagazine ? bulletMagazine : totalBullet;
        remainBullet = totalBullet - currentBulletMagazine;
        reload = false;
    }
    public string GetMagazine()
    {
        return currentBulletMagazine + "/" + (remainBullet);
    }
}

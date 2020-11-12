using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firearm : MonoBehaviour
{
    public float reloadTime;
    public float fireRate;
    public float damage;
    public float fireForce;
    public Bullet bulletPrefab;


    private float shotCooldown;

    public void Fire(Vector3 crosshairPosition)
    {
        if(Time.time > shotCooldown)
        {
            Vector3 shootingDirection = crosshairPosition - transform.position;
            Bullet newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as Bullet;
            newBullet.transform.up = shootingDirection;
            newBullet.SetDamage(damage);
            Rigidbody newBulletRB = newBullet.GetComponent<Rigidbody>();
            newBulletRB.AddForce(shootingDirection.normalized * fireForce);
            Destroy(newBullet, 2);
            shotCooldown = Time.time + fireRate;
            
        }
        
    }

    public void Reload(float reloadTime)
    {
        // initiate reload
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float explosionRange;
    public float explosionPower;
    public float explosionDamage;
    public float maxRange;
    public Rigidbody grenadeRb;
    public GameObject explosionEffect;
    
    public void CalculatePath(Transform playerPosition, Vector3 crosshairPosition)
    {
        float heightAdjustment = playerPosition.position.y - crosshairPosition.y;
        Vector3 z = crosshairPosition - new Vector3(playerPosition.position.x, playerPosition.position.y + 1f, playerPosition.position.z);
        float grenadeVelocity = Mathf.Sqrt((z.magnitude * Physics.gravity.magnitude) / Mathf.Sin(2 * 45)) * 0.9f - heightAdjustment * 0.5f;
        grenadeRb.velocity = (playerPosition.forward + playerPosition.up).normalized * grenadeVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {

        GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(explosion, 1);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRange);
        foreach(Collider hitObject in colliders)
        {
            if (hitObject.CompareTag("Zombie"))
            {
                Zombie zombie = hitObject.GetComponent<Zombie>();
                if (zombie.handleDamage(explosionDamage, hitObject.transform))
                {
                    Rigidbody[] rigidbodies = zombie.GetComponentsInChildren<Rigidbody>();
                    foreach(Rigidbody zombieRb in rigidbodies)
                    {
                        zombieRb.AddExplosionForce(explosionPower, transform.position, explosionRange);
                    }
                    
                }
                    
            }
        }
        Destroy(this.gameObject);
    }


}

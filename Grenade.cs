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

    float counter;
    bool endReached;
    Vector3 startPos;
    float speed = 1f;
    Vector3 traveldistance;
    
    public void CalculatePath(Transform playerPosition, Vector3 crosshairPosition)
    {
        
        Vector3 z = crosshairPosition - new Vector3(playerPosition.position.x, playerPosition.position.y + 1f, playerPosition.position.z);
        float grenadeVelocity = Mathf.Sqrt((z.magnitude * Physics.gravity.magnitude) / Mathf.Sin(2*45)) * 0.9f;
        grenadeRb.velocity = (playerPosition.forward + playerPosition.up).normalized * grenadeVelocity;
    }
    



}
